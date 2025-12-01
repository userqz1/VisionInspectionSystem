using System;
using System.Collections.Generic;
using System.Linq;
using VisionInspectionSystem.Common;
using VisionInspectionSystem.DAL;
using VisionInspectionSystem.Models;

namespace VisionInspectionSystem.BLL
{
    /// <summary>
    /// 权限管理类
    /// </summary>
    public class AuthenticationManager
    {
        #region 单例模式

        private static AuthenticationManager _instance;
        private static readonly object _lockObj = new object();

        public static AuthenticationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AuthenticationManager();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region 私有字段

        private List<UserInfo> _users;

        #endregion

        #region 公共属性

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public UserInfo CurrentUser => GlobalVariables.CurrentUser;

        /// <summary>
        /// 是否已登录
        /// </summary>
        public bool IsLoggedIn => GlobalVariables.IsLoggedIn;

        #endregion

        #region 事件

        /// <summary>
        /// 用户登录事件
        /// </summary>
        public event EventHandler<UserInfo> UserLoggedIn;

        /// <summary>
        /// 用户注销事件
        /// </summary>
        public event EventHandler UserLoggedOut;

        #endregion

        #region 构造函数

        private AuthenticationManager()
        {
            LoadUsers();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 用户登录
        /// </summary>
        public bool Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    LogHelper.Warn("Auth", "用户名或密码为空");
                    return false;
                }

                string encryptedPassword = Utils.MD5Encrypt(password);
                UserInfo user = _users.FirstOrDefault(u =>
                    u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                    u.Password == encryptedPassword);

                if (user == null)
                {
                    LogHelper.Warn("Auth", $"登录失败: {username}");
                    return false;
                }

                // 更新最后登录时间
                user.LastLoginTime = DateTime.Now;
                SaveUsers();

                // 设置当前用户
                GlobalVariables.CurrentUser = user;

                LogHelper.Info("Auth", $"用户登录成功: {username} ({user.Level})");
                UserLoggedIn?.Invoke(this, user);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Auth", $"登录异常: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 用户注销
        /// </summary>
        public void Logout()
        {
            if (GlobalVariables.CurrentUser != null)
            {
                LogHelper.Info("Auth", $"用户注销: {GlobalVariables.CurrentUser.UserName}");
                GlobalVariables.CurrentUser = null;
                UserLoggedOut?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        public bool HasPermission(PermissionType permission)
        {
            return GlobalVariables.HasPermission(permission);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        public bool AddUser(string username, string password, UserLevel level, string remark = "")
        {
            try
            {
                if (!HasPermission(PermissionType.UserManagement))
                {
                    LogHelper.Warn("Auth", "没有用户管理权限");
                    return false;
                }

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return false;
                }

                // 检查用户名是否已存在
                if (_users.Any(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)))
                {
                    LogHelper.Warn("Auth", $"用户名已存在: {username}");
                    return false;
                }

                UserInfo newUser = new UserInfo
                {
                    UserName = username,
                    Password = Utils.MD5Encrypt(password),
                    Level = level,
                    Remark = remark,
                    CreateTime = DateTime.Now,
                    LastLoginTime = DateTime.Now
                };

                _users.Add(newUser);
                SaveUsers();

                LogHelper.Info("Auth", $"添加用户: {username} ({level})");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Auth", $"添加用户失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        public bool DeleteUser(string username)
        {
            try
            {
                if (!HasPermission(PermissionType.UserManagement))
                {
                    return false;
                }

                // 不能删除当前登录用户
                if (CurrentUser?.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) == true)
                {
                    LogHelper.Warn("Auth", "不能删除当前登录用户");
                    return false;
                }

                // 不能删除最后一个管理员
                var user = _users.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (user?.Level == UserLevel.Admin)
                {
                    int adminCount = _users.Count(u => u.Level == UserLevel.Admin);
                    if (adminCount <= 1)
                    {
                        LogHelper.Warn("Auth", "不能删除最后一个管理员");
                        return false;
                    }
                }

                int removed = _users.RemoveAll(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (removed > 0)
                {
                    SaveUsers();
                    LogHelper.Info("Auth", $"删除用户: {username}");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Auth", $"删除用户失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        public bool UpdateUser(string username, string newPassword = null, UserLevel? newLevel = null, string remark = null)
        {
            try
            {
                if (!HasPermission(PermissionType.UserManagement))
                {
                    return false;
                }

                var user = _users.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (user == null)
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(newPassword))
                {
                    user.Password = Utils.MD5Encrypt(newPassword);
                }

                if (newLevel.HasValue)
                {
                    // 不能降级最后一个管理员
                    if (user.Level == UserLevel.Admin && newLevel.Value != UserLevel.Admin)
                    {
                        int adminCount = _users.Count(u => u.Level == UserLevel.Admin);
                        if (adminCount <= 1)
                        {
                            LogHelper.Warn("Auth", "不能降级最后一个管理员");
                            return false;
                        }
                    }
                    user.Level = newLevel.Value;
                }

                if (remark != null)
                {
                    user.Remark = remark;
                }

                SaveUsers();
                LogHelper.Info("Auth", $"修改用户: {username}");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Auth", $"修改用户失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 修改密码（用户自己修改）
        /// </summary>
        public bool ChangePassword(string oldPassword, string newPassword)
        {
            try
            {
                if (CurrentUser == null)
                {
                    return false;
                }

                if (Utils.MD5Encrypt(oldPassword) != CurrentUser.Password)
                {
                    LogHelper.Warn("Auth", "原密码错误");
                    return false;
                }

                CurrentUser.Password = Utils.MD5Encrypt(newPassword);
                SaveUsers();

                LogHelper.Info("Auth", $"用户修改密码: {CurrentUser.UserName}");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Auth", $"修改密码失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        public List<UserInfo> GetAllUsers()
        {
            return _users.ToList();
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        public UserInfo GetUser(string username)
        {
            return _users.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 刷新用户列表
        /// </summary>
        public void RefreshUsers()
        {
            LoadUsers();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 加载用户列表
        /// </summary>
        private void LoadUsers()
        {
            _users = ConfigHelper.LoadUsers();
        }

        /// <summary>
        /// 保存用户列表
        /// </summary>
        private void SaveUsers()
        {
            ConfigHelper.SaveUsers(_users);
        }

        #endregion
    }
}
