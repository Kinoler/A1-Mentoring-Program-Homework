/*
1.	������� �� ������� Customers ���� ����������, ����������� � USA � Canada. 
	������ ������� � ������ ������� ��������� IN. ���������� ������� � ������ 
	������������ � ��������� ������ � ����������� �������. ����������� ���������� 
	������� �� ����� ���������� � �� ����� ����������.
*/
SELECT 
	ContactName,
	Country
FROM dbo.Customers
WHERE Country IN('USA', 'Canada')
ORDER BY ContactName, Country
GO

/*
2.	������� �� ������� Customers ���� ����������, �� ����������� � USA � Canada. 
	������ ������� � ������� ��������� IN. ���������� ������� � ������ 
	������������ � ��������� ������ � ����������� �������. 
	����������� ���������� ������� �� ����� ����������.
*/
SELECT 
	ContactName,
	Country
FROM dbo.Customers
WHERE Country NOT IN('USA', 'Canada')
ORDER BY ContactName
GO

/*
3.	������� �� ������� Customers ��� ������, � ������� ��������� ���������. 
	������ ������ ���� ��������� ������ ���� ��� � ������ ������������ �� ��������. 
	�� ������������ ����������� GROUP BY. 
	���������� ������ ���� ������� � ����������� �������. 
*/
SELECT DISTINCT
	Country
FROM dbo.Customers
ORDER BY Country DESC
GO