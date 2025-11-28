using System;
using System.Windows.Forms;
using VisionInspectionSystem.Forms;
using VisionInspectionSystem.DAL;

namespace VisionInspectionSystem
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 初始化日志系统
            LogHelper.Initialize();
            LogHelper.Info("System", "程序启动");

            // 初始化配置
            ConfigHelper.Initialize();

            // 显示登录窗口
            LoginForm loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // 登录成功，显示主窗口
                Application.Run(new MainForm());
            }

            LogHelper.Info("System", "程序退出");
        }
    }
}
