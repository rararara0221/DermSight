USE [master]
GO
/****** Object:  Database [DermSight]    Script Date: 2024/6/16 下午 02:23:53 ******/
CREATE DATABASE [DermSight]
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
/****** Object:  Table [dbo].[Clinic]    Script Date: 2024/6/16 下午 02:23:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clinic](
	[clinicId] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](20) NULL,
	[photo] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[clinicId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Disease]    Script Date: 2024/6/16 下午 02:23:53 ******/
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
/****** Object:  Table [dbo].[DiseaseRecord]    Script Date: 2024/6/16 下午 02:23:53 ******/
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
/****** Object:  Table [dbo].[News]    Script Date: 2024/6/16 下午 02:23:53 ******/
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
/****** Object:  Table [dbo].[Photo]    Script Date: 2024/6/16 下午 02:23:53 ******/
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
/****** Object:  Table [dbo].[RecordPhoto]    Script Date: 2024/6/16 下午 02:23:53 ******/
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
/****** Object:  Table [dbo].[Symptom]    Script Date: 2024/6/16 下午 02:23:53 ******/
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
/****** Object:  Table [dbo].[User]    Script Date: 2024/6/16 下午 02:23:53 ******/
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
