using System;
using VisionInspectionSystem.Common;

namespace VisionInspectionSystem.Models
{
    /// <summary>
    /// 运行配置
    /// </summary>
    public class RunConfig
    {
        /// <summary>
        /// 运行开始命令
        /// </summary>
        public string StartCommand { get; set; } = "1";

        /// <summary>
        /// 相机拍照方式
        /// </summary>
        public CameraTriggerType CameraTriggerType { get; set; } = CameraTriggerType.SoftwareTrigger;

        /// <summary>
        /// 版型名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// VPP文件路径
        /// </summary>
        public string VppFilePath { get; set; }

        /// <summary>
        /// NG发送命令
        /// </summary>
        public string NgCommand { get; set; } = "0";

        /// <summary>
        /// OK发送命令
        /// </summary>
        public string OkCommand { get; set; } = "1";

        /// <summary>
        /// 是否自动发送结果
        /// </summary>
        public bool AutoSendResult { get; set; } = true;

        /// <summary>
        /// 是否保存NG图像
        /// </summary>
        public bool SaveNgImage { get; set; } = true;

        /// <summary>
        /// 是否保存OK图像
        /// </summary>
        public bool SaveOkImage { get; set; } = false;
    }

    /// <summary>
    /// 相机触发类型
    /// </summary>
    public enum CameraTriggerType
    {
        /// <summary>
        /// 软触发
        /// </summary>
        SoftwareTrigger = 0,

        /// <summary>
        /// 单次采集
        /// </summary>
        SingleShot = 1,

        /// <summary>
        /// 自动连续触发
        /// </summary>
        AutoContinuous = 2
    }
}
