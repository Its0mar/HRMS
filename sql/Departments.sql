CREATE TABLE Departments(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name varchar(30) UNIQUE,
	Code varchar(6) UNIQUE,
	Description varchar(300),
	OrganizationId INT, 
	--todo : ManagerEmployeeId 
	IsActive BIT default(1),
	IsDeleted BIT DEFAULT(0),
	CreatedAt DATETIME2 DEFAULT(GETDATE()),
	UpdatedAt DATETIME2


    CONSTRAINT fk_Org_Dep
		FOREIGN KEY (OrganizationId)
		REFERENCES Organizations(Id)
		ON DELETE CASCADE
);



CREATE OR ALTER PROCEDURE SP_CreateDepartment
@Name varchar(30),
@Code varchar(6),
@Description varchar(300) = NULL,
@OrganizationId INT
AS
BEGIN
	INSERT INTO Departments (Name, Code, Description, OrganizationId)
	VALUES (@Name, @Code, @Description, @OrganizationId);
	
	SELECT SCOPE_IDENTITY();
END;



CREATE OR ALTER PROCEDURE dbo.Departments_NameExist
	@Name varchar(30),
    @OrganizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	    SELECT CAST(
        CASE
            WHEN EXISTS
            (
                SELECT 1
                FROM Departments
                WHERE Name = @Name
                  AND IsDeleted = 0
                  AND OrganizationId = @OrganizationId
            )
            THEN 1
            ELSE 0
        END
    AS BIT);
END;


CREATE OR ALTER PROCEDURE dbo.Departments_CodeExist
	@Code varchar(6),
    @OrganizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	    SELECT CAST(
        CASE
            WHEN EXISTS
            (
                SELECT 1
                FROM Departments
                WHERE Code = @Code
                  AND IsDeleted = 0
                  AND OrganizationId = @OrganizationId
            )
            THEN 1
            ELSE 0
        END
    AS BIT);
END;

SELECT * FROM USERS;
SELECT * FROM Organizations
SELECT * FROM Departments