--Change script command to MyMDb database...
USE [MyMDb]
GO
--Remove GetTopGenreCount and its assosicated objects...
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTopGenreCount]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[GetTopGenreCount]
GO
--Create the GetTopGenreCount Procedure...
CREATE PROCEDURE GetTopGenreCount(@Series bit)
AS
DECLARE @tot AS float
IF @Series IS NULL
BEGIN
	SET @tot = CONVERT(float,(SELECT COUNT(R.Genre_Id) FROM [dbo].[MediaEntry_Genre_Rel] R JOIN [dbo].[MediaEntry] ME ON R.[MediaEntry_Id]=ME.[Id] WHERE ME.[Id]=ME.[Parent_Id]))
	SELECT COUNT(R.[Genre_Id]) AS [GenreCount],G.[Name] AS [Genre], ROUND((CONVERT(float,COUNT(R.[Genre_Id])) / @tot) * 100,2) [Percentage]
		FROM [dbo].[MediaEntry_Genre_Rel] R
		JOIN [dbo].[Genre] G ON R.[Genre_Id]=G.[Id]
		JOIN [dbo].[MediaEntry] ME ON R.[MediaEntry_Id]=ME.[Id]
		WHERE ME.[Id]=ME.[Parent_Id]
		GROUP BY R.[Genre_Id],G.[Name]
		ORDER BY COUNT(R.[Genre_Id]) DESC
END
ELSE
BEGIN
	SET @tot = CONVERT(float,(SELECT COUNT(R.Genre_Id) FROM [dbo].[MediaEntry_Genre_Rel] R JOIN [dbo].[MediaEntry] ME ON R.[MediaEntry_Id]=ME.[Id] WHERE ME.[Series]=@Series AND ME.[Id]=ME.[Parent_Id]))
	SELECT COUNT(R.[Genre_Id]) AS [GenreCount],G.[Name] AS [Genre], ROUND((CONVERT(float,COUNT(R.[Genre_Id])) / @tot) * 100,2) [Percentage]
		FROM [dbo].[MediaEntry_Genre_Rel] R
		JOIN [dbo].[Genre] G ON R.[Genre_Id]=G.[Id]
		JOIN [dbo].[MediaEntry] ME ON R.[MediaEntry_Id]=ME.[Id]
		WHERE ME.[Series]=@Series AND ME.[Id]=ME.[Parent_Id]
		GROUP BY R.[Genre_Id],G.[Name]
		ORDER BY COUNT(R.[Genre_Id]) DESC
END
GO
--Create Documentation for the GetTopGenreCount Procedure...
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Gets the top types of Genres, if @Series is omitted, then both series and movies are calculated', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'GetTopGenreCount'
EXEC sys.sp_addextendedproperty @name=N'Version', @value=N'1.0.0.0', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'GetTopGenreCount'
GO
--Remove GetTopYearCount and its assosicated objects...
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTopYearCount]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[GetTopYearCount]
GO
--Create the GetTopYearCount Procedure...
CREATE PROCEDURE GetTopYearCount(@Series bit)
AS
DECLARE @tot AS float
IF @Series IS NULL
BEGIN
	SET @tot = CONVERT(float,(SELECT COUNT(Id) FROM [dbo].[MediaEntry] WHERE [Id]=[Parent_Id]))
	SELECT COUNT([Id]) AS [YearCount],[Year], ROUND((CONVERT(float,COUNT([Id])) / @tot) * 100,2) [Percentage]
		FROM [dbo].[MediaEntry] 
		WHERE [Id]=[Parent_Id]
		GROUP BY [Year]
		ORDER BY COUNT([Id]) DESC
END
ELSE
BEGIN
	SET @tot = CONVERT(float,(SELECT COUNT(Id) FROM [dbo].[MediaEntry] WHERE [Series]=@Series AND [Id]=[Parent_Id]))
	SELECT COUNT([Id]) AS [YearCount],[Year], ROUND((CONVERT(float,COUNT([Id])) / @tot) * 100,2) [Percentage]
		FROM [dbo].[MediaEntry] 
		WHERE [Series]=@Series AND [Id]=[Parent_Id]
		GROUP BY [Year]
		ORDER BY COUNT([Id]) DESC
END
GO
--Create Documentation for the GetTopYearCount Procedure...
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Gets the top Years, if @Series is omitted, then both series and movies are calculated', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'GetTopYearCount'
EXEC sys.sp_addextendedproperty @name=N'Version', @value=N'1.0.0.0', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'GetTopYearCount'