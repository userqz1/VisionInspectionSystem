using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace VisionInspectionSystem.Common
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Utils
    {
        #region 字符串加密解密

        /// <summary>
        /// MD5加密
        /// </summary>
        public static string MD5Encrypt(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 简单加密（Base64）
        /// </summary>
        public static string SimpleEncrypt(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 简单解密（Base64）
        /// </summary>
        public static string SimpleDecrypt(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            try
            {
                byte[] bytes = Convert.FromBase64String(input);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region 文件操作

        /// <summary>
        /// 确保目录存在
        /// </summary>
        public static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 获取唯一文件名
        /// </summary>
        public static string GetUniqueFileName(string basePath, string prefix, string extension)
        {
            string timestamp = DateTime.Now.ToString(Constants.TIMESTAMP_FORMAT);
            return Path.Combine(basePath, $"{prefix}_{timestamp}{extension}");
        }

        /// <summary>
        /// 清理过期文件
        /// </summary>
        public static void CleanupOldFiles(string directory, int maxDays, string searchPattern = "*.*")
        {
            if (!Directory.Exists(directory)) return;

            DateTime cutoffDate = DateTime.Now.AddDays(-maxDays);
            string[] files = Directory.GetFiles(directory, searchPattern);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.LastWriteTime < cutoffDate)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch { }
                }
            }
        }

        #endregion

        #region 图像操作

        /// <summary>
        /// 保存图像
        /// </summary>
        public static bool SaveImage(Bitmap image, string filePath, ImageFormat format = ImageFormat.Jpg)
        {
            try
            {
                EnsureDirectoryExists(Path.GetDirectoryName(filePath));

                System.Drawing.Imaging.ImageFormat imgFormat;
                switch (format)
                {
                    case ImageFormat.Bmp:
                        imgFormat = System.Drawing.Imaging.ImageFormat.Bmp;
                        break;
                    case ImageFormat.Png:
                        imgFormat = System.Drawing.Imaging.ImageFormat.Png;
                        break;
                    case ImageFormat.Tiff:
                        imgFormat = System.Drawing.Imaging.ImageFormat.Tiff;
                        break;
                    default:
                        imgFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                }

                image.Save(filePath, imgFormat);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 加载图像
        /// </summary>
        public static Bitmap LoadImage(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return null;
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    return new Bitmap(stream);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 缩放图像
        /// </summary>
        public static Bitmap ResizeImage(Bitmap source, int width, int height)
        {
            if (source == null) return null;

            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(source, 0, 0, width, height);
            }
            return result;
        }

        #endregion

        #region 网络操作

        /// <summary>
        /// 获取本机IP地址列表
        /// </summary>
        public static List<string> GetLocalIPAddresses()
        {
            List<string> ipList = new List<string>();
            try
            {
                string hostName = Dns.GetHostName();
                IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
                foreach (IPAddress ip in hostEntry.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipList.Add(ip.ToString());
                    }
                }
            }
            catch { }
            return ipList;
        }

        /// <summary>
        /// 验证IP地址格式
        /// </summary>
        public static bool IsValidIPAddress(string ip)
        {
            return IPAddress.TryParse(ip, out _);
        }

        /// <summary>
        /// 验证端口号
        /// </summary>
        public static bool IsValidPort(int port)
        {
            return port > 0 && port <= 65535;
        }

        #endregion

        #region 数值操作

        /// <summary>
        /// 限制数值范围
        /// </summary>
        public static double Clamp(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        /// <summary>
        /// 限制数值范围（整数）
        /// </summary>
        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        #endregion

        #region 时间操作

        /// <summary>
        /// 格式化时间跨度
        /// </summary>
        public static string FormatTimeSpan(TimeSpan ts)
        {
            if (ts.TotalHours >= 1)
            {
                return $"{(int)ts.TotalHours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
            }
            else
            {
                return $"{ts.Minutes:D2}:{ts.Seconds:D2}";
            }
        }

        #endregion
    }
}
