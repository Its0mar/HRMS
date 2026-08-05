
CREATE OR ALTER PROCEDURE Roles_GetAll
@OrganizationId AS INT
AS
BEGIN
	SELECT Id, Name FROM ROLES
	WHERE OrganizationId = @OrganizationId
END;