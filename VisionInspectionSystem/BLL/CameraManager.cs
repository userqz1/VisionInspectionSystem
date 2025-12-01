using System;
using System.Collections.Generic;
using System.Drawing;
using VisionInspectionSystem.Common;
using VisionInspectionSystem.DAL;
using VisionInspectionSystem.HAL;
using VisionInspectionSystem.Models;

namespace VisionInspectionSystem.BLL
{
    /// <summary>
    /// 相机管理类
    /// </summary>
    public class CameraManager : IDisposable
    {
        #region 单例模式

        private static CameraManager _instance;
        private static readonly object _lockObj = new object();

        public static CameraManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new CameraManager();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region 私有字段

        private BaslerCamera _camera;
        private CameraConfig _config;
        private bool _isDisposed;

        #endregion

        #region 公共属性

        /// <summary>
        /// 相机对象
        /// </summary>
        public BaslerCamera Camera => _camera;

        /// <summary>
        /// 相机配置
        /// </summary>
        public CameraConfig Config => _config;

        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected => _camera?.IsConnected ?? false;

        /// <summary>
        /// 是否正在采集
        /// </summary>
        public bool IsGrabbing => _camera?.IsGrabbing ?? false;

        /// <summary>
        /// 最后一张图像
        /// </summary>
        public Bitmap LastImage => _camera?.LastImage;

        #endregion

        #region 事件

        /// <summary>
        /// 图像采集事件
        /// </summary>
        public event EventHandler<Bitmap> ImageGrabbed;

        /// <summary>
        /// 状态变化事件
        /// </summary>
        public event EventHandler<CameraState> StateChanged;

        /// <summary>
        /// 错误事件
        /// </summary>
        public event EventHandler<string> ErrorOccurred;

        #endregion

        #region 构造函数

        private CameraManager()
        {
            _camera = new BaslerCamera();
            _camera.ImageGrabbed += Camera_ImageGrabbed;
            _camera.StateChanged += Camera_StateChanged;
            _camera.ErrorOccurred += Camera_ErrorOccurred;

            // 加载配置
            _config = ConfigHelper.LoadCameraConfig();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 枚举相机列表
        /// </summary>
        public List<CameraInfo> EnumerateCameras()
        {
            return BaslerCamera.EnumerateCameras();
        }

        /// <summary>
        /// 连接相机
        /// </summary>
        public bool Connect(string serialNumber)
        {
            try
            {
                if (IsConnected)
                {
                    Disconnect();
                }

                if (_camera.Open(serialNumber))
                {
                    // 应用配置参数
                    if (_config != null && _config.SerialNumber == serialNumber)
                    {
                        _camera.LoadParameters(_config);
                    }

                    LogHelper.Info("CameraManager", $"相机连接成功: {serialNumber}");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Error("CameraManager", $"连接相机失败: {ex.Message}");
                OnError($"连接相机失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 断开相机
        /// </summary>
        public bool Disconnect()
        {
            try
            {
                return _camera.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error("CameraManager", $"断开相机失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 单帧采集
        /// </summary>
        public Bitmap GrabOne()
        {
            return _camera?.GrabOne();
        }

        /// <summary>
        /// 开始连续采集
        /// </summary>
        public bool StartGrabbing()
        {
            return _camera?.StartGrabbing() ?? false;
        }

        /// <summary>
        /// 停止连续采集
        /// </summary>
        public bool StopGrabbing()
        {
            return _camera?.StopGrabbing() ?? false;
        }

        /// <summary>
        /// 软触发
        /// </summary>
        public bool SoftwareTrigger()
        {
            return _camera?.SoftwareTrigger() ?? false;
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        public void SetExposureTime(double value)
        {
            if (_camera != null)
            {
                _camera.ExposureTime = value;
                _config.ExposureTime = value;
            }
        }

        /// <summary>
        /// 设置增益
        /// </summary>
        public void SetGain(double value)
        {
            if (_camera != null)
            {
                _camera.Gain = value;
                _config.Gain = value;
            }
        }

        /// <summary>
        /// 设置触发模式
        /// </summary>
        public void SetTriggerMode(TriggerMode mode)
        {
            if (_camera != null)
            {
                _camera.TriggerMode = mode;
                _config.TriggerMode = mode;
            }
        }

        /// <summary>
        /// 设置触发源
        /// </summary>
        public void SetTriggerSource(TriggerSource source)
        {
            if (_camera != null)
            {
                _camera.TriggerSource = source;
                _config.TriggerSource = source;
            }
        }

        /// <summary>
        /// 保存图像
        /// </summary>
        public string SaveImage(bool isPass, string modelName)
        {
            if (_camera?.LastImage == null) return null;
            return ImageStorage.SaveInspectionImage(_camera.LastImage, isPass, modelName);
        }

        /// <summary>
        /// 保存当前配置
        /// </summary>
        public bool SaveConfig()
        {
            if (_camera == null) return false;

            _config = _camera.SaveParameters();
            return ConfigHelper.SaveCameraConfig(_config);
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        public bool LoadConfig()
        {
            _config = ConfigHelper.LoadCameraConfig();
            if (_camera != null && IsConnected)
            {
                return _camera.LoadParameters(_config);
            }
            return true;
        }

        /// <summary>
        /// 应用版型相机参数
        /// </summary>
        public bool ApplyModelSettings(ModelConfig model)
        {
            if (model?.CameraSettings == null || _camera == null)
                return false;

            return _camera.LoadParameters(model.CameraSettings);
        }

        #endregion

        #region 事件处理

        private void Camera_ImageGrabbed(object sender, Bitmap e)
        {
            ImageGrabbed?.Invoke(this, e);
        }

        private void Camera_StateChanged(object sender, CameraState e)
        {
            StateChanged?.Invoke(this, e);
        }

        private void Camera_ErrorOccurred(object sender, string e)
        {
            OnError(e);
        }

        private void OnError(string message)
        {
            ErrorOccurred?.Invoke(this, message);
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
                _camera?.Dispose();
            }

            _isDisposed = true;
        }

        #endregion
    }
}
