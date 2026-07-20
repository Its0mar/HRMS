CREATE TABLE Departments(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name varchar(30),
	Code varchar(6),
	Description varchar(300),
	OrganizationId INT, 
	ManagerEmployeeId INT,
	IsActive BIT default(1),
	IsDeleted BIT DEFAULT(0),
	CreatedAt DATETIME2 DEFAULT(GETDATE()),
	UpdatedAt DATETIME2


    CONSTRAINT fk_Org_Dep
		FOREIGN KEY (OrganizationId)
		REFERENCES Organizations(Id)
		ON DELETE CASCADE
);
ALTER TABLE Departments
ADD CONSTRAINT fk_Manager_Dep
		FOREIGN KEY (ManagerEmployeeId)
		REFERENCES Users(Id)
		


CREATE OR ALTER PROCEDURE SP_CreateDepartment
@Name varchar(30),
@Code varchar(6),
@Description varchar(300) = NULL,
@ManagerEmployeeId INT = NULL,
@OrganizationId INT
AS
BEGIN
	INSERT INTO Departments (Name, Code, Description, ManagerEmployeeId, OrganizationId)
	VALUES (@Name, @Code, @Description,@ManagerEmployeeId, @OrganizationId);
	
	SELECT SCOPE_IDENTITY();
END;

CREATE OR ALTER PROCEDURE dbo.Departments_Update
@Id INT,
@Name varchar(30) = NULL,
@Description varchar(300) = NULL,
@ManagerEmployeeId INT = NULL,
@OrganizationId INT
AS
BEGIN
	UPDATE Departments
    SET Name = @Name,
    Description = @Description,
    ManagerEmployeeId = @ManagerEmployeeId
    WHERE Id = @Id and OrganizationId = @OrganizationId;
	
END;


CREATE OR ALTER PROCEDURE dbo.Departments_GetById
@Id INT,
@OrganizationId INT
AS
BEGIN
	SELECT * FROM Departments
    WHERE Id = @Id and OrganizationId = @OrganizationId;
	
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


EXEC Departments_GetById @Id = 2,@OrganizationId = 21;