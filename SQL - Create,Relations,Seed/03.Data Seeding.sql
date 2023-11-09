INSERT INTO [HCMSImmedis].[dbo].[Roles] ([Id], [Name], [Description])
VALUES 
(1, 'ADMIN', 'ADMIN DESCRIPTION'),
(2, 'AGENT', 'AGENT DESCRIPTION'),
(3, 'EMPLOYEE', 'EMPLOYEE DESCRIPTION');

INSERT INTO [HCMSImmedis].[dbo].[Users] ([Id],[Username],[Password],[Email],[RegisterDate])
VALUES
--USER PASSWORD IS: 123456 <- Bcrypted
('9498BD98-544B-45EF-9271-7AB391452F03', 'user1', '$2a$12$100BtjputgnzWP6L5sQ7/.K68bcIU9re.bWyxmpYbhURKGn3ETG2.', 'user1@example.com', GETDATE()),
('43528CFE-C558-4794-BFBC-67E54298A3B2', 'user2', '$2a$12$/PIGSpNwFT4FXwDjHwOAfeqgCSZBhNOgSCYiZViSy2/vahXPKvfs6', 'user2@example.com', GETDATE()),
('099C8C62-4F31-4908-8C75-F45C030E3535', 'user3', '$2a$12$vViisHVXUUFkto0GC4ttH.hPIB2hwFtdbtvjIoG3cu4ul1UiKnM1.', 'user3@example.com', GETDATE()),
('60FB72A5-8DD9-49A1-8766-4346D039217C', 'user4', '$2a$12$1bUiEugw3Xr2qN2w9ovZIOn.nNMVc6UkeR8PYELNLYtyzWunIV6Oi', 'user4@example.com', GETDATE()),
('A491C479-E3D1-4730-B1F5-9F50A8F8EB00', 'user5', '$2a$12$s.P6Un6YQI6i3/yMTo57VOKRgbKRUa6Vc2GAOqzqaHgE0.erS7iu.', 'user5@example.com', GETDATE());

INSERT INTO [HCMSImmedis].[dbo].[UsersRoles] ([UserId], [RoleId])
VALUES
('9498BD98-544B-45EF-9271-7AB391452F03', 3),
('9498BD98-544B-45EF-9271-7AB391452F03', 2),
('9498BD98-544B-45EF-9271-7AB391452F03', 1),
('43528CFE-C558-4794-BFBC-67E54298A3B2', 3),
('43528CFE-C558-4794-BFBC-67E54298A3B2', 2),
('099C8C62-4F31-4908-8C75-F45C030E3535', 3),
('60FB72A5-8DD9-49A1-8766-4346D039217C', 3),
('A491C479-E3D1-4730-B1F5-9F50A8F8EB00', 3);

INSERT INTO [HCMSImmedis].[dbo].[Locations] ([Id], [Address], [State], [Country], [OwnerType], [OwnerId])
VALUES
--For Companies
('6E9CED52-9F09-40AB-BFEC-DDDFC0FA6208', '123 Main Street', 'California', 'USA', 'Company', 'AE8749F4-A33A-4B89-988B-F153377FB006'),
('583603CF-5A53-447D-8E4F-021AFB15E27A', '456 Elm Street', 'New York', 'USA', 'Company', '1D62C7D5-EB39-4148-863C-D0D26D24C7BF'),
('E0416A03-E8B0-42CD-9B64-BF51EA70F3DE', '789 Oak Street', 'Texas', 'USA', 'Company', '283EF4E0-E400-498C-BE8B-7D50A1C1AB29'),
('EFC3EAB8-E39B-4614-977B-0872171131E9', '101 Pine Street', 'Florida', 'USA', 'Company', '71EE4FF8-F78A-474F-8E42-86C9D2966380'),
('092A9C3E-1C09-4C42-95E4-0C52982631C5', '202 Maple Street', 'Arizona', 'USA', 'Company', 'B51CA6FF-550E-4953-869C-CF70CBE695AE'),
--For Employees
('A1E8F343-5C94-4B0E-9CDB-9F29D1171AB3', '303 Cedar Street', 'Illinois', 'USA','Employee','E70FCEE0-F324-452E-A5DF-6FEC0117330E'),
('B868F0F2-1A6A-43F2-8D26-95648A18A8E1', '404 Birch Street', 'Georgia', 'USA','Employee','675E3F0D-F0E4-4335-BCCE-F6696D49BAED'),
('F2D8D65B-C55B-46E2-A5CE-D702C82E2A6D', '505 Pineapple Avenue', 'Florida', 'USA','Employee','AA66F1CF-2EB7-494F-B073-9FB224441E33'),
('84C3A546-F95D-40C0-97E3-18C8E4AA852A', '606 Orange Drive', 'California', 'USA','Employee','1E809572-73DD-47BF-B8F9-618195743F7F'),
('D57C0A7D-E19A-4D08-A02C-A63C88FD8FD6', '707 Lemon Lane', 'Texas', 'USA','Employee','D5BCEF42-6495-4F86-B279-C1ADBEF8C952'),
--For Educations
('1D166AC5-5422-4E2F-A29B-3FE700BC3D59', '808 Grape Grove', 'New York', 'USA', 'Education', 'B7E57DFC-8658-48A5-8341-1E1B8A8E6788'),
('5BCF05F9-0A97-4F7F-BAF6-1B0C54DD0839', '909 Cherry Circle', 'Arizona', 'USA','Education', '30645271-1AB6-43EB-9360-F94B86F7AB82'),
('237D6FA3-80EC-4DFC-9268-76E2A8F745F5', '1010 Apple Avenue', 'Ohio', 'USA','Education', '5EE3A257-272E-4456-89B3-DEB940DF7EC1'),
('7EE286C2-6E1C-4AF2-84D5-FD18513E17E3', '1111 Banana Boulevard', 'Florida', 'USA','Education', '46F7A0A8-F5D5-4DD2-A376-B54B54DAB5E7'),
('F0C19235-1F75-4E62-A308-E5E319A284D2', '1212 Mango Mall', 'California', 'USA','Education', '012C5CC1-2A38-4DC1-8597-93A055280C9D');

