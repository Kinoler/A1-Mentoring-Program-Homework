/*
1.	�� ������� Orders ����� ���������� ������� � ������������ �� �����. 
	� ����������� ������� ���� ���������� ��� ������� c ���������� Year � Total. 
	�������� ����������� ������, ������� ��������� ���������� ���� �������.
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
2.	�� ������� Orders ����� ���������� �������, c�������� ������ ���������. 
	����� ��� ���������� �������� � ��� ����� ������ � ������� Orders, 
	��� � ������� EmployeeID ������ �������� ��� ������� ��������. 
	� ����������� ������� ���� ���������� ������� � ������ �������� 
	(������ ������������� ��� ���������� ������������� LastName & FirstName. 
	��� ������ LastName & FirstName ������ ���� �������� ��������� �������� � 
	������� ��������� �������. ����� �������� ������ ������ ������������ 
	����������� �� EmployeeID.) � ��������� ������� �Seller� � ������� c 
	����������� ������� ���������� � ��������� 'Amount'. 
	���������� ������� ������ ���� ����������� �� �������� ���������� �������. 
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
3.	�� ������� Orders ����� ���������� �������, ��������� ������ ��������� � 
	��� ������� ����������. 
	���������� ���������� ��� ������ ��� �������, ��������� � 1998 ����.
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
4.	����� ����������� � ���������, ������� ����� � ����� ������. 
	���� � ������ ����� ������ ���� ��� ��������� ���������, ��� ������ 
	���� ��� ��������� �����������, �� ���������� � ����� ���������� � 
	��������� �� ������ �������� � �������������� �����. 
	�� ������������ ����������� JOIN. 
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
5.	����� ���� �����������, ������� ����� � ����� ������. 
*/
/* ������ ������� */
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

/* ������ ������� */
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
6.	�� ������� Employees ����� ��� ������� �������� ��� ������������.
*/
SELECT
	employee.FirstName AS Employee,
	(SELECT innerC.FirstName
		FROM dbo.Employees AS innerC
		WHERE innerC.EmployeeID = employee.ReportsTo) AS ReportsTo
FROM dbo.Employees AS employee
GO