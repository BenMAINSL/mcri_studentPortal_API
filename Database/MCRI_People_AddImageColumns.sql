/* ============================================================
   MCRI People Portal - store profile images in the database

   Interim setup: images live in the People table instead of a
   Supabase bucket, so the portal works with nothing but LocalDB.
   Safe to run more than once.
   ============================================================ */

USE MCRIPeoplePortal;
GO

IF COL_LENGTH('dbo.People', 'ImageData') IS NULL
    ALTER TABLE dbo.People ADD ImageData VARBINARY(MAX) NULL;
GO

IF COL_LENGTH('dbo.People', 'ImageContentType') IS NULL
    ALTER TABLE dbo.People ADD ImageContentType NVARCHAR(100) NULL;
GO

/* Gender / Cohort / Phase were added after the original setup script. */

IF COL_LENGTH('dbo.People', 'Gender') IS NULL
    ALTER TABLE dbo.People ADD Gender NVARCHAR(50) NULL;
GO

IF COL_LENGTH('dbo.People', 'Cohort') IS NULL
    ALTER TABLE dbo.People ADD Cohort NVARCHAR(100) NULL;
GO

IF COL_LENGTH('dbo.People', 'Phase') IS NULL
    ALTER TABLE dbo.People ADD Phase NVARCHAR(100) NULL;
GO

/* ---------- Check ---------- */

SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'People'
ORDER BY ORDINAL_POSITION;
GO
