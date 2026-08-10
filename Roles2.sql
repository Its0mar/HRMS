SELECT * FROM ROLES
SELECT * FROM RolePermissions
SELECT * FROM Permissions




CREATE OR ALTER PROCEDURE dbo.Roles_GetByOrganization
    @OrganizationId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        r.Id AS RoleId,
        r.Name AS RoleName,
        p.Id AS PermissionId,
        p.Code AS PermissionCode
    FROM dbo.Roles AS r
    LEFT JOIN dbo.RolePermissions AS rp
        ON rp.RoleId = r.Id
    LEFT JOIN dbo.Permissions AS p
        ON p.Id = rp.PermissionId
    WHERE r.OrganizationId = @OrganizationId
    ORDER BY r.Name, p.Code;
END;


CREATE OR ALTER PROCEDURE dbo.Roles_GetById
    @Id INT,
    @OrganizationId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        r.Id AS RoleId,
        r.Name AS RoleName,
        p.Id AS PermissionId,
        p.Code AS PermissionCode
    FROM dbo.Roles AS r
    LEFT JOIN dbo.RolePermissions AS rp
        ON rp.RoleId = r.Id
    LEFT JOIN dbo.Permissions AS p
        ON p.Id = rp.PermissionId
    WHERE r.Id = @Id
      AND r.OrganizationId = @OrganizationId
    ORDER BY p.Code;
END;

CREATE OR ALTER PROCEDURE dbo.Role_Update
    @Id INT,
    @OrganizationId INT,
    @Name VARCHAR(30),
    @PermissionIds dbo.IntIdList READONLY
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Roles
            WHERE Id = @Id
              AND OrganizationId = @OrganizationId
        )
        BEGIN
            THROW 50001, 'The role was not found.', 1;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM dbo.Roles
            WHERE OrganizationId = @OrganizationId
              AND Name = @Name
              AND Id <> @Id
        )
        BEGIN
            THROW 50002, 'A role with this name already exists.', 1;
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
            THROW 50003, 'One or more permissions do not exist.', 1;
        END;

        UPDATE dbo.Roles
        SET Name = @Name
        WHERE Id = @Id
          AND OrganizationId = @OrganizationId;

        DELETE FROM dbo.RolePermissions
        WHERE RoleId = @Id;

        INSERT INTO dbo.RolePermissions
        (
            RoleId,
            PermissionId
        )
        SELECT
            @Id,
            Id
        FROM @PermissionIds;

        COMMIT TRANSACTION;

        SELECT CAST(1 AS INT);
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        THROW;
    END CATCH;
END;