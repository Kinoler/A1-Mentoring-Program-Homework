/*
1.	Определить продавцов, которые обслуживают регион 'Western' (таблица Region). 
*/
SELECT DISTINCT
	employee.FirstName
FROM dbo.Employees AS employee
JOIN dbo.EmployeeTerritories AS employeeTerritories
	ON employeeTerritories.EmployeeID = employee.EmployeeID
JOIN dbo.Territories AS territory
	ON territory.TerritoryID = employeeTerritories.TerritoryID
JOIN dbo.Region AS region
	ON region.RegionID = territory.RegionID
WHERE region.RegionDescription = 'Western'
GO

/*
2.	Выдать в результатах запроса имена всех заказчиков из таблицы Customers и 
	суммарное количество их заказов из таблицы Orders. 
	Принять во внимание, что у некоторых заказчиков нет заказов, но они также 
	должны быть выведены в результатах запроса. 
	Упорядочить результаты запроса по возрастанию количества заказов.
*/
SELECT
	customer.ContactName,
	COUNT(OrderID) AS CountOrders
FROM dbo.Customers AS customer
LEFT JOIN dbo.Orders AS orders
	ON orders.CustomerID = customer.CustomerID
GROUP BY customer.CustomerID, customer.ContactName
ORDER BY CountOrders