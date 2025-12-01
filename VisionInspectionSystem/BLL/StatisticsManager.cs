using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VisionInspectionSystem.Common;
using VisionInspectionSystem.DAL;
using VisionInspectionSystem.Models;

namespace VisionInspectionSystem.BLL
{
    /// <summary>
    /// 统计管理类
    /// </summary>
    public class StatisticsManager
    {
        #region 单例模式

        private static StatisticsManager _instance;
        private static readonly object _lockObj = new object();

        public static StatisticsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new StatisticsManager();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region 构造函数

        private StatisticsManager()
        {
            DataStorage.Initialize();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取实时统计
        /// </summary>
        public RealtimeStatistics GetRealtimeStatistics()
        {
            return InspectionManager.Instance.RealtimeStats;
        }

        /// <summary>
        /// 获取今日统计
        /// </summary>
        public StatisticsData GetTodayStatistics(string modelName = null)
        {
            return GetStatistics(DateTime.Today, DateTime.Today, modelName);
        }

        /// <summary>
        /// 获取日期范围统计
        /// </summary>
        public StatisticsData GetStatistics(DateTime startDate, DateTime endDate, string modelName = null)
        {
            return DataStorage.GetSummaryStatistics(startDate, endDate, modelName);
        }

        /// <summary>
        /// 获取详细统计列表
        /// </summary>
        public List<StatisticsData> GetStatisticsList(DateTime startDate, DateTime endDate, string modelName = null)
        {
            return DataStorage.GetStatistics(startDate, endDate, modelName);
        }

        /// <summary>
        /// 获取最近N天的统计
        /// </summary>
        public List<StatisticsData> GetRecentStatistics(int days, string modelName = null)
        {
            DateTime endDate = DateTime.Today;
            DateTime startDate = endDate.AddDays(-(days - 1));
            return GetStatisticsList(startDate, endDate, modelName);
        }

        /// <summary>
        /// 导出统计到CSV
        /// </summary>
        public bool ExportToCsv(List<StatisticsData> dataList, string filePath)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                // 写入标题行
                sb.AppendLine("日期,版型,总数,OK数,NG数,良率(%),平均耗时(ms),最大耗时(ms),最小耗时(ms)");

                // 写入数据行
                foreach (var data in dataList)
                {
                    sb.AppendLine($"{data.Date:yyyy-MM-dd},{data.ModelName},{data.TotalCount},{data.OkCount},{data.NgCount},{data.YieldRate:F2},{data.AverageRunTime:F1},{data.MaxRunTime:F1},{data.MinRunTime:F1}");
                }

                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                LogHelper.Info("Statistics", $"导出统计到: {filePath}");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Statistics", $"导出统计失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 导出检测结果到CSV
        /// </summary>
        public bool ExportResultsToCsv(DateTime date, string filePath)
        {
            try
            {
                var results = DataStorage.GetInspectionResults(date);
                if (results.Count == 0)
                {
                    return false;
                }

                StringBuilder sb = new StringBuilder();

                // 写入标题行
                sb.AppendLine("时间,版型,结果,X,Y,角度,分数,耗时(ms)");

                // 写入数据行
                foreach (var result in results)
                {
                    sb.AppendLine($"{result.TimeStamp:yyyy-MM-dd HH:mm:ss.fff},{result.ModelName},{(result.IsPass ? "OK" : "NG")},{result.X:F3},{result.Y:F3},{result.Angle:F3},{result.Score:F2},{result.RunTime:F1}");
                }

                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                LogHelper.Info("Statistics", $"导出检测结果到: {filePath}");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Statistics", $"导出检测结果失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 清理过期数据
        /// </summary>
        public void CleanupOldData(int maxDays = 30)
        {
            DataStorage.CleanupOldData(maxDays);
            ImageStorage.CleanupOldImages(maxDays);
            LogHelper.CleanupOldLogs(maxDays);
        }

        /// <summary>
        /// 获取存储空间使用情况
        /// </summary>
        public StorageInfo GetStorageInfo()
        {
            return new StorageInfo
            {
                ImageStorageSize = ImageStorage.GetStorageSize(),
                TodayImageCount = ImageStorage.GetTodayImageCount()
            };
        }

        #endregion
    }

    /// <summary>
    /// 存储信息
    /// </summary>
    public class StorageInfo
    {
        /// <summary>
        /// 图像存储大小（MB）
        /// </summary>
        public double ImageStorageSize { get; set; }

        /// <summary>
        /// 今日图像数量
        /// </summary>
        public int TodayImageCount { get; set; }
    }
}
