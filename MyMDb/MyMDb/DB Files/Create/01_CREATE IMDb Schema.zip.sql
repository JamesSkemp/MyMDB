--Change script command to MyMDb database...
USE [MyMDb]
GO
--Remove the imdb schema...
IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'imdb')
	DROP SCHEMA [imdb]
GO
--Create the imdb schema...
CREATE SCHEMA [imdb] AUTHORIZATION [dbo]
GO
--Create Documentation for the imdb schema...
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Handles merge information between IMDB and Your database' , @level0type=N'SCHEMA',@level0name=N'imdb'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge' , @level0type=N'SCHEMA',@level0name=N'imdb'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com' , @level0type=N'SCHEMA',@level0name=N'imdb'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge' , @level0type=N'SCHEMA',@level0name=N'imdb'