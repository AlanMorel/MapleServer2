
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/03/2021 19:59:22
-- Generated from EDMX file: D:\Maplestory 2 Emulator\MapleStory 2 Emu\DatabaseHandler\MS2DBModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Maplestory2DB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AccountID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Character] DROP CONSTRAINT [FK_AccountID];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Account]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Account];
GO
IF OBJECT_ID(N'[dbo].[Character]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Character];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Accounts'
CREATE TABLE [dbo].[Accounts] (
    [Account_ID] bigint IDENTITY(1,1) NOT NULL,
    [Character_ID] bigint  NULL,
    [Username] nvarchar(10)  NULL,
    [Password] nvarchar(12)  NULL,
    [Creation_Time] datetime  NOT NULL
);
GO

-- Creating table 'Characters'
CREATE TABLE [dbo].[Characters] (
    [Character_ID] bigint IDENTITY(1,1) NOT NULL,
    [Account_ID] bigint  NOT NULL,
    [Level] smallint  NOT NULL,
    [Name] nvarchar(15)  NULL,
    [Gender] tinyint  NULL,
    [Job_ID] smallint  NULL,
    [Mesos] bigint  NULL,
    [Map_ID] int  NULL,
    [Creation_Time] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Account_ID] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [PK_Accounts]
    PRIMARY KEY CLUSTERED ([Account_ID] ASC);
GO

-- Creating primary key on [Character_ID] in table 'Characters'
ALTER TABLE [dbo].[Characters]
ADD CONSTRAINT [PK_Characters]
    PRIMARY KEY CLUSTERED ([Character_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Account_ID] in table 'Characters'
ALTER TABLE [dbo].[Characters]
ADD CONSTRAINT [FK_AccountID]
    FOREIGN KEY ([Account_ID])
    REFERENCES [dbo].[Accounts]
        ([Account_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountID'
CREATE INDEX [IX_FK_AccountID]
ON [dbo].[Characters]
    ([Account_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------