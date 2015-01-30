--Change script command to MyMDb database...
USE [MyMDb]
GO
--Remove Tagline and its assosicated objects...
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TagLine_MediaEntry]') AND parent_object_id = OBJECT_ID(N'[dbo].[TagLine]'))
	ALTER TABLE [dbo].[TagLine] DROP CONSTRAINT [FK_TagLine_MediaEntry]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TagLine]') AND type in (N'U'))
	DROP TABLE [dbo].[TagLine]
GO
--Create the TagLine Table...
CREATE TABLE [dbo].[TagLine]
(
	[Id] [uniqueidentifier] NOT NULL,
	[TagIndex] [tinyint] NOT NULL,
	[Text] [varchar](2000) NOT NULL,
	CONSTRAINT [PK_TagLine] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC,
		[TagIndex] ASC
	),
	CONSTRAINT [FK_TagLine_MediaEntry] FOREIGN KEY ([Id]) REFERENCES [dbo].[MediaEntry]([Id])
)
GO
--Create Documentation for the TagLine Table...
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of the Primary Key, and foreign key to the MediaEntry Table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TagLine', @level2type=N'COLUMN',@level2name=N'Id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Index of the tag line' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TagLine', @level2type=N'COLUMN',@level2name=N'TagIndex'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tag Line (Displayed on posters or covers)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TagLine', @level2type=N'COLUMN',@level2name=N'Text'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contains taglines for the movies' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TagLine'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary Clustered Key/Index for the Taglines', @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'TagLine', @level2type=N'CONSTRAINT', @level2name=N'PK_TagLine'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Relationship between Media and taglines' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE', @level1name=N'TagLine', @level2type=N'CONSTRAINT', @level2name=N'FK_TagLine_MediaEntry'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TagLine'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TagLine'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TagLine'
EXEC sys.sp_addextendedproperty @name=N'Version', @value=N'1.0.0.0', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TagLine'
EXEC sys.sp_addextendedproperty @name=N'Last Sync Time', @value=N'Never', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TagLine'