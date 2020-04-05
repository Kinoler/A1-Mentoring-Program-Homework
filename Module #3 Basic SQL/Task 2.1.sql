/*
1.	����� ����� ����� ���� ������� �� ������� Order Details � ������ ���������� 
	����������� ������� � ������ �� ���. ����������� ������� ������ ���� ���� 
	������ � ����� �������� � ��������� ������� 'Totals'.
*/
SELECT 
	SUM((UnitPrice - Discount) * Quantity) AS 'Totals'
FROM dbo.[Order Details]
GO

/*
2.	�� ������� Orders ����� ���������� �������, ������� ��� �� ���� ���������� 
	(�.�. � ������� ShippedDate ��� �������� ���� ��������). 
	������������ ��� ���� ������� ������ �������� COUNT. 
	�� ������������ ����������� WHERE � GROUP.
*/
SELECT 
	COUNT(
		CASE
			WHEN ShippedDate IS NULL THEN 1
			ELSE NULL
		END) AS CountNotArrived
FROM dbo.Orders
GO

/*
3.	�� ������� Orders ����� ���������� ��������� ����������� (CustomerID), 
	��������� ������. 
	������������ ������� COUNT � �� ������������ ����������� WHERE � GROUP.
*/
SELECT
	COUNT(DISTINCT CustomerID)
FROM dbo.Orders
GO