-- Module 1: Authentication & Authorization

CREATE TABLE Organization(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name varchar(30) UNIQUE NOT NULL,
	Code varchar(10) UNIQUE NOT NULL,
	Email varchar(40) UNIQUE NOT NULL,
	Address varchar(100),
	Website varchar(100),
	LogoUrl varchar(100),

	IsActive BIT NOT NULL Default(1),
	IsDeleted BIT NOT NULL Default(0),

	CreatedBy INT,
	UpdatedBy INT,
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedAt DATETIME2
	
)


CREATE TABLE Users (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Username VARCHAR(20) UNIQUE NOT NULL,
	Email varchar(40) UNIQUE NOT NULL,
	PasswordHash varchar(MAX) NOT NULL,
	FirstName varchar(20) NOT NULL,
	LastName varchar(20) NOT NULL,
	IsActive BIT NOT NULL Default(1),
	IsDeleted BIT NOT NULL Default(0),
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	UpdatedAt DATETIME2,

	OrgId INT NOT NULL,

	CONSTRAINT fk_Org
		FOREIGN KEY (OrgId)
		REFERENCES Organization(Id)
		ON DELETE CASCADE

)


CREATE TABLE RefreshTokens (
	Id INT IDENTITY(1,1) PRIMARY KEY,

	UserId INT NOT NULL
	,
	CONSTRAINT fk_User
		FOREIGN KEY (UserId)
		REFERENCES USERS(Id),

	Token varchar(MAX) NOT NULL,
	ExpiresAt DATETIME2 NOT NULL,
	RevokedAt DATETIME2,
	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
)

CREATE TABLE Roles(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name varchar(30) NOT NULL,

	OrgId INT NOT NULL,

	CONSTRAINT fk_OrgId
		FOREIGN KEY (OrgId)
		REFERENCES Organization(Id)
)

CREATE TABLE Permissions(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name varchar(50) NOT NULL,
)

CREATE TABLE RolePermissions
(
    RoleId INT NOT NULL,
    PermissionId INT NOT NULL,

    PRIMARY KEY(RoleId, PermissionId),

    FOREIGN KEY(RoleId)
        REFERENCES Roles(Id)
        ON DELETE CASCADE,

    FOREIGN KEY(PermissionId)
        REFERENCES Permissions(Id)
        ON DELETE CASCADE
)


CREATE TABLE UserRoles(
	UserId INT NOT NULL,
	RoleId INT NOT NULL,

	CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT PK_UserRoles 
        PRIMARY KEY (UserId, RoleId),

    CONSTRAINT FK_UserRoles_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(Id)
        ON DELETE CASCADE,

    CONSTRAINT FK_UserRoles_Roles
        FOREIGN KEY (RoleId)
        REFERENCES Roles(Id)
        ON DELETE CASCADE
)

