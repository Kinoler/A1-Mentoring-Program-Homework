/*
1.	Выдать всех поставщиков (колонка CompanyName в таблице Suppliers), 
	у которых нет хотя бы одного продукта на складе (UnitsInStock в таблице Products равно 0). 
	Использовать вложенный SELECT для этого запроса с использованием оператора IN. 
*/
SELECT
	supplier.CompanyName
FROM dbo.Suppliers AS supplier
WHERE supplier.SupplierID IN (
	SELECT
		product.SupplierID
	FROM dbo.Products AS product
	WHERE product.UnitsInStock = 0
)
GO

/*
2.	Выдать всех продавцов, которые имеют более 150 заказов. Использовать вложенный SELECT.
*/
SELECT
	employee.FirstName
FROM dbo.Employees AS employee
WHERE employee.EmployeeID IN (
	SELECT
		orders.EmployeeID
	FROM dbo.Orders AS orders
	GROUP BY orders.EmployeeID
	HAVING COUNT(orders.OrderID) > 150
)
GO

/*
3.	Выдать всех заказчиков (таблица Customers), которые не имеют 
	ни одного заказа (подзапрос по таблице Orders). 
	Использовать оператор EXISTS.
*/
SELECT
	customer.ContactName
FROM dbo.Customers AS customer
WHERE NOT EXISTS (
	SELECT
		orders.EmployeeID
	FROM dbo.Orders AS orders
	WHERE orders.CustomerID = customer.CustomerID
)
GO