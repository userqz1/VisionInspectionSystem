using System;

namespace VisionInspectionSystem.Models
{
    /// <summary>
    /// 用户信息类
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码（加密存储）
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户等级
        /// </summary>
        public UserLevel Level { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public UserInfo()
        {
            CreateTime = DateTime.Now;
            LastLoginTime = DateTime.Now;
            Level = UserLevel.Operator;
        }

        public UserInfo(string userName, string password, UserLevel level)
        {
            UserName = userName;
            Password = password;
            Level = level;
            CreateTime = DateTime.Now;
            LastLoginTime = DateTime.Now;
        }
    }

    /// <summary>
    /// 用户等级枚举
    /// </summary>
    public enum UserLevel
    {
        /// <summary>
        /// 操作员 - 仅查看权限
        /// </summary>
        Operator = 1,

        /// <summary>
        /// 工程师 - 可修改参数
        /// </summary>
        Engineer = 2,

        /// <summary>
        /// 管理员 - 全部权限
        /// </summary>
        Admin = 3
    }
}
