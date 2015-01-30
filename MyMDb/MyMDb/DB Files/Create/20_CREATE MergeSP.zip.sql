--Change script command to MyMDb database...
USE [MyMDb]
GO
--Remove MergeMediaEntry and its assosicated objects...
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MergeMediaEntry]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[MergeMediaEntry]
GO
--Create the MergeMediaEntry Procedure...
CREATE PROCEDURE [dbo].[MergeMediaEntry]
AS
	DECLARE @mergeME TABLE (ACT VARCHAR(20));
	MERGE INTO [dbo].[MediaEntry] T USING [imdb].[MediaEntry] S ON T.[Id]=S.[Id]
	WHEN NOT MATCHED THEN
		INSERT 
		(
			[Id]
			,[Parent_Id]
			,[Series]
			,[Title]
			,[Year]
			,[TitleVersionOfTheYear]
			,[Season]
			,[Episode]
			,[SubTitle]
			,[Dated]
			,[IMDbUrlId]
		)
		VALUES
        (
			S.[Id]
			,S.[Parent_Id]
			,S.[Series]
			,S.[Title]
			,S.[Year]
			,S.[TitleVersionOfTheYear]
			,S.[Season]
			,S.[Episode]
			,S.[SubTitle]
			,S.[Dated]
			,S.[IMDbUrlId]
		)
		OUTPUT $ACTION INTO @mergeME;
		--Return total handled items...
		SELECT COUNT(IME.Id) AS [TotalHandled] FROM [imdb].[MediaEntry] IME
		--Clear temporary table...
		--DELETE FROM [imdb].[MediaEntry]
		--Return How many actions we did...
		SELECT [INSERT], [UPDATE], [DELETE] FROM (SELECT 'NOOP' ACT UNION ALL SELECT * FROM @mergeME) p PIVOT (COUNT(ACT) FOR ACT IN ([INSERT],[UPDATE],[DELETE])) AS Pvt
GO
--Create Documentation for the MergeMediaEntry Procedure...
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Procedure to handle inserts of only the new entries.', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeMediaEntry'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeMediaEntry'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeMediaEntry'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeMediaEntry'
EXEC sys.sp_addextendedproperty @name=N'Version', @value=N'1.0.0.0', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeMediaEntry'
GO
--Remove MergeTagLine and its assosicated objects...
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MergeTagLine]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[MergeTagLine]
GO
--Create the MergeTagLine Procedure...
CREATE PROCEDURE [dbo].[MergeTagLine]
AS
	DECLARE @mergeTL TABLE (ACT VARCHAR(20));
	DELETE FROM [imdb].[TagLine] WHERE [imdb].[TagLine].[Id] NOT IN (SELECT [dbo].[MediaEntry].[Id] FROM [dbo].[MediaEntry]); --Remove Dirty Entries
	WITH DelFrom as (SELECT [Id], [TagIndex], row_number() OVER (PARTITION BY [Id], [TagIndex] ORDER BY [TagIndex]) AS [DelList] FROM [imdb].[TagLine]) --build table for deletion of duplicated entries
	DELETE DelFrom WHERE [DelList] > 1; --Delete duplicated entries but keep one.
	MERGE INTO [dbo].[TagLine] T USING [imdb].[TagLine] S ON T.[Id]=S.[Id] AND T.[TagIndex]=S.[TagIndex]
	WHEN MATCHED THEN
		UPDATE SET T.[Text]=S.[Text]
	WHEN NOT MATCHED THEN
		INSERT 
		(
			[Id]
			,[TagIndex]
			,[Text]
		)
		VALUES
        (
			S.[Id]
			,S.[TagIndex]
			,S.[Text]
		)
		OUTPUT $ACTION INTO @mergeTL;
		--Return total handled items...
		SELECT COUNT(ITL.Id) AS [TotalHandled] FROM [imdb].[TagLine] ITL
		--Clear temporary table...
		--DELETE FROM [imdb].[TagLine]
		--Return How many actions we did...
		SELECT [INSERT], [UPDATE], [DELETE] FROM (SELECT 'NOOP' ACT UNION ALL SELECT * FROM @mergeTL) p PIVOT (COUNT(ACT) FOR ACT IN ([INSERT],[UPDATE],[DELETE])) AS Pvt
