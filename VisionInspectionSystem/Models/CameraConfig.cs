using System;

namespace VisionInspectionSystem.Models
{
    /// <summary>
    /// 相机配置类
    /// </summary>
    [Serializable]
    public class CameraConfig
    {
        /// <summary>
        /// 相机名称
        /// </summary>
        public string CameraName { get; set; }

        /// <summary>
        /// 相机序列号
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 相机型号
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// IP地址（GigE相机）
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 曝光时间（微秒）
        /// </summary>
        public double ExposureTime { get; set; } = 10000;

        /// <summary>
        /// 增益（dB）
        /// </summary>
        public double Gain { get; set; } = 0;

        /// <summary>
        /// 触发模式
        /// </summary>
        public TriggerMode TriggerMode { get; set; } = TriggerMode.Off;

        /// <summary>
        /// 触发源
        /// </summary>
        public TriggerSource TriggerSource { get; set; } = TriggerSource.Software;

        /// <summary>
        /// 图像宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 图像高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 曝光时间最小值
        /// </summary>
        public double ExposureTimeMin { get; set; } = 100;

        /// <summary>
        /// 曝光时间最大值
        /// </summary>
        public double ExposureTimeMax { get; set; } = 1000000;

        /// <summary>
        /// 增益最小值
        /// </summary>
        public double GainMin { get; set; } = 0;

        /// <summary>
        /// 增益最大值
        /// </summary>
        public double GainMax { get; set; } = 24;

        public CameraConfig()
        {
        }

        public CameraConfig(string serialNumber)
        {
            SerialNumber = serialNumber;
        }
    }

    /// <summary>
    /// 触发模式枚举
    /// </summary>
    public enum TriggerMode
    {
        /// <summary>
        /// 关闭触发（自由运行）
        /// </summary>
        Off = 0,

        /// <summary>
        /// 开启触发模式
        /// </summary>
        On = 1
    }

    /// <summary>
    /// 触发源枚举
    /// </summary>
    public enum TriggerSource
    {
        /// <summary>
        /// 软件触发
        /// </summary>
        Software = 0,

        /// <summary>
        /// 外部触发 Line1
        /// </summary>
        Line1 = 1,

        /// <summary>
        /// 外部触发 Line2
        /// </summary>
        Line2 = 2,

        /// <summary>
        /// 外部触发 Line3
        /// </summary>
        Line3 = 3
    }

    /// <summary>
    /// 相机信息（用于枚举相机列表）
    /// </summary>
    public class CameraInfo
    {
        public string SerialNumber { get; set; }
        public string ModelName { get; set; }
        public string UserDefinedName { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public bool IsConnected { get; set; }

        public override string ToString()
        {
            return $"{UserDefinedName ?? ModelName} ({SerialNumber})";
        }
    }
}
