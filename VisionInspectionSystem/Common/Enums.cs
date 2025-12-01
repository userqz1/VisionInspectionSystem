namespace VisionInspectionSystem.Common
{
    /// <summary>
    /// 系统运行状态
    /// </summary>
    public enum SystemState
    {
        /// <summary>
        /// 空闲
        /// </summary>
        Idle = 0,

        /// <summary>
        /// 就绪
        /// </summary>
        Ready = 1,

        /// <summary>
        /// 运行中
        /// </summary>
        Running = 2,

        /// <summary>
        /// 暂停
        /// </summary>
        Paused = 3,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 4
    }

    /// <summary>
    /// 相机状态
    /// </summary>
    public enum CameraState
    {
        /// <summary>
        /// 未连接
        /// </summary>
        Disconnected = 0,

        /// <summary>
        /// 已连接
        /// </summary>
        Connected = 1,

        /// <summary>
        /// 采集中
        /// </summary>
        Grabbing = 2,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 3
    }

    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warn = 2,
        Error = 3,
        Fatal = 4
    }

    /// <summary>
    /// 图像格式
    /// </summary>
    public enum ImageFormat
    {
        Bmp,
        Jpg,
        Png,
        Tiff
    }

    /// <summary>
    /// 检测模式
    /// </summary>
    public enum InspectionMode
    {
        /// <summary>
        /// 在线检测
        /// </summary>
        Online = 0,

        /// <summary>
        /// 离线测试
        /// </summary>
        Offline = 1
    }

    /// <summary>
    /// 权限类型
    /// </summary>
    public enum PermissionType
    {
        /// <summary>
        /// 运行检测
        /// </summary>
        RunInspection,

        /// <summary>
        /// 查看结果
        /// </summary>
        ViewResult,

        /// <summary>
        /// 查看统计
        /// </summary>
        ViewStatistics,

        /// <summary>
        /// 相机参数调整
        /// </summary>
        CameraSettings,

        /// <summary>
        /// 版型切换
        /// </summary>
        ModelSwitch,

        /// <summary>
        /// 离线测试
        /// </summary>
        OfflineTest,

        /// <summary>
        /// 通讯配置
        /// </summary>
        CommunicationSettings,

        /// <summary>
        /// 版型编辑
        /// </summary>
        ModelEdit,

        /// <summary>
        /// 用户管理
        /// </summary>
        UserManagement,

        /// <summary>
        /// 系统配置
        /// </summary>
        SystemSettings
    }
}
