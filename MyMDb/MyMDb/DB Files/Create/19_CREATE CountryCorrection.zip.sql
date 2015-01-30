--Change script command to MyMDb database...
USE [MyMDb]
GO
--Remove CountryCorrection and its assosicated objects...
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CountryCorrection]') AND type in (N'U'))
	DROP TABLE [imdb].[CountryCorrection]
GO
--Create the CountryCorrection Table...
CREATE TABLE [imdb].[CountryCorrection]
(
	IncorrectValue varchar(80) NOT NULL,
	CorrectValue char(3) NOT NULL,
	CONSTRAINT PK_CountryCorrection PRIMARY KEY CLUSTERED 
	(
		IncorrectValue
	)
)
GO
--Create Documentation for the CountryCorrection Table...
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IMDb''s incorrect country value (NON ISO3166 valid)', @level0type=N'SCHEMA',@level0name=N'imdb', @level1type=N'TABLE',@level1name=N'CountryCorrection', @level2type=N'COLUMN', @level2name=N'IncorrectValue'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Correct ISO3166 value', @level0type=N'SCHEMA',@level0name=N'imdb', @level1type=N'TABLE',@level1name=N'CountryCorrection', @level2type=N'COLUMN', @level2name=N'CorrectValue'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contains vital information to correct IMDbs incorrect Country standard (NON ISO3166)', @level0type=N'SCHEMA',@level0name=N'imdb', @level1type=N'TABLE',@level1name=N'CountryCorrection'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary Clustered Key/Index for the CountryCorrection' , @level0type=N'SCHEMA',@level0name=N'imdb', @level1type=N'TABLE', @level1name=N'CountryCorrection', @level2type=N'CONSTRAINT', @level2name=N'PK_CountryCorrection'
EXEC sys.sp_addextendedproperty @name=N'Author', @value=N'Paw Jershauge' , @level0type=N'SCHEMA',@level0name=N'imdb', @level1type=N'TABLE',@level1name=N'CountryCorrection'
EXEC sys.sp_addextendedproperty @name=N'Blog', @value=N'http://PawJershauge.blogspot.com' , @level0type=N'SCHEMA',@level0name=N'imdb', @level1type=N'TABLE',@level1name=N'CountryCorrection'
EXEC sys.sp_addextendedproperty @name=N'CodeProject.com', @value=N'http://www.codeproject.com/Members/Paw-Jershauge' , @level0type=N'SCHEMA',@level0name=N'imdb', @level1type=N'TABLE',@level1name=N'CountryCorrection'
EXEC sys.sp_addextendedproperty @name=N'Version', @value=N'1.0.0.0', @level0type=N'SCHEMA',@level0name=N'imdb', @level1type=N'TABLE',@level1name=N'CountryCorrection'
GO
--Insetring known values into the CountryCorrection Table...
INSERT INTO [imdb].[CountryCorrection] VALUES('Bosnia and Herzegovina','BIH')
INSERT INTO [imdb].[CountryCorrection] VALUES('Burma','MMR')
INSERT INTO [imdb].[CountryCorrection] VALUES('Croatia','HRV')
INSERT INTO [imdb].[CountryCorrection] VALUES('Czechoslovakia','CZE')
INSERT INTO [imdb].[CountryCorrection] VALUES('Democratic Republic of Congo','COG')
INSERT INTO [imdb].[CountryCorrection] VALUES('Democratic Republic of the Congo','COG')
INSERT INTO [imdb].[CountryCorrection] VALUES('Deutschland','DEU')
INSERT INTO [imdb].[CountryCorrection] VALUES('East Germany','DEU')
INSERT INTO [imdb].[CountryCorrection] VALUES('Federal Republic of Yugoslavia','YUG')
INSERT INTO [imdb].[CountryCorrection] VALUES('Federated States of Micronesia','FSM')
INSERT INTO [imdb].[CountryCorrection] VALUES('Iran','IRN')
INSERT INTO [imdb].[CountryCorrection] VALUES('Ivory Coast','CIV')
INSERT INTO [imdb].[CountryCorrection] VALUES('Korea','KOR')
INSERT INTO [imdb].[CountryCorrection] VALUES('Laos','LAO')
INSERT INTO [imdb].[CountryCorrection] VALUES('Libya','LBY')
INSERT INTO [imdb].[CountryCorrection] VALUES('Macao','MAC')
INSERT INTO [imdb].[CountryCorrection] VALUES('Moldova','MDA')
INSERT INTO [imdb].[CountryCorrection] VALUES('North Korea','PRK')
INSERT INTO [imdb].[CountryCorrection] VALUES('North Vietnam','VNM')
INSERT INTO [imdb].[CountryCorrection] VALUES('Occupied Palestinian Territory','PSE')
INSERT INTO [imdb].[CountryCorrection] VALUES('Palestine','PSE')
INSERT INTO [imdb].[CountryCorrection] VALUES('Republic of Macedonia','MKD')
INSERT INTO [imdb].[CountryCorrection] VALUES('Russia','RUS')
INSERT INTO [imdb].[CountryCorrection] VALUES('Saint Helena','SHN')
INSERT INTO [imdb].[CountryCorrection] VALUES('Serbia and Montenegro','SRB')
INSERT INTO [imdb].[CountryCorrection] VALUES('Siam','THA')
INSERT INTO [imdb].[CountryCorrection] VALUES('Slovakia','SVK')
INSERT INTO [imdb].[CountryCorrection] VALUES('South Korea','KOR')
INSERT INTO [imdb].[CountryCorrection] VALUES('Soviet Union','RUS')
INSERT INTO [imdb].[CountryCorrection] VALUES('Syria','SYR')
INSERT INTO [imdb].[CountryCorrection] VALUES('Taiwan','TWN')
INSERT INTO [imdb].[CountryCorrection] VALUES('Tanzania','TZA')
INSERT INTO [imdb].[CountryCorrection] VALUES('U.S. Virgin Islands','VIR')
INSERT INTO [imdb].[CountryCorrection] VALUES('UK','GBR')
INSERT INTO [imdb].[CountryCorrection] VALUES('Vietnam','VNM')
INSERT INTO [imdb].[CountryCorrection] VALUES('West Germany','DEU')