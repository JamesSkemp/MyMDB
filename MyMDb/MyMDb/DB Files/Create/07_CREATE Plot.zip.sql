--Change script command to MyMDb database...
USE [MyMDb]
GO
--Remove Plot and its assosicated objects...
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Plot_MediaEntry]') AND parent_object_id = OBJECT_ID(N'[dbo].[Plot]'))
	ALTER TABLE [dbo].[Plot] DROP CONSTRAINT [FK_Plot_MediaEntry]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Plot]') AND type in (N'U'))
	DROP TABLE [dbo].[Plot]
GO
--Create the Plot Table...
CREATE TABLE [dbo].[Plot]
(
	[Id] uniqueidentifier NOT NULL,
	[PlotIndex] tinyint NOT NULL,
	[Text] text NOT NULL,
	[By] varchar(300) NOT NULL,
	CONSTRAINT [PK_Plot] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC,
		[PlotIndex] ASC
	),
	CONSTRAINT [FK_Plot_MediaEntry] FOREIGN KEY ([Id]) REFERENCES [dbo].[MediaEntry]([Id])
)
GO
--Create Documentation for the Plot Table...
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of the Primary Key, and foreign key to the MediaEntry Table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Plot', @level2type=N'COLUMN',@level2name=N'Id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Index of the plot' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Plot', @level2type=N'COLUMN',@level2name=N'PlotIndex'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Plot' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Plot', @level2type=N'COLUMN',@level2name=N'Text'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Plot written by' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Plot', @level2type=N'COLUMN',@level2name=N'By'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contains Plot''s for the movies' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Plot'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary Clustered Key/Index for the Plot', @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Plot', @level2type=N'CONSTRAINT', @level2name=N'PK_Plot'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Relationship between Media and Plot' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Plot', @level2type=N'CONSTRAINT', @level2name=N'FK_Plot_MediaEntry'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Plot'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Plot'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Plot'
EXEC sys.sp_addextendedproperty @name=N'Version', @value=N'1.0.0.0', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Plot'
EXEC sys.sp_addextendedproperty @name=N'Last Sync Time', @value=N'Never', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Plot'