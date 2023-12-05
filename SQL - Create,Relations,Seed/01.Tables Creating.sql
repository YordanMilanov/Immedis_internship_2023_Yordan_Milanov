 --Create Database HCMSImmedis;
 --use HCMSImmedis;

--TABLES

--Authentication
CREATE TABLE "Users" (
    "Id" UNIQUEIDENTIFIER PRIMARY KEY, --GUID
	"Username" VARCHAR(50) NOT NULL UNIQUE,
	"Password" VARCHAR(1000) NOT NULL,
	"Email" VARCHAR(100) NOT NULL UNIQUE,
	"RegisterDate" DATETIME NOT NULL
);

--Roles
CREATE TABLE "Roles" (
    "Id" INT PRIMARY KEY,
    "Name" VARCHAR(50) NOT NULL UNIQUE,
	"Description" varchar(max) NULL,
);

--Info about Workers / Employees
CREATE TABLE "Employees"(
	"Id" UNIQUEIDENTIFIER PRIMARY KEY, --GUID
	"FirstName" VARCHAR(50) NOT NULL,
    "LastName" VARCHAR(50) NOT NULL,
	"Email" VARCHAR(100) NOT NULL UNIQUE,
	"PhoneNumber" VARCHAR(50) NOT NULL UNIQUE,
	"PhotoURL" varchar(max) NULL,
    "DateOfBirth" DATETIME NOT NULL,
	"AddDate" DATETIME NOT NULL,
	"CompanyId" UNIQUEIDENTIFIER NULL,
	"UserId" UNIQUEIDENTIFIER NULL,
	"LocationId" UNIQUEIDENTIFIER NULL,
);

-- Info about employers / companies
CREATE TABLE "Companies" (
	"Id" UNIQUEIDENTIFIER PRIMARY KEY, --GUID
    "Name" VARCHAR(50) NOT NULL UNIQUE,
	"Description" varchar(max) NULL,
	"IndustryField" VARCHAR(50) NULL,
	"LocationId" UNIQUEIDENTIFIER NULL,
);

-- Info about experience of particular employee
CREATE TABLE "WorkRecords" (
	"Id" UNIQUEIDENTIFIER PRIMARY KEY, --GUID
	"Position" VARCHAR(50) NOT NULL,
	"Department" VARCHAR(50) NULL,
    "Salary" DECIMAL(19, 2) NOT NULL,
	"StartDate" DATETIME NOT NULL,
	"EndDate" DATETIME NULL, --If Null - currently at that company
	"AddDate" DATETIME NOT NULL,
	"CompanyId" UNIQUEIDENTIFIER NOT NULL,
	"EmployeeId" UNIQUEIDENTIFIER NOT NULL,
);

CREATE TABLE "Adverts" (
		"Id" UNIQUEIDENTIFIER PRIMARY KEY, --GUID
		"Position" VARCHAR(50) NOT NULL,
	    "Department" VARCHAR(50) NOT NULL,
		"Description" TEXT NOT NULL,
		"RemoteOption" BIT NOT NULL, 
		"Salary" DECIMAL(19, 2) NOT NULL,
		"AddDate" DATETIME NOT NULL,
		"CompanyId" UNIQUEIDENTIFIER NOT NULL,
);

CREATE TABLE "Applications" (
	"Id" UNIQUEIDENTIFIER PRIMARY KEY, --GUID
    "CoverLetter" varchar(max) NULL,
	"AddDate" DATETIME NOT NULL,
	"AdvertId" UNIQUEIDENTIFIER NOT NULL,
	"FromEmployeeId" UNIQUEIDENTIFIER NOT NULL,
);

-- Info about particular education of an employee 
CREATE TABLE "Educations" (
	"Id" UNIQUEIDENTIFIER PRIMARY KEY, --GUID
	"StartDate" DATETIME NOT NULL,
	"EndDate" DATETIME NULL,
	"Degree" VARCHAR(50) NOT NULL,
	"FieldOfEducation" VARCHAR(50) NOT NULL,
	"University" VARCHAR(50) NOT NULL,
	"Grade" DECIMAL(19,2) NOT NULL,
	"EmployeeId" UNIQUEIDENTIFIER NOT NULL,
	"LocationId" UNIQUEIDENTIFIER NULL,
);

-- Recommendation about employee directed to specific company
CREATE TABLE "Recommendations" (
	"Id" UNIQUEIDENTIFIER PRIMARY KEY, --GUID
	"Description" varchar(max) NOT NULL,
	"RecommendDate" DATETIME NOT NULL,
	"ForEmployeeId" UNIQUEIDENTIFIER NOT NULL,
	"ToCompanyId" UNIQUEIDENTIFIER NOT NULL,
	"FromEmployeeId" UNIQUEIDENTIFIER NOT NULL,
);

--Location(Address) of specific entity
CREATE TABLE "Locations" (
	"Id"UNIQUEIDENTIFIER PRIMARY KEY, --GUID
	"Address" VARCHAR(50) NULL,
	"State" VARCHAR(50) NOT NULL,
	"Country" VARCHAR(50) NOT NULL,
	OwnerId UNIQUEIDENTIFIER NOT NULL,
    OwnerType VARCHAR(255) NOT NULL
);

--Mapping table (User - Roles)
CREATE TABLE "UsersRoles" (
	"UserId" UNIQUEIDENTIFIER, --GUID
	"RoleId" INT,
	PRIMARY KEY (UserId, RoleId)
);

--User Claims 
CREATE TABLE "UsersClaims" (
	"Id" UNIQUEIDENTIFIER, --GUID
	"UserId" UNIQUEIDENTIFIER,
	"ClaimType" VARCHAR(255) NOT NULL,
	"ClaimValue" varchar(max)
);


