/*
1.	������ ���� ����������� (������� CompanyName � ������� Suppliers), 
	� ������� ��� ���� �� ������ �������� �� ������ (UnitsInStock � ������� Products ����� 0). 
	������������ ��������� SELECT ��� ����� ������� � �������������� ��������� IN. 
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
2.	������ ���� ���������, ������� ����� ����� 150 �������. ������������ ��������� SELECT.
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
3.	������ ���� ���������� (������� Customers), ������� �� ����� 
	�� ������ ������ (��������� �� ������� Orders). 
	������������ �������� EXISTS.
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