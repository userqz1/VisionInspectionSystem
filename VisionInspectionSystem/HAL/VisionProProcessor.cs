using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using VisionInspectionSystem.Models;
using VisionInspectionSystem.DAL;

// VisionPro 引用
using Cognex.VisionPro;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.ImageFile;

namespace VisionInspectionSystem.HAL
{
    /// <summary>
    /// VisionPro处理器封装类 - 使用CogToolBlock
    /// </summary>
    public class VisionProProcessor : IDisposable
    {
        #region 私有字段

        private bool _isDisposed;
        private bool _isJobLoaded;
        private string _jobFilePath;
        private readonly object _lockObj = new object();
        private Stopwatch _stopwatch = new Stopwatch();

        // VisionPro ToolBlock对象
        private CogToolBlock _toolBlock;
        private string _inputImageName = "OutputImage"; // 默认输入图像名称，可根据VPP调整

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
        /// ToolBlock对象（用于外部访问）
        /// </summary>
        public CogToolBlock ToolBlock => _toolBlock;

        /// <summary>
        /// 输入图像的参数名称
        /// </summary>
        public string InputImageName
        {
            get => _inputImageName;
            set => _inputImageName = value;
        }

        /// <summary>
        /// 最后一次运行的所有输出参数
        /// </summary>
        public Dictionary<string, VppOutputItem> LastOutputs { get; private set; } = new Dictionary<string, VppOutputItem>();

        /// <summary>
        /// 所有输入参数名称列表
        /// </summary>
        public List<string> InputNames { get; private set; } = new List<string>();

        /// <summary>
        /// 所有输出参数名称列表
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
        /// 加载VisionPro ToolBlock文件（.vpp）
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

                    // 加载VPP文件为CogToolBlock
                    _toolBlock = CogSerializer.LoadObjectFromFile(vppFilePath) as CogToolBlock;

                    if (_toolBlock == null)
                    {
                        OnError("VPP文件不是有效的ToolBlock");
                        return false;
                    }

                    _jobFilePath = vppFilePath;
                    _isJobLoaded = true;

                    // 获取输入输出信息
                    CollectInputOutputNames();
                    LogToolBlockInfo();

                    LogHelper.Info("VisionPro", $"加载ToolBlock成功: {vppFilePath}");
                    LogHelper.Info("VisionPro", $"输入参数: {InputNames.Count}个, 输出参数: {OutputNames.Count}个");

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
                    if (_toolBlock != null)
                    {
                        _toolBlock = null;
                    }

                    _jobFilePath = null;
                    _isJobLoaded = false;
                    LastOutputs.Clear();
                    InputNames.Clear();
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
        /// 执行检测（传入Bitmap）
        /// </summary>
        public InspectionResult Run(Bitmap image)
        {
            if (image == null)
            {
                OnError("输入图像为空");
                return CreateErrorResult("输入图像为空");
            }

            // 将Bitmap转换为CogImage
            ICogImage cogImage = ConvertBitmapToCogImage(image);
            if (cogImage == null)
            {
                return CreateErrorResult("图像转换失败");
            }

            return Run(cogImage);
        }

