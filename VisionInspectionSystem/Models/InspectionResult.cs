using System;
using System.Collections.Generic;
using System.Drawing;

namespace VisionInspectionSystem.Models
{
    /// <summary>
    /// 检测结果类
    /// </summary>
    [Serializable]
    public class InspectionResult
    {
        /// <summary>
        /// 结果ID
        /// </summary>
        public string ResultId { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public bool IsPass { get; set; }

        /// <summary>
        /// X坐标
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y坐标
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// 角度
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// 匹配分数
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// 运行时间（毫秒）
        /// </summary>
        public double RunTime { get; set; }

        /// <summary>
        /// 结果消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 结果图像（不序列化）
        /// </summary>
        [NonSerialized]
        private Bitmap _resultImage;

        public Bitmap ResultImage
        {
            get => _resultImage;
            set => _resultImage = value;
        }

        /// <summary>
        /// 原始图像路径
        /// </summary>
        public string OriginalImagePath { get; set; }

        /// <summary>
        /// 结果图像路径
        /// </summary>
        public string ResultImagePath { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// 版型名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 工具结果列表
        /// </summary>
        public List<ToolResult> ToolResults { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public Dictionary<string, object> ExtraData { get; set; }

        public InspectionResult()
        {
            ResultId = Guid.NewGuid().ToString("N");
            TimeStamp = DateTime.Now;
            ToolResults = new List<ToolResult>();
            ExtraData = new Dictionary<string, object>();
        }

        /// <summary>
        /// 获取格式化的结果字符串
        /// </summary>
        public string GetFormattedResult(string format, int decimalPlaces = 3)
        {
            string result = format
                .Replace("{X}", X.ToString($"F{decimalPlaces}"))
                .Replace("{Y}", Y.ToString($"F{decimalPlaces}"))
                .Replace("{R}", Angle.ToString($"F{decimalPlaces}"))
                .Replace("{Score}", Score.ToString($"F{decimalPlaces}"))
                .Replace("{Result}", IsPass ? "OK" : "NG");

            return result;
        }

        public override string ToString()
        {
            return $"{(IsPass ? "OK" : "NG")}, X:{X:F3}, Y:{Y:F3}, R:{Angle:F3}, Score:{Score:F2}%, Time:{RunTime:F1}ms";
        }
    }

    /// <summary>
    /// 单个工具的结果
    /// </summary>
    [Serializable]
    public class ToolResult
    {
        /// <summary>
        /// 工具名称
        /// </summary>
        public string ToolName { get; set; }

        /// <summary>
        /// 工具类型
        /// </summary>
        public string ToolType { get; set; }

        /// <summary>
        /// 是否通过
        /// </summary>
        public bool IsPass { get; set; }

        /// <summary>
        /// 结果值
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// 运行时间
        /// </summary>
        public double RunTime { get; set; }

        /// <summary>
        /// 结果消息
        /// </summary>
        public string Message { get; set; }

        public override string ToString()
        {
            return $"{ToolName}: {(IsPass ? "Pass" : "Fail")} - {Value:F3}";
        }
    }
}
