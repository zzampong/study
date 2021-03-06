﻿/*
DapperDemoDB의 배포 스크립트

이 코드는 도구를 사용하여 생성되었습니다.
파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
변경 내용이 손실됩니다.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "DapperDemoDB"
:setvar DefaultFilePrefix "DapperDemoDB"
:setvar DefaultDataPath "C:\Program Files (x86)\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files (x86)\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\"

GO
:on error exit
GO
/*
SQLCMD 모드가 지원되지 않으면 SQLCMD 모드를 검색하고 스크립트를 실행하지 않습니다.
SQLCMD 모드를 설정한 후에 이 스크립트를 다시 사용하려면 다음을 실행합니다.
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'이 스크립트를 실행하려면 SQLCMD 모드를 사용하도록 설정해야 합니다.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'[dbo].[GetTablesWithPaging]을(를) 만드는 중...';


GO
CREATE PROCEDURE [dbo].[GetTablesWithPaging]
	@PageIndex int = 0,		-- 페이지 인덱스  0,1,2,3....
	@PageSize int = 5		-- 한페이지에 표시할 레코드수
AS
	SELECT [Id],[Note] From (Select ROW_NUMBER() Over (Order By Id Desc) As RowNumbers, Id, Note From Tables)
	As TempRowTables
	Where RowNumbers Between(@PageIndex) * @PageSize + 1 And (@PageIndex + 1) * @PageSize
GO
PRINT N'업데이트가 완료되었습니다.';


GO
