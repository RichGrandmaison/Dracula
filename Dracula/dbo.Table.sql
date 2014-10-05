CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Text] NVARCHAR(MAX) NOT NULL, 
    [Author] NVARCHAR(50) NOT NULL, 
    [Date] DATE NOT NULL, 
    [Medium] NVARCHAR(50) NOT NULL, 
    [Recipient] NVARCHAR(50) NULL
)
