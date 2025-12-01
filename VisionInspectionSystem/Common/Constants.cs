namespace VisionInspectionSystem.Common
{
    /// <summary>
    /// 系统常量定义
    /// </summary>
    public static class Constants
    {
        #region 系统信息
        public const string APP_NAME = "视觉检测系统";
        public const string APP_VERSION = "1.0.0";
        public const string APP_COMPANY = "";
        #endregion

        #region 路径常量
        public const string CONFIG_FOLDER = "Config";
        public const string DATA_FOLDER = "Data";
        public const string LOG_FOLDER = "Data\\Logs";
        public const string IMAGE_FOLDER = "Data\\Images";
        public const string STATISTICS_FOLDER = "Data\\Statistics";
        public const string MODEL_FOLDER = "Models";

        public const string SYSTEM_CONFIG_FILE = "Config\\SystemConfig.xml";
        public const string CAMERA_CONFIG_FILE = "Config\\CameraConfig.xml";
        public const string COMM_CONFIG_FILE = "Config\\CommunicationConfig.xml";
        public const string USER_CONFIG_FILE = "Config\\UserConfig.xml";
        #endregion

        #region 默认用户
        public const string DEFAULT_ADMIN_USER = "admin";
        public const string DEFAULT_ADMIN_PASSWORD = "admin123";
        public const string DEFAULT_ENGINEER_USER = "engineer";
        public const string DEFAULT_ENGINEER_PASSWORD = "eng123";
        public const string DEFAULT_OPERATOR_USER = "operator";
        public const string DEFAULT_OPERATOR_PASSWORD = "op123";
        #endregion

        #region 通讯协议
        public const string CMD_TRIGGER = "T1";
        public const string CMD_HEARTBEAT = "H1";
        public const string CMD_GET_RESULT = "G1";
        public const string RESP_OK = "OK";
        public const string RESP_NG = "NG";
        public const char CMD_SEPARATOR = ',';
        #endregion

        #region 相机默认参数
        public const double DEFAULT_EXPOSURE_TIME = 10000;  // 微秒
        public const double DEFAULT_GAIN = 0;               // dB
        public const double MIN_EXPOSURE_TIME = 100;
        public const double MAX_EXPOSURE_TIME = 1000000;
        public const double MIN_GAIN = 0;
        public const double MAX_GAIN = 24;
        #endregion

        #region 界面相关
        public const int MAX_LOG_LINES = 1000;
        public const int MAX_RECENT_RESULTS = 100;
        public const int IMAGE_DISPLAY_WIDTH = 640;
        public const int IMAGE_DISPLAY_HEIGHT = 480;
        #endregion

        #region 文件扩展名
        public const string VPP_EXTENSION = ".vpp";
        public const string XML_EXTENSION = ".xml";
        public const string LOG_EXTENSION = ".log";
        #endregion

        #region 日期格式
        public const string DATE_FORMAT = "yyyy-MM-dd";
        public const string TIME_FORMAT = "HH:mm:ss";
        public const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string TIMESTAMP_FORMAT = "yyyyMMdd_HHmmss_fff";
        #endregion
    }
}
