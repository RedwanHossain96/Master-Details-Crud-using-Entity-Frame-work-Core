use Master
Go

Create Database EmployeeDB
Go

Use EmployeeDB
Go

Create Table Employee
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[EmpName] varchar(50) NULL,
	[EmpAddress] varchar(50) NULL,
	[PhoneNumber] nvarchar(50) NULL,
	[Age] int NULL,
	[Salary] decimal NULL,
	[DOB] date NULL
	PRIMARY KEY CLUSTERED ([Id] ASC) 
);
Go
--Store proc

Create Proc sp_CreateEmp
	@Name varchar(50),
	@Address varchar(50),
	@Phone nvarchar(50),
	@Age int,
	@Salary decimal,
	@DOB date
as
Begin
	Insert Into Employee(EmpName, EmpAddress, PhoneNumber, Age, Salary, DOB)
	Values (@Name, @Address, @Phone,@Age, @Salary, @DOB)
End
GO

Create Proc sp_UpdateEmp
	@Id int,
	@Name varchar(50),
	@Address varchar(50),
	@Phone nvarchar(50),
	@Age int,
	@Salary decimal,
	@DOB date
as
Begin
	Update Employee
	set EmpName= @Name, EmpAddress = @Address, PhoneNumber = @Phone, Age = @Age, Salary = @Salary, DOB = @DOB
	where Id = @Id
End
GO

Create Proc sp_DeleteEmp
	@Id int
	
as
Begin
	Delete from Employee
	where Id = @Id
End
GO

Create Proc sp_GetAllEmp
	
as
Begin
	Select * from Employee
End
GO

Create Proc sp_GetEmpById
	@Id int
as
Begin
	Select * from Employee
	Where Id = @Id
End
GO
