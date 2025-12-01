using System;
using System.Collections.Generic;

namespace VisionInspectionSystem.Models
{
    /// <summary>
    /// 统计数据类
    /// </summary>
    [Serializable]
    public class StatisticsData
    {
        /// <summary>
        /// 统计日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 版型名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 总检测数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// OK数量
        /// </summary>
        public int OkCount { get; set; }

        /// <summary>
        /// NG数量
        /// </summary>
        public int NgCount { get; set; }

        /// <summary>
        /// 良率
        /// </summary>
        public double YieldRate => TotalCount > 0 ? (double)OkCount / TotalCount * 100 : 0;

        /// <summary>
        /// 平均检测时间（毫秒）
        /// </summary>
        public double AverageRunTime { get; set; }

        /// <summary>
        /// 最大检测时间
        /// </summary>
        public double MaxRunTime { get; set; }

        /// <summary>
        /// 最小检测时间
        /// </summary>
        public double MinRunTime { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 运行时长（秒）
        /// </summary>
        public double RunDuration => (EndTime - StartTime).TotalSeconds;

        public StatisticsData()
        {
            Date = DateTime.Today;
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
        }

        public StatisticsData(string modelName) : this()
        {
            ModelName = modelName;
        }

        /// <summary>
        /// 添加检测结果
        /// </summary>
        public void AddResult(InspectionResult result)
        {
            TotalCount++;
            if (result.IsPass)
                OkCount++;
            else
                NgCount++;

            // 更新运行时间统计
            if (TotalCount == 1)
            {
                AverageRunTime = result.RunTime;
                MaxRunTime = result.RunTime;
                MinRunTime = result.RunTime;
            }
            else
            {
                AverageRunTime = ((AverageRunTime * (TotalCount - 1)) + result.RunTime) / TotalCount;
                if (result.RunTime > MaxRunTime) MaxRunTime = result.RunTime;
                if (result.RunTime < MinRunTime) MinRunTime = result.RunTime;
            }

            EndTime = DateTime.Now;
        }

        /// <summary>
        /// 重置统计
        /// </summary>
        public void Reset()
        {
            TotalCount = 0;
            OkCount = 0;
            NgCount = 0;
            AverageRunTime = 0;
            MaxRunTime = 0;
            MinRunTime = 0;
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd} {ModelName}: Total={TotalCount}, OK={OkCount}, NG={NgCount}, Yield={YieldRate:F2}%";
        }
    }

    /// <summary>
    /// 实时统计数据（当前批次）
    /// </summary>
    public class RealtimeStatistics
    {
        /// <summary>
        /// 总检测数
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// OK数量
        /// </summary>
        public int OkCount { get; private set; }

        /// <summary>
        /// NG数量
        /// </summary>
        public int NgCount { get; private set; }

        /// <summary>
        /// 良率
        /// </summary>
        public double YieldRate => TotalCount > 0 ? (double)OkCount / TotalCount * 100 : 0;

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// 运行时长
        /// </summary>
        public TimeSpan RunDuration => DateTime.Now - StartTime;

        /// <summary>
        /// 最近的检测结果列表
        /// </summary>
        public List<InspectionResult> RecentResults { get; private set; }

        /// <summary>
        /// 最大保存结果数量
        /// </summary>
        public int MaxRecentResults { get; set; } = 100;

        public RealtimeStatistics()
        {
            StartTime = DateTime.Now;
            RecentResults = new List<InspectionResult>();
        }

        /// <summary>
        /// 添加检测结果
        /// </summary>
        public void AddResult(InspectionResult result)
        {
            TotalCount++;
            if (result.IsPass)
                OkCount++;
            else
                NgCount++;

            RecentResults.Add(result);
            if (RecentResults.Count > MaxRecentResults)
            {
                RecentResults.RemoveAt(0);
            }
        }

        /// <summary>
        /// 重置统计
        /// </summary>
        public void Reset()
        {
            TotalCount = 0;
            OkCount = 0;
            NgCount = 0;
            StartTime = DateTime.Now;
            RecentResults.Clear();
        }
    }
}
