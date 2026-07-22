CREATE TABLE Employee_Personal_Information(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Employee_Number INT UNIQUE NOT NULL,
	FirstName VARCHAR (20) NOT NULL,
	LastName VARCHAR (20) NOT NULL,
	DateOfBirth DATETIME NOT NULL,
	Gender CHAR NOT NULL,
	NationalId INT NOT NULL UNIQUE,
	Nationality VARCHAR(20) NOT NULL,
	Marital_Status INT NOT NULL,
	Phone VARCHAR(20) UNIQUE NOT NULL,
	Email VARCHAR(30) UNIQUE NOT NULL,
	Address VARCHAR(150) NOT NULL,
	ProfilePictureUrl VARCHAR(100)
);

CREATE TABLE Employee_Employment_Information(
	ID INT PRIMARY KEY,
	DepartmentId INT NOT NULL,
	PositionId INT NOT NULL,
	ManagerId INT,
	HireDate DATETIME NOT NULL,
	EmploymentType int NOT NULL,
	EmploymentStatus int NOT NULL,
	WorkEmail varchar(30) UNIQUE NOT NULL,
	WorkPhone varchar(30) UNIQUE
);

CREATE TABLE Employee_Documents (
		Id INT IDENTITY(1,1) PRIMARY KEY,
		DocumentType int NOT NULL,
		FileName VARCHAR(20) NOT NULL UNIQUE,
		Path VARCHAR(100) NOT NULL UNIQUE,
		UploadedAt DATETIME2 NOT NULL
);

CREATE TABLE Employee_Emergency_Contact(
	ID INT IDENTITY(1,1),
	EmployeeId INT NOT NULL,
	Name VARCHAR(30) NOT NULL,
	Relationship VARCHAR(30) NOT NULL,
	Phone VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE Employee_Notes (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	EmployeeId INT NOT NULL,
	Title VARCHAR(30) NOT NULL,
	NOTE VARCHAR(500) NOT NULL,
	CreatedBy INT NOT NULL,
	CreatedAt DATETIME2 NOT NULL
);

