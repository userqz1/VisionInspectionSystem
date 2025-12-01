using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using Basler.Pylon;
using VisionInspectionSystem.Models;
using VisionInspectionSystem.Common;
using VisionInspectionSystem.DAL;

namespace VisionInspectionSystem.HAL
{
    /// <summary>
    /// Basler相机封装类
    /// </summary>
    public class BaslerCamera : IDisposable
    {
        #region 私有字段

        private bool _isDisposed;
        private bool _isConnected;
        private bool _isGrabbing;
        private Bitmap _lastImage;
        private readonly object _lockObj = new object();
        private CancellationTokenSource _cts;

        // Pylon相机对象
        private Camera _camera;
        private PixelDataConverter _converter;

        #endregion

        #region 公共属性

        /// <summary>
        /// 相机名称
        /// </summary>
        public string CameraName { get; private set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public string SerialNumber { get; private set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string ModelName { get; private set; }

        /// <summary>
        /// 相机ID
        /// </summary>
        public string CameraID => SerialNumber;

        /// <summary>
        /// IP地址（GigE相机）
        /// </summary>
        public string IpAddress { get; private set; }

        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected => _isConnected;

        /// <summary>
        /// 是否正在采集
        /// </summary>
        public bool IsGrabbing => _isGrabbing;

        /// <summary>
        /// 曝光时间（微秒）
        /// </summary>
        public double ExposureTime
        {
            get => _exposureTime;
            set
            {
                if (value >= ExposureTimeMin && value <= ExposureTimeMax)
                {
                    _exposureTime = value;
                    SetExposureTime(value);
                }
            }
        }
        private double _exposureTime = 10000;

        /// <summary>
        /// 增益（dB）
        /// </summary>
        public double Gain
        {
            get => _gain;
            set
            {
                if (value >= GainMin && value <= GainMax)
                {
                    _gain = value;
                    SetGain(value);
                }
            }
        }
        private double _gain = 0;

        /// <summary>
        /// 触发模式
        /// </summary>
        public TriggerMode TriggerMode
        {
            get => _triggerMode;
            set
            {
                _triggerMode = value;
                SetTriggerMode(value);
            }
        }
        private TriggerMode _triggerMode = TriggerMode.Off;

        /// <summary>
        /// 触发源
        /// </summary>
        public TriggerSource TriggerSource
        {
            get => _triggerSource;
            set
            {
                _triggerSource = value;
                SetTriggerSource(value);
            }
        }
        private TriggerSource _triggerSource = TriggerSource.Software;

        /// <summary>
        /// 图像宽度
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// 图像高度
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// 曝光时间最小值
        /// </summary>
        public double ExposureTimeMin { get; private set; } = 100;

        /// <summary>
        /// 曝光时间最大值
        /// </summary>
        public double ExposureTimeMax { get; private set; } = 1000000;

        /// <summary>
        /// 增益最小值
        /// </summary>
        public double GainMin { get; private set; } = 0;

        /// <summary>
        /// 增益最大值
        /// </summary>
        public double GainMax { get; private set; } = 24;

        /// <summary>
        /// 最后一张图像
        /// </summary>
        public Bitmap LastImage
        {
            get
            {
                lock (_lockObj)
                {
                    return _lastImage?.Clone() as Bitmap;
                }
            }
        }

        #endregion

        #region 事件

        /// <summary>
        /// 图像采集完成事件
        /// </summary>
        public event EventHandler<Bitmap> ImageGrabbed;

        /// <summary>
        /// 错误发生事件
        /// </summary>
        public event EventHandler<string> ErrorOccurred;

        /// <summary>
        /// 状态变化事件
        /// </summary>
        public event EventHandler<CameraState> StateChanged;

        #endregion

        #region 构造函数

        public BaslerCamera()
        {
            _converter = new PixelDataConverter();
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 枚举所有相机
        /// </summary>
        public static List<CameraInfo> EnumerateCameras()
        {
            List<CameraInfo> cameras = new List<CameraInfo>();

            try
            {
                // 方法1: 使用CameraFinder枚举所有设备
                List<ICameraInfo> allCameras = CameraFinder.Enumerate();

                LogHelper.Info("Camera", $"CameraFinder.Enumerate() 返回 {allCameras.Count} 个设备");

                if (allCameras.Count > 0)
                {
                    foreach (var cam in allCameras)
                    {
                        var info = new CameraInfo();

                        try { info.SerialNumber = cam[CameraInfoKey.SerialNumber]; }
                        catch { info.SerialNumber = "Unknown"; }

                        try { info.ModelName = cam[CameraInfoKey.ModelName]; }
                        catch { info.ModelName = "Unknown"; }

                        try
                        {
                            info.UserDefinedName = cam[CameraInfoKey.UserDefinedName];
                            if (string.IsNullOrEmpty(info.UserDefinedName))
                            {
                                info.UserDefinedName = cam[CameraInfoKey.FriendlyName];
                            }
                        }
                        catch { info.UserDefinedName = info.ModelName; }

                        try
                        {
                            if (cam.ContainsKey(CameraInfoKey.DeviceIpAddress))
                            {
                                info.IpAddress = cam[CameraInfoKey.DeviceIpAddress];
                            }
                        }
                        catch { }

                        cameras.Add(info);
                        LogHelper.Info("Camera", $"发现相机: {info.UserDefinedName} ({info.SerialNumber}) - {info.ModelName}");
                    }
                }
                else
                {
                    // 方法2: CameraFinder返回空时，尝试直接创建Camera对象
                    LogHelper.Info("Camera", "CameraFinder返回空，尝试直接创建Camera对象...");
                    try
                    {
                        using (Camera testCamera = new Camera())
                        {
                            var info = new CameraInfo
                            {
                                SerialNumber = testCamera.CameraInfo[CameraInfoKey.SerialNumber],
                                ModelName = testCamera.CameraInfo[CameraInfoKey.ModelName],
                                UserDefinedName = testCamera.CameraInfo[CameraInfoKey.FriendlyName]
                            };

                            try
                            {
                                if (testCamera.CameraInfo.ContainsKey(CameraInfoKey.DeviceIpAddress))
                                {
                                    info.IpAddress = testCamera.CameraInfo[CameraInfoKey.DeviceIpAddress];
                                }
                            }
                            catch { }

                            cameras.Add(info);
                            LogHelper.Info("Camera", $"通过Camera()发现相机: {info.UserDefinedName} ({info.SerialNumber})");
                        }
                    }
                    catch (Exception ex2)
                    {
                        LogHelper.Info("Camera", $"直接创建Camera失败: {ex2.Message}");
                    }
                }

                LogHelper.Info("Camera", $"最终找到 {cameras.Count} 个相机");
            }
            catch (Exception ex)
            {
                LogHelper.Error("Camera", $"枚举相机失败: {ex.Message}");
                LogHelper.Error("Camera", $"异常详情: {ex.ToString()}");
            }

            return cameras;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 打开相机
        /// </summary>
        public bool Open(string serialNumber)
        {
            try
            {
                if (_isConnected)
                {
                    Close();
                }

                SerialNumber = serialNumber;

                _camera = new Camera(serialNumber);
                _camera.Open();

                // 获取相机信息
                CameraName = _camera.CameraInfo[CameraInfoKey.UserDefinedName];
                if (string.IsNullOrEmpty(CameraName))
                {
                    CameraName = _camera.CameraInfo[CameraInfoKey.FriendlyName];
                }
                ModelName = _camera.CameraInfo[CameraInfoKey.ModelName];

                // GigE相机才有IP地址
                if (_camera.CameraInfo.ContainsKey(CameraInfoKey.DeviceIpAddress))
                {
                    IpAddress = _camera.CameraInfo[CameraInfoKey.DeviceIpAddress];
                }

                // 获取图像尺寸
                Width = (int)_camera.Parameters[PLCamera.Width].GetValue();
                Height = (int)_camera.Parameters[PLCamera.Height].GetValue();

                // 获取曝光参数范围
                if (_camera.Parameters[PLCamera.ExposureTime].IsReadable)
                {
                    ExposureTimeMin = _camera.Parameters[PLCamera.ExposureTime].GetMinimum();
                    ExposureTimeMax = _camera.Parameters[PLCamera.ExposureTime].GetMaximum();
                    _exposureTime = _camera.Parameters[PLCamera.ExposureTime].GetValue();
                }

                // 获取增益参数范围
                if (_camera.Parameters[PLCamera.Gain].IsReadable)
                {
                    GainMin = _camera.Parameters[PLCamera.Gain].GetMinimum();
                    GainMax = _camera.Parameters[PLCamera.Gain].GetMaximum();
                    _gain = _camera.Parameters[PLCamera.Gain].GetValue();
                }

                // 设置像素格式转换
                _converter.OutputPixelFormat = PixelType.BGR8packed;

                _isConnected = true;

                LogHelper.Info("Camera", $"相机连接成功: {CameraName} ({SerialNumber})");
                OnStateChanged(CameraState.Connected);
                return true;
            }
            catch (Exception ex)
            {
                OnError($"打开相机失败: {ex.Message}");
                OnStateChanged(CameraState.Error);
                return false;
            }
        }

        /// <summary>
        /// 关闭相机
        /// </summary>
        public bool Close()
        {
            try
            {
                if (_isGrabbing)
                {
                    StopGrabbing();
                }

                if (_camera != null)
                {
                    _camera.Close();
                    _camera.Dispose();
                    _camera = null;
                }

                _isConnected = false;

                LogHelper.Info("Camera", "相机已关闭");
                OnStateChanged(CameraState.Disconnected);
                return true;
            }
            catch (Exception ex)
            {
                OnError($"关闭相机失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 开始连续采集
        /// </summary>
        public bool StartGrabbing()
        {
            try
            {
                if (!_isConnected || _camera == null)
                {
                    OnError("相机未连接");
                    return false;
                }

                if (_isGrabbing)
                {
                    return true;
                }

                _camera.StreamGrabber.ImageGrabbed += StreamGrabber_ImageGrabbed;
                _camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);

                _isGrabbing = true;
                _cts = new CancellationTokenSource();

                LogHelper.Info("Camera", "开始连续采集");
                OnStateChanged(CameraState.Grabbing);
                return true;
            }
            catch (Exception ex)
            {
                OnError($"开始采集失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 停止连续采集
        /// </summary>
        public bool StopGrabbing()
        {
            try
            {
                if (!_isGrabbing)
                {
                    return true;
                }

                _cts?.Cancel();

                if (_camera != null)
                {
                    _camera.StreamGrabber.Stop();
                    _camera.StreamGrabber.ImageGrabbed -= StreamGrabber_ImageGrabbed;
                }

                _isGrabbing = false;

                LogHelper.Info("Camera", "停止连续采集");
                OnStateChanged(CameraState.Connected);
                return true;
            }
            catch (Exception ex)
            {
                OnError($"停止采集失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 单帧采集
        /// </summary>
        public Bitmap GrabOne()
        {
            try
            {
                if (!_isConnected || _camera == null)
                {
                    OnError("相机未连接");
                    return null;
                }

                // 如果正在连续采集，先停止
                bool wasGrabbing = _isGrabbing;
                if (wasGrabbing)
                {
                    StopGrabbing();
                }

                Bitmap image = null;

                using (IGrabResult grabResult = _camera.StreamGrabber.GrabOne(5000, TimeoutHandling.ThrowException))
                {
                    if (grabResult.GrabSucceeded)
                    {
                        image = GrabResultToBitmap(grabResult);

                        lock (_lockObj)
                        {
                            _lastImage?.Dispose();
                            _lastImage = image?.Clone() as Bitmap;
                        }

                        OnImageGrabbed(image);
                        LogHelper.Debug("Camera", "单帧采集完成");
                    }
                    else
                    {
                        OnError($"采集失败: {grabResult.ErrorCode} - {grabResult.ErrorDescription}");
                    }
                }

                // 如果之前在连续采集，重新开始
                if (wasGrabbing)
                {
                    StartGrabbing();
                }

                return image;
            }
            catch (Exception ex)
            {
                OnError($"单帧采集失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 软件触发
        /// </summary>
        public bool SoftwareTrigger()
        {
            try
            {
                if (!_isConnected || _camera == null)
                {
                    OnError("相机未连接");
                    return false;
                }

                if (_triggerMode != TriggerMode.On)
                {
                    OnError("未启用触发模式");
                    return false;
                }

                if (_camera.CanWaitForFrameTriggerReady)
                {
                    _camera.WaitForFrameTriggerReady(1000, TimeoutHandling.ThrowException);
                }
                _camera.ExecuteSoftwareTrigger();

                LogHelper.Debug("Camera", "执行软触发");
                return true;
            }
            catch (Exception ex)
            {
                OnError($"软触发失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 保存图像
        /// </summary>
        public bool SaveImage(string path, Common.ImageFormat format = Common.ImageFormat.Jpg)
        {
            try
            {
                Bitmap image = LastImage;
                if (image == null)
                {
                    OnError("没有可保存的图像");
                    return false;
                }

                return Utils.SaveImage(image, path, format);
            }
            catch (Exception ex)
            {
                OnError($"保存图像失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 加载参数
        /// </summary>
        public bool LoadParameters(CameraConfig config)
        {
            try
            {
                if (config == null) return false;

                ExposureTime = config.ExposureTime;
                Gain = config.Gain;
                TriggerMode = config.TriggerMode;
                TriggerSource = config.TriggerSource;

                LogHelper.Info("Camera", "加载相机参数完成");
                return true;
            }
            catch (Exception ex)
            {
                OnError($"加载参数失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 保存参数
        /// </summary>
        public CameraConfig SaveParameters()
        {
            return new CameraConfig
            {
                CameraName = CameraName,
                SerialNumber = SerialNumber,
                ModelName = ModelName,
                IpAddress = IpAddress,
                ExposureTime = ExposureTime,
                Gain = Gain,
                TriggerMode = TriggerMode,
                TriggerSource = TriggerSource,
                Width = Width,
                Height = Height,
                ExposureTimeMin = ExposureTimeMin,
                ExposureTimeMax = ExposureTimeMax,
                GainMin = GainMin,
                GainMax = GainMax
            };
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 图像采集回调
        /// </summary>
        private void StreamGrabber_ImageGrabbed(object sender, ImageGrabbedEventArgs e)
        {
            try
            {
                IGrabResult grabResult = e.GrabResult;

                if (grabResult.GrabSucceeded)
                {
                    Bitmap image = GrabResultToBitmap(grabResult);

                    if (image != null)
                    {
                        lock (_lockObj)
                        {
                            _lastImage?.Dispose();
                            _lastImage = image.Clone() as Bitmap;
                        }

                        OnImageGrabbed(image);
                    }
                }
                else
                {
                    OnError($"采集失败: {grabResult.ErrorCode} - {grabResult.ErrorDescription}");
                }
            }
            catch (Exception ex)
            {
                OnError($"图像处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 将GrabResult转换为Bitmap
        /// </summary>
        private Bitmap GrabResultToBitmap(IGrabResult grabResult)
        {
            try
            {
                Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format24bppRgb);
                BitmapData bmpData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format24bppRgb);

                _converter.OutputPixelFormat = PixelType.BGR8packed;
                _converter.Convert(bmpData.Scan0, bmpData.Stride * bitmap.Height, grabResult);

                bitmap.UnlockBits(bmpData);
                return bitmap;
            }
            catch (Exception ex)
            {
                OnError($"图像转换失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        private void SetExposureTime(double value)
        {
            if (!_isConnected || _camera == null) return;

            try
            {
                if (_camera.Parameters[PLCamera.ExposureTime].IsWritable)
                {
                    _camera.Parameters[PLCamera.ExposureTime].SetValue(value);
                    LogHelper.Debug("Camera", $"设置曝光时间: {value} μs");
                }
            }
            catch (Exception ex)
            {
                OnError($"设置曝光时间失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 设置增益
        /// </summary>
        private void SetGain(double value)
        {
            if (!_isConnected || _camera == null) return;

            try
            {
                if (_camera.Parameters[PLCamera.Gain].IsWritable)
                {
                    _camera.Parameters[PLCamera.Gain].SetValue(value);
                    LogHelper.Debug("Camera", $"设置增益: {value} dB");
                }
            }
            catch (Exception ex)
            {
                OnError($"设置增益失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 设置触发模式
        /// </summary>
        private void SetTriggerMode(TriggerMode mode)
        {
            if (!_isConnected || _camera == null) return;

            try
            {
                if (_camera.Parameters[PLCamera.TriggerMode].IsWritable)
                {
                    _camera.Parameters[PLCamera.TriggerMode].SetValue(mode == TriggerMode.On ? "On" : "Off");
                    LogHelper.Debug("Camera", $"设置触发模式: {mode}");
                }
            }
            catch (Exception ex)
            {
                OnError($"设置触发模式失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 设置触发源
        /// </summary>
        private void SetTriggerSource(TriggerSource source)
        {
            if (!_isConnected || _camera == null) return;

            try
            {
                if (_camera.Parameters[PLCamera.TriggerSource].IsWritable)
                {
                    string sourceStr;
                    switch (source)
                    {
                        case TriggerSource.Software:
                            sourceStr = "Software";
                            break;
                        case TriggerSource.Line1:
                            sourceStr = "Line1";
                            break;
                        case TriggerSource.Line2:
                            sourceStr = "Line2";
                            break;
                        case TriggerSource.Line3:
                            sourceStr = "Line3";
                            break;
                        default:
                            sourceStr = "Software";
                            break;
                    }
                    _camera.Parameters[PLCamera.TriggerSource].SetValue(sourceStr);
                    LogHelper.Debug("Camera", $"设置触发源: {source}");
                }
            }
            catch (Exception ex)
            {
                OnError($"设置触发源失败: {ex.Message}");
            }
        }

        #endregion

        #region 事件触发

        protected virtual void OnImageGrabbed(Bitmap image)
        {
            ImageGrabbed?.Invoke(this, image);
        }

        protected virtual void OnError(string message)
        {
            LogHelper.Error("Camera", message);
            ErrorOccurred?.Invoke(this, message);
        }

        protected virtual void OnStateChanged(CameraState state)
        {
            StateChanged?.Invoke(this, state);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                Close();
                _lastImage?.Dispose();
                _cts?.Dispose();
                _converter?.Dispose();
            }

            _isDisposed = true;
        }

        #endregion
    }
}
