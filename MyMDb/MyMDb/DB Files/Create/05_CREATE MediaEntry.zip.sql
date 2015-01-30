--Change script command to MyMDb database...
USE [MyMDb]
GO
--Remove MediaEntry and its assosicated objects...
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MediaEntry]') AND name = N'IX_Title')
	DROP INDEX [IX_Title] ON [dbo].[MediaEntry]
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MediaEntry]') AND name = N'IX_Title_SubTitle')
	DROP INDEX [IX_Title_SubTitle] ON [dbo].[MediaEntry]
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MediaEntry]') AND name = N'IX_Title_Year')
	DROP INDEX [IX_Title_Year] ON [dbo].[MediaEntry]
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MediaEntry]') AND name = N'IX_Title_Year_Subtitle')
	DROP INDEX [IX_Title_Year_Subtitle] ON [dbo].[MediaEntry]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaEntry]') AND type in (N'U'))
	DROP TABLE [dbo].[MediaEntry]
GO
--Create the MediaEntry Table...
CREATE TABLE dbo.MediaEntry
(
	Id uniqueidentifier NOT NULL,
	Parent_Id uniqueidentifier NOT NULL,
	Series bit NOT NULL,
	Title varchar(500) NOT NULL,
	Year smallint NOT NULL,
	TitleVersionOfTheYear tinyint NOT NULL,
	Season smallint NOT NULL,
	Episode smallint NOT NULL,
	SubTitle varchar(350) NULL,
	Dated smalldatetime NULL,
	IMDbUrlId varchar(15) NULL,
	CONSTRAINT PK_MediaEntry PRIMARY KEY CLUSTERED 
	(
		Id ASC
	)
)
GO
--Create Documentation for the MediaEntry Table...
EXECUTE sp_addextendedproperty N'MS_Description', N'Contains Series and Movies', N'SCHEMA', N'dbo', N'TABLE', N'MediaEntry', NULL, NULL
EXECUTE sp_addextendedproperty N'MS_Description', N'Localized Primary Key for the media based on the IMDb Key', N'SCHEMA', N'dbo', N'TABLE', N'MediaEntry', N'COLUMN', N'Id'
EXECUTE sp_addextendedproperty N'MS_Description', N'If not the same as Id this gives the relations between parent and child entry (mostly used in Series)', N'SCHEMA', N'dbo', N'TABLE', N'MediaEntry', N'COLUMN', N'Parent_Id'
EXECUTE sp_addextendedproperty N'MS_Description', N'Is the Entry an Serie or a Movie', N'SCHEMA', N'dbo', N'TABLE', N'MediaEntry', N'COLUMN', N'Series'
EXECUTE sp_addextendedproperty N'MS_Description', N'Title of the Entry', N'SCHEMA', N'dbo', N'TABLE', N'MediaEntry', N'COLUMN', N'Title'
EXECUTE sp_addextendedproperty N'MS_Description', N'Year of the Entry', N'SCHEMA', N'dbo', N'TABLE', N'MediaEntry', N'COLUMN', N'Year'
EXECUTE sp_addextendedproperty N'MS_Description', N'If on the same year, multiple titles with the same name is represented, this will show the version of that title.', N'SCHEMA', N'dbo', N'TABLE', N'MediaEntry', N'COLUMN', N'TitleVersionOfTheYear'
EXECUTE sp_addextendedproperty N'MS_Description', N'Season number of the Entry', N'SCHEMA', N'dbo', N'TABLE', N'MediaEntry', N'COLUMN', N'Season'
EXECUTE sp_addextendedproperty N'MS_Description', N'Episode number of the Entry', N'SCHEMA', N'dbo', N'TABLE', N'MediaEntry', N'COLUMN', N'Episode'
EXECUTE sp_addextendedproperty N'MS_Description', N'Title of an Series Episode', N'SCHEMA', N'dbo', N'TABLE', N'MediaEntry', N'COLUMN', N'SubTitle'
EXECUTE sp_addextendedproperty N'MS_Description', N'Dated is used to show when the entry was aired...', N'SCHEMA', N'dbo', N'TABLE', N'MediaEntry', N'COLUMN', N'Dated'
EXECUTE sp_addextendedproperty N'MS_Description', N'IMDb Url id of the entry', N'SCHEMA', N'dbo', N'TABLE', N'MediaEntry', N'COLUMN', N'IMDbUrlId'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary Clustered Key/Index for the Media Entries', @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'MediaEntry', @level2type=N'CONSTRAINT', @level2name=N'PK_MediaEntry'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'CONSTRAINT', @level2name=N'PK_MediaEntry'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'CONSTRAINT', @level2name=N'PK_MediaEntry'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'CONSTRAINT', @level2name=N'PK_MediaEntry'
EXEC sys.sp_addextendedproperty @name=N'Version', @value=N'1.0.0.0', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry'
EXEC sys.sp_addextendedproperty @name=N'Last Sync Time', @value=N'Never', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry'
GO
--Create the IX_Title Index on MediaEntry...
CREATE NONCLUSTERED INDEX [IX_Title] ON [dbo].[MediaEntry]
(
	[Title] ASC
)
WITH 
(
	PAD_INDEX = OFF, 
	STATISTICS_NORECOMPUTE = OFF, 
	SORT_IN_TEMPDB = OFF, 
	DROP_EXISTING = OFF, 
	ONLINE = OFF, 
	ALLOW_ROW_LOCKS = ON, 
	ALLOW_PAGE_LOCKS = ON
)
GO
--Create the IX_Title_SubTitle Index on MediaEntry...
CREATE NONCLUSTERED INDEX [IX_Title_SubTitle] ON [dbo].[MediaEntry]
(
	[Title] ASC,
	[SubTitle] ASC
)
WITH 
(
	PAD_INDEX = OFF, 
	STATISTICS_NORECOMPUTE = OFF, 
	SORT_IN_TEMPDB = OFF, 
	DROP_EXISTING = OFF, 
	ONLINE = OFF, 
	ALLOW_ROW_LOCKS = ON, 
	ALLOW_PAGE_LOCKS = ON
)
GO
--Create the IX_Title_Year Index on MediaEntry...
CREATE NONCLUSTERED INDEX [IX_Title_Year] ON [dbo].[MediaEntry]
(
	[Title] ASC,
	[Year] ASC
)
WITH 
(
	PAD_INDEX = OFF, 
	STATISTICS_NORECOMPUTE = OFF, 
	SORT_IN_TEMPDB = OFF, 
	DROP_EXISTING = OFF, 
	ONLINE = OFF, 
	ALLOW_ROW_LOCKS = ON, 
	ALLOW_PAGE_LOCKS = ON
)
GO
--Create the IX_Title_Year_Subtitle Index on MediaEntry...
CREATE NONCLUSTERED INDEX [IX_Title_Year_Subtitle] ON [dbo].[MediaEntry]
(
	[Title] ASC,
	[Year] ASC,
	[SubTitle] ASC
)
WITH 
(
	PAD_INDEX = OFF, 
	STATISTICS_NORECOMPUTE = OFF, 
	SORT_IN_TEMPDB = OFF, 
	DROP_EXISTING = OFF, 
	ONLINE = OFF, 
	ALLOW_ROW_LOCKS = ON, 
	ALLOW_PAGE_LOCKS = ON
)
GO
--Create Documentation for the MediaEntry Indexes...
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Index for searching for titles, years and subtitles together.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'INDEX',@level2name=N'IX_Title_Year_Subtitle'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'INDEX',@level2name=N'IX_Title_Year_Subtitle'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'INDEX',@level2name=N'IX_Title_Year_Subtitle'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'INDEX',@level2name=N'IX_Title_Year_Subtitle'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Index for searching for titles and years together.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'INDEX',@level2name=N'IX_Title_Year'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'INDEX',@level2name=N'IX_Title_Year'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'INDEX',@level2name=N'IX_Title_Year'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'INDEX',@level2name=N'IX_Title_Year'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Index for searching for titles.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'INDEX',@level2name=N'IX_Title_SubTitle'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'INDEX',@level2name=N'IX_Title_SubTitle'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'INDEX',@level2name=N'IX_Title_SubTitle'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'INDEX',@level2name=N'IX_Title_SubTitle'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Index for searching for titles and subtitles together.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'INDEX',@level2name=N'IX_Title'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'INDEX',@level2name=N'IX_Title'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'INDEX',@level2name=N'IX_Title'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry', @level2type=N'INDEX',@level2name=N'IX_Title'