GO
--Create Documentation for the MergeTagLine Procedure...
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Procedure to handle inserts and updates of the taglines.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeTagLine'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeTagLine'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeTagLine'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeTagLine'
EXEC sys.sp_addextendedproperty @name=N'Version', @value=N'1.0.0.0', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeTagLine'
GO
--Remove MergePlots and its assosicated objects...
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MergePlots]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[MergePlots]
GO
--Create the MergeTagLine Procedure...
CREATE PROCEDURE [dbo].[MergePlots]
AS
	DECLARE @mergeTL TABLE (ACT VARCHAR(20));
	DELETE FROM [imdb].[Plot] WHERE [imdb].[Plot].[Id] NOT IN (SELECT [dbo].[MediaEntry].[Id] FROM [dbo].[MediaEntry]); --Remove Dirty Entries
	WITH DelFrom as (SELECT [Id], [PlotIndex], row_number() OVER (PARTITION BY [Id], [PlotIndex] ORDER BY [PlotIndex]) AS [DelList] FROM [imdb].[Plot]) --build table for deletion of duplicated entries
	DELETE DelFrom WHERE [DelList] > 1; --Delete duplicated entries but keep one.
	MERGE INTO [dbo].[Plot] T USING [imdb].[Plot] S ON T.[Id]=S.[Id] AND T.[PlotIndex]=S.[PlotIndex]
	WHEN MATCHED THEN
		UPDATE SET T.[Text]=S.[Text], T.[By] = S.[By]
	WHEN NOT MATCHED THEN
		INSERT 
		(
			[Id]
			,[PlotIndex]
			,[Text]
			,[By]
		)
		VALUES
        (
			S.[Id]
			,S.[PlotIndex]
			,S.[Text]
			,S.[By]
		)
		OUTPUT $ACTION INTO @mergeTL;
		--Return total handled items...
		SELECT COUNT(ITL.Id) AS [TotalHandled] FROM [imdb].[Plot] ITL
		--Clear temporary table...
		--DELETE FROM [imdb].[Plot]
		--Return How many actions we did...
		SELECT [INSERT], [UPDATE], [DELETE] FROM (SELECT 'NOOP' ACT UNION ALL SELECT * FROM @mergeTL) p PIVOT (COUNT(ACT) FOR ACT IN ([INSERT],[UPDATE],[DELETE])) AS Pvt
GO
--Create Documentation for the MergeTagLine Procedure...
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Procedure to handle inserts and updates of the plots.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergePlots'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergePlots'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergePlots'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergePlots'
EXEC sys.sp_addextendedproperty @name=N'Version', @value=N'1.0.0.0', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergePlots'
GO
--Remove MergeGenres and its assosicated objects...
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MergeGenres]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[MergeGenres]
GO
--Create the MergeTagLine Procedure...
CREATE PROCEDURE [dbo].[MergeGenres]
AS
	DECLARE @mergeGL TABLE (ACT VARCHAR(20));
	DELETE FROM [imdb].[MediaEntry_Genre_Rel] WHERE [imdb].[MediaEntry_Genre_Rel].[MediaEntry_Id] NOT IN (SELECT [dbo].[MediaEntry].[Id] FROM [dbo].[MediaEntry]); --Remove Dirty Entries
	WITH DelFrom as (SELECT [MediaEntry_Id], [Genre_Id], row_number() OVER (PARTITION BY [MediaEntry_Id], [Genre_Id] ORDER BY [Genre_Id]) AS [DelList] FROM [imdb].[MediaEntry_Genre_Rel]) --build table for deletion of duplicated entries
	DELETE DelFrom WHERE [DelList] > 1; --Delete duplicated entries but keep one.
	MERGE INTO [dbo].[MediaEntry_Genre_Rel] T USING [imdb].[MediaEntry_Genre_Rel] S ON T.[MediaEntry_Id]=S.[MediaEntry_Id] AND T.[Genre_Id]=S.[Genre_Id]
	WHEN NOT MATCHED THEN
		INSERT 
		(
			[MediaEntry_Id]
			,[Genre_Id]
		)
		VALUES
        (
			S.[MediaEntry_Id]
			,S.[Genre_Id]
		)
		OUTPUT $ACTION INTO @mergeGL;
		--Return total handled items...
		SELECT COUNT(ITL.MediaEntry_Id) AS [TotalHandled] FROM [imdb].[MediaEntry_Genre_Rel] ITL
		--Clear temporary table...
		--DELETE FROM [imdb].[Plot]
		--Return How many actions we did...
		SELECT [INSERT], [UPDATE], [DELETE] FROM (SELECT 'NOOP' ACT UNION ALL SELECT * FROM @mergeGL) p PIVOT (COUNT(ACT) FOR ACT IN ([INSERT],[UPDATE],[DELETE])) AS Pvt
