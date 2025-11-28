using System;
using System.Collections.Generic;
using System.IO;
using VisionInspectionSystem.Common;
using VisionInspectionSystem.Models;

namespace VisionInspectionSystem.DAL
{
    /// <summary>
    /// 版型存储类
    /// </summary>
    public static class ModelStorage
    {
        private static string _modelPath;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            _modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.MODEL_FOLDER);
            Utils.EnsureDirectoryExists(_modelPath);
        }

        /// <summary>
        /// 创建新版型
        /// </summary>
        public static bool CreateModel(string modelName, string description = "", string creator = "")
        {
            try
            {
                if (string.IsNullOrEmpty(modelName))
                {
                    LogHelper.Error("ModelStorage", "版型名称不能为空");
                    return false;
                }

                string modelDir = Path.Combine(_modelPath, modelName);
                if (Directory.Exists(modelDir))
                {
                    LogHelper.Error("ModelStorage", $"版型已存在: {modelName}");
                    return false;
                }

                // 创建版型目录
                Directory.CreateDirectory(modelDir);

                // 创建版型配置
                ModelConfig config = new ModelConfig(modelName)
                {
                    Description = description,
                    Creator = creator,
                    ModelPath = modelDir,
                    VppFilePath = Path.Combine(modelDir, "VisionProJob.vpp")
                };

                return SaveModelConfig(config);
            }
            catch (Exception ex)
            {
                LogHelper.Error("ModelStorage", $"创建版型失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 保存版型配置
        /// </summary>
        public static bool SaveModelConfig(ModelConfig config)
        {
            if (config == null || string.IsNullOrEmpty(config.ModelName))
                return false;

            try
            {
                string modelDir = Path.Combine(_modelPath, config.ModelName);
                Utils.EnsureDirectoryExists(modelDir);

                config.ModelPath = modelDir;
                config.UpdateModifyTime();

                string configFile = Path.Combine(modelDir, "ModelConfig.xml");
                return ConfigHelper.SaveToXml(config, configFile);
            }
            catch (Exception ex)
            {
                LogHelper.Error("ModelStorage", $"保存版型配置失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 加载版型配置
        /// </summary>
        public static ModelConfig LoadModelConfig(string modelName)
        {
            try
            {
                string configFile = Path.Combine(_modelPath, modelName, "ModelConfig.xml");
                if (!File.Exists(configFile))
                {
                    return null;
                }

                return ConfigHelper.LoadFromXml<ModelConfig>(configFile);
            }
            catch (Exception ex)
            {
                LogHelper.Error("ModelStorage", $"加载版型配置失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取所有版型
        /// </summary>
        public static List<ModelConfig> GetAllModels()
        {
            List<ModelConfig> models = new List<ModelConfig>();

            try
            {
                if (!Directory.Exists(_modelPath)) return models;

                foreach (string dir in Directory.GetDirectories(_modelPath))
                {
                    string modelName = Path.GetFileName(dir);
                    ModelConfig config = LoadModelConfig(modelName);
                    if (config != null)
                    {
                        models.Add(config);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ModelStorage", $"获取版型列表失败: {ex.Message}");
            }

            return models;
        }

        /// <summary>
        /// 获取所有版型名称
        /// </summary>
        public static List<string> GetAllModelNames()
        {
            List<string> names = new List<string>();

            try
            {
                if (!Directory.Exists(_modelPath)) return names;

                foreach (string dir in Directory.GetDirectories(_modelPath))
                {
                    string configFile = Path.Combine(dir, "ModelConfig.xml");
                    if (File.Exists(configFile))
                    {
                        names.Add(Path.GetFileName(dir));
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ModelStorage", $"获取版型名称列表失败: {ex.Message}");
            }

            return names;
        }

        /// <summary>
        /// 删除版型
        /// </summary>
        public static bool DeleteModel(string modelName)
        {
            try
            {
                string modelDir = Path.Combine(_modelPath, modelName);
                if (!Directory.Exists(modelDir))
                {
                    return false;
                }

                Directory.Delete(modelDir, true);
                LogHelper.Info("ModelStorage", $"删除版型: {modelName}");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("ModelStorage", $"删除版型失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 复制版型
        /// </summary>
        public static bool CopyModel(string sourceModelName, string newModelName)
        {
            try
            {
                string sourceDir = Path.Combine(_modelPath, sourceModelName);
                string destDir = Path.Combine(_modelPath, newModelName);

                if (!Directory.Exists(sourceDir))
                {
                    LogHelper.Error("ModelStorage", $"源版型不存在: {sourceModelName}");
                    return false;
                }

                if (Directory.Exists(destDir))
                {
                    LogHelper.Error("ModelStorage", $"目标版型已存在: {newModelName}");
                    return false;
                }

                // 复制目录
                CopyDirectory(sourceDir, destDir);

                // 更新配置
                ModelConfig config = LoadModelConfig(newModelName);
                if (config != null)
                {
                    config.ModelName = newModelName;
                    config.ModelPath = destDir;
                    config.CreateTime = DateTime.Now;
                    config.VppFilePath = Path.Combine(destDir, "VisionProJob.vpp");
                    SaveModelConfig(config);
                }

                LogHelper.Info("ModelStorage", $"复制版型成功: {sourceModelName} -> {newModelName}");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("ModelStorage", $"复制版型失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 重命名版型
        /// </summary>
        public static bool RenameModel(string oldName, string newName)
        {
            try
            {
                string oldDir = Path.Combine(_modelPath, oldName);
                string newDir = Path.Combine(_modelPath, newName);

                if (!Directory.Exists(oldDir))
                {
                    return false;
                }

                if (Directory.Exists(newDir))
                {
                    LogHelper.Error("ModelStorage", $"版型名称已存在: {newName}");
                    return false;
                }

                Directory.Move(oldDir, newDir);

                // 更新配置
                ModelConfig config = LoadModelConfig(newName);
                if (config != null)
                {
                    config.ModelName = newName;
                    config.ModelPath = newDir;
                    config.VppFilePath = Path.Combine(newDir, "VisionProJob.vpp");
                    SaveModelConfig(config);
                }

                LogHelper.Info("ModelStorage", $"重命名版型: {oldName} -> {newName}");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("ModelStorage", $"重命名版型失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 检查版型是否存在
        /// </summary>
        public static bool ModelExists(string modelName)
        {
            string modelDir = Path.Combine(_modelPath, modelName);
            string configFile = Path.Combine(modelDir, "ModelConfig.xml");
            return File.Exists(configFile);
        }

        /// <summary>
        /// 获取版型VPP文件路径
        /// </summary>
        public static string GetVppFilePath(string modelName)
        {
            ModelConfig config = LoadModelConfig(modelName);
            return config?.VppFilePath;
        }

        /// <summary>
        /// 复制目录
        /// </summary>
        private static void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);

            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            foreach (string dir in Directory.GetDirectories(sourceDir))
            {
                string destSubDir = Path.Combine(destDir, Path.GetFileName(dir));
                CopyDirectory(dir, destSubDir);
            }
        }
    }
}
