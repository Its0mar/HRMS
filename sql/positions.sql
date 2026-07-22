CREATE TABLE Positions(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Title varchar(20) NOT NULL,
	Description varchar(300),
	IsActive BIT DEFAULT(1),
	IsDeleted BIT DEFAULT(0),
	OrganizationId INT NOT NULL
	--Level

	CONSTRAINT FK_POS_ORG
		FOREIGN KEY (OrganizationId)
		REFERENCES Organizations(Id)
		ON DELETE CASCADE
);

CREATE OR ALTER PROCEDURE Positions_Create
@Title varchar(20),
@Description varchar(300) = NULL,
@OrganizationId INT

AS
BEGIN
	INSERT INTO Positions (Title, Description, OrganizationId)
	VALUES (@Title, @Description, @OrganizationId);

	SELECT SCOPE_IDENTITY();
END;


CREATE OR ALTER PROCEDURE Positions_GetAll
@OrganizationId INT

AS
BEGIN
	SELECT * FROM Positions 
	WHERE OrganizationId = @OrganizationId
END;



CREATE OR ALTER PROCEDURE dbo.Positions_TitleExist
	@Title varchar(20),
    @OrganizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	    SELECT CAST(
        CASE
            WHEN EXISTS
            (
                SELECT 1
                FROM Positions
                WHERE Title = @Title
                  AND IsDeleted = 0
                  AND OrganizationId = @OrganizationId
            )
            THEN 1
            ELSE 0
        END
    AS BIT);
END;

SELECT * FROM Positions;