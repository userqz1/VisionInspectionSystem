using System;

namespace VisionInspectionSystem.Models
{
    /// <summary>
    /// 通讯配置类
    /// </summary>
    [Serializable]
    public class CommunicationConfig
    {
        /// <summary>
        /// 通讯模式（服务端/客户端）
        /// </summary>
        public CommunicationMode Mode { get; set; } = CommunicationMode.Client;

        /// <summary>
        /// 本地IP地址
        /// </summary>
        public string LocalIP { get; set; } = "127.0.0.1";

        /// <summary>
        /// 本地端口
        /// </summary>
        public int LocalPort { get; set; } = 8000;

        /// <summary>
        /// 远程IP地址
        /// </summary>
        public string RemoteIP { get; set; } = "127.0.0.1";

        /// <summary>
        /// 远程端口
        /// </summary>
        public int RemotePort { get; set; } = 8001;

        /// <summary>
        /// 连接超时时间（毫秒）
        /// </summary>
        public int Timeout { get; set; } = 5000;

        /// <summary>
        /// 是否自动重连
        /// </summary>
        public bool AutoReconnect { get; set; } = true;

        /// <summary>
        /// 重连间隔（毫秒）
        /// </summary>
        public int ReconnectInterval { get; set; } = 3000;

        /// <summary>
        /// 心跳间隔（毫秒）
        /// </summary>
        public int HeartbeatInterval { get; set; } = 5000;

        /// <summary>
        /// 是否启用心跳
        /// </summary>
        public bool EnableHeartbeat { get; set; } = false;

        /// <summary>
        /// 触发命令
        /// </summary>
        public string TriggerCommand { get; set; } = "T1";

        /// <summary>
        /// 心跳命令
        /// </summary>
        public string HeartbeatCommand { get; set; } = "H1";

        /// <summary>
        /// 结果格式
        /// </summary>
        public string ResultFormat { get; set; } = "OK,{X},{Y},{R}";

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        public CommunicationConfig()
        {
        }
    }

    /// <summary>
    /// 通讯模式枚举
    /// </summary>
    public enum CommunicationMode
    {
        /// <summary>
        /// 服务端模式
        /// </summary>
        Server = 0,

        /// <summary>
        /// 客户端模式
        /// </summary>
        Client = 1
    }

    /// <summary>
    /// 连接状态枚举
    /// </summary>
    public enum ConnectionState
    {
        /// <summary>
        /// 断开
        /// </summary>
        Disconnected = 0,

        /// <summary>
        /// 连接中
        /// </summary>
        Connecting = 1,

        /// <summary>
        /// 已连接
        /// </summary>
        Connected = 2,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 3
    }
}
