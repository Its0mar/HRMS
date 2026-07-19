--Check organization code
CREATE OR ALTER PROCEDURE dbo.Organization_CodeExists
    @Code VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CAST(
        CASE
            WHEN EXISTS
            (
                SELECT 1
                FROM Organizations
                WHERE Code = @Code
                  AND IsDeleted = 0
            )
            THEN 1
            ELSE 0
        END
    AS BIT);
END;



--Check organization email
CREATE OR ALTER PROCEDURE dbo.Organization_EmailExists
    @Email VARCHAR(40)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CAST(
        CASE
            WHEN EXISTS
            (
                SELECT 1
                FROM Organizations
                WHERE Email = @Email
                  AND IsDeleted = 0
            )
            THEN 1
            ELSE 0
        END
    AS BIT);
END;

--Check user email
CREATE OR ALTER PROCEDURE dbo.User_EmailExists
    @Email VARCHAR(40)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CAST(
        CASE
            WHEN EXISTS
            (
                SELECT 1
                FROM Users
                WHERE Email = @Email
                  AND IsDeleted = 0
            )
            THEN 1
            ELSE 0
        END
    AS BIT);
END;

--Check username
CREATE OR ALTER PROCEDURE dbo.User_UsernameExists
    @Username VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CAST(
        CASE
            WHEN EXISTS
            (
                SELECT 1
                FROM Users
                WHERE Username = @Username
                  AND IsDeleted = 0
            )
            THEN 1
            ELSE 0
        END
    AS BIT);
END;

--Create organization
CREATE OR ALTER PROCEDURE dbo.Organization_Create
    @Name VARCHAR(30),
    @Code VARCHAR(10),
    @Email VARCHAR(40),
    @Address VARCHAR(100) = NULL,
    @Website VARCHAR(100) = NULL,
    @LogoUrl VARCHAR(100) = NULL,
    @IsActive BIT,
    @IsDeleted BIT,
    @CreatedAt DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Organizations
    (
        Name,
        Code,
        Email,
        Address,
        Website,
        LogoUrl,
        IsActive,
        IsDeleted,
        CreatedAt
    )
    VALUES
    (
        @Name,
        @Code,
        @Email,
        @Address,
        @Website,
        @LogoUrl,
        @IsActive,
        @IsDeleted,
        @CreatedAt
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;


--Create owner user
CREATE OR ALTER PROCEDURE dbo.User_Create
    @OrganizationId INT,
    @Username VARCHAR(20),
    @Email VARCHAR(40),
    @PasswordHash VARCHAR(MAX),
    @FirstName VARCHAR(20),
    @LastName VARCHAR(20),
    @IsActive BIT,
    @IsDeleted BIT,
    @CreatedAt DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Users
    (
        OrganizationId,
        Username,
        Email,
        PasswordHash,
        FirstName,
        LastName,
        IsActive,
        IsDeleted,
        CreatedAt
    )
    VALUES
    (
        @OrganizationId,
        @Username,
        @Email,
        @PasswordHash,
        @FirstName,
        @LastName,
        @IsActive,
        @IsDeleted,
        @CreatedAt
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;


--Create organization-owner role
CREATE OR ALTER PROCEDURE dbo.Role_Create
    @OrganizationId INT,
    @Name VARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Roles
    (
        OrganizationId,
        Name
    )
    VALUES
    (
        @OrganizationId,
        @Name
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;



ALTER TABLE Roles
ADD CONSTRAINT UQ_Roles_OrganizationId_Name
UNIQUE (OrganizationId, Name);
