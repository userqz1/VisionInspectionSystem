using System;
using System.Diagnostics;
using System.Drawing;
using VisionInspectionSystem.Models;
using VisionInspectionSystem.DAL;

// 注意: 实际使用时需要引用 Cognex.VisionPro.dll 相关程序集
// 以下代码提供了完整的接口封装，实际图像处理需要安装VisionPro

namespace VisionInspectionSystem.HAL
{
    /// <summary>
    /// VisionPro处理器封装类
    /// </summary>
    public class VisionProProcessor : IDisposable
    {
        #region 私有字段

        private bool _isDisposed;
        private bool _isJobLoaded;
        private string _jobFilePath;
        private readonly object _lockObj = new object();
        private Stopwatch _stopwatch = new Stopwatch();

        // VisionPro对象（实际使用时取消注释）
        // private CogJobManager _jobManager;
        // private CogJob _job;
        // private CogToolBlock _toolBlock;

        #endregion

        #region 公共属性

        /// <summary>
        /// 作业文件路径
        /// </summary>
        public string JobFilePath => _jobFilePath;

        /// <summary>
        /// 是否已加载作业
        /// </summary>
        public bool IsJobLoaded => _isJobLoaded;

        /// <summary>
        /// 上次运行时间（毫秒）
        /// </summary>
        public double LastRunTime { get; private set; }

        #endregion

        #region 事件

        /// <summary>
        /// 检测完成事件
        /// </summary>
        public event EventHandler<InspectionResult> InspectionCompleted;

        /// <summary>
        /// 错误发生事件
        /// </summary>
        public event EventHandler<string> ErrorOccurred;

        #endregion

        #region 构造函数

