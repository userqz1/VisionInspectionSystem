# 视觉检测系统 (Vision Inspection System)

## 项目信息

| 项目 | 说明 |
|------|------|
| 开发语言 | C# WinForms |
| .NET版本 | .NET Framework 4.7.2 |
| 开发平台 | Visual Studio 2022 |
| 运行环境 | Windows 10/11 x64 |
| 相机 | Basler (Pylon SDK) |
| 图像处理 | Cognex VisionPro |

## 项目结构

```
VisionInspectionSystem/
├── VisionInspectionSystem.sln          # 解决方案文件
├── VisionInspectionSystem/             # 主项目
│   ├── Program.cs                      # 程序入口
│   ├── App.config                      # 应用配置
│   ├── Forms/                          # 窗体
│   │   ├── MainForm.cs                 # 主窗体
│   │   ├── LoginForm.cs                # 登录窗体
│   │   ├── CameraSettingForm.cs        # 相机设置
│   │   ├── CommunicationForm.cs        # 通讯设置
│   │   ├── InspectionForm.cs           # 检测界面
│   │   ├── StatisticsForm.cs           # 数据统计
│   │   ├── ModelManageForm.cs          # 版型管理
│   │   └── UserManageForm.cs           # 用户管理
│   ├── UserControls/                   # 用户控件
│   ├── BLL/                            # 业务逻辑层
│   │   ├── AuthenticationManager.cs    # 权限管理
│   │   ├── CameraManager.cs            # 相机管理
│   │   ├── CommunicationManager.cs     # 通讯管理
│   │   ├── InspectionManager.cs        # 检测管理
│   │   ├── ModelManager.cs             # 版型管理
│   │   └── StatisticsManager.cs        # 统计管理
│   ├── DAL/                            # 数据访问层
│   │   ├── ConfigHelper.cs             # 配置助手
│   │   ├── LogHelper.cs                # 日志助手
│   │   ├── DataStorage.cs              # 数据存储
│   │   ├── ImageStorage.cs             # 图像存储
│   │   └── ModelStorage.cs             # 版型存储
│   ├── HAL/                            # 硬件抽象层
│   │   ├── BaslerCamera.cs             # Basler相机封装
│   │   ├── TcpCommunication.cs         # TCP通讯封装
│   │   └── VisionProProcessor.cs       # VisionPro封装
│   ├── Models/                         # 数据模型
│   └── Common/                         # 公共类
├── Config/                             # 配置文件
├── Data/                               # 数据目录
├── Models/                             # 版型目录
└── Docs/                               # 文档
```

## 功能模块

### 1. TCP/IP 通讯
- 支持服务端/客户端模式
- 支持与 SocketTool 等工具通讯测试
- 自动重连机制
- 命令解析和响应

### 2. Basler 相机
- 相机枚举和连接
- 单帧/连续采集
- 曝光时间、增益调整
- 触发模式切换（软触发/外触发）
- 图像保存

### 3. VisionPro 集成
- 加载 .vpp 作业文件
- 执行图像检测
- 获取检测结果（坐标、角度、分数）
- 离线测试功能

### 4. 三级权限管理
- 操作员：运行检测、查看结果
- 工程师：相机参数、版型切换、离线测试
- 管理员：版型编辑、用户管理、系统配置

### 5. 版型管理
- 创建/编辑/删除版型
- 版型切换
- 保存相机参数

### 6. 数据统计
- 实时统计（OK/NG数量、良率）
- 历史数据查询
- 导出 CSV 报表

## 默认账户

| 用户名 | 密码 | 权限 |
|--------|------|------|
| admin | admin123 | 管理员 |
| engineer | eng123 | 工程师 |
| operator | op123 | 操作员 |

## 使用说明

### 1. 打开项目
用 Visual Studio 2022 打开 `VisionInspectionSystem.sln`

### 2. 添加引用（重要）
项目需要添加以下引用：
- **Basler.Pylon.dll** - 从 Pylon SDK 安装目录获取
- **Cognex.VisionPro.dll** - 从 VisionPro 安装目录获取

### 3. 编译运行
按 F5 编译运行，使用默认账户 admin/admin123 登录

## 开发说明

当前代码框架中，相机和 VisionPro 模块使用模拟数据运行。
实际使用时需要：

1. 安装 Basler Pylon SDK
2. 安装 Cognex VisionPro
3. 取消 HAL 层代码中的注释，启用真实 SDK 调用

## 通讯协议

```
触发命令: T1
心跳命令: H1
查询结果: G1

响应格式:
OK,X:123.456,Y:234.567,R:45.123
NG,ErrorCode
```
