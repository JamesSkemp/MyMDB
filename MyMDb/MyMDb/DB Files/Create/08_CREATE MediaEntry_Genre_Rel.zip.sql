--Change script command to MyMDb database...
USE [MyMDb]
GO
--Remove MediaEntry to Genre Relationship and its assosicated objects...
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MediaEntry]') AND parent_object_id = OBJECT_ID(N'[dbo].[MediaEntry_Genre_Rel]'))
	ALTER TABLE [dbo].[MediaEntry_Genre_Rel] DROP CONSTRAINT [FK_MediaEntry]
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Genre]') AND parent_object_id = OBJECT_ID(N'[dbo].[MediaEntry_Genre_Rel]'))
	ALTER TABLE [dbo].[MediaEntry_Genre_Rel] DROP CONSTRAINT [FK_Genre]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaEntry_Genre_Rel]') AND type in (N'U'))
	DROP TABLE [dbo].[MediaEntry_Genre_Rel]
GO
--Create the MediaEntry to Genre Relationship Table...
CREATE TABLE [dbo].[MediaEntry_Genre_Rel]
(
	[MediaEntry_Id] uniqueidentifier NOT NULL,
	[Genre_Id] smallint NOT NULL,
	CONSTRAINT [PK_MediaEntry_Genre_Rel] PRIMARY KEY CLUSTERED 
	(
		[MediaEntry_Id] ASC,
		[Genre_Id] ASC
	),
	CONSTRAINT [FK_MediaEntry] FOREIGN KEY ([MediaEntry_Id]) REFERENCES [dbo].[MediaEntry]([Id]),
	CONSTRAINT [FK_Genre] FOREIGN KEY ([Genre_Id]) REFERENCES [dbo].[Genre]([Id])
)
GO
--Create Documentation for the MediaEntry to Genre Relationship Table...
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pointer to the MediaEntry Id', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry_Genre_Rel', @level2type=N'COLUMN', @level2name=N'MediaEntry_Id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pointer to the Genre Id', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry_Genre_Rel', @level2type=N'COLUMN', @level2name=N'Genre_Id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Relations between Media Entries and its Genre', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry_Genre_Rel'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary Clustered Key/Index for the Media/Genre Relationship' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE', @level1name=N'MediaEntry_Genre_Rel', @level2type=N'CONSTRAINT', @level2name=N'PK_MediaEntry_Genre_Rel'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Relationship to the MediaEntry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE', @level1name=N'MediaEntry_Genre_Rel', @level2type=N'CONSTRAINT', @level2name=N'FK_MediaEntry'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Relationship to the Genre' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE', @level1name=N'MediaEntry_Genre_Rel', @level2type=N'CONSTRAINT', @level2name=N'FK_Genre'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry_Genre_Rel'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry_Genre_Rel'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry_Genre_Rel'
EXEC sys.sp_addextendedproperty @name=N'Version', @value=N'1.0.0.0', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry_Genre_Rel'
EXEC sys.sp_addextendedproperty @name=N'Last Sync Time', @value=N'Never', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MediaEntry_Genre_Rel'