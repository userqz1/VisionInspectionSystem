using System;
using VisionInspectionSystem.Common;
using VisionInspectionSystem.DAL;
using VisionInspectionSystem.HAL;
using VisionInspectionSystem.Models;

namespace VisionInspectionSystem.BLL
{
    /// <summary>
    /// 通讯管理类
    /// </summary>
    public class CommunicationManager : IDisposable
    {
        #region 单例模式

        private static CommunicationManager _instance;
        private static readonly object _lockObj = new object();

        public static CommunicationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new CommunicationManager();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region 私有字段

        private TcpCommunication _tcp;
        private CommunicationConfig _config;
        private bool _isDisposed;

        #endregion

        #region 公共属性

        /// <summary>
        /// TCP通讯对象
        /// </summary>
        public TcpCommunication Tcp => _tcp;

        /// <summary>
        /// 通讯配置
        /// </summary>
        public CommunicationConfig Config => _config;

        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected => _tcp?.IsConnected ?? false;

        #endregion

        #region 事件

        /// <summary>
        /// 数据接收事件
        /// </summary>
        public event EventHandler<string> DataReceived;

        /// <summary>
        /// 连接状态变化事件
        /// </summary>
        public event EventHandler<ConnectionState> ConnectionStateChanged;

        /// <summary>
        /// 命令接收事件（已解析的命令）
        /// </summary>
        public event EventHandler<CommandEventArgs> CommandReceived;

        /// <summary>
        /// 错误事件
        /// </summary>
        public event EventHandler<string> ErrorOccurred;

        #endregion

        #region 构造函数

        private CommunicationManager()
        {
            _tcp = new TcpCommunication();
            _tcp.DataReceived += Tcp_DataReceived;
            _tcp.ConnectionStateChanged += Tcp_ConnectionStateChanged;
            _tcp.ErrorOccurred += Tcp_ErrorOccurred;
            _tcp.ClientConnected += Tcp_ClientConnected;
            _tcp.ClientDisconnected += Tcp_ClientDisconnected;

            // 加载配置
            LoadConfig();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 启动通讯（根据配置模式）
        /// </summary>
        public bool Start()
        {
            try
            {
                if (_config.Mode == CommunicationMode.Server)
                {
                    return _tcp.StartServer();
                }
                else
                {
                    return _tcp.Connect();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("CommManager", $"启动通讯失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 停止通讯
        /// </summary>
        public bool Stop()
        {
            try
            {
                if (_config.Mode == CommunicationMode.Server)
                {
                    return _tcp.StopServer();
                }
                else
                {
                    return _tcp.Disconnect();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("CommManager", $"停止通讯失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        public bool Send(string message)
        {
            LogHelper.Debug("CommManager", $"发送: {message}");
            return _tcp?.Send(message) ?? false;
        }

        /// <summary>
        /// 发送检测结果
        /// </summary>
        public bool SendResult(InspectionResult result)
        {
            if (result == null) return false;

            string message = result.GetFormattedResult(_config.ResultFormat, 3);
            return Send(message);
        }

        /// <summary>
        /// 发送OK响应
        /// </summary>
        public bool SendOK(string data = "")
        {
            string message = string.IsNullOrEmpty(data) ? Constants.RESP_OK : $"{Constants.RESP_OK},{data}";
            return Send(message);
        }

        /// <summary>
        /// 发送NG响应
        /// </summary>
        public bool SendNG(string errorCode = "")
        {
            string message = string.IsNullOrEmpty(errorCode) ? Constants.RESP_NG : $"{Constants.RESP_NG},{errorCode}";
            return Send(message);
        }

        /// <summary>
        /// 更新配置
        /// </summary>
        public void UpdateConfig(CommunicationConfig config)
        {
            if (config == null) return;

            _config = config;
            _tcp.LoadConfig(config);
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public bool SaveConfig()
        {
            return ConfigHelper.SaveCommunicationConfig(_config);
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        public void LoadConfig()
        {
            _config = ConfigHelper.LoadCommunicationConfig();
            _tcp.LoadConfig(_config);
        }

        #endregion

        #region 事件处理

        private void Tcp_DataReceived(object sender, string e)
        {
            DataReceived?.Invoke(this, e);
            ParseCommand(e);
        }

        private void Tcp_ConnectionStateChanged(object sender, ConnectionState e)
        {
            ConnectionStateChanged?.Invoke(this, e);
        }

        private void Tcp_ErrorOccurred(object sender, string e)
        {
            ErrorOccurred?.Invoke(this, e);
        }

        private void Tcp_ClientConnected(object sender, string e)
        {
            LogHelper.Info("CommManager", $"客户端连接: {e}");
        }

        private void Tcp_ClientDisconnected(object sender, string e)
        {
            LogHelper.Info("CommManager", $"客户端断开: {e}");
        }

        /// <summary>
        /// 解析命令
        /// </summary>
        private void ParseCommand(string data)
        {
            if (string.IsNullOrEmpty(data)) return;

            string command = data.Trim();
            string parameter = "";

            // 解析命令和参数
            int separatorIndex = command.IndexOf(Constants.CMD_SEPARATOR);
            if (separatorIndex > 0)
            {
                parameter = command.Substring(separatorIndex + 1);
                command = command.Substring(0, separatorIndex);
            }

            CommandType cmdType = CommandType.Unknown;

            // 识别命令类型
            if (command.Equals(_config.TriggerCommand, StringComparison.OrdinalIgnoreCase))
            {
                cmdType = CommandType.Trigger;
            }
            else if (command.Equals(_config.HeartbeatCommand, StringComparison.OrdinalIgnoreCase))
            {
                cmdType = CommandType.Heartbeat;
            }
            else if (command.Equals(Constants.CMD_GET_RESULT, StringComparison.OrdinalIgnoreCase))
            {
                cmdType = CommandType.Query;
            }

            // 触发命令事件
            CommandReceived?.Invoke(this, new CommandEventArgs
            {
                CommandType = cmdType,
                RawCommand = data,
                Command = command,
                Parameter = parameter
            });
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
                _tcp?.Dispose();
            }

            _isDisposed = true;
        }

        #endregion
    }

    #region 命令相关类型

    /// <summary>
    /// 命令类型枚举
    /// </summary>
    public enum CommandType
    {
        Unknown,
        Trigger,
        Heartbeat,
        Query,
        Stop,
        Reset
    }

    /// <summary>
    /// 命令事件参数
    /// </summary>
    public class CommandEventArgs : EventArgs
    {
        public CommandType CommandType { get; set; }
        public string RawCommand { get; set; }
        public string Command { get; set; }
        public string Parameter { get; set; }
    }

    #endregion
}
