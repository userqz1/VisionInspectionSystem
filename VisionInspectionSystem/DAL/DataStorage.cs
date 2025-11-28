using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VisionInspectionSystem.Common;
using VisionInspectionSystem.Models;

namespace VisionInspectionSystem.DAL
{
    /// <summary>
    /// 数据存储类
    /// </summary>
    public static class DataStorage
    {
        private static string _statisticsPath;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            _statisticsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.STATISTICS_FOLDER);
            Utils.EnsureDirectoryExists(_statisticsPath);
        }

        #region 统计数据

        /// <summary>
        /// 保存统计数据
        /// </summary>
        public static bool SaveStatistics(StatisticsData data)
        {
            if (data == null) return false;

            string fileName = $"{data.Date:yyyyMMdd}_{data.ModelName}.xml";
            string filePath = Path.Combine(_statisticsPath, fileName);
            return ConfigHelper.SaveToXml(data, filePath);
        }

        /// <summary>
        /// 加载统计数据
        /// </summary>
        public static StatisticsData LoadStatistics(DateTime date, string modelName)
        {
            string fileName = $"{date:yyyyMMdd}_{modelName}.xml";
            string filePath = Path.Combine(_statisticsPath, fileName);
            return ConfigHelper.LoadFromXml<StatisticsData>(filePath);
        }

        /// <summary>
        /// 获取日期范围内的统计数据
        /// </summary>
        public static List<StatisticsData> GetStatistics(DateTime startDate, DateTime endDate, string modelName = null)
        {
            List<StatisticsData> result = new List<StatisticsData>();

            try
            {
                if (!Directory.Exists(_statisticsPath)) return result;

                var files = Directory.GetFiles(_statisticsPath, "*.xml");
                foreach (var file in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    string[] parts = fileName.Split('_');

                    if (parts.Length >= 2)
                    {
                        if (DateTime.TryParseExact(parts[0], "yyyyMMdd", null,
                            System.Globalization.DateTimeStyles.None, out DateTime fileDate))
                        {
                            if (fileDate >= startDate && fileDate <= endDate)
                            {
                                string fileModel = string.Join("_", parts.Skip(1));
                                if (string.IsNullOrEmpty(modelName) || fileModel == modelName)
                                {
                                    var data = ConfigHelper.LoadFromXml<StatisticsData>(file);
                                    if (data != null)
                                    {
                                        result.Add(data);
                                    }
                                }
                            }
                        }
                    }
                }

                result = result.OrderBy(x => x.Date).ThenBy(x => x.ModelName).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error("DataStorage", $"获取统计数据失败: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// 获取汇总统计
        /// </summary>
        public static StatisticsData GetSummaryStatistics(DateTime startDate, DateTime endDate, string modelName = null)
        {
            var dataList = GetStatistics(startDate, endDate, modelName);
            if (dataList.Count == 0) return null;

            StatisticsData summary = new StatisticsData
            {
                Date = startDate,
                ModelName = modelName ?? "All",
                TotalCount = dataList.Sum(x => x.TotalCount),
                OkCount = dataList.Sum(x => x.OkCount),
                NgCount = dataList.Sum(x => x.NgCount),
                AverageRunTime = dataList.Average(x => x.AverageRunTime),
                MaxRunTime = dataList.Max(x => x.MaxRunTime),
                MinRunTime = dataList.Min(x => x.MinRunTime),
                StartTime = dataList.Min(x => x.StartTime),
                EndTime = dataList.Max(x => x.EndTime)
            };

            return summary;
        }

        #endregion

        #region 检测结果记录

        /// <summary>
        /// 保存检测结果记录
        /// </summary>
        public static bool SaveInspectionResult(InspectionResult result)
        {
            if (result == null) return false;

            try
            {
                string datePath = Path.Combine(_statisticsPath, "Results", result.TimeStamp.ToString("yyyyMMdd"));
                Utils.EnsureDirectoryExists(datePath);

                string fileName = $"{result.TimeStamp:HHmmss_fff}_{(result.IsPass ? "OK" : "NG")}.xml";
                string filePath = Path.Combine(datePath, fileName);
                return ConfigHelper.SaveToXml(result, filePath);
            }
            catch (Exception ex)
            {
                LogHelper.Error("DataStorage", $"保存检测结果失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取检测结果记录
        /// </summary>
        public static List<InspectionResult> GetInspectionResults(DateTime date, int maxCount = 100)
        {
            List<InspectionResult> results = new List<InspectionResult>();

            try
            {
                string datePath = Path.Combine(_statisticsPath, "Results", date.ToString("yyyyMMdd"));
                if (!Directory.Exists(datePath)) return results;

                var files = Directory.GetFiles(datePath, "*.xml")
                    .OrderByDescending(x => x)
                    .Take(maxCount);

                foreach (var file in files)
                {
                    var result = ConfigHelper.LoadFromXml<InspectionResult>(file);
                    if (result != null)
                    {
                        results.Add(result);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("DataStorage", $"获取检测结果失败: {ex.Message}");
            }

            return results;
        }

        #endregion

        #region 数据清理

        /// <summary>
        /// 清理过期数据
        /// </summary>
        public static void CleanupOldData(int maxDays = 30)
        {
            try
            {
                // 清理统计文件
                Utils.CleanupOldFiles(_statisticsPath, maxDays, "*.xml");

                // 清理结果记录目录
                string resultsPath = Path.Combine(_statisticsPath, "Results");
                if (Directory.Exists(resultsPath))
                {
                    DateTime cutoffDate = DateTime.Now.AddDays(-maxDays);
                    foreach (var dir in Directory.GetDirectories(resultsPath))
                    {
                        string dirName = Path.GetFileName(dir);
                        if (DateTime.TryParseExact(dirName, "yyyyMMdd", null,
                            System.Globalization.DateTimeStyles.None, out DateTime dirDate))
                        {
                            if (dirDate < cutoffDate)
                            {
                                Directory.Delete(dir, true);
                            }
                        }
                    }
                }

                LogHelper.Info("DataStorage", $"清理 {maxDays} 天前的数据完成");
            }
            catch (Exception ex)
            {
                LogHelper.Error("DataStorage", $"清理数据失败: {ex.Message}");
            }
        }

        #endregion
    }
}
