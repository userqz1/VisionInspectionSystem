using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using VisionInspectionSystem.Models;
using VisionInspectionSystem.DAL;

// VisionPro 引用
using Cognex.VisionPro;


namespace VisionInspectionSystem.HAL
{
    /// <summary>
    /// VisionPro处理器封装类 - 支持QuickBuild Job
    /// </summary>
    public class VisionProProcessor : IDisposable
    {
        #region 私有字段

        private bool _isDisposed;
        private bool _isJobLoaded;
        private string _jobFilePath;
        private readonly object _lockObj = new object();
        private Stopwatch _stopwatch = new Stopwatch();

        // VisionPro QuickBuild对象
        private CogJobManager _jobManager;
        private CogJob _job;
        private CogJobIndependent _jobIndependent;

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

        /// <summary>
        /// JobManager对象（用于外部访问）
        /// </summary>
        public CogJobManager JobManager => _jobManager;

        /// <summary>
        /// 当前Job对象
        /// </summary>
        public CogJob Job => _job;

        /// <summary>
        /// 最后一次运行的所有输出参数
        /// </summary>
        public Dictionary<string, VppOutputItem> LastOutputs { get; private set; } = new Dictionary<string, VppOutputItem>();

        /// <summary>
        /// Job中的所有输出名称列表
        /// </summary>
        public List<string> OutputNames { get; private set; } = new List<string>();

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
            InitializeVisionPro();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 加载VisionPro作业文件（.vpp）
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

                    // 加载VPP文件
                    _jobManager = (CogJobManager)CogSerializer.LoadObjectFromFile(vppFilePath);

                    if (_jobManager == null || _jobManager.JobCount == 0)
                    {
                        OnError("VPP文件中没有Job");
                        return false;
                    }

                    // 获取第一个Job
                    _job = _jobManager.Job(0);
                    _jobIndependent = _job.OwnedIndependent;

                    _jobFilePath = vppFilePath;
                    _isJobLoaded = true;

                    // 获取输出信息
                    LogJobInfo();

                    LogHelper.Info("VisionPro", $"加载作业成功: {vppFilePath}");
                    LogHelper.Info("VisionPro", $"Job名称: {_job.Name}, Job数量: {_jobManager.JobCount}");

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
                    if (_jobManager != null)
                    {
                        _jobManager.Shutdown();
                        _jobManager = null;
                    }

                    _job = null;
                    _jobIndependent = null;
                    _jobFilePath = null;
                    _isJobLoaded = false;
                    LastOutputs.Clear();
                    OutputNames.Clear();

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
            if (!_isJobLoaded || _job == null)
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

                    // 将Bitmap转换为CogImage
                    ICogImage cogImage = ConvertBitmapToCogImage(image);
                    if (cogImage == null)
                    {
                        return CreateErrorResult("图像转换失败");
                    }

                    // 设置输入图像并运行
                    _jobManager.UserQueueFlush();
                    _jobManager.UserResultQueueFlush();

                    // 将图像放入队列
                    _job.ImageQueueFlush();
                    _jobIndependent.RealTimeQueueFlush();

                    // 输入图像到Job
                    ICogAcqInfo acqInfo = new CogAcqInfo();
                    _jobManager.UserQueueAppend(cogImage, acqInfo, null);

                    // 运行Job
                    _jobManager.Run();

                    // 等待完成并获取结果
                    ICogJobResult jobResult = null;
                    int timeout = 30000; // 30秒超时
                    DateTime startTime = DateTime.Now;

                    while ((DateTime.Now - startTime).TotalMilliseconds < timeout)
                    {
                        if (_jobManager.UserResultQueueCount > 0)
                        {
                            jobResult = _jobManager.UserResultQueuePop();
                            break;
                        }
                        System.Threading.Thread.Sleep(10);
                    }

                    _stopwatch.Stop();

