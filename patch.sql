-- Patch: create Users table and add missing Game columns
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Users](
        [UserID] [int] NOT NULL,
        [Name] [varchar](100) NOT NULL,
        [Email] [varchar](100) NOT NULL,
        [PasswordHash] [varchar](255) NULL,
        [Age] [int] NULL,
        [ProfileInfo] [text] NULL,
        [RoleID] [int] NULL,
     CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserID] ASC)
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

-- Add UserID to Memberships if it doesn't exist
IF COL_LENGTH('dbo.Memberships','UserID') IS NULL
    ALTER TABLE [dbo].[Memberships] ADD [UserID] [int] NULL;

-- Add matching foreign key and unique index for the EF relationship
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK__Membershi__UserI__59FA5E80')
BEGIN
    ALTER TABLE [dbo].[Memberships] WITH CHECK ADD CONSTRAINT [FK__Membershi__UserI__59FA5E80]
        FOREIGN KEY([UserID]) REFERENCES [dbo].[Users] ([UserID]);
    ALTER TABLE [dbo].[Memberships] CHECK CONSTRAINT [FK__Membershi__UserI__59FA5E80];
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ__Membersh__1788CCADDB5B030F' AND object_id = OBJECT_ID('dbo.Memberships'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [UQ__Membersh__1788CCADDB5B030F]
        ON [dbo].[Memberships] ([UserID] ASC)
        WHERE ([UserID] IS NOT NULL);
END

-- Add columns to Games if they don't exist
IF COL_LENGTH('dbo.Games','DeveloperName') IS NULL
    ALTER TABLE [dbo].[Games] ADD [DeveloperName] [varchar](10) NULL;
IF COL_LENGTH('dbo.Games','Genre') IS NULL
    ALTER TABLE [dbo].[Games] ADD [Genre] [varchar](50) NULL;
IF COL_LENGTH('dbo.Games','IsAvailableOnMembership') IS NULL
    ALTER TABLE [dbo].[Games] ADD [IsAvailableOnMembership] [text] NULL;
