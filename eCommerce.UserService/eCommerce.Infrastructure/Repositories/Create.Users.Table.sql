Create Table Users (
    UserId UUID Primary Key,
    PersonName VARCHAR(50) Not Null,
    Email VARCHAR(100) Not Null,
    PasswordHash VARCHAR(256) Not Null,
    Gender VARCHAR(10) Null
);