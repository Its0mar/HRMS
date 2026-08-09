
CREATE OR ALTER PROCEDURE Roles_GetAll
@OrganizationId AS INT
AS
BEGIN
	SELECT  * FROM ROLES
	WHERE OrganizationId = @OrganizationId
END;

CREATE TYPE dbo.IntIdList AS TABLE
(
    Id INT NOT NULL PRIMARY KEY
);


CREATE UNIQUE INDEX UX_Roles_Organization_Name
ON dbo.Roles(OrganizationId, Name);

CREATE OR ALTER PROCEDURE dbo.Role_Create
    @OrganizationId INT,
    @Name VARCHAR(30),
    @PermissionIds dbo.IntIdList READONLY
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF EXISTS
        (
            SELECT 1
            FROM dbo.Roles
            WHERE OrganizationId = @OrganizationId
              AND Name = @Name
        )
        BEGIN
            THROW 50001, 'A role with this name already exists.', 1;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM @PermissionIds AS requested
            LEFT JOIN dbo.Permissions AS permission
                ON permission.Id = requested.Id
            WHERE permission.Id IS NULL
        )
        BEGIN
            THROW 50002, 'One or more permissions do not exist.', 1;
        END;

        INSERT INTO dbo.Roles
        (
            Name,
            OrganizationId
        )
        VALUES
        (
            @Name,
            @OrganizationId
        );

        DECLARE @RoleId INT = SCOPE_IDENTITY();

        INSERT INTO dbo.RolePermissions
        (
            RoleId,
            PermissionId
        )
        SELECT
            @RoleId,
            Id
        FROM @PermissionIds;

        COMMIT TRANSACTION;

        SELECT @RoleId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;

CREATE OR ALTER PROCEDURE Permissions_GetAll
AS
BEGIN
    SELECT * FROM Permissions;
END;


SELECT * FROM Permissions
SELECT * FROM ROLES
SELECT * FROM RolePermissions
EXEC Permissions_GetAll