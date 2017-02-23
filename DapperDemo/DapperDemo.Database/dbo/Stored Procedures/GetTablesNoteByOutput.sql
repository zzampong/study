CREATE PROCEDURE [dbo].[GetTablesNoteByOutput]
	@Id int = 0,
	@Note NVarChar(Max) Output
AS
	SELECT @Note = Note From Tables Where Id=@Id
Go
