CREATE TABLE [dbo].[Airports]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1, 1), 
    [AirportName] VARCHAR(100) NOT NULL, 
    [City] VARCHAR(100) NOT NULL, 
    [Country] VARCHAR(100) NOT NULL, 
    [Latitude] FLOAT NOT NULL, 
    [Longitude] FLOAT NOT NULL
)
