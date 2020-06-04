CREATE TABLE dbo.Authors (
	Id			int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Firstname	nvarchar(50) NULL,
	Lastname	nvarchar(50) NULL,
	Bio			nvarchar(max) NULL
)

CREATE TABLE dbo.Books (
	Id			int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Title		nvarchar(100) NULL,
	[Year]		int NULL,
	ISBN		nvarchar(50) NULL,
	Summary		nvarchar(150) NULL,
	[Image]		nvarchar(150) NULL,
	Price		money NULL,
	AuthorId	int NULL FOREIGN KEY REFERENCES Authors(Id)
)