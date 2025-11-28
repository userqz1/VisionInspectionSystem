using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VisionInspectionSystem.Models;
using VisionInspectionSystem.DAL;

namespace VisionInspectionSystem.HAL
{
    /// <summary>
    /// TCP通讯封装类
    /// </summary>
    public class TcpCommunication : IDisposable
    {
        #region 私有字段

        private TcpListener _server;
        private TcpClient _client;
        private NetworkStream _stream;
        private Thread _receiveThread;
        private CancellationTokenSource _cts;
        private bool _isDisposed;
        private readonly object _lockObj = new object();

        #endregion

        #region 公共属性

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
        /// 通讯模式
        /// </summary>
        public CommunicationMode Mode { get; set; } = CommunicationMode.Client;

        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected
        {
            get
            {
                try
                {
                    return _client != null && _client.Connected;
                }
                catch
                {
                    return false;
                }
            }
        }

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
        /// 编码方式
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        #endregion

        #region 事件

        /// <summary>
        /// 数据接收事件
        /// </summary>
        public event EventHandler<string> DataReceived;

        /// <summary>
        /// 字节数据接收事件
        /// </summary>
        public event EventHandler<byte[]> BytesReceived;

        /// <summary>
        /// 连接状态变化事件
        /// </summary>
        public event EventHandler<ConnectionState> ConnectionStateChanged;

        /// <summary>
        /// 错误发生事件
        /// </summary>
        public event EventHandler<string> ErrorOccurred;

        /// <summary>
        /// 客户端连接事件（服务端模式）
        /// </summary>
        public event EventHandler<string> ClientConnected;

        /// <summary>
        /// 客户端断开事件（服务端模式）
        /// </summary>
        public event EventHandler<string> ClientDisconnected;

        #endregion

        #region 构造函数

        public TcpCommunication()
        {
        }

