--Change script command to master database...
USE [master]
GO
--Entering Single User Mode, please wait...
ALTER DATABASE [MyMDb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
--Remove the MyMDb Database...
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'MyMDb')
	DROP DATABASE [MyMDb]
GO
--Create the MyMDb Database...
CREATE DATABASE [MyMDb]
GO
--Altering MyMDb Database files...
ALTER DATABASE [MyMDb] MODIFY FILE ( NAME = N'MyMDb', SIZE = 1GB , MAXSIZE = UNLIMITED, FILEGROWTH = 100MB  )
ALTER DATABASE [MyMDb] MODIFY FILE ( NAME = N'MyMDb_log', SIZE = 500MB , MAXSIZE = UNLIMITED, FILEGROWTH = 100MB )
GO
--Entering Multi User Mode, please wait...
ALTER DATABASE [MyMDb] SET MULTI_USER WITH ROLLBACK IMMEDIATE
GO
--Change script command to new database MyMDb...
USE [MyMDb]
GO
--Create Documentation for the MyMDb database...
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Your very own IMDB database'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge'
EXEC sys.sp_addextendedproperty @name=N'Version', @value=N'1.0.0.0'
EXEC sys.sp_addextendedproperty @name=N'Last Sync Time', @value=N'Never'