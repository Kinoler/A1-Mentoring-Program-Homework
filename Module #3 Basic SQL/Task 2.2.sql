/*
1.	�� ������� Orders ����� ���������� ������� � ������������ �� �����. 
	� ����������� ������� ���� ���������� ��� ������� c ���������� Year � Total. 
	�������� ����������� ������, ������� ��������� ���������� ���� �������.
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
3.	�� ������� Orders ����� ���������� �������, ��������� ������ ��������� � 
	��� ������� ����������. 
	���������� ���������� ��� ������ ��� �������, ��������� � 1998 ����.
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
4.	����� ����������� � ���������, ������� ����� � ����� ������. 
	���� � ������ ����� ������ ���� ��� ��������� ���������, ��� ������ 
	���� ��� ��������� �����������, �� ���������� � ����� ���������� � 
	��������� �� ������ �������� � �������������� �����. 
	�� ������������ ����������� JOIN. 
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
5.	����� ���� �����������, ������� ����� � ����� ������. 
*/
/* ������ ������� */
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

/* ������ ������� */
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
6.	�� ������� Employees ����� ��� ������� �������� ��� ������������.
*/
SELECT 
	c.FirstName as Employee,
	(SELECT innerC.FirstName
		FROM dbo.Employees as innerC
		WHERE innerC.EmployeeID = c.ReportsTo) as ReportsTo
FROM dbo.Employees as c
GO