        public TcpCommunication(CommunicationConfig config)
        {
            LoadConfig(config);
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 加载配置
        /// </summary>
        public void LoadConfig(CommunicationConfig config)
        {
            if (config == null) return;

            Mode = config.Mode;
            LocalIP = config.LocalIP;
            LocalPort = config.LocalPort;
            RemoteIP = config.RemoteIP;
            RemotePort = config.RemotePort;
            Timeout = config.Timeout;
            AutoReconnect = config.AutoReconnect;
            ReconnectInterval = config.ReconnectInterval;
        }

        /// <summary>
        /// 启动服务端
        /// </summary>
        public bool StartServer()
        {
            try
            {
                if (Mode != CommunicationMode.Server)
                {
                    OnError("当前模式不是服务端模式");
                    return false;
                }

                IPAddress ip = IPAddress.Parse(LocalIP);
                _server = new TcpListener(ip, LocalPort);
                _server.Start();

                _cts = new CancellationTokenSource();

                // 开始异步接受客户端连接
                Task.Run(() => AcceptClientsAsync(_cts.Token));

                LogHelper.Info("TCP", $"服务端启动成功: {LocalIP}:{LocalPort}");
                OnConnectionStateChanged(ConnectionState.Connected);
                return true;
            }
            catch (Exception ex)
            {
                OnError($"服务端启动失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 停止服务端
        /// </summary>
        public bool StopServer()
        {
            try
            {
                _cts?.Cancel();

                if (_client != null)
                {
                    _client.Close();
                    _client = null;
                }

                if (_server != null)
                {
                    _server.Stop();
                    _server = null;
                }

                LogHelper.Info("TCP", "服务端已停止");
                OnConnectionStateChanged(ConnectionState.Disconnected);
                return true;
            }
            catch (Exception ex)
            {
                OnError($"停止服务端失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 连接到服务端（客户端模式）
        /// </summary>
        public bool Connect()
        {
            try
            {
                if (Mode != CommunicationMode.Client)
                {
                    OnError("当前模式不是客户端模式");
                    return false;
                }

                if (IsConnected)
                {
                    return true;
                }

                OnConnectionStateChanged(ConnectionState.Connecting);

                _client = new TcpClient();
                var result = _client.BeginConnect(RemoteIP, RemotePort, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(Timeout));

                if (!success || !_client.Connected)
                {
                    _client.Close();
                    _client = null;
                    OnError("连接超时");
                    OnConnectionStateChanged(ConnectionState.Disconnected);
                    return false;
                }

                _client.EndConnect(result);
                _stream = _client.GetStream();

                // 启动接收线程
                _cts = new CancellationTokenSource();
                _receiveThread = new Thread(() => ReceiveData(_cts.Token))
                {
                    IsBackground = true
                };
                _receiveThread.Start();

                LogHelper.Info("TCP", $"连接成功: {RemoteIP}:{RemotePort}");
                OnConnectionStateChanged(ConnectionState.Connected);
                return true;
            }
            catch (Exception ex)
            {
                OnError($"连接失败: {ex.Message}");
                OnConnectionStateChanged(ConnectionState.Error);
                return false;
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public bool Disconnect()
        {
            try
            {
                _cts?.Cancel();

                if (_stream != null)
                {
                    _stream.Close();
                    _stream = null;
                }

                if (_client != null)
                {
                    _client.Close();
                    _client = null;
                }

                LogHelper.Info("TCP", "连接已断开");
                OnConnectionStateChanged(ConnectionState.Disconnected);
                return true;
            }
            catch (Exception ex)
            {
                OnError($"断开连接失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 发送字符串数据
        /// </summary>
        public bool Send(string message)
        {
            if (string.IsNullOrEmpty(message)) return false;
            return Send(Encoding.GetBytes(message));
        }

        /// <summary>
        /// 发送字节数据
        /// </summary>
        public bool Send(byte[] data)
        {
            try
            {
                if (!IsConnected || _stream == null)
                {
                    OnError("未连接，无法发送数据");
                    return false;
                }

                lock (_lockObj)
                {
                    _stream.Write(data, 0, data.Length);
                    _stream.Flush();
                }

                LogHelper.Debug("TCP", $"发送: {Encoding.GetString(data)}");
                return true;
            }
            catch (Exception ex)
            {
                OnError($"发送失败: {ex.Message}");
                HandleDisconnection();
                return false;
            }
        }

        /// <summary>
        /// 发送并等待响应
        /// </summary>
        public string SendAndReceive(string message, int timeout = 5000)
        {
            string response = null;
            ManualResetEvent responseEvent = new ManualResetEvent(false);

            EventHandler<string> handler = (s, data) =>
            {
                response = data;
                responseEvent.Set();
            };

            DataReceived += handler;

            try
            {
                if (Send(message))
                {
                    responseEvent.WaitOne(timeout);
                }
            }
            finally
            {
                DataReceived -= handler;
            }

            return response;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 异步接受客户端连接（服务端模式）
        /// </summary>
        private async Task AcceptClientsAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    TcpClient client = await _server.AcceptTcpClientAsync();
                    string clientEndPoint = client.Client.RemoteEndPoint.ToString();

                    // 如果已有客户端连接，关闭之前的
                    if (_client != null)
                    {
                        _client.Close();
                        OnClientDisconnected("Previous client");
                    }

                    _client = client;
                    _stream = _client.GetStream();

                    LogHelper.Info("TCP", $"客户端连接: {clientEndPoint}");
                    OnClientConnected(clientEndPoint);

                    // 启动接收线程
                    Thread receiveThread = new Thread(() => ReceiveData(token))
                    {
                        IsBackground = true
                    };
                    receiveThread.Start();
                }
            }
            catch (ObjectDisposedException)
            {
                // 服务端已停止
            }
            catch (Exception ex)
            {
                if (!token.IsCancellationRequested)
                {
                    OnError($"接受客户端连接异常: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        private void ReceiveData(CancellationToken token)
        {
            byte[] buffer = new byte[4096];

            try
            {
                while (!token.IsCancellationRequested && IsConnected)
                {
                    if (_stream == null || !_stream.DataAvailable)
                    {
                        Thread.Sleep(10);
                        continue;
                    }

                    int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        byte[] data = new byte[bytesRead];
                        Array.Copy(buffer, data, bytesRead);

                        string message = Encoding.GetString(data).Trim();
                        LogHelper.Debug("TCP", $"接收: {message}");

                        OnDataReceived(message);
                        OnBytesReceived(data);
                    }
                    else
                    {
                        // 连接断开
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (!token.IsCancellationRequested)
                {
                    OnError($"接收数据异常: {ex.Message}");
                }
            }

            HandleDisconnection();
        }

        /// <summary>
        /// 处理断开连接
        /// </summary>
        private void HandleDisconnection()
        {
            OnConnectionStateChanged(ConnectionState.Disconnected);

            if (Mode == CommunicationMode.Server)
            {
                OnClientDisconnected("Client");
            }
            else if (AutoReconnect && !_isDisposed)
            {
                // 自动重连
                Task.Run(async () =>
                {
                    await Task.Delay(ReconnectInterval);
                    if (!_isDisposed && !IsConnected)
                    {
                        LogHelper.Info("TCP", "尝试重新连接...");
                        Connect();
                    }
                });
            }
        }

        #endregion

        #region 事件触发

        protected virtual void OnDataReceived(string data)
        {
            DataReceived?.Invoke(this, data);
        }

        protected virtual void OnBytesReceived(byte[] data)
        {
            BytesReceived?.Invoke(this, data);
        }

        protected virtual void OnConnectionStateChanged(ConnectionState state)
        {
            ConnectionStateChanged?.Invoke(this, state);
        }

        protected virtual void OnError(string message)
        {
            LogHelper.Error("TCP", message);
            ErrorOccurred?.Invoke(this, message);
        }

        protected virtual void OnClientConnected(string endPoint)
        {
            ClientConnected?.Invoke(this, endPoint);
        }

        protected virtual void OnClientDisconnected(string endPoint)
        {
            ClientDisconnected?.Invoke(this, endPoint);
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
                _isDisposed = true;
                _cts?.Cancel();

                if (Mode == CommunicationMode.Server)
                {
                    StopServer();
                }
                else
                {
                    Disconnect();
                }

                _cts?.Dispose();
            }

            _isDisposed = true;
        }

        #endregion
    }
}
