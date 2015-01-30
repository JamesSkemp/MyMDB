--Change script command to MyMDb database...
USE [MyMDb]
GO
--Clean up the bulk tables...
DROP TABLE [imdb].[MediaEntry]
DROP TABLE [imdb].[TagLine]
DROP TABLE [imdb].[Plot]
DROP TABLE [imdb].[MediaEntry_Genre_Rel]
DROP TABLE [imdb].[MediaEntry_Country_Rel]
GO
--Shrinking Data file...
DBCC SHRINKFILE (N'MyMDb' , 0, TRUNCATEONLY)
GO
--Shrinking Log file...
DBCC SHRINKFILE (N'MyMDb_log' , 0, TRUNCATEONLY)
GO