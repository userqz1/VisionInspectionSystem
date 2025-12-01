using System;
using VisionInspectionSystem.Models;

namespace VisionInspectionSystem.Common
{
    /// <summary>
    /// 全局变量类
    /// </summary>
    public static class GlobalVariables
    {
        /// <summary>
        /// 应用程序根路径
        /// </summary>
        public static string AppPath { get; set; }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public static UserInfo CurrentUser { get; set; }

        /// <summary>
        /// 当前版型
        /// </summary>
        public static ModelConfig CurrentModel { get; set; }

        /// <summary>
        /// 系统运行状态
        /// </summary>
        public static SystemState SystemState { get; set; } = SystemState.Idle;

        /// <summary>
        /// 是否已登录
        /// </summary>
        public static bool IsLoggedIn => CurrentUser != null;

        /// <summary>
        /// 是否为管理员
        /// </summary>
        public static bool IsAdmin => CurrentUser?.Level == UserLevel.Admin;

        /// <summary>
        /// 是否为工程师或更高权限
        /// </summary>
        public static bool IsEngineerOrAbove => CurrentUser?.Level >= UserLevel.Engineer;

        /// <summary>
        /// 初始化全局变量
        /// </summary>
        public static void Initialize()
        {
            AppPath = AppDomain.CurrentDomain.BaseDirectory;
            SystemState = SystemState.Idle;
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        public static bool HasPermission(PermissionType permission)
        {
            if (CurrentUser == null) return false;

            switch (permission)
            {
                case PermissionType.RunInspection:
                case PermissionType.ViewResult:
                case PermissionType.ViewStatistics:
                    return true; // 所有用户都可以

                case PermissionType.CameraSettings:
                case PermissionType.ModelSwitch:
                case PermissionType.OfflineTest:
                case PermissionType.CommunicationSettings:
                    return CurrentUser.Level >= UserLevel.Engineer;

                case PermissionType.ModelEdit:
                case PermissionType.UserManagement:
                case PermissionType.SystemSettings:
                    return CurrentUser.Level >= UserLevel.Admin;

                default:
                    return false;
            }
        }

        /// <summary>
        /// 获取完整路径
        /// </summary>
        public static string GetFullPath(string relativePath)
        {
            return System.IO.Path.Combine(AppPath, relativePath);
        }
    }
}
