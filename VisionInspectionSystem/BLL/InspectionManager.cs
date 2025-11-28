using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using VisionInspectionSystem.Common;
using VisionInspectionSystem.DAL;
using VisionInspectionSystem.HAL;
using VisionInspectionSystem.Models;

namespace VisionInspectionSystem.BLL
{
    /// <summary>
    /// 检测管理类
    /// </summary>
    public class InspectionManager : IDisposable
    {
        #region 单例模式

        private static InspectionManager _instance;
        private static readonly object _lockObj = new object();

        public static InspectionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new InspectionManager();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region 私有字段

        private VisionProProcessor _processor;
        private CancellationTokenSource _cts;
        private bool _isDisposed;
        private bool _isRunning;
        private InspectionResult _lastResult;
        private RealtimeStatistics _realtimeStats;

        #endregion

        #region 公共属性

        /// <summary>
        /// VisionPro处理器
        /// </summary>
        public VisionProProcessor Processor => _processor;

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool IsRunning => _isRunning;

        /// <summary>
        /// 检测模式
        /// </summary>
        public InspectionMode Mode { get; set; } = InspectionMode.Online;

        /// <summary>
        /// 最后一次检测结果
        /// </summary>
        public InspectionResult LastResult => _lastResult;

        /// <summary>
        /// 实时统计数据
        /// </summary>
        public RealtimeStatistics RealtimeStats => _realtimeStats;

        /// <summary>
        /// 是否自动保存图像
        /// </summary>
        public bool AutoSaveImage { get; set; } = true;

        /// <summary>
        /// 是否自动发送结果
        /// </summary>
        public bool AutoSendResult { get; set; } = true;

        #endregion

        #region 事件

        /// <summary>
        /// 检测完成事件
        /// </summary>
        public event EventHandler<InspectionResult> InspectionCompleted;

        /// <summary>
        /// 状态变化事件
        /// </summary>
        public event EventHandler<SystemState> StateChanged;

        /// <summary>
        /// 错误事件
        /// </summary>
        public event EventHandler<string> ErrorOccurred;

        #endregion

        #region 构造函数

