/*
1.	Выдать всех поставщиков (колонка CompanyName в таблице Suppliers), 
	у которых нет хотя бы одного продукта на складе (UnitsInStock в таблице Products равно 0). 
	Использовать вложенный SELECT для этого запроса с использованием оператора IN. 
*/
SELECT
	Sup.CompanyName
FROM dbo.Suppliers as Sup
WHERE Sup.SupplierID IN (
	SELECT
		Prod.SupplierID
	FROM dbo.Products as Prod
	WHERE Prod.UnitsInStock = 0
)
GO

/*
2.	Выдать всех продавцов, которые имеют более 150 заказов. Использовать вложенный SELECT.
*/
SELECT
	Emp.FirstName
FROM dbo.Employees as Emp
WHERE Emp.EmployeeID IN (
	SELECT
		Ord.EmployeeID
	FROM dbo.Orders as Ord
	GROUP BY Ord.EmployeeID
	HAVING COUNT(*) > 150
)
GO

/*
3.	Выдать всех заказчиков (таблица Customers), которые не имеют 
	ни одного заказа (подзапрос по таблице Orders). 
	Использовать оператор EXISTS.
*/
SELECT
	Cus.ContactName
FROM dbo.Customers as Cus
WHERE NOT EXISTS (
	SELECT
		Ord.EmployeeID
	FROM dbo.Orders as Ord
	WHERE Ord.CustomerID = Cus.CustomerID
)
GO