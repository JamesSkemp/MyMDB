--Change script command to MyMDb database...
USE [MyMDb]
GO
--Remove GetMedia and its assosicated objects...
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMedia]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[GetMedia]
GO
--Create the GetMedia Procedure...
CREATE PROCEDURE [dbo].[GetMedia](@Id as uniqueidentifier)
AS
IF @Id IS NOT NULL
BEGIN
	SELECT * FROM [dbo].[MediaEntry] WHERE [Id] = @Id
	SELECT 'Genre' as [MetaType],ROW_NUMBER() OVER (ORDER BY G.[Name]) as [Index],G.[Name] AS [Genre] FROM [dbo].[Genre] G JOIN [dbo].[MediaEntry_Genre_Rel] R ON G.Id=R.Genre_Id WHERE R.[MediaEntry_Id] = @Id
		UNION ALL
	SELECT 'Country' as [MetaType],ROW_NUMBER() OVER (ORDER BY C.[Name]) as [Index],C.[Name] AS [Country] FROM [dbo].[ISO3166] C JOIN [dbo].[MediaEntry_Country_Rel] R ON C.[Alpha3]=R.[ISO3166_Alpha3] WHERE R.[MediaEntry_Id] = @Id
		UNION ALL
	SELECT 'Plot' as [MetaType],P.[PlotIndex] as [Index],P.[Text] FROM [dbo].[Plot] P WHERE P.[Id] = @Id
		UNION ALL
	SELECT 'TagLine' as [MetaType],T.[TagIndex] as [Index],T.[Text] FROM [dbo].[TagLine] T WHERE T.[Id] = @Id
END
GO
--Create Documentation for the GetMedia Procedure...
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Gets the top types of Genres, if @Series is omitted, then both series and movies are calculated', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'GetMedia'
EXEC sys.sp_addextendedproperty @name=N'Version', @value=N'1.0.0.0', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'GetMedia'