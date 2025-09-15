CREATE TABLE [dbo].[CartItems] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Quantity] INT            NOT NULL,
    [Price]    INT            NOT NULL,
    [UserId]   INT            NOT NULL,
    [FoodId]   INT            DEFAULT ((0)) NOT NULL,
    [FoodName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_CartItems] PRIMARY KEY CLUSTERED ([Id] ASC)
);