INSERT INTO [HCMSImmedis].[dbo].[Companies] ([Id],[Name],[Description],[IndustryField],[LocationId]
)
VALUES
    ('AE8749F4-A33A-4B89-988B-F153377FB006', 'Company 1', 'Description 1', 'Technology', '6E9CED52-9F09-40AB-BFEC-DDDFC0FA6208'),
    ('1D62C7D5-EB39-4148-863C-D0D26D24C7BF', 'Company 2', 'Description 2', 'Finance', '583603CF-5A53-447D-8E4F-021AFB15E27A'),
    ('283EF4E0-E400-498C-BE8B-7D50A1C1AB29', 'Company 3', 'Description 3', 'Healthcare', 'E0416A03-E8B0-42CD-9B64-BF51EA70F3DE'),
    ('71EE4FF8-F78A-474F-8E42-86C9D2966380', 'Company 4', 'Description 4', 'Manufacturing', 'EFC3EAB8-E39B-4614-977B-0872171131E9'),
    ('B51CA6FF-550E-4953-869C-CF70CBE695AE', 'Company 5', 'Description 5', 'Education', '092A9C3E-1C09-4C42-95E4-0C52982631C5');

INSERT INTO [HCMSImmedis].[dbo].[Employees] ([Id],[FirstName],[LastName],[Email],[PhoneNumber],[PhotoURL],[DateOfBirth],[AddDate],[CompanyId],[UserId],[LocationId])
VALUES
    ('E70FCEE0-F324-452E-A5DF-6FEC0117330E', 'John', 'Doe', 'john.doe@example.com', '123-456-7890', 'photo1.jpg', '1990-01-15', GETDATE(), 'AE8749F4-A33A-4B89-988B-F153377FB006', '9498BD98-544B-45EF-9271-7AB391452F03', 'A1E8F343-5C94-4B0E-9CDB-9F29D1171AB3'),
    ('675E3F0D-F0E4-4335-BCCE-F6696D49BAED', 'Jane', 'Smith', 'jane.smith@example.com', '987-654-3210', 'photo2.jpg', '1985-03-20', GETDATE(), '1D62C7D5-EB39-4148-863C-D0D26D24C7BF', '43528CFE-C558-4794-BFBC-67E54298A3B2', 'B868F0F2-1A6A-43F2-8D26-95648A18A8E1'),
    ('AA66F1CF-2EB7-494F-B073-9FB224441E33', 'Bob', 'Johnson', 'bob.johnson@example.com', '555-123-4567', 'photo3.jpg', '1995-07-10', GETDATE(), '283EF4E0-E400-498C-BE8B-7D50A1C1AB29', '099C8C62-4F31-4908-8C75-F45C030E3535', 'F2D8D65B-C55B-46E2-A5CE-D702C82E2A6D'),
    ('1E809572-73DD-47BF-B8F9-618195743F7F', 'Alice', 'Brown', 'alice.brown@example.com', '444-789-1234', 'photo4.jpg', '1980-12-05', GETDATE(), '71EE4FF8-F78A-474F-8E42-86C9D2966380', '60FB72A5-8DD9-49A1-8766-4346D039217C', '84C3A546-F95D-40C0-97E3-18C8E4AA852A'),
    ('D5BCEF42-6495-4F86-B279-C1ADBEF8C952', 'David', 'Wilson', 'david.wilson@example.com', '111-222-3333', 'photo5.jpg', '1992-09-25', GETDATE(), 'B51CA6FF-550E-4953-869C-CF70CBE695AE', 'A491C479-E3D1-4730-B1F5-9F50A8F8EB00', 'D57C0A7D-E19A-4D08-A02C-A63C88FD8FD6');


