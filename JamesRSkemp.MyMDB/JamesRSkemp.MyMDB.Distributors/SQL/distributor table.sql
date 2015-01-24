/****** Object:  Table [dbo].[Distributor]    Script Date: 1/6/2015 8:24:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

--drop table [Distributor]

CREATE TABLE [dbo].[Distributor](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RawData] [varchar](1200) NOT NULL,
	[Title] [varchar](500) NOT NULL,
	[YearPlayed] [varchar](50) NULL,
	[EpisodeName] [varchar](200) NULL,
	[Distributor] [varchar](200) NULL,
	[CountryCode] [varchar](50) NULL,
	[YearDistributed] [varchar](50) NULL,
	[Country] [varchar](50) NULL,
	[Format] [varchar](50) NULL,
 CONSTRAINT [PK_Distributor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


