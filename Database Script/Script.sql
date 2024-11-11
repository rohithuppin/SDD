
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
DROP TABLE [dbo].[Users]
GO

CREATE TABLE dbo.Users (
    UserId INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
	EmailId NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(500) NOT NULL,
    CreatedDateTime DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDateTime DATETIME NULL,
	CreatedBy int NULL,
	UpdatedBy int NULL,
    IsActive BIT NOT NULL DEFAULT 1
);
Go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditTrails]') AND type in (N'U'))
DROP TABLE [dbo].AuditTrails
GO
CREATE TABLE AuditTrails (
    AuditId INT PRIMARY KEY IDENTITY,
    [ActionName] NVARCHAR(50),
    UserId INT,
    [Timestamp] DATETIME DEFAULT GETDATE()
);