                    // 收集结果
                    InspectionResult result = CollectJobResults(jobResult);
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
                    InspectionResult result = Run(image);
                    result.OriginalImagePath = imagePath;
                    return result;
                }
            }
            catch (Exception ex)
            {
                OnError($"离线测试失败: {ex.Message}");
                return CreateErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取输入输出参数的格式化字符串（用于显示）
        /// </summary>
        public string GetInputOutputSummary()
        {
            if (_job == null) return "未加载作业";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendLine($"=== Job信息 ===");
            sb.AppendLine($"  Job名称: {_job.Name}");
            sb.AppendLine($"  Job数量: {_jobManager?.JobCount ?? 0}");
            sb.AppendLine();

            sb.AppendLine("=== 上次运行输出 ===");
            if (LastOutputs.Count > 0)
            {
                foreach (var output in LastOutputs)
                {
                    sb.AppendLine($"  [{output.Value.ValueType}] {output.Key}: {FormatValue(output.Value.Value)}");
                }
            }
            else
            {
                sb.AppendLine("  (请先运行检测)");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 保存作业
        /// </summary>
        public bool SaveJob(string vppFilePath)
        {
            if (!_isJobLoaded || _jobManager == null)
            {
                OnError("没有已加载的作业");
                return false;
            }

            try
            {
                CogSerializer.SaveObjectToFile(_jobManager, vppFilePath);
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
                LogHelper.Info("VisionPro", "VisionPro环境初始化完成");
            }
            catch (Exception ex)
            {
                LogHelper.Error("VisionPro", $"VisionPro初始化失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 记录Job信息到日志
        /// </summary>
        private void LogJobInfo()
        {
            if (_job == null) return;

            LogHelper.Info("VisionPro", $"--- Job信息 ---");
            LogHelper.Info("VisionPro", $"  名称: {_job.Name}");
            LogHelper.Info("VisionPro", $"  Job数量: {_jobManager?.JobCount ?? 0}");
        }

        /// <summary>
        /// 将Bitmap转换为CogImage
        /// </summary>
        private ICogImage ConvertBitmapToCogImage(Bitmap bitmap)
        {
            try
            {
                if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                {
                    return new CogImage8Grey(bitmap);
                }
                else
                {
                    Bitmap rgb24 = new Bitmap(bitmap.Width, bitmap.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    using (Graphics g = Graphics.FromImage(rgb24))
                    {
                        g.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
                    }
                    return new CogImage24PlanarColor(rgb24);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("VisionPro", $"图像转换失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 收集Job运行结果
        /// </summary>
        private InspectionResult CollectJobResults(ICogJobResult jobResult)
        {
            InspectionResult result = new InspectionResult();
            LastOutputs.Clear();

            try
            {
                if (jobResult == null)
                {
                    result.IsPass = false;
                    result.Message = "未获取到Job结果（超时或失败）";
                    return result;
                }

                // 获取运行状态
                result.IsPass = (jobResult.RunStatus.Result == CogToolResultConstants.Accept);
                result.Message = jobResult.RunStatus.Message ?? (result.IsPass ? "OK" : "NG");

                // 获取所有输出
                // 遍历JobResult中的所有内容
                CogRecord topRecord = jobResult.CreateResultRecord();
                if (topRecord != null)
                {
                    CollectRecordOutputs(topRecord, result, "");
                }

                LogHelper.Info("VisionPro", $"收集到 {LastOutputs.Count} 个输出参数");
            }
            catch (Exception ex)
            {
                LogHelper.Error("VisionPro", $"收集Job结果失败: {ex.Message}");
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 递归收集Record中的输出
        /// </summary>
        private void CollectRecordOutputs(CogRecord record, InspectionResult result, string prefix)
        {
            if (record == null) return;

            try
            {
                string recordKey = string.IsNullOrEmpty(prefix) ? record.RecordKey : $"{prefix}.{record.RecordKey}";

                // 记录当前内容
                if (record.Content != null)
                {
                    string typeName = record.Content.GetType().Name;

                    LastOutputs[recordKey] = new VppOutputItem
                    {
                        Name = recordKey,
                        ValueType = typeName,
                        Value = record.Content,
                        Description = record.Annotation ?? ""
                    };

                    result.ExtraData[recordKey] = record.Content;

                    // 解析常见输出
                    ParseCommonOutput(result, recordKey, record.Content);

                    // 检查是否是图像
                    if (record.Content is ICogImage cogImage && result.ResultImage == null)
                    {
                        try
                        {
                            result.ResultImage = cogImage.ToBitmap();
                        }
                        catch { }
                    }

                    LogHelper.Debug("VisionPro", $"输出: [{typeName}] {recordKey}");
                }

                // 递归处理子记录
                if (record.SubRecords != null)
                {
                    foreach (CogRecord subRecord in record.SubRecords)
                    {
                        CollectRecordOutputs(subRecord, result, recordKey);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Debug("VisionPro", $"处理Record异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 解析常见的输出参数
        /// </summary>
        private void ParseCommonOutput(InspectionResult result, string name, object value)
        {
            if (value == null) return;

            string nameLower = name.ToLower();

            try
            {
                // X坐标
                if (nameLower.Contains("x") && !nameLower.Contains("max") && !nameLower.Contains("index"))
                {
                    if (value is double d) result.X = d;
                    else if (value is float f) result.X = f;
                    else if (double.TryParse(value.ToString(), out double x)) result.X = x;
                }
                // Y坐标
                else if (nameLower.Contains("y") && !nameLower.Contains("may"))
                {
                    if (value is double d) result.Y = d;
                    else if (value is float f) result.Y = f;
                    else if (double.TryParse(value.ToString(), out double y)) result.Y = y;
                }
                // 角度
                else if (nameLower.Contains("angle") || nameLower.Contains("rotation"))
                {
                    if (value is double d) result.Angle = d;
                    else if (value is float f) result.Angle = f;
                    else if (double.TryParse(value.ToString(), out double angle)) result.Angle = angle;
                }
                // 分数
                else if (nameLower.Contains("score") || nameLower.Contains("match"))
                {
                    if (value is double d) result.Score = d;
                    else if (value is float f) result.Score = f;
                    else if (double.TryParse(value.ToString(), out double score)) result.Score = score;
                }
                // 结果
                else if (nameLower.Contains("result") || nameLower.Contains("pass"))
                {
                    if (value is bool b)
                    {
                        result.IsPass = b;
                        result.ExtraData["_PassSet"] = true;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 格式化值用于显示
        /// </summary>
        private string FormatValue(object value)
        {
            if (value == null) return "null";

            if (value is double d) return d.ToString("F4");
            if (value is float f) return f.ToString("F4");
            if (value is ICogImage) return "[CogImage]";
            if (value is ICogGraphic) return "[CogGraphic]";
            if (value is ICogRecord) return "[CogRecord]";

            return value.ToString();
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

    #region VPP参数类型

    /// <summary>
    /// VPP输入参数项
    /// </summary>
    public class VppInputItem
    {
        public string Name { get; set; }
        public string ValueType { get; set; }
        public object Value { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"[{ValueType}] {Name}: {Value}";
        }
    }

    /// <summary>
    /// VPP输出参数项
    /// </summary>
    public class VppOutputItem
    {
        public string Name { get; set; }
        public string ValueType { get; set; }
        public object Value { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"[{ValueType}] {Name}: {Value}";
        }
    }

    /// <summary>
    /// 简单的AcqInfo实现
    /// </summary>
    internal class CogAcqInfo : ICogAcqInfo
    {
        public double TriggerTimeStamp => 0;
        public int TriggerNumber => 0;
        public object User => null;
    }

    #endregion
}
