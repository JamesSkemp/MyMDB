--Change script command to MyMDb database...
USE [MyMDb]
GO
--Remove Genre and its assosicated objects...
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Genre]') AND type in (N'U'))
	DROP TABLE [dbo].[Genre]
GO
--Create the Genre Table...
CREATE TABLE [dbo].[Genre]
	(
	Id smallint NOT NULL,
	Name varchar(100) NOT NULL,
	CONSTRAINT PK_Genre PRIMARY KEY CLUSTERED
		(
			Id ASC
		)
	)  ON [PRIMARY]
GO
--Create Documentation for the Genre Table...
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contains Genre Definitions', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Genre'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary Key for the Genre', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Genre', @level2type=N'COLUMN',@level2name=N'Id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the Genre', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Genre', @level2type=N'COLUMN',@level2name=N'Name'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Genre'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Genre'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Genre'
EXEC sys.sp_addextendedproperty @name=N'Version', @value=N'1.0.0.0', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Genre'
DECLARE @lst as varchar(20)
SET @lst = GETDATE()
EXEC sys.sp_addextendedproperty @name=N'Last Sync Time', @value=@lst, @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Genre'
GO
--Insert values into the Genre Table...
INSERT INTO [dbo].[Genre] VALUES(1,'Action')
INSERT INTO [dbo].[Genre] VALUES(2,'Adventure')
INSERT INTO [dbo].[Genre] VALUES(3,'Animation')
INSERT INTO [dbo].[Genre] VALUES(4,'Biography')
INSERT INTO [dbo].[Genre] VALUES(5,'Comedy')
INSERT INTO [dbo].[Genre] VALUES(6,'Crime')
INSERT INTO [dbo].[Genre] VALUES(7,'Documentary')
INSERT INTO [dbo].[Genre] VALUES(8,'Drama')
INSERT INTO [dbo].[Genre] VALUES(9,'Family')
INSERT INTO [dbo].[Genre] VALUES(10,'Fantasy')
INSERT INTO [dbo].[Genre] VALUES(11,'Film-Noir')
INSERT INTO [dbo].[Genre] VALUES(12,'Game-Show')
INSERT INTO [dbo].[Genre] VALUES(13,'History')
INSERT INTO [dbo].[Genre] VALUES(14,'Horror')
INSERT INTO [dbo].[Genre] VALUES(15,'Music')
INSERT INTO [dbo].[Genre] VALUES(16,'Musical')
INSERT INTO [dbo].[Genre] VALUES(17,'Mystery')
INSERT INTO [dbo].[Genre] VALUES(18,'News')
INSERT INTO [dbo].[Genre] VALUES(19,'Reality-TV')
INSERT INTO [dbo].[Genre] VALUES(20,'Romance')
INSERT INTO [dbo].[Genre] VALUES(21,'Sci-Fi')
INSERT INTO [dbo].[Genre] VALUES(22,'Sport')
INSERT INTO [dbo].[Genre] VALUES(23,'Talk-Show')
INSERT INTO [dbo].[Genre] VALUES(24,'Thriller')
INSERT INTO [dbo].[Genre] VALUES(25,'War')
INSERT INTO [dbo].[Genre] VALUES(26,'Western')