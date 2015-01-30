--Change script command to MyMDb database...
USE [MyMDb]
GO
--Preparing the bulk tables...
SELECT TOP(0)* INTO [imdb].[MediaEntry] FROM [dbo].[MediaEntry]
SELECT TOP(0)* INTO [imdb].[TagLine] FROM [dbo].[TagLine]
SELECT TOP(0)* INTO [imdb].[Plot] FROM [dbo].[Plot]
SELECT TOP(0)* INTO [imdb].[MediaEntry_Genre_Rel] FROM [dbo].[MediaEntry_Genre_Rel]
SELECT TOP(0)* INTO [imdb].[MediaEntry_Country_Rel] FROM [dbo].[MediaEntry_Country_Rel]
GO