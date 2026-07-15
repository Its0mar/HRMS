-- CREATE ORG SP

CREATE PROCEDURE SP_CreateOrganization(
	@Name varchar(30),
	@Code varchar(10),
	@Email varchar(40),
	@Address varchar(100) = NULL,
	@Website varchar(100) = NULL,
	@LogoUrl varchar(100) = NULL)
AS
BEGIN
	INSERT INTO Organization (Name, Code, Email, Address, Website, LogoUrl)
	VALUES (@Name, @Code, @Email, @Address, @Website, @LogoUrl);

	SELECT SCOPE_IDENTITY() AS Id;
END;

--create user sp

CREATE PROCEDURE SP_CreateUser(
	@Username VARCHAR(20),
	@Email varchar(40),
	@PasswordHash varchar(MAX),
	@FirstName varchar(20),
	@LastName varchar(20),
	@OrganizationId INT)
AS
BEGIN
	INSERT INTO Users (Username, Email, PasswordHash, FirstName, LastName, OrganizationId)
	VALUES			  (@Username, @Email, @PasswordHash, @FirstName, @LastName, @OrganizationId);

	SELECT SCOPE_IDENTITY() AS Id;
END;

EXEC SP_CreateOrganization @Name = "OR1", @Code = "O1", @Email = "o@gmail.com", @Address = "add"
, @Website = "add.web", @LogoUrl = "logo.url"

SELECT * FROM Users;
SELECT * FROM Organization

DELETE Organization