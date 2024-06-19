USE [master]
GO
/****** Object:  Database [DermSight]    Script Date: 2024/6/17 上午 01:53:02 ******/
CREATE DATABASE [DermSight]
GO
ALTER DATABASE [DermSight] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DermSight].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DermSight] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DermSight] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DermSight] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DermSight] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DermSight] SET ARITHABORT OFF 
GO
ALTER DATABASE [DermSight] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [DermSight] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DermSight] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DermSight] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DermSight] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DermSight] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DermSight] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DermSight] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DermSight] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DermSight] SET  ENABLE_BROKER 
GO
ALTER DATABASE [DermSight] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DermSight] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DermSight] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DermSight] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DermSight] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DermSight] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DermSight] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DermSight] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DermSight] SET  MULTI_USER 
GO
ALTER DATABASE [DermSight] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DermSight] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DermSight] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DermSight] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DermSight] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DermSight] SET QUERY_STORE = OFF
GO
USE [DermSight]
GO
/****** Object:  Table [dbo].[City]    Script Date: 2024/6/17 上午 01:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[City](
	[cityId] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[cityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clinic]    Script Date: 2024/6/17 上午 01:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clinic](
	[clinicId] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](20) NOT NULL,
	[cityId] [int] NOT NULL,
	[address] [nvarchar](50) NOT NULL,
	[isDelete] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Disease]    Script Date: 2024/6/17 上午 01:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Disease](
	[diseaseId] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](20) NULL,
	[description] [nvarchar](max) NULL,
	[isDelete] [bit] NULL,
	[time] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[diseaseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DiseaseRecord]    Script Date: 2024/6/17 上午 01:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DiseaseRecord](
	[recordId] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NULL,
	[isCorrect] [bit] NULL,
	[diseaseld] [int] NULL,
	[time] [datetime] NULL,
	[isDelete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[recordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[News]    Script Date: 2024/6/17 上午 01:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News](
	[newsId] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NULL,
	[type] [nvarchar](10) NULL,
	[title] [nvarchar](30) NULL,
	[content] [nvarchar](500) NULL,
	[time] [datetime] NULL,
	[pin] [bit] NULL,
	[isDelete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[newsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Photo]    Script Date: 2024/6/17 上午 01:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Photo](
	[photoId] [int] IDENTITY(1,1) NOT NULL,
	[diseaseld] [int] NULL,
	[route] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[photoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RecordPhoto]    Script Date: 2024/6/17 上午 01:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecordPhoto](
	[recordPhotoId] [int] IDENTITY(1,1) NOT NULL,
	[recordId] [int] NULL,
	[route] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[recordPhotoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Symptom]    Script Date: 2024/6/17 上午 01:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Symptom](
	[symptomId] [int] IDENTITY(1,1) NOT NULL,
	[diseaseId] [int] NULL,
	[content] [nvarchar](max) NULL,
	[isDelete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[symptomId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2024/6/17 上午 01:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[userId] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](10) NULL,
	[photo] [nvarchar](max) NULL,
	[account] [varchar](30) NULL,
	[password] [varchar](max) NULL,
	[mail] [varchar](50) NULL,
	[authcode] [varchar](10) NULL,
	[role] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[City] ON 
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (1, N'台北市')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (2, N'新北市')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (3, N'桃園市')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (4, N'台中市')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (5, N'台南市')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (6, N'高雄市')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (7, N'基隆市')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (8, N'新竹市')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (9, N'嘉義市')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (10, N'新竹縣')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (11, N'苗栗縣')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (12, N'彰化縣')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (13, N'南投縣')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (14, N'雲林縣')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (15, N'嘉義縣')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (16, N'屏東縣')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (17, N'宜蘭縣')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (18, N'花蓮縣')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (19, N'台東縣')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (20, N'澎湖縣')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (21, N'金門縣')
GO
INSERT [dbo].[City] ([cityId], [name]) VALUES (22, N'連江縣')
GO
SET IDENTITY_INSERT [dbo].[City] OFF
GO
SET IDENTITY_INSERT [dbo].[Clinic] ON 
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (1, N'家樂診所', 4, N'南區XX路XX號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (2, N'幸福診所', 1, N'中正區和平東路一段10號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (3, N'健康診所', 2, N'板橋區文化路二段22號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (4, N'和康診所', 3, N'中壢區中山東路30號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (5, N'仁愛診所', 4, N'西屯區台中港路一段50號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (6, N'長庚診所', 5, N'安平區健康路三段15號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (7, N'信義診所', 6, N'前鎮區中山二路100號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (8, N'幸福健康診所', 7, N'信義區忠孝東路四段200號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (9, N'安康診所', 8, N'東區民生路50號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (10, N'仁心診所', 9, N'東區中山路一段60號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (11, N'慈濟診所', 10, N'竹北市勝利路80號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (12, N'康泰診所', 11, N'苗栗市中正路20號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (13, N'華康診所', 12, N'彰化市中山路200號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (14, N'安心診所', 13, N'南投市光復路10號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (15, N'康健診所', 14, N'斗六市中正路120號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (16, N'信康診所', 15, N'嘉義市博愛路100號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (17, N'仁康診所', 16, N'屏東市民生路80號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (18, N'信義醫療', 17, N'宜蘭市中山路三段30號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (19, N'華仁診所', 18, N'花蓮市中山路四段50號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (20, N'安康診所', 19, N'台東市中華路二段70號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (21, N'長春診所', 20, N'馬公市中山路60號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (22, N'華泰診所', 21, N'金城鎮中正路50號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (23, N'安泰診所', 22, N'南竿鄉馬祖路100號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (24, N'康寧診所', 1, N'大安區信義路三段88號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (25, N'祥和診所', 2, N'三重區重新路五段100號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (26, N'博仁診所', 3, N'蘆竹區南崁路60號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (27, N'德仁診所', 4, N'北屯區文心路一段15號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (28, N'宏泰診所', 5, N'永康區中華路20號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (29, N'仁安診所', 6, N'鳳山區鳳頂路120號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (30, N'康美診所', 7, N'中山區民權東路六段70號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (31, N'和平診所', 8, N'北區中山路二段100號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (32, N'長安診所', 9, N'西區林森路五段50號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (33, N'仁心醫療', 10, N'竹東鎮復興路90號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (34, N'博愛診所', 11, N'頭份市中正路100號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (35, N'信泰診所', 12, N'鹿港鎮中山路五段80號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (36, N'德安診所', 13, N'埔里鎮南昌路60號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (37, N'華信診所', 14, N'虎尾鎮林森路90號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (38, N'安和診所', 15, N'水上鄉中華路50號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (39, N'康寧診所', 16, N'潮州鎮四維路200號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (40, N'仁安醫療', 17, N'羅東鎮中山路六段40號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (41, N'長壽診所', 18, N'吉安鄉中正路四段70號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (42, N'健康診所', 19, N'成功鎮中華路五段30號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (43, N'安泰醫療', 20, N'湖西鄉中山路20號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (44, N'仁康診所', 21, N'金湖鎮和平路80號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (45, N'宏仁診所', 22, N'北竿鄉中山路10號', 0)
GO
INSERT [dbo].[Clinic] ([clinicId], [name], [cityId], [address], [isDelete]) VALUES (46, N'炸魚診所', 4, N'北區三民路55號', 0)
GO
SET IDENTITY_INSERT [dbo].[Clinic] OFF
GO
SET IDENTITY_INSERT [dbo].[Disease] ON 
GO
INSERT [dbo].[Disease] ([diseaseId], [name], [description], [isDelete], [time]) VALUES (1, N'機肌腹肌機', N'描述', 1, CAST(N'2024-06-14T15:27:50.790' AS DateTime))
GO
INSERT [dbo].[Disease] ([diseaseId], [name], [description], [isDelete], [time]) VALUES (2, N'疾病2', N'很可怕很可怕很可怕很可怕', 0, CAST(N'2024-06-14T15:28:39.020' AS DateTime))
GO
INSERT [dbo].[Disease] ([diseaseId], [name], [description], [isDelete], [time]) VALUES (3, N'疾病病2', N'很可怕很可怕很可怕很可怕', 0, CAST(N'2024-06-14T16:55:53.663' AS DateTime))
GO
INSERT [dbo].[Disease] ([diseaseId], [name], [description], [isDelete], [time]) VALUES (4, N'疾病病3', N'很可怕很可怕很可怕很可怕', 0, CAST(N'2024-06-14T16:57:03.043' AS DateTime))
GO
INSERT [dbo].[Disease] ([diseaseId], [name], [description], [isDelete], [time]) VALUES (5, N'疾病病病病5', N'描述', 0, CAST(N'2024-06-14T16:58:24.523' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Disease] OFF
GO
SET IDENTITY_INSERT [dbo].[Symptom] ON 
GO
INSERT [dbo].[Symptom] ([symptomId], [diseaseId], [content], [isDelete]) VALUES (1, 5, N'哭哭', 0)
GO
INSERT [dbo].[Symptom] ([symptomId], [diseaseId], [content], [isDelete]) VALUES (3, 5, N'無力', 0)
GO
INSERT [dbo].[Symptom] ([symptomId], [diseaseId], [content], [isDelete]) VALUES (4, 5, N'過敏', 0)
GO
SET IDENTITY_INSERT [dbo].[Symptom] OFF
GO


ALTER TABLE [dbo].[Clinic] ADD  CONSTRAINT [DF_Clinic_isDelete]  DEFAULT ((0)) FOR [isDelete]
GO
ALTER TABLE [dbo].[Disease] ADD  DEFAULT ((0)) FOR [isDelete]
GO
ALTER TABLE [dbo].[Disease] ADD  CONSTRAINT [DF_Disease_time]  DEFAULT (getdate()) FOR [time]
GO
ALTER TABLE [dbo].[DiseaseRecord] ADD  DEFAULT (getdate()) FOR [time]
GO
ALTER TABLE [dbo].[DiseaseRecord] ADD  DEFAULT ((0)) FOR [isDelete]
GO
ALTER TABLE [dbo].[News] ADD  DEFAULT (getdate()) FOR [time]
GO
ALTER TABLE [dbo].[News] ADD  DEFAULT ((0)) FOR [pin]
GO
ALTER TABLE [dbo].[News] ADD  DEFAULT ((0)) FOR [isDelete]
GO
ALTER TABLE [dbo].[Symptom] ADD  DEFAULT ((0)) FOR [isDelete]
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT ((1)) FOR [role]
GO
USE [master]
GO
ALTER DATABASE [DermSight] SET  READ_WRITE 
GO