        /// <summary>
        /// 执行检测（传入ICogImage）
        /// </summary>
        public InspectionResult Run(ICogImage cogImage)
        {
            if (!_isJobLoaded || _toolBlock == null)
            {
                OnError("未加载作业文件");
                return CreateErrorResult("未加载作业文件");
            }

            if (cogImage == null)
            {
                OnError("输入图像为空");
                return CreateErrorResult("输入图像为空");
            }

            try
            {
                lock (_lockObj)
                {
                    _stopwatch.Restart();

                    // 设置输入图像
                    if (_toolBlock.Inputs[_inputImageName] != null)
                    {
                        _toolBlock.Inputs[_inputImageName].Value = cogImage;
                        LogHelper.Debug("VisionPro", $"设置输入图像: {_inputImageName}");
                    }
                    else
                    {
                        // 尝试查找图像类型的输入
                        bool imageSet = false;
                        foreach (CogToolBlockTerminal input in _toolBlock.Inputs)
                        {
                            if (input.ValueType != null &&
                                (input.ValueType.Name.Contains("ICogImage") ||
                                 input.ValueType.Name.Contains("CogImage")))
                            {
                                input.Value = cogImage;
                                LogHelper.Debug("VisionPro", $"设置输入图像: {input.Name}");
                                imageSet = true;
                                break;
                            }
                        }

                        if (!imageSet)
                        {
                            LogHelper.Info("VisionPro", $"未找到输入图像参数 '{_inputImageName}'，尝试设置第一个输入");
                            if (_toolBlock.Inputs.Count > 0)
                            {
                                _toolBlock.Inputs[0].Value = cogImage;
                            }
                        }
                    }

                    // 运行ToolBlock
                    _toolBlock.Run();

                    _stopwatch.Stop();

                    // 收集结果
                    InspectionResult result = CollectToolBlockResults();
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
            if (_toolBlock == null) return "未加载作业";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendLine($"=== ToolBlock信息 ===");
            sb.AppendLine($"  名称: {_toolBlock.Name}");
            sb.AppendLine();

            sb.AppendLine("=== 输入参数 ===");
            foreach (CogToolBlockTerminal input in _toolBlock.Inputs)
            {
                sb.AppendLine($"  [{input.ValueType?.Name ?? "unknown"}] {input.Name}");
            }
            sb.AppendLine();

            sb.AppendLine("=== 输出参数 ===");
            foreach (CogToolBlockTerminal output in _toolBlock.Outputs)
            {
                sb.AppendLine($"  [{output.ValueType?.Name ?? "unknown"}] {output.Name}");
            }
            sb.AppendLine();

            sb.AppendLine("=== 上次运行输出值 ===");
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
        /// 获取所有输入参数（用于UI显示）
        /// </summary>
        public List<VppInputItem> GetAllInputs()
        {
            var inputs = new List<VppInputItem>();

            if (!_isJobLoaded || _toolBlock == null)
            {
                return inputs;
            }

            try
            {
                foreach (CogToolBlockTerminal input in _toolBlock.Inputs)
                {
                    inputs.Add(new VppInputItem
                    {
                        Name = input.Name,
                        ValueType = input.ValueType?.Name ?? "unknown",
                        Value = input.Value,
                        Description = ""
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("VisionPro", $"获取输入参数失败: {ex.Message}");
            }

            return inputs;
        }

        /// <summary>
        /// 保存作业
        /// </summary>
        public bool SaveJob(string vppFilePath)
        {
            if (!_isJobLoaded || _toolBlock == null)
            {
                OnError("没有已加载的作业");
                return false;
            }

            try
            {
                CogSerializer.SaveObjectToFile(_toolBlock, vppFilePath);
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
        /// 收集输入输出参数名称
        /// </summary>
        private void CollectInputOutputNames()
        {
            InputNames.Clear();
            OutputNames.Clear();

            if (_toolBlock == null) return;

            foreach (CogToolBlockTerminal input in _toolBlock.Inputs)
            {
                InputNames.Add(input.Name);
            }

            foreach (CogToolBlockTerminal output in _toolBlock.Outputs)
            {
                OutputNames.Add(output.Name);
            }
        }

        /// <summary>
        /// 记录ToolBlock信息到日志
        /// </summary>
        private void LogToolBlockInfo()
        {
            if (_toolBlock == null) return;

            LogHelper.Info("VisionPro", $"--- ToolBlock信息 ---");
            LogHelper.Info("VisionPro", $"  名称: {_toolBlock.Name}");
            LogHelper.Info("VisionPro", $"  输入参数数量: {_toolBlock.Inputs.Count}");
            LogHelper.Info("VisionPro", $"  输出参数数量: {_toolBlock.Outputs.Count}");

            LogHelper.Info("VisionPro", "  输入参数:");
            foreach (CogToolBlockTerminal input in _toolBlock.Inputs)
            {
                LogHelper.Info("VisionPro", $"    [{input.ValueType?.Name}] {input.Name}");
            }

            LogHelper.Info("VisionPro", "  输出参数:");
            foreach (CogToolBlockTerminal output in _toolBlock.Outputs)
            {
                LogHelper.Info("VisionPro", $"    [{output.ValueType?.Name}] {output.Name}");
            }
        }

        /// <summary>
        /// 收集ToolBlock运行结果
        /// </summary>
        private InspectionResult CollectToolBlockResults()
        {
            InspectionResult result = new InspectionResult();
            LastOutputs.Clear();

            try
            {
                // 获取运行状态
                result.IsPass = (_toolBlock.RunStatus.Result == CogToolResultConstants.Accept);
                result.Message = _toolBlock.RunStatus.Message ?? (result.IsPass ? "OK" : "NG");

                // 收集所有输出参数
                foreach (CogToolBlockTerminal output in _toolBlock.Outputs)
                {
                    try
                    {
                        object value = output.Value;
                        string typeName = value?.GetType().Name ?? output.ValueType?.Name ?? "null";

                        LastOutputs[output.Name] = new VppOutputItem
                        {
                            Name = output.Name,
                            ValueType = typeName,
                            Value = value,
                            Description = ""
                        };

                        result.ExtraData[output.Name] = value;

                        // 解析常见输出
                        ParseCommonOutput(result, output.Name, value);

                        // 检查是否是图像
                        if (value is ICogImage cogImage && result.ResultImage == null)
                        {
                            try
                            {
                                result.ResultImage = cogImage.ToBitmap();
                            }
                            catch { }
                        }

                        LogHelper.Debug("VisionPro", $"输出: [{typeName}] {output.Name} = {FormatValue(value)}");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Debug("VisionPro", $"获取输出 {output.Name} 失败: {ex.Message}");
                    }
                }

                LogHelper.Info("VisionPro", $"收集到 {LastOutputs.Count} 个输出参数");
            }
            catch (Exception ex)
            {
                LogHelper.Error("VisionPro", $"收集结果失败: {ex.Message}");
                result.Message = ex.Message;
            }

            return result;
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
                    // 转换为24位RGB
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

    #endregion
}