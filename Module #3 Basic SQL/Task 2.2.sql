/*
1.	По таблице Orders найти количество заказов с группировкой по годам. 
	В результатах запроса надо возвращать две колонки c названиями Year и Total. 
	Написать проверочный запрос, который вычисляет количество всех заказов.
*/
SELECT 
	YEAR(o.OrderDate) as 'Year',
	COUNT(*) as 'Total'
FROM dbo.Orders as o
GROUP BY YEAR(o.OrderDate)
GO

SELECT 
	COUNT(*) as 'Total'
FROM dbo.Orders as o
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
		CONCAT(e.LastName, ' ',e.FirstName)
		FROM dbo.Employees as e
		WHERE e.EmployeeID = o.EmployeeID
	) as 'Seller',
	COUNT(*) as Amount
FROM dbo.Orders as o
GROUP BY o.EmployeeID
ORDER BY Amount
GO

/*
3.	По таблице Orders найти количество заказов, сделанных каждым продавцом и 
	для каждого покупателя. 
	Необходимо определить это только для заказов, сделанных в 1998 году.
*/
SELECT 
	o.EmployeeID,
	o.CustomerID,
	COUNT(*) as 'Total'
FROM dbo.Orders as o
WHERE YEAR(o.OrderDate) = '1998'
GROUP BY o.EmployeeID, o.CustomerID
GO

/*
4.	Найти покупателей и продавцов, которые живут в одном городе. 
	Если в городе живут только один или несколько продавцов, или только 
	один или несколько покупателей, то информация о таких покупателя и 
	продавцах не должна попадать в результирующий набор. 
	Не использовать конструкцию JOIN. 
*/
SELECT 
	o.EmployeeID,
	o.CustomerID
FROM dbo.Orders as o
WHERE 
	(SELECT c.City FROM dbo.Customers as c WHERE c.CustomerID = o.CustomerID) = 
	(SELECT e.City FROM dbo.Employees as e WHERE e.EmployeeID = o.EmployeeID)
GO

/*	
5.	Найти всех покупателей, которые живут в одном городе. 
*/
/* Первый вариант */
SELECT 
	c.City,
	STUFF(
		(SELECT ', ' + innerC.ContactName 
			FROM dbo.Customers as innerC
			WHERE (innerC.City = c.City) 
			FOR XML PATH(''), TYPE
		).value('(./text())[1]','VARCHAR(MAX)'),
		1,
		2,
		''
		) AS Customers
FROM dbo.Customers as c
GROUP BY c.City
HAVING COUNT(*) > 1
GO

/* Второй вариант */
SELECT 
	c.ContactName 
FROM dbo.Customers as c
WHERE c.City in 
	(SELECT innerC.City 
		FROM dbo.Customers as innerC
		GROUP BY innerC.City
		HAVING COUNT(*) > 1)
GO

/*
6.	По таблице Employees найти для каждого продавца его руководителя.
*/
SELECT 
	c.FirstName as Employee,
	(SELECT innerC.FirstName
		FROM dbo.Employees as innerC
		WHERE innerC.EmployeeID = c.ReportsTo) as ReportsTo
FROM dbo.Employees as c
GO