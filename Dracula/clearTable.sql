DROP TABLE draco;

CREATE TABLE [dbo].[draco] (
    [Id]        INT            NOT NULL IDENTITY,
    [Text]      NVARCHAR (MAX) NOT NULL,
    [Author]    NVARCHAR (50)  NOT NULL,
    [Date]      DATE           NOT NULL,
    [Medium]    NVARCHAR (50)  NOT NULL,
    [Recipient] NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

