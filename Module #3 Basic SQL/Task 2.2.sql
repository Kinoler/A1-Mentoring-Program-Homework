/*
1.	По таблице Orders найти количество заказов с группировкой по годам. 
	В результатах запроса надо возвращать две колонки c названиями Year и Total. 
	Написать проверочный запрос, который вычисляет количество всех заказов.
*/
SELECT 
	YEAR(orders.OrderDate) AS 'Year',
	COUNT(orders.OrderID) AS 'Total'
FROM dbo.Orders AS orders
GROUP BY YEAR(orders.OrderDate)
GO

SELECT 
	COUNT(orders.OrderID) as 'Total'
FROM dbo.Orders as orders
GO

/*
2.	По таблице Orders найти количество заказов, cделанных каждым продавцом. 
	Заказ для указанного продавца – это любая запись в таблице Orders, 
	где в колонке EmployeeID задано значение для данного продавца. 
	В результатах запроса надо возвращать колонку с именем продавца 
	(Должно высвечиваться имя полученное конкатенацией LastName & FirstName. 
	Эта строка LastName & FirstName должна быть получена отдельным запросом в 
	колонке основного запроса. Также основной запрос должен использовать 
	группировку по EmployeeID.) с названием колонки ‘Seller’ и колонку c 
	количеством заказов возвращать с названием 'Amount'. 
	Результаты запроса должны быть упорядочены по убыванию количества заказов. 
*/
SELECT 
	(SELECT 
		CONCAT(employee.LastName, ' ',employee.FirstName)
		FROM dbo.Employees AS employee
		WHERE employee.EmployeeID = orders.EmployeeID
	) AS 'Seller',
	COUNT(orders.OrderID) AS Amount
FROM dbo.Orders AS orders
GROUP BY orders.EmployeeID
ORDER BY Amount
GO

/*
3.	По таблице Orders найти количество заказов, сделанных каждым продавцом и 
	для каждого покупателя. 
	Необходимо определить это только для заказов, сделанных в 1998 году.
*/
SELECT 
	orders.EmployeeID,
	orders.CustomerID,
	COUNT(orders.OrderID) AS 'Total'
FROM dbo.Orders AS orders
WHERE YEAR(orders.OrderDate) = '1998'
GROUP BY orders.EmployeeID, orders.CustomerID
GO

/*
4.	Найти покупателей и продавцов, которые живут в одном городе. 
	Если в городе живут только один или несколько продавцов, или только 
	один или несколько покупателей, то информация о таких покупателя и 
	продавцах не должна попадать в результирующий набор. 
	Не использовать конструкцию JOIN. 
*/
SELECT 
	orders.EmployeeID,
	orders.CustomerID
FROM dbo.Orders AS orders
WHERE 
	(SELECT customer.City FROM dbo.Customers AS customer WHERE customer.CustomerID = orders.CustomerID) = 
	(SELECT employee.City FROM dbo.Employees AS employee WHERE employee.EmployeeID = orders.EmployeeID)
GO

/*	
5.	Найти всех покупателей, которые живут в одном городе. 
*/
/* Первый вариант */
SELECT 
	customer.City,
	STUFF(
		(SELECT ', ' + innerC.ContactName 
			FROM dbo.Customers AS innerC
			WHERE (innerC.City = customer.City) 
			FOR XML PATH(''), TYPE
		).value('(./text())[1]','VARCHAR(MAX)'),
		1,
		2,
		''
		) AS Customers
FROM dbo.Customers AS customer
GROUP BY customer.City
HAVING COUNT(customer.City) > 1
GO

/* Второй вариант */
SELECT 
	customer.ContactName 
FROM dbo.Customers AS customer
WHERE customer.City IN
	(SELECT innerC.City 
		FROM dbo.Customers AS innerC
		GROUP BY innerC.City
		HAVING COUNT(innerC.City) > 1)
GO

/*
6.	По таблице Employees найти для каждого продавца его руководителя.
*/
SELECT
	employee.FirstName AS Employee,
	(SELECT innerC.FirstName
		FROM dbo.Employees AS innerC
		WHERE innerC.EmployeeID = employee.ReportsTo) AS ReportsTo
FROM dbo.Employees AS employee
GO