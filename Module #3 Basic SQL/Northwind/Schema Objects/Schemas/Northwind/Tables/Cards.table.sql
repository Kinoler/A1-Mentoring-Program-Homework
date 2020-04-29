CREATE TABLE [Northwind].[Cards](
		CardID			INT IDENTITY(1,1) NOT NULL,
		CardNumber		INT NOT NULL,
		DateEnd			DATE NULL,
		CustomerID		INT NULL,
		CardHolderName	NVARCHAR(50) NULL,
		CONSTRAINT [PK_Card] PRIMARY KEY ([CardID] ASC)	
)
