/*
1.	������� ��� ������ (OrderID) �� ������� Order Details (������ �� ������ �����������), 
	��� ����������� �������� � ����������� �� 3 �� 10 ������������ � ��� ������� Quantity 
	� ������� Order Details. ������������ �������� BETWEEN. 
	������ ������ ���������� ������ ������� OrderID.
*/
SELECT DISTINCT
	OrderID
FROM dbo.[Order Details]
WHERE Quantity BETWEEN 3 and 10
GO

/*
2.	������� ���� ���������� �� ������� Customers, � ������� �������� ������ ���������� 
	�� ����� �� ��������� b � g. ������������ �������� BETWEEN. 
	���������, ��� � ���������� ������� �������� Germany. 
	������ ������ ���������� ������ ������� CustomerID � Country � ������������ �� Country.
*/
SELECT 
	CustomerID,
	Country
FROM dbo.Customers
WHERE SUBSTRING(Country, 1, 1) BETWEEN 'b' and 'g'
ORDER BY Country
GO

/*
3.	������� ���� ���������� �� ������� Customers, � ������� �������� ������ ���������� 
	�� ����� �� ��������� b � g, �� ��������� �������� BETWEEN. 
*/
SELECT 
	CustomerID,
	Country
FROM dbo.Customers
WHERE SUBSTRING(Country, 1, 1) >= 'b' and SUBSTRING(Country, 1, 1) <= 'g'
GO