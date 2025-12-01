using System;
using System.Collections.Generic;
using VisionInspectionSystem.Common;
using VisionInspectionSystem.DAL;
using VisionInspectionSystem.Models;

namespace VisionInspectionSystem.BLL
{
    /// <summary>
    /// 版型管理类
    /// </summary>
    public class ModelManager
    {
        #region 单例模式

        private static ModelManager _instance;
        private static readonly object _lockObj = new object();

        public static ModelManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new ModelManager();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region 公共属性

        /// <summary>
        /// 当前版型
        /// </summary>
        public ModelConfig CurrentModel => GlobalVariables.CurrentModel;

        #endregion

        #region 事件

        /// <summary>
        /// 版型切换事件
        /// </summary>
        public event EventHandler<ModelConfig> ModelChanged;

        #endregion

        #region 构造函数

        private ModelManager()
        {
            ModelStorage.Initialize();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 创建版型
        /// </summary>
        public bool CreateModel(string modelName, string description = "")
        {
            if (!GlobalVariables.HasPermission(PermissionType.ModelEdit))
            {
                LogHelper.Warn("ModelManager", "没有版型编辑权限");
                return false;
            }

            string creator = GlobalVariables.CurrentUser?.UserName ?? "";
            return ModelStorage.CreateModel(modelName, description, creator);
        }

        /// <summary>
        /// 删除版型
        /// </summary>
        public bool DeleteModel(string modelName)
        {
            if (!GlobalVariables.HasPermission(PermissionType.ModelEdit))
            {
                LogHelper.Warn("ModelManager", "没有版型编辑权限");
                return false;
            }

            // 不能删除当前正在使用的版型
            if (CurrentModel?.ModelName == modelName)
            {
                LogHelper.Warn("ModelManager", "不能删除当前正在使用的版型");
                return false;
            }

            return ModelStorage.DeleteModel(modelName);
        }

        /// <summary>
        /// 复制版型
        /// </summary>
        public bool CopyModel(string sourceModelName, string newModelName)
        {
            if (!GlobalVariables.HasPermission(PermissionType.ModelEdit))
            {
                return false;
            }

            return ModelStorage.CopyModel(sourceModelName, newModelName);
        }

        /// <summary>
        /// 重命名版型
        /// </summary>
        public bool RenameModel(string oldName, string newName)
        {
            if (!GlobalVariables.HasPermission(PermissionType.ModelEdit))
            {
                return false;
            }

            bool result = ModelStorage.RenameModel(oldName, newName);

            // 如果重命名的是当前版型，更新引用
            if (result && CurrentModel?.ModelName == oldName)
            {
                GlobalVariables.CurrentModel = ModelStorage.LoadModelConfig(newName);
            }

            return result;
        }

        /// <summary>
        /// 切换版型
        /// </summary>
        public bool SwitchModel(string modelName)
        {
            if (!GlobalVariables.HasPermission(PermissionType.ModelSwitch))
            {
                LogHelper.Warn("ModelManager", "没有版型切换权限");
                return false;
            }

            ModelConfig config = ModelStorage.LoadModelConfig(modelName);
            if (config == null)
            {
                LogHelper.Error("ModelManager", $"版型不存在: {modelName}");
                return false;
            }

            // 加载版型到检测管理器
            if (InspectionManager.Instance.LoadModel(config))
            {
                OnModelChanged(config);
                LogHelper.Info("ModelManager", $"切换版型: {modelName}");
                return true;
            }

            return false;
        }

        /// <summary>
        /// 保存版型配置
        /// </summary>
        public bool SaveModelConfig(ModelConfig config)
        {
            if (!GlobalVariables.HasPermission(PermissionType.ModelEdit))
            {
                return false;
            }

            return ModelStorage.SaveModelConfig(config);
        }

        /// <summary>
        /// 保存当前版型配置
        /// </summary>
        public bool SaveCurrentModel()
        {
            if (CurrentModel == null) return false;

            // 更新相机参数
            if (CameraManager.Instance.IsConnected)
            {
                CurrentModel.CameraSettings = CameraManager.Instance.Camera.SaveParameters();
            }

            return SaveModelConfig(CurrentModel);
        }

        /// <summary>
        /// 加载版型配置
        /// </summary>
        public ModelConfig LoadModelConfig(string modelName)
        {
            return ModelStorage.LoadModelConfig(modelName);
        }

        /// <summary>
        /// 获取所有版型
        /// </summary>
        public List<ModelConfig> GetAllModels()
        {
            return ModelStorage.GetAllModels();
        }

        /// <summary>
        /// 获取所有版型名称
        /// </summary>
        public List<string> GetAllModelNames()
        {
            return ModelStorage.GetAllModelNames();
        }

        /// <summary>
        /// 检查版型是否存在
        /// </summary>
        public bool ModelExists(string modelName)
        {
            return ModelStorage.ModelExists(modelName);
        }

        #endregion

        #region 事件触发

        private void OnModelChanged(ModelConfig config)
        {
            ModelChanged?.Invoke(this, config);
        }

        #endregion
    }
}
