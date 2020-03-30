/*
1.	������ ���� ����������� (������� CompanyName � ������� Suppliers), 
	� ������� ��� ���� �� ������ �������� �� ������ (UnitsInStock � ������� Products ����� 0). 
	������������ ��������� SELECT ��� ����� ������� � �������������� ��������� IN. 
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
2.	������ ���� ���������, ������� ����� ����� 150 �������. ������������ ��������� SELECT.
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
3.	������ ���� ���������� (������� Customers), ������� �� ����� 
	�� ������ ������ (��������� �� ������� Orders). 
	������������ �������� EXISTS.
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