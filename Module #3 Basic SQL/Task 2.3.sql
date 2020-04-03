/*
1.	Определить продавцов, которые обслуживают регион 'Western' (таблица Region). 
*/
SELECT DISTINCT
	Emp.FirstName
FROM dbo.Employees as Emp
JOIN dbo.EmployeeTerritories as EmpT
	ON EmpT.EmployeeID = Emp.EmployeeID
JOIN dbo.Territories as Ter
	ON Ter.TerritoryID = EmpT.TerritoryID
JOIN dbo.Region as Reg
	ON Reg.RegionID = Ter.RegionID
WHERE Reg.RegionDescription = 'Western'
GO

/*
2.	Выдать в результатах запроса имена всех заказчиков из таблицы Customers и 
	суммарное количество их заказов из таблицы Orders. 
	Принять во внимание, что у некоторых заказчиков нет заказов, но они также 
	должны быть выведены в результатах запроса. 
	Упорядочить результаты запроса по возрастанию количества заказов.
*/
SELECT
	Cust.ContactName,
	(CASE
		WHEN (
			SELECT CustomersJoinedWithOrderWithRowNumber.OrdID
			FROM (
				SELECT
					CustomersJoinedWithOrders.CustID,
					CustomersJoinedWithOrders.OrdID,
					ROW_NUMBER() OVER (PARTITION BY CustomersJoinedWithOrders.CustID ORDER BY CustomersJoinedWithOrders.CustID) as Num
				FROM (
					SELECT
						innerCust.CustomerID as CustID,
						innerOrd.OrderID as OrdID
					FROM dbo.Customers as innerCust
					LEFT JOIN dbo.Orders as innerOrd
						ON innerOrd.CustomerID = innerCust.CustomerID
				) as CustomersJoinedWithOrders
			) as CustomersJoinedWithOrderWithRowNumber
			WHERE 
				CustomersJoinedWithOrderWithRowNumber.Num = 1 AND
				CustomersJoinedWithOrderWithRowNumber.CustID = Cust.CustomerID
		) IS NULL THEN 0
		ELSE COUNT(*)
	END) as CountOrders
FROM dbo.Customers as Cust
LEFT JOIN dbo.Orders as Ord
	ON Ord.CustomerID = Cust.CustomerID
GROUP BY Cust.CustomerID, Cust.ContactName
ORDER BY CountOrders