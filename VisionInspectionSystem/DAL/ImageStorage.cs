using System;
using System.Drawing;
using System.IO;
using VisionInspectionSystem.Common;

namespace VisionInspectionSystem.DAL
{
    /// <summary>
    /// 图像存储类
    /// </summary>
    public static class ImageStorage
    {
        private static string _imagePath;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            _imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.IMAGE_FOLDER);
            Utils.EnsureDirectoryExists(_imagePath);
        }

        /// <summary>
        /// 保存检测图像
        /// </summary>
        /// <param name="image">图像</param>
        /// <param name="isPass">是否合格</param>
        /// <param name="modelName">版型名称</param>
        /// <param name="format">图像格式</param>
        /// <returns>保存路径</returns>
        public static string SaveInspectionImage(Bitmap image, bool isPass, string modelName,
            Common.ImageFormat format = Common.ImageFormat.Jpg)
        {
            if (image == null) return null;

            try
            {
                // 创建目录结构: Images/日期/版型/OK或NG/
                DateTime now = DateTime.Now;
                string datePath = now.ToString("yyyyMMdd");
                string resultPath = isPass ? "OK" : "NG";
                string savePath = Path.Combine(_imagePath, datePath, modelName ?? "Default", resultPath);
                Utils.EnsureDirectoryExists(savePath);

                // 生成文件名
                string extension = GetExtension(format);
                string fileName = $"{now:HHmmss_fff}{extension}";
                string filePath = Path.Combine(savePath, fileName);

                // 保存图像
                if (Utils.SaveImage(image, filePath, format))
                {
                    return filePath;
                }

                return null;
            }
            catch (Exception ex)
            {
                LogHelper.Error("ImageStorage", $"保存图像失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 保存原始图像
        /// </summary>
        public static string SaveOriginalImage(Bitmap image, string prefix = "Original")
        {
            if (image == null) return null;

            try
            {
                DateTime now = DateTime.Now;
                string datePath = now.ToString("yyyyMMdd");
                string savePath = Path.Combine(_imagePath, datePath, "Original");
                Utils.EnsureDirectoryExists(savePath);

                string fileName = $"{prefix}_{now:HHmmss_fff}.bmp";
                string filePath = Path.Combine(savePath, fileName);

                if (Utils.SaveImage(image, filePath, Common.ImageFormat.Bmp))
                {
                    return filePath;
                }

                return null;
            }
            catch (Exception ex)
            {
                LogHelper.Error("ImageStorage", $"保存原始图像失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 加载图像
        /// </summary>
        public static Bitmap LoadImage(string filePath)
        {
            return Utils.LoadImage(filePath);
        }

        /// <summary>
        /// 获取今日图像数量
        /// </summary>
        public static int GetTodayImageCount()
        {
            try
            {
                string todayPath = Path.Combine(_imagePath, DateTime.Now.ToString("yyyyMMdd"));
                if (!Directory.Exists(todayPath)) return 0;

                return Directory.GetFiles(todayPath, "*.*", SearchOption.AllDirectories).Length;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取图像存储目录大小（MB）
        /// </summary>
        public static double GetStorageSize()
        {
            try
            {
                if (!Directory.Exists(_imagePath)) return 0;

                long totalSize = 0;
                foreach (string file in Directory.GetFiles(_imagePath, "*.*", SearchOption.AllDirectories))
                {
                    totalSize += new FileInfo(file).Length;
                }
                return totalSize / (1024.0 * 1024.0);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 清理过期图像
        /// </summary>
        public static void CleanupOldImages(int maxDays = 30)
        {
            try
            {
                if (!Directory.Exists(_imagePath)) return;

                DateTime cutoffDate = DateTime.Now.AddDays(-maxDays);

                foreach (var dir in Directory.GetDirectories(_imagePath))
                {
                    string dirName = Path.GetFileName(dir);
                    if (DateTime.TryParseExact(dirName, "yyyyMMdd", null,
                        System.Globalization.DateTimeStyles.None, out DateTime dirDate))
                    {
                        if (dirDate < cutoffDate)
                        {
                            Directory.Delete(dir, true);
                            LogHelper.Info("ImageStorage", $"删除过期图像目录: {dir}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ImageStorage", $"清理图像失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        private static string GetExtension(Common.ImageFormat format)
        {
            switch (format)
            {
                case Common.ImageFormat.Bmp: return ".bmp";
                case Common.ImageFormat.Png: return ".png";
                case Common.ImageFormat.Tiff: return ".tiff";
                default: return ".jpg";
            }
        }
    }
}
