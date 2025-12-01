using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using VisionInspectionSystem.Common;
using VisionInspectionSystem.Models;

namespace VisionInspectionSystem.DAL
{
    /// <summary>
    /// 配置助手类
    /// </summary>
    public static class ConfigHelper
    {
        private static string _configPath;

        /// <summary>
        /// 初始化配置系统
        /// </summary>
        public static void Initialize()
        {
            _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.CONFIG_FOLDER);
            Utils.EnsureDirectoryExists(_configPath);

            // 创建默认配置文件
            CreateDefaultConfigs();
        }

        #region 通用方法

        /// <summary>
        /// 保存对象到XML文件
        /// </summary>
        public static bool SaveToXml<T>(T obj, string filePath)
        {
            try
            {
                Utils.EnsureDirectoryExists(Path.GetDirectoryName(filePath));

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, obj);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Config", $"保存配置失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 从XML文件加载对象
        /// </summary>
        public static T LoadFromXml<T>(string filePath) where T : class, new()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new T();
                }

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return serializer.Deserialize(reader) as T;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Config", $"加载配置失败: {ex.Message}");
                return new T();
            }
        }

        #endregion

        #region 用户配置

        /// <summary>
        /// 保存用户列表
        /// </summary>
        public static bool SaveUsers(List<UserInfo> users)
        {
            string filePath = GetFullPath(Constants.USER_CONFIG_FILE);
            return SaveToXml(users, filePath);
        }

        /// <summary>
        /// 加载用户列表
        /// </summary>
        public static List<UserInfo> LoadUsers()
        {
            string filePath = GetFullPath(Constants.USER_CONFIG_FILE);
            var users = LoadFromXml<List<UserInfo>>(filePath);
            if (users == null || users.Count == 0)
            {
                users = CreateDefaultUsers();
                SaveUsers(users);
            }
            return users;
        }

        /// <summary>
        /// 创建默认用户
        /// </summary>
        private static List<UserInfo> CreateDefaultUsers()
        {
            return new List<UserInfo>
            {
                new UserInfo(Constants.DEFAULT_ADMIN_USER,
                    Utils.MD5Encrypt(Constants.DEFAULT_ADMIN_PASSWORD),
                    UserLevel.Admin),
                new UserInfo(Constants.DEFAULT_ENGINEER_USER,
                    Utils.MD5Encrypt(Constants.DEFAULT_ENGINEER_PASSWORD),
                    UserLevel.Engineer),
                new UserInfo(Constants.DEFAULT_OPERATOR_USER,
                    Utils.MD5Encrypt(Constants.DEFAULT_OPERATOR_PASSWORD),
                    UserLevel.Operator)
            };
        }

        #endregion

        #region 相机配置

        /// <summary>
        /// 保存相机配置
        /// </summary>
        public static bool SaveCameraConfig(CameraConfig config)
        {
            string filePath = GetFullPath(Constants.CAMERA_CONFIG_FILE);
            return SaveToXml(config, filePath);
        }

        /// <summary>
        /// 加载相机配置
        /// </summary>
        public static CameraConfig LoadCameraConfig()
        {
            string filePath = GetFullPath(Constants.CAMERA_CONFIG_FILE);
            return LoadFromXml<CameraConfig>(filePath);
        }

        #endregion

        #region 通讯配置

        /// <summary>
        /// 保存通讯配置
        /// </summary>
        public static bool SaveCommunicationConfig(CommunicationConfig config)
        {
            string filePath = GetFullPath(Constants.COMM_CONFIG_FILE);
            return SaveToXml(config, filePath);
        }

        /// <summary>
        /// 加载通讯配置
        /// </summary>
        public static CommunicationConfig LoadCommunicationConfig()
        {
            string filePath = GetFullPath(Constants.COMM_CONFIG_FILE);
            var config = LoadFromXml<CommunicationConfig>(filePath);

            // 验证并修复无效的配置值
            if (config.LocalPort <= 0 || config.LocalPort > 65535)
            {
                config.LocalPort = 8000;
            }
            if (config.RemotePort <= 0 || config.RemotePort > 65535)
            {
                config.RemotePort = 8001;
            }
            if (string.IsNullOrEmpty(config.LocalIP))
            {
                config.LocalIP = "127.0.0.1";
            }
            if (string.IsNullOrEmpty(config.RemoteIP))
            {
                config.RemoteIP = "127.0.0.1";
            }
            if (config.Timeout <= 0)
            {
                config.Timeout = 5000;
            }
            if (config.ReconnectInterval <= 0)
            {
                config.ReconnectInterval = 3000;
            }

            return config;
        }

        #endregion

        #region 版型配置

        /// <summary>
        /// 保存版型配置
        /// </summary>
        public static bool SaveModelConfig(ModelConfig config)
        {
            if (config == null || string.IsNullOrEmpty(config.ModelName))
                return false;

            string modelPath = Path.Combine(GetFullPath(Constants.MODEL_FOLDER), config.ModelName);
            Utils.EnsureDirectoryExists(modelPath);

            string filePath = Path.Combine(modelPath, "ModelConfig.xml");
            config.ModelPath = modelPath;
            return SaveToXml(config, filePath);
        }

        /// <summary>
        /// 加载版型配置
        /// </summary>
        public static ModelConfig LoadModelConfig(string modelName)
        {
            string filePath = Path.Combine(GetFullPath(Constants.MODEL_FOLDER), modelName, "ModelConfig.xml");
            return LoadFromXml<ModelConfig>(filePath);
        }

        /// <summary>
        /// 获取所有版型名称
        /// </summary>
        public static List<string> GetAllModelNames()
        {
            List<string> models = new List<string>();
            string modelPath = GetFullPath(Constants.MODEL_FOLDER);

            if (Directory.Exists(modelPath))
            {
                foreach (string dir in Directory.GetDirectories(modelPath))
                {
                    string configFile = Path.Combine(dir, "ModelConfig.xml");
                    if (File.Exists(configFile))
                    {
                        models.Add(Path.GetFileName(dir));
                    }
                }
            }

            return models;
        }

        /// <summary>
        /// 删除版型
        /// </summary>
        public static bool DeleteModel(string modelName)
        {
            try
            {
                string modelPath = Path.Combine(GetFullPath(Constants.MODEL_FOLDER), modelName);
                if (Directory.Exists(modelPath))
                {
                    Directory.Delete(modelPath, true);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Config", $"删除版型失败: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取完整路径
        /// </summary>
        private static string GetFullPath(string relativePath)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        }

        /// <summary>
        /// 创建默认配置文件
        /// </summary>
        private static void CreateDefaultConfigs()
        {
            // 用户配置
            string userConfigPath = GetFullPath(Constants.USER_CONFIG_FILE);
            if (!File.Exists(userConfigPath))
            {
                SaveUsers(CreateDefaultUsers());
            }

            // 通讯配置
            string commConfigPath = GetFullPath(Constants.COMM_CONFIG_FILE);
            if (!File.Exists(commConfigPath))
            {
                SaveCommunicationConfig(new CommunicationConfig());
            }

            // 相机配置
            string cameraConfigPath = GetFullPath(Constants.CAMERA_CONFIG_FILE);
            if (!File.Exists(cameraConfigPath))
            {
                SaveCameraConfig(new CameraConfig());
            }

            // 创建必要的目录
            Utils.EnsureDirectoryExists(GetFullPath(Constants.LOG_FOLDER));
            Utils.EnsureDirectoryExists(GetFullPath(Constants.IMAGE_FOLDER));
            Utils.EnsureDirectoryExists(GetFullPath(Constants.STATISTICS_FOLDER));
            Utils.EnsureDirectoryExists(GetFullPath(Constants.MODEL_FOLDER));
        }

        #endregion
    }
}