        public VisionProProcessor()
        {
            // 初始化VisionPro环境
            InitializeVisionPro();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 加载VisionPro作业文件
        /// </summary>
        public bool LoadJob(string vppFilePath)
        {
            try
            {
                if (string.IsNullOrEmpty(vppFilePath))
                {
                    OnError("作业文件路径为空");
                    return false;
                }

                if (!System.IO.File.Exists(vppFilePath))
                {
                    OnError($"作业文件不存在: {vppFilePath}");
                    return false;
                }

                lock (_lockObj)
                {
                    // 先卸载当前作业
                    if (_isJobLoaded)
                    {
                        UnloadJob();
                    }

                    // 实际使用VisionPro时的代码：
                    /*
                    _jobManager = (CogJobManager)CogSerializer.LoadObjectFromFile(vppFilePath);
                    _job = _jobManager.Job(0);
                    _toolBlock = _job.VisionTool as CogToolBlock;
                    */

                    _jobFilePath = vppFilePath;
                    _isJobLoaded = true;

                    LogHelper.Info("VisionPro", $"加载作业成功: {vppFilePath}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnError($"加载作业失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 卸载作业
        /// </summary>
        public bool UnloadJob()
        {
            try
            {
                lock (_lockObj)
                {
                    // 实际使用VisionPro时的代码：
                    /*
                    if (_jobManager != null)
                    {
                        _jobManager.Shutdown();
                        _jobManager = null;
                    }
                    _job = null;
                    _toolBlock = null;
                    */

                    _jobFilePath = null;
                    _isJobLoaded = false;

                    LogHelper.Info("VisionPro", "卸载作业完成");
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnError($"卸载作业失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 执行检测
        /// </summary>
        public InspectionResult Run(Bitmap image)
        {
            if (!_isJobLoaded)
            {
                OnError("未加载作业文件");
                return CreateErrorResult("未加载作业文件");
            }

            if (image == null)
            {
                OnError("输入图像为空");
                return CreateErrorResult("输入图像为空");
            }

            try
            {
                lock (_lockObj)
                {
                    _stopwatch.Restart();

                    InspectionResult result = new InspectionResult();

                    // 实际使用VisionPro时的代码：
                    /*
                    // 设置输入图像
                    CogImage8Grey cogImage = new CogImage8Grey(image);
                    _toolBlock.Inputs["InputImage"].Value = cogImage;

                    // 运行
                    _toolBlock.Run();

                    // 获取结果
                    result.IsPass = (bool)_toolBlock.Outputs["Result"].Value;
                    result.X = (double)_toolBlock.Outputs["X"].Value;
                    result.Y = (double)_toolBlock.Outputs["Y"].Value;
                    result.Angle = (double)_toolBlock.Outputs["Angle"].Value;
                    result.Score = (double)_toolBlock.Outputs["Score"].Value;

                    // 获取结果图像
                    CogImage resultImage = _toolBlock.Outputs["OutputImage"].Value as CogImage;
                    if (resultImage != null)
                    {
                        result.ResultImage = resultImage.ToBitmap();
                    }
                    */

                    // 模拟检测结果（测试用）
                    result = SimulateInspection(image);

                    _stopwatch.Stop();
                    result.RunTime = _stopwatch.Elapsed.TotalMilliseconds;
                    LastRunTime = result.RunTime;

                    LogHelper.Debug("VisionPro", $"检测完成: {result}");
                    OnInspectionCompleted(result);
                    return result;
                }
            }
            catch (Exception ex)
            {
                OnError($"检测执行失败: {ex.Message}");
                return CreateErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 离线测试（从文件加载图像）
        /// </summary>
        public InspectionResult RunOffline(string imagePath)
        {
            try
            {
                if (!System.IO.File.Exists(imagePath))
                {
                    OnError($"图像文件不存在: {imagePath}");
                    return CreateErrorResult("图像文件不存在");
                }

                using (Bitmap image = new Bitmap(imagePath))
                {
                    return Run(image);
                }
            }
            catch (Exception ex)
            {
                OnError($"离线测试失败: {ex.Message}");
                return CreateErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取工具结果
        /// </summary>
        public object GetToolResult(string toolName)
        {
            if (!_isJobLoaded)
            {
                return null;
            }

            try
            {
                // 实际使用VisionPro时的代码：
                /*
                if (_toolBlock.Outputs[toolName] != null)
                {
                    return _toolBlock.Outputs[toolName].Value;
                }
                */

                return null;
            }
            catch (Exception ex)
            {
                OnError($"获取工具结果失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 保存作业
        /// </summary>
        public bool SaveJob(string vppFilePath)
        {
            if (!_isJobLoaded)
            {
                OnError("没有已加载的作业");
                return false;
            }

            try
            {
                // 实际使用VisionPro时的代码：
                /*
                CogSerializer.SaveObjectToFile(_jobManager, vppFilePath);
                */

                LogHelper.Info("VisionPro", $"保存作业成功: {vppFilePath}");
                return true;
            }
            catch (Exception ex)
            {
                OnError($"保存作业失败: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化VisionPro环境
        /// </summary>
        private void InitializeVisionPro()
        {
            try
            {
                // 实际使用VisionPro时的代码：
                /*
                // 检查许可证
                CogFrameGrabbers.FrameGrabbers.Initialize();
                */

                LogHelper.Info("VisionPro", "VisionPro环境初始化完成");
            }
            catch (Exception ex)
            {
                LogHelper.Error("VisionPro", $"VisionPro初始化失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 模拟检测（测试用）
        /// </summary>
        private InspectionResult SimulateInspection(Bitmap image)
        {
            Random rand = new Random();
            bool isPass = rand.Next(100) < 95; // 95%合格率

            InspectionResult result = new InspectionResult
            {
                IsPass = isPass,
                X = 100 + rand.NextDouble() * 10,
                Y = 200 + rand.NextDouble() * 10,
                Angle = rand.NextDouble() * 5,
                Score = isPass ? 95 + rand.NextDouble() * 5 : 50 + rand.NextDouble() * 30,
                Message = isPass ? "OK" : "NG - Pattern not found",
                ResultImage = CreateResultImage(image, isPass)
            };

            return result;
        }

        /// <summary>
        /// 创建结果图像（测试用）
        /// </summary>
        private Bitmap CreateResultImage(Bitmap source, bool isPass)
        {
            Bitmap result = source.Clone() as Bitmap;
            using (Graphics g = Graphics.FromImage(result))
            {
                // 绘制结果标记
                Color color = isPass ? Color.Green : Color.Red;
                using (Pen pen = new Pen(color, 3))
                {
                    g.DrawRectangle(pen, 100, 100, 200, 150);
                }

                // 绘制结果文字
                string text = isPass ? "OK" : "NG";
                using (Font font = new Font("Arial", 48, FontStyle.Bold))
                using (Brush brush = new SolidBrush(color))
                {
                    g.DrawString(text, font, brush, 150, 300);
                }
            }
            return result;
        }

        /// <summary>
        /// 创建错误结果
        /// </summary>
        private InspectionResult CreateErrorResult(string message)
        {
            return new InspectionResult
            {
                IsPass = false,
                ErrorCode = -1,
                Message = message
            };
        }

        #endregion

        #region 事件触发

        protected virtual void OnInspectionCompleted(InspectionResult result)
        {
            InspectionCompleted?.Invoke(this, result);
        }

        protected virtual void OnError(string message)
        {
            LogHelper.Error("VisionPro", message);
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
                UnloadJob();
            }

            _isDisposed = true;
        }

        #endregion
    }
}
