/*
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
/*
[dbo].[Tables] 테이블의 Note 열을 NULL에서 NOT NULL로 변경해야 합니다. 테이블에 데이터가 있으면 ALTER 스크립트가 실행되지 않습니다. 이러한 문제를 방지하려면 모든 행에 대해 이 열에 값을 추가 또는 해당 열을 NULL 값을 허용하도록 표시하거나 스마트 기본값을 배포 옵션으로서 생성할 수 있도록 하십시오.

[dbo].[Tables] 테이블의 rDate 열을 NULL에서 NOT NULL로 변경해야 합니다. 테이블에 데이터가 있으면 ALTER 스크립트가 실행되지 않습니다. 이러한 문제를 방지하려면 모든 행에 대해 이 열에 값을 추가 또는 해당 열을 NULL 값을 허용하도록 표시하거나 스마트 기본값을 배포 옵션으로서 생성할 수 있도록 하십시오.
*/

IF EXISTS (select top 1 1 from [dbo].[Tables])
    RAISERROR (N'행이 발견되었습니다. 데이터가 손실될 수 있으므로 스키마 업데이트가 종료됩니다.', 16, 127) WITH NOWAIT

GO
PRINT N'[dbo].[Tables] 변경 중...';


GO
ALTER TABLE [dbo].[Tables] ALTER COLUMN [Note] NVARCHAR (MAX) NOT NULL;

ALTER TABLE [dbo].[Tables] ALTER COLUMN [rDate] DATE NOT NULL;


GO
PRINT N'[dbo].[Tables]에 대한 명명되지 않은 제약 조건을(를) 만드는 중...';


GO
ALTER TABLE [dbo].[Tables]
    ADD DEFAULT getdate() FOR [rDate];


GO
PRINT N'업데이트가 완료되었습니다.';


GO