        private InspectionManager()
        {
            _processor = new VisionProProcessor();
            _processor.InspectionCompleted += Processor_InspectionCompleted;
            _processor.ErrorOccurred += Processor_ErrorOccurred;

            _realtimeStats = new RealtimeStatistics();

            // 订阅通讯命令事件
            CommunicationManager.Instance.CommandReceived += OnCommandReceived;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 加载版型
        /// </summary>
        public bool LoadModel(string modelName)
        {
            try
            {
                ModelConfig config = ModelStorage.LoadModelConfig(modelName);
                if (config == null)
                {
                    OnError($"版型不存在: {modelName}");
                    return false;
                }

                return LoadModel(config);
            }
            catch (Exception ex)
            {
                OnError($"加载版型失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 加载版型
        /// </summary>
        public bool LoadModel(ModelConfig config)
        {
            try
            {
                if (config == null) return false;

                // 加载VisionPro作业
                if (!string.IsNullOrEmpty(config.VppFilePath))
                {
                    if (!_processor.LoadJob(config.VppFilePath))
                    {
                        LogHelper.Warn("InspectionManager", $"VisionPro作业文件不存在: {config.VppFilePath}");
                        // 不阻止加载，可能只是测试
                    }
                }

                // 应用相机参数
                if (config.CameraSettings != null)
                {
                    CameraManager.Instance.ApplyModelSettings(config);
                }

                GlobalVariables.CurrentModel = config;
                _realtimeStats.Reset();

                LogHelper.Info("InspectionManager", $"加载版型: {config.ModelName}");
                return true;
            }
            catch (Exception ex)
            {
                OnError($"加载版型失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 开始运行
        /// </summary>
        public bool Start()
        {
            try
            {
                if (_isRunning)
                {
                    return true;
                }

                if (GlobalVariables.CurrentModel == null)
                {
                    OnError("请先选择版型");
                    return false;
                }

                _isRunning = true;
                _cts = new CancellationTokenSource();
                _realtimeStats.Reset();

                GlobalVariables.SystemState = SystemState.Running;
                OnStateChanged(SystemState.Running);

                LogHelper.Info("InspectionManager", "开始运行");
                return true;
            }
            catch (Exception ex)
            {
                OnError($"启动失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 停止运行
        /// </summary>
        public bool Stop()
        {
            try
            {
                _isRunning = false;
                _cts?.Cancel();

                GlobalVariables.SystemState = SystemState.Ready;
                OnStateChanged(SystemState.Ready);

                // 保存统计数据
                SaveTodayStatistics();

                LogHelper.Info("InspectionManager", "停止运行");
                return true;
            }
            catch (Exception ex)
            {
                OnError($"停止失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 执行单次检测
        /// </summary>
        public InspectionResult RunOnce()
        {
            try
            {
                if (!_isRunning && Mode == InspectionMode.Online)
                {
                    OnError("系统未运行");
                    return null;
                }

                // 获取图像
                Bitmap image = CameraManager.Instance.GrabOne();
                if (image == null)
                {
                    OnError("取图失败");
                    return null;
                }

                // 执行检测
                return RunInspection(image);
            }
            catch (Exception ex)
            {
                OnError($"检测失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 执行检测（带图像）
        /// </summary>
        public InspectionResult RunInspection(Bitmap image)
        {
            if (image == null) return null;

            try
            {
                // 执行VisionPro检测
                InspectionResult result = _processor.Run(image);
                if (result == null)
                {
                    result = new InspectionResult
                    {
                        IsPass = false,
                        Message = "检测执行失败"
                    };
                }

                result.ModelName = GlobalVariables.CurrentModel?.ModelName;
                ProcessResult(result);

                return result;
            }
            catch (Exception ex)
            {
                OnError($"检测执行失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 离线测试
        /// </summary>
        public InspectionResult RunOffline(string imagePath)
        {
            try
            {
                Mode = InspectionMode.Offline;
                InspectionResult result = _processor.RunOffline(imagePath);
                if (result != null)
                {
                    result.ModelName = GlobalVariables.CurrentModel?.ModelName;
                    result.OriginalImagePath = imagePath;
                    ProcessResult(result);
                }
                return result;
            }
            catch (Exception ex)
            {
                OnError($"离线测试失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 重置统计
        /// </summary>
        public void ResetStatistics()
        {
            _realtimeStats.Reset();
            LogHelper.Info("InspectionManager", "重置统计");
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 处理检测结果
        /// </summary>
        private void ProcessResult(InspectionResult result)
        {
            _lastResult = result;

            // 更新统计
            _realtimeStats.AddResult(result);

            // 保存图像
            if (AutoSaveImage && CameraManager.Instance.LastImage != null)
            {
                var model = GlobalVariables.CurrentModel;
                bool saveOk = model?.SaveOkImage ?? true;
                bool saveNg = model?.SaveNgImage ?? true;

                if ((result.IsPass && saveOk) || (!result.IsPass && saveNg))
                {
                    result.ResultImagePath = ImageStorage.SaveInspectionImage(
                        CameraManager.Instance.LastImage,
                        result.IsPass,
                        model?.ModelName);
                }
            }

            // 发送结果
            if (AutoSendResult && CommunicationManager.Instance.IsConnected)
            {
                CommunicationManager.Instance.SendResult(result);
            }

            // 触发事件
            OnInspectionCompleted(result);
        }

        /// <summary>
        /// 保存今日统计
        /// </summary>
        private void SaveTodayStatistics()
        {
            if (_realtimeStats.TotalCount == 0) return;

            try
            {
                StatisticsData stats = new StatisticsData
                {
                    Date = DateTime.Today,
                    ModelName = GlobalVariables.CurrentModel?.ModelName ?? "Unknown",
                    TotalCount = _realtimeStats.TotalCount,
                    OkCount = _realtimeStats.OkCount,
                    NgCount = _realtimeStats.NgCount,
                    StartTime = _realtimeStats.StartTime,
                    EndTime = DateTime.Now
                };

                DataStorage.SaveStatistics(stats);
            }
            catch (Exception ex)
            {
                LogHelper.Error("InspectionManager", $"保存统计失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理通讯命令
        /// </summary>
        private void OnCommandReceived(object sender, CommandEventArgs e)
        {
            switch (e.CommandType)
            {
                case CommandType.Trigger:
                    // 触发检测
                    Task.Run(() => RunOnce());
                    break;

                case CommandType.Heartbeat:
                    // 响应心跳
                    CommunicationManager.Instance.SendOK();
                    break;

                case CommandType.Query:
                    // 查询结果
                    if (_lastResult != null)
                    {
                        CommunicationManager.Instance.SendResult(_lastResult);
                    }
                    else
                    {
                        CommunicationManager.Instance.SendNG("NoResult");
                    }
                    break;
            }
        }

        #endregion

        #region 事件处理

        private void Processor_InspectionCompleted(object sender, InspectionResult e)
        {
            // 由ProcessResult统一处理
        }

        private void Processor_ErrorOccurred(object sender, string e)
        {
            OnError(e);
        }

        private void OnInspectionCompleted(InspectionResult result)
        {
            InspectionCompleted?.Invoke(this, result);
        }

        private void OnStateChanged(SystemState state)
        {
            StateChanged?.Invoke(this, state);
        }

        private void OnError(string message)
        {
            LogHelper.Error("InspectionManager", message);
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
                Stop();
                _processor?.Dispose();
                _cts?.Dispose();
            }

            _isDisposed = true;
        }

        #endregion
    }
}
