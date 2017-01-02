
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/02/2017 18:57:56
-- Generated from EDMX file: D:\Dev\FactoriesManager\FactoriesManager\FactoriesManagerModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [FactoriesManager];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_TestToCategories_Category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TestToCategories] DROP CONSTRAINT [FK_TestToCategories_Category];
GO
IF OBJECT_ID(N'[dbo].[FK_TestToCategories_Test]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TestToCategories] DROP CONSTRAINT [FK_TestToCategories_Test];
GO
IF OBJECT_ID(N'[dbo].[FK_TestSourceTestReport]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TestReports] DROP CONSTRAINT [FK_TestSourceTestReport];
GO
IF OBJECT_ID(N'[dbo].[FK_TestReportTestRunResult]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TestRunResults] DROP CONSTRAINT [FK_TestReportTestRunResult];
GO
IF OBJECT_ID(N'[dbo].[FK_TestSourceTest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tests] DROP CONSTRAINT [FK_TestSourceTest];
GO
IF OBJECT_ID(N'[dbo].[FK_Branch_inherits_Tag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tags_Branch] DROP CONSTRAINT [FK_Branch_inherits_Tag];
GO
IF OBJECT_ID(N'[dbo].[FK_Client_inherits_Tag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tags_Client] DROP CONSTRAINT [FK_Client_inherits_Tag];
GO
IF OBJECT_ID(N'[dbo].[FK_Impact_inherits_Tag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tags_Impact] DROP CONSTRAINT [FK_Impact_inherits_Tag];
GO
IF OBJECT_ID(N'[dbo].[FK_Asset_inherits_Tag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tags_Asset] DROP CONSTRAINT [FK_Asset_inherits_Tag];
GO
IF OBJECT_ID(N'[dbo].[FK_Category_inherits_Tag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tags_Category] DROP CONSTRAINT [FK_Category_inherits_Tag];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[TestSources]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TestSources];
GO
IF OBJECT_ID(N'[dbo].[TestReports]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TestReports];
GO
IF OBJECT_ID(N'[dbo].[TestRunResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TestRunResults];
GO
IF OBJECT_ID(N'[dbo].[Tests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tests];
GO
IF OBJECT_ID(N'[dbo].[Tags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tags];
GO
IF OBJECT_ID(N'[dbo].[Tags_Branch]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tags_Branch];
GO
IF OBJECT_ID(N'[dbo].[Tags_Client]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tags_Client];
GO
IF OBJECT_ID(N'[dbo].[Tags_Impact]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tags_Impact];
GO
IF OBJECT_ID(N'[dbo].[Tags_Asset]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tags_Asset];
GO
IF OBJECT_ID(N'[dbo].[Tags_Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tags_Category];
GO
IF OBJECT_ID(N'[dbo].[TestToCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TestToCategories];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'TestSources'
CREATE TABLE [dbo].[TestSources] (
    [Name] varchar(500)  NOT NULL,
    [LastReportId] int  NOT NULL,
    [RowVersion] rowversion  NOT NULL
);
GO

-- Creating table 'TestReports'
CREATE TABLE [dbo].[TestReports] (
    [Name] varchar(500)  NOT NULL,
    [Id] int  NOT NULL,
    [Status] smallint  NOT NULL,
    [LastRunResultId] int  NOT NULL,
    [RowVersion] rowversion  NOT NULL
);
GO

-- Creating table 'TestRunResults'
CREATE TABLE [dbo].[TestRunResults] (
    [SourceName] varchar(500)  NOT NULL,
    [ReportId] int  NOT NULL,
    [Id] int  NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [RowVersion] rowversion  NOT NULL
);
GO

-- Creating table 'Tests'
CREATE TABLE [dbo].[Tests] (
    [FullName] varchar(4000)  NOT NULL,
    [SourceName] varchar(500)  NOT NULL
);
GO

-- Creating table 'Tags'
CREATE TABLE [dbo].[Tags] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Tags_Branch'
CREATE TABLE [dbo].[Tags_Branch] (
    [Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Tags_Client'
CREATE TABLE [dbo].[Tags_Client] (
    [Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Tags_Impact'
CREATE TABLE [dbo].[Tags_Impact] (
    [Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Tags_Asset'
CREATE TABLE [dbo].[Tags_Asset] (
    [Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Tags_Category'
CREATE TABLE [dbo].[Tags_Category] (
    [Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'TestToCategories'
CREATE TABLE [dbo].[TestToCategories] (
    [Tags_Id] uniqueidentifier  NOT NULL,
    [Test_FullName] varchar(4000)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Name] in table 'TestSources'
ALTER TABLE [dbo].[TestSources]
ADD CONSTRAINT [PK_TestSources]
    PRIMARY KEY CLUSTERED ([Name] ASC);
GO

-- Creating primary key on [Id], [Name] in table 'TestReports'
ALTER TABLE [dbo].[TestReports]
ADD CONSTRAINT [PK_TestReports]
    PRIMARY KEY CLUSTERED ([Id], [Name] ASC);
GO

-- Creating primary key on [Id], [ReportId], [SourceName] in table 'TestRunResults'
ALTER TABLE [dbo].[TestRunResults]
ADD CONSTRAINT [PK_TestRunResults]
    PRIMARY KEY CLUSTERED ([Id], [ReportId], [SourceName] ASC);
GO

-- Creating primary key on [FullName] in table 'Tests'
ALTER TABLE [dbo].[Tests]
ADD CONSTRAINT [PK_Tests]
    PRIMARY KEY CLUSTERED ([FullName] ASC);
GO

-- Creating primary key on [Id] in table 'Tags'
ALTER TABLE [dbo].[Tags]
ADD CONSTRAINT [PK_Tags]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tags_Branch'
ALTER TABLE [dbo].[Tags_Branch]
ADD CONSTRAINT [PK_Tags_Branch]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tags_Client'
ALTER TABLE [dbo].[Tags_Client]
ADD CONSTRAINT [PK_Tags_Client]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tags_Impact'
ALTER TABLE [dbo].[Tags_Impact]
ADD CONSTRAINT [PK_Tags_Impact]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tags_Asset'
ALTER TABLE [dbo].[Tags_Asset]
ADD CONSTRAINT [PK_Tags_Asset]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tags_Category'
ALTER TABLE [dbo].[Tags_Category]
ADD CONSTRAINT [PK_Tags_Category]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Tags_Id], [Test_FullName] in table 'TestToCategories'
ALTER TABLE [dbo].[TestToCategories]
ADD CONSTRAINT [PK_TestToCategories]
    PRIMARY KEY CLUSTERED ([Tags_Id], [Test_FullName] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Tags_Id] in table 'TestToCategories'
ALTER TABLE [dbo].[TestToCategories]
ADD CONSTRAINT [FK_TestToCategories_Category]
    FOREIGN KEY ([Tags_Id])
    REFERENCES [dbo].[Tags]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Test_FullName] in table 'TestToCategories'
ALTER TABLE [dbo].[TestToCategories]
ADD CONSTRAINT [FK_TestToCategories_Test]
    FOREIGN KEY ([Test_FullName])
    REFERENCES [dbo].[Tests]
        ([FullName])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TestToCategories_Test'
CREATE INDEX [IX_FK_TestToCategories_Test]
ON [dbo].[TestToCategories]
    ([Test_FullName]);
GO

-- Creating foreign key on [Name] in table 'TestReports'
ALTER TABLE [dbo].[TestReports]
ADD CONSTRAINT [FK_TestSourceTestReport]
    FOREIGN KEY ([Name])
    REFERENCES [dbo].[TestSources]
        ([Name])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TestSourceTestReport'
CREATE INDEX [IX_FK_TestSourceTestReport]
ON [dbo].[TestReports]
    ([Name]);
GO

-- Creating foreign key on [ReportId], [SourceName] in table 'TestRunResults'
ALTER TABLE [dbo].[TestRunResults]
ADD CONSTRAINT [FK_TestReportTestRunResult]
    FOREIGN KEY ([ReportId], [SourceName])
    REFERENCES [dbo].[TestReports]
        ([Id], [Name])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TestReportTestRunResult'
CREATE INDEX [IX_FK_TestReportTestRunResult]
ON [dbo].[TestRunResults]
    ([ReportId], [SourceName]);
GO

-- Creating foreign key on [SourceName] in table 'Tests'
ALTER TABLE [dbo].[Tests]
ADD CONSTRAINT [FK_TestSourceTest]
    FOREIGN KEY ([SourceName])
    REFERENCES [dbo].[TestSources]
        ([Name])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TestSourceTest'
CREATE INDEX [IX_FK_TestSourceTest]
ON [dbo].[Tests]
    ([SourceName]);
GO

-- Creating foreign key on [Id] in table 'Tags_Branch'
ALTER TABLE [dbo].[Tags_Branch]
ADD CONSTRAINT [FK_Branch_inherits_Tag]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Tags]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Tags_Client'
ALTER TABLE [dbo].[Tags_Client]
ADD CONSTRAINT [FK_Client_inherits_Tag]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Tags]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Tags_Impact'
ALTER TABLE [dbo].[Tags_Impact]
ADD CONSTRAINT [FK_Impact_inherits_Tag]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Tags]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Tags_Asset'
ALTER TABLE [dbo].[Tags_Asset]
ADD CONSTRAINT [FK_Asset_inherits_Tag]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Tags]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Tags_Category'
ALTER TABLE [dbo].[Tags_Category]
ADD CONSTRAINT [FK_Category_inherits_Tag]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Tags]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------