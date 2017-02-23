CREATE PROCEDURE [dbo].[GetTablesWithPaging]
	@PageIndex int = 0,		-- 페이지 인덱스  0,1,2,3....
	@PageSize int = 5		-- 한페이지에 표시할 레코드수
AS
	SELECT [Id],[Note] From (Select ROW_NUMBER() Over (Order By Id Desc) As RowNumbers, Id, Note From Tables)
	As TempRowTables
	Where RowNumbers Between(@PageIndex) * @PageSize + 1 And (@PageIndex + 1) * @PageSize

Go