GO
--Create Documentation for the MergeTagLine Procedure...
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Procedure to handle inserts and updates of the MediaEntry And Genre Relationship.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeGenres'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeGenres'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeGenres'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeGenres'
EXEC sys.sp_addextendedproperty @name=N'Version', @value=N'1.0.0.0', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeGenres'
GO
--Remove MergeCountry and its assosicated objects...
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MergeCountry]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[MergeCountry]
GO
--Create the MergeTagLine Procedure...
CREATE PROCEDURE [dbo].[MergeCountry]
AS
	DECLARE @mergeCL TABLE (ACT VARCHAR(20));
	DELETE FROM [imdb].[MediaEntry_Country_Rel] WHERE [imdb].[MediaEntry_Country_Rel].[MediaEntry_Id] NOT IN (SELECT [dbo].[MediaEntry].[Id] FROM [dbo].[MediaEntry]); --Remove Dirty Entries
	WITH DelFrom as (SELECT [MediaEntry_Id], [ISO3166_Alpha3], row_number() OVER (PARTITION BY [MediaEntry_Id], [ISO3166_Alpha3] ORDER BY [ISO3166_Alpha3]) AS [DelList] FROM [imdb].[MediaEntry_Country_Rel]) --build table for deletion of duplicated entries
	DELETE DelFrom WHERE [DelList] > 1; --Delete duplicated entries but keep one.
	MERGE INTO [dbo].[MediaEntry_Country_Rel] T USING [imdb].[MediaEntry_Country_Rel] S ON T.[MediaEntry_Id]=S.[MediaEntry_Id] AND T.[ISO3166_Alpha3]=S.[ISO3166_Alpha3]
	WHEN NOT MATCHED THEN
		INSERT 
		(
			[MediaEntry_Id]
			,[ISO3166_Alpha3]
		)
		VALUES
        (
			S.[MediaEntry_Id]
			,S.[ISO3166_Alpha3]
		)
		OUTPUT $ACTION INTO @mergeCL;
		--Return total handled items...
		SELECT COUNT(ITL.MediaEntry_Id) AS [TotalHandled] FROM [imdb].[MediaEntry_Country_Rel] ITL
		--Clear temporary table...
		--DELETE FROM [imdb].[Plot]
		--Return How many actions we did...
		SELECT [INSERT], [UPDATE], [DELETE] FROM (SELECT 'NOOP' ACT UNION ALL SELECT * FROM @mergeCL) p PIVOT (COUNT(ACT) FOR ACT IN ([INSERT],[UPDATE],[DELETE])) AS Pvt
GO
--Create Documentation for the MergeTagLine Procedure...
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Procedure to handle inserts and updates of the MediaEntry And Country Relationship.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeCountry'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeCountry'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeCountry'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeCountry'
EXEC sys.sp_addextendedproperty @name=N'Version', @value=N'1.0.0.0', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'MergeCountry'