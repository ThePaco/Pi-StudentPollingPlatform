﻿USE [master]
GO
/****** Object:  Database [piSudentPollingPlat]    Script Date: 4.12.2024. 19:04:36 ******/
CREATE DATABASE [piSudentPollingPlat]
GO
ALTER DATABASE [piSudentPollingPlat] SET COMPATIBILITY_LEVEL = 140
GO
ALTER DATABASE [piSudentPollingPlat] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET ARITHABORT OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [piSudentPollingPlat] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [piSudentPollingPlat] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET  DISABLE_BROKER 
GO
ALTER DATABASE [piSudentPollingPlat] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [piSudentPollingPlat] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [piSudentPollingPlat] SET  MULTI_USER 
GO
ALTER DATABASE [piSudentPollingPlat] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [piSudentPollingPlat] SET DB_CHAINING OFF 
GO
ALTER DATABASE [piSudentPollingPlat] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [piSudentPollingPlat] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [piSudentPollingPlat] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [piSudentPollingPlat] SET QUERY_STORE = ON
GO
ALTER DATABASE [piSudentPollingPlat] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [piSudentPollingPlat]
GO
/****** Object:  Table [dbo].[Poll]    Script Date: 4.12.2024. 19:04:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Poll](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Tekst] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Poll] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Question]    Script Date: 4.12.2024. 19:04:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Question](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Tekst] [nvarchar](255) NOT NULL,
	[PollID] [int] NOT NULL,
 CONSTRAINT [PK_Question] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 4.12.2024. 19:04:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 4.12.2024. 19:04:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](256) NOT NULL, 
	[Email] [nvarchar](50) NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logs]    Script Date: 4.12.2024. 19:04:37 ******/
CREATE TABLE [dbo].[Logs](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Timestamp] [datetime2](7) NOT NULL,
    [Level] [nvarchar](50) NULL,
	[Message] [nvarchar](max)  NULL
)
GO


/****** Object:  Table [dbo].[Kolegij]    Script Date: 4.12.2024. 19:04:37 ******/
CREATE TABLE [dbo].[Kolegij](
IDKolegij int primary key identity,
KolegijName nvarchar(200)
)

ALTER TABLE [dbo].[Poll]
ADD PollDate datetime;

ALTER TABLE [dbo].[Poll]
ADD KolegijID int foreign key references Kolegij(IDKolegij)

GO

	
/****** Object:  Table [dbo].[Studij]    Script Date: 4.12.2024. 19:04:37 ******/

CREATE TABLE [dbo].[Studij]
(
    [IDStudij] INT PRIMARY KEY IDENTITY,
    [StudijName] NVARCHAR(200)
);

ALTER TABLE [dbo].[Poll]
ADD [StudijID] INT FOREIGN KEY REFERENCES [dbo].[Studij]([IDStudij])

GO

/****** Object:  Table [dbo].[Godina]    Script Date: 4.12.2024. 19:04:37 ******/


CREATE TABLE [dbo].[Godina]
(
	IDGodina INT PRIMARY KEY IDENTITY,
	BrojGodine INT
)

ALTER TABLE [dbo].[Poll]
ADD GodinaID int foreign key references Godina(IDGodina)

/****** Object:  Table [dbo].[UserAnswer]    Script Date: 4.12.2024. 19:04:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAnswer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[QuestionID] [int] NOT NULL,
	[Answer] [int] NOT NULL,
 CONSTRAINT [PK_UserAnswer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Question]  WITH CHECK ADD  CONSTRAINT [FK_Question_Poll] FOREIGN KEY([PollID])
REFERENCES [dbo].[Poll] ([ID])
GO
ALTER TABLE [dbo].[Question] CHECK CONSTRAINT [FK_Question_Poll]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
ALTER TABLE [dbo].[UserAnswer]  WITH CHECK ADD  CONSTRAINT [FK_UserAnswer_Question] FOREIGN KEY([QuestionID])
REFERENCES [dbo].[Question] ([ID])
GO
ALTER TABLE [dbo].[UserAnswer] CHECK CONSTRAINT [FK_UserAnswer_Question]
GO
ALTER TABLE [dbo].[UserAnswer]  WITH CHECK ADD  CONSTRAINT [FK_UserAnswer_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[UserAnswer] CHECK CONSTRAINT [FK_UserAnswer_User]
GO
USE [master]
GO
ALTER DATABASE [piSudentPollingPlat] SET  READ_WRITE 
GO
