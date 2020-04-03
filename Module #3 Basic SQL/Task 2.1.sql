/*
1.	����� ����� ����� ���� ������� �� ������� Order Details � ������ ���������� 
	����������� ������� � ������ �� ���. ����������� ������� ������ ���� ���� 
	������ � ����� �������� � ��������� ������� 'Totals'.
*/
SELECT 
	SUM((UnitPrice- Discount) * Quantity) as 'Totals'
FROM dbo.[Order Details]
GO

/*
2.	�� ������� Orders ����� ���������� �������, ������� ��� �� ���� ���������� 
	(�.�. � ������� ShippedDate ��� �������� ���� ��������). 
	������������ ��� ���� ������� ������ �������� COUNT. 
	�� ������������ ����������� WHERE � GROUP.
*/
SELECT 
	COUNT(*)
FROM dbo.Orders
WHERE ShippedDate is null
GO

/*
3.	�� ������� Orders ����� ���������� ��������� ����������� (CustomerID), 
	��������� ������. 
	������������ ������� COUNT � �� ������������ ����������� WHERE � GROUP.
*/
SELECT
	COUNT(UnicCustomerIDs.CustomerID)
FROM (SELECT DISTINCT CustomerID FROM dbo.Orders) as UnicCustomerIDs
GO