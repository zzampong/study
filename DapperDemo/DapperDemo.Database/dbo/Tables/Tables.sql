﻿CREATE TABLE [dbo].[Tables]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Note] NVARCHAR(MAX) NULL, 
    [rDate] DATE NULL DEFAULT getdate() 
)
GO