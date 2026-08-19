/* ============================================================
   MCRI People Portal - local SQL Server setup
   Run this in SSMS, then scaffold the model from it.
   ============================================================ */

IF DB_ID('MCRIPeoplePortal') IS NULL
    CREATE DATABASE MCRIPeoplePortal;
GO

USE MCRIPeoplePortal;
GO

/* ---------- Table ---------- */

DROP TABLE IF EXISTS dbo.People;
GO

CREATE TABLE dbo.People
(
    Id                    INT            IDENTITY(1,1) NOT NULL,
    FirstName             NVARCHAR(100)  NOT NULL,
    LastName              NVARCHAR(100)  NOT NULL,
    PersonType            NVARCHAR(20)   NOT NULL,
    DepartmentOrProgramme NVARCHAR(150)  NOT NULL,
    Email                 NVARCHAR(200)  NOT NULL,
    FunFact               NVARCHAR(500)  NULL,
    ImageUrl              NVARCHAR(500)  NULL,

    CONSTRAINT PK_People PRIMARY KEY (Id),
    CONSTRAINT CK_People_PersonType CHECK (PersonType IN ('Student', 'Employee'))
);
GO

/* Helps the "filter by Student or Employee" feature. */
CREATE INDEX IX_People_PersonType ON dbo.People (PersonType);
GO

/* ---------- Seed data ----------
   Placeholders. Replace with the real MCRI students and employees so
   participants see themselves when they open the app. */

INSERT INTO dbo.People (FirstName, LastName, PersonType, DepartmentOrProgramme, Email, FunFact)
VALUES
    ('Benon',   'Marava',    'Employee', 'Software Engineering',             'benon.marava@mcri.org',              'Runs the SDLC workshop this portal was built for.'),
    ('Tendai',  'Moyo',      'Employee', 'Data & Analytics',                'tendai.moyo@mcri.org',               'Can name every SQL join type from memory.'),
    ('Grace',   'Chikwanha', 'Employee', 'Human Resources',                 'grace.chikwanha@mcri.org',           'Has interviewed more than 500 candidates.'),
    ('Farai',   'Nyathi',    'Employee', 'Research Operations',             'farai.nyathi@mcri.org',              'Keeps a spreadsheet of every coffee tried.'),
    ('Rudo',    'Mutasa',    'Student',  'BSc Computer Science',            'rudo.mutasa@students.mcri.org',      'Built a first website at age eleven.'),
    ('Kudzai',  'Banda',     'Student',  'BSc Information Systems',         'kudzai.banda@students.mcri.org',     'Plays bass in a weekend band.'),
    ('Nyasha',  'Dube',      'Student',  'BSc Data Science',                'nyasha.dube@students.mcri.org',      'Once won a hackathon with a team of one.'),
    ('Tapiwa',  'Sibanda',   'Student',  'Diploma in Software Development', 'tapiwa.sibanda@students.mcri.org',   'Speaks four languages.'),
    ('Chiedza', 'Marufu',    'Student',  'BSc Computer Science',            'chiedza.marufu@students.mcri.org',   'Volunteers teaching kids to code on Saturdays.'),
    ('Simba',   'Ncube',     'Student',  'BSc Information Systems',         'simba.ncube@students.mcri.org',      'Collects vintage keyboards.');
GO

/* ---------- Quick checks, one per endpoint ---------- */

-- GET api/people
SELECT * FROM dbo.People ORDER BY FirstName, LastName;

-- GET api/people?search=gra
SELECT * FROM dbo.People
WHERE FirstName LIKE '%gra%' OR LastName LIKE '%gra%';

-- GET api/people?personType=Student
SELECT * FROM dbo.People WHERE PersonType = 'Student';

-- GET api/people/1
SELECT * FROM dbo.People WHERE Id = 1;
GO
