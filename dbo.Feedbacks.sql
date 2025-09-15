CREATE TABLE [dbo].[Feedbacks] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Username]  NVARCHAR (MAX) NULL,
    [CreatedAt] DATETIME2 (7)   NOT NULL,
    [Message]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Feedbacks] PRIMARY KEY CLUSTERED ([Id] ASC)
);

