using System;
using System.IO;
using System.Text;
using VisionInspectionSystem.Common;

namespace VisionInspectionSystem.DAL
{
    /// <summary>
    /// 日志助手类
    /// </summary>
    public static class LogHelper
    {
        private static readonly object _lockObj = new object();
        private static string _logPath;
        private static LogLevel _minLevel = LogLevel.Debug;

        /// <summary>
        /// 日志事件
        /// </summary>
        public static event EventHandler<LogEventArgs> LogWritten;

        /// <summary>
        /// 初始化日志系统
        /// </summary>
        public static void Initialize()
        {
            _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.LOG_FOLDER);
            Utils.EnsureDirectoryExists(_logPath);
        }

        /// <summary>
        /// 设置最小日志级别
        /// </summary>
        public static void SetMinLevel(LogLevel level)
        {
            _minLevel = level;
        }

        /// <summary>
        /// 写入调试日志
        /// </summary>
        public static void Debug(string module, string message)
        {
            WriteLog(LogLevel.Debug, module, message);
        }

        /// <summary>
        /// 写入信息日志
        /// </summary>
        public static void Info(string module, string message)
        {
            WriteLog(LogLevel.Info, module, message);
        }

        /// <summary>
        /// 写入警告日志
        /// </summary>
        public static void Warn(string module, string message)
        {
            WriteLog(LogLevel.Warn, module, message);
        }

        /// <summary>
        /// 写入错误日志
        /// </summary>
        public static void Error(string module, string message)
        {
            WriteLog(LogLevel.Error, module, message);
        }

        /// <summary>
        /// 写入错误日志（带异常）
        /// </summary>
        public static void Error(string module, string message, Exception ex)
        {
            WriteLog(LogLevel.Error, module, $"{message}\r\n{ex}");
        }

        /// <summary>
        /// 写入致命错误日志
        /// </summary>
        public static void Fatal(string module, string message)
        {
            WriteLog(LogLevel.Fatal, module, message);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        private static void WriteLog(LogLevel level, string module, string message)
        {
            if (level < _minLevel) return;

            try
            {
                DateTime now = DateTime.Now;
                string logLine = $"[{now:yyyy-MM-dd HH:mm:ss.fff}] [{level,-5}] [{module,-10}] {message}";

                // 写入文件
                lock (_lockObj)
                {
                    string fileName = Path.Combine(_logPath, $"{now:yyyyMMdd}.log");
                    File.AppendAllText(fileName, logLine + Environment.NewLine, Encoding.UTF8);
                }

                // 触发事件
                LogWritten?.Invoke(null, new LogEventArgs
                {
                    Time = now,
                    Level = level,
                    Module = module,
                    Message = message,
                    FullMessage = logLine
                });

                // 控制台输出（调试用）
                #if DEBUG
                Console.WriteLine(logLine);
                #endif
            }
            catch
            {
                // 日志写入失败时不抛出异常
            }
        }

        /// <summary>
        /// 清理过期日志
        /// </summary>
        public static void CleanupOldLogs(int maxDays = 30)
        {
            Utils.CleanupOldFiles(_logPath, maxDays, "*.log");
        }
    }

    /// <summary>
    /// 日志事件参数
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        public DateTime Time { get; set; }
        public LogLevel Level { get; set; }
        public string Module { get; set; }
        public string Message { get; set; }
        public string FullMessage { get; set; }
    }
}
