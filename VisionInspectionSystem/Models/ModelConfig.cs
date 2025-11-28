using System;

namespace VisionInspectionSystem.Models
{
    /// <summary>
    /// 版型配置类
    /// </summary>
    [Serializable]
    public class ModelConfig
    {
        /// <summary>
        /// 版型名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 版型描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 版型路径
        /// </summary>
        public string ModelPath { get; set; }

        /// <summary>
        /// VisionPro作业文件路径
        /// </summary>
        public string VppFilePath { get; set; }

        /// <summary>
        /// 相机配置
        /// </summary>
        public CameraConfig CameraSettings { get; set; }

        /// <summary>
        /// 输出结果格式
        /// </summary>
        public string ResultFormat { get; set; } = "OK,{X},{Y},{R}";

        /// <summary>
        /// 小数位数
        /// </summary>
        public int DecimalPlaces { get; set; } = 3;

        /// <summary>
        /// 是否保存OK图像
        /// </summary>
        public bool SaveOkImage { get; set; } = true;

        /// <summary>
        /// 是否保存NG图像
        /// </summary>
        public bool SaveNgImage { get; set; } = true;

        /// <summary>
        /// 图像保存格式
        /// </summary>
        public string ImageFormat { get; set; } = "jpg";

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        public ModelConfig()
        {
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;
            CameraSettings = new CameraConfig();
        }

        public ModelConfig(string modelName) : this()
        {
            ModelName = modelName;
        }

        /// <summary>
        /// 更新修改时间
        /// </summary>
        public void UpdateModifyTime()
        {
            ModifyTime = DateTime.Now;
        }
    }
}