INSERT INTO [HCMSImmedis].[dbo].[Educations] ([Id],[StartDate],[EndDate],[Degree],[FieldOfEducation],[Grade],[EmployeeId],[LocationId],[University])
VALUES
    ('B7E57DFC-8658-48A5-8341-1E1B8A8E6788', '2022-01-01', '2023-01-01', 'Bachelor', 'Computer Science', 6.0,'E70FCEE0-F324-452E-A5DF-6FEC0117330E','1D166AC5-5422-4E2F-A29B-3FE700BC3D59','University-1'),
    ('30645271-1AB6-43EB-9360-F94B86F7AB82', '2020-01-01', '2021-01-01', 'Master', 'Engineering', 5.5,'675E3F0D-F0E4-4335-BCCE-F6696D49BAED','5BCF05F9-0A97-4F7F-BAF6-1B0C54DD0839','University-2'),
    ('5EE3A257-272E-4456-89B3-DEB940DF7EC1', '2019-01-01', '2020-01-01', 'Bachelor', 'Physics',5.7, 'AA66F1CF-2EB7-494F-B073-9FB224441E33', '237D6FA3-80EC-4DFC-9268-76E2A8F745F5','University-3'),
    ('46F7A0A8-F5D5-4DD2-A376-B54B54DAB5E7', '2021-01-01', '2022-01-01', 'Doctorate', 'Mathematics', 6.0, '1E809572-73DD-47BF-B8F9-618195743F7F', '7EE286C2-6E1C-4AF2-84D5-FD18513E17E3','University-4'),
    ('012C5CC1-2A38-4DC1-8597-93A055280C9D', '2018-01-01', '2019-01-01', 'Master', 'Chemistry', 5.8, 'D5BCEF42-6495-4F86-B279-C1ADBEF8C952', 'F0C19235-1F75-4E62-A308-E5E319A284D2','University-5');


INSERT INTO [HCMSImmedis].[dbo].[Recommendations] ([Id],[Description],[RecommendDate],[ForEmployeeId],[ToCompanyId])
VALUES
('6F0BABE2-785E-461D-ABFB-E02CE08E1AE2', 'Highly recommended for teamwork.', '2022-05-15', 'E70FCEE0-F324-452E-A5DF-6FEC0117330E', 'AE8749F4-A33A-4B89-988B-F153377FB006'),
('47652875-1F80-4113-9DB2-61F4DD5AF65E', 'Excellent communication and leadership skills.', '2021-11-20', '675E3F0D-F0E4-4335-BCCE-F6696D49BAED', '1D62C7D5-EB39-4148-863C-D0D26D24C7BF'),
('6C98D972-F05F-446A-ABEA-E90CD603489C', 'Outstanding performance in project management.', '2023-01-10', 'AA66F1CF-2EB7-494F-B073-9FB224441E33', '283EF4E0-E400-498C-BE8B-7D50A1C1AB29'),
('E832809C-7425-43A9-8372-B452AB9A18AC', 'Exceptional problem-solving abilities.', '2020-08-05', '1E809572-73DD-47BF-B8F9-618195743F7F', '71EE4FF8-F78A-474F-8E42-86C9D2966380'),
('2D495428-F8F0-422A-9668-42AD36062484', 'Strong analytical and technical skills.', '2022-09-30', 'D5BCEF42-6495-4F86-B279-C1ADBEF8C952', 'B51CA6FF-550E-4953-869C-CF70CBE695AE');

INSERT INTO [HCMSImmedis].[dbo].[Applications] ([Id],[Position],[Department],[CoverLetter],[AddDate],[ToCompanyId],[FromEmployeeId])
VALUES
('416F2C75-2B02-47DA-AC15-D6A8C100CBB7', 'Software Engineer', 'Engineering', 'I am excited to join your team.', GETDATE(), 'AE8749F4-A33A-4B89-988B-F153377FB006', 'E70FCEE0-F324-452E-A5DF-6FEC0117330E'),
('A6786C6D-B2C6-4673-AA49-799D5EE45507', 'Financial Analyst', 'Finance', 'I have strong financial analysis skills.', GETDATE(), '1D62C7D5-EB39-4148-863C-D0D26D24C7BF', '675E3F0D-F0E4-4335-BCCE-F6696D49BAED'),
('7A58FA1B-F4A2-4869-A65D-C8869233D563', 'Data Scientist', 'Data Science', 'I am passionate about data analysis.', GETDATE(), '283EF4E0-E400-498C-BE8B-7D50A1C1AB29', 'AA66F1CF-2EB7-494F-B073-9FB224441E33'),
('B0A38E3A-97A9-444F-8E5B-F6DA82375577', 'Marketing Specialist', 'Marketing', 'I excel in marketing strategies.', GETDATE(), '71EE4FF8-F78A-474F-8E42-86C9D2966380', '1E809572-73DD-47BF-B8F9-618195743F7F'),
('5A1C040A-EEB3-472F-B96C-71B442E813AC', 'HR Manager', 'Human Resources', 'I have strong HR management skills.', GETDATE(), 'B51CA6FF-550E-4953-869C-CF70CBE695AE', 'D5BCEF42-6495-4F86-B279-C1ADBEF8C952');
