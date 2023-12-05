--use Immedis;
--use HCMSImmedis;

--FKs

--One to One Employees-Users
ALTER TABLE "Employees"
ADD CONSTRAINT "FK_Employees_Users"
FOREIGN KEY (UserId) REFERENCES Users(Id),
UNIQUE (UserId);

--Many to One Employees-Locations
ALTER TABLE "Employees"
ADD CONSTRAINT "FK_Employees_Locations"
FOREIGN KEY (LocationId) REFERENCES Locations(Id);

--Many to One Employees-Locations
ALTER TABLE "Employees"
ADD CONSTRAINT "FK_Employees_Companies"
FOREIGN KEY (CompanyId) REFERENCES Companies(Id);

--Many to One Companies-Locations
ALTER TABLE "Companies"
ADD CONSTRAINT "FK_Companies_Locations"
FOREIGN KEY (LocationId) REFERENCES Locations(Id);

--Many to One Records-Locations
ALTER TABLE "WorkRecords"
ADD CONSTRAINT "FK_WorkRecords_Companies"
FOREIGN KEY (CompanyId) REFERENCES Companies(Id);

--Many to One Records-Employees
ALTER TABLE "WorkRecords"
ADD CONSTRAINT "FK_WorkRecords_Employees"
FOREIGN KEY (EmployeeId) REFERENCES Employees(Id);

--Many to One Applications-Adverts
ALTER TABLE "Applications"
ADD CONSTRAINT "FK_Applications_Adverts"
FOREIGN KEY (AdvertId) REFERENCES Adverts(Id);

--Many to One Applications-Employees
ALTER TABLE "Applications"
ADD CONSTRAINT "FK_Applications_Employees"
FOREIGN KEY (FromEmployeeId) REFERENCES Employees(Id);

--Many to One Educations-Locations
ALTER TABLE "Educations"
ADD CONSTRAINT "FK_Educations_Locations"
FOREIGN KEY (LocationId) REFERENCES Locations(Id);

--Many to One Educations-Employees
ALTER TABLE "Educations"
ADD CONSTRAINT "FK_Educations_Employees"
FOREIGN KEY (EmployeeId) REFERENCES Employees(Id);

--Many to One Recommendations-Employees
ALTER TABLE "Recommendations"
ADD CONSTRAINT "FK_Recommendations_Employees"
FOREIGN KEY (ForEmployeeId) REFERENCES Employees(Id);

--Many to One Recommendations-Companies
ALTER TABLE "Recommendations"
ADD CONSTRAINT "FK_Recommendations_to_Companies"
FOREIGN KEY (ToCompanyId) REFERENCES Companies(Id);

--Many to One Recommendations-Employees
ALTER TABLE "Recommendations"
ADD CONSTRAINT "FK_Recommendations_from_Employees"
FOREIGN KEY (FromEmployeeId) REFERENCES Employees(Id);

--Many to One Advert-Companies
ALTER TABLE "Adverts"
ADD CONSTRAINT "FK_Adverts_Companies"
FOREIGN KEY (CompanyId) REFERENCES Companies(Id);

--Many to Many User - UserRoles - Roles
ALTER TABLE "UsersRoles"
ADD CONSTRAINT "FK_Users_Roles"
FOREIGN KEY (UserId) REFERENCES Users(Id);

ALTER TABLE "UsersRoles"
ADD CONSTRAINT "FK_Roles_Users"
FOREIGN KEY (RoleId) REFERENCES Roles(Id);

--Many to One Users-UsersClaims
ALTER TABLE "UsersClaims"
ADD CONSTRAINT "FK_Users_Claims"
FOREIGN KEY (UserId) REFERENCES Users(Id);