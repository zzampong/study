CREATE PROCEDURE [dbo].[GetTableById]
@Id int
As
	Select [Id],[Note] From Tables Where Id=@Id
	
Go
