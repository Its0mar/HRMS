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