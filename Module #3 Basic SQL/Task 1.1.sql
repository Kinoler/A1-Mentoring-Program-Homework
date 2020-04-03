/*
1.	������� � ������� Orders ������, ������� ���� ���������� ����� 6 ��� 1998 ���� 
	(������� ShippedDate) ������������ � ������� ���������� � ShipVia >= 2. 
	������ ������ ���������� ������ ������� OrderID, ShippedDate � ShipVia. 
*/
SELECT 
	OrderID, 
	ShippedDate, 
	ShipVia
FROM dbo.Orders
WHERE ShippedDate > '1998-05-05 23:59:59.999' and ShipVia >= 2
GO

/*
2.	�������� ������, ������� ������� ������ �������������� ������ �� ������� Orders. 
	� ����������� ������� ���������� ��� ������� ShippedDate ������ �������� NULL 
	������ �Not Shipped� (������������ ��������� ������� CAS�). 
	������ ������ ���������� ������ ������� OrderID � ShippedDate.
*/
SELECT 
	OrderID, 
	CASE
		WHEN ShippedDate IS NULL THEN 'Not Shipped'
	END as ShippedDate
FROM dbo.Orders
WHERE ShippedDate is null
GO

/*
3.	������� � ������� Orders ������, ������� ���� ���������� ����� 6 ��� 1998 ���� 
	(ShippedDate) �� ������� ��� ���� ��� ������� ��� �� ����������. 
	� ������� ������ ������������ ������ ������� OrderID (������������� � Order Number) � 
	ShippedDate (������������� � Shipped Date). � ����������� ������� ���������� ��� 
	������� ShippedDate ������ �������� NULL ������ �Not Shipped�, ��� ��������� �������� 
	���������� ���� � ������� �� ���������.
*/
SELECT 
	OrderID as 'Order Number', 
	CASE
		WHEN ShippedDate IS NULL THEN 'Not Shipped'
		ELSE convert(varchar(25), ShippedDate, 120) 
	END as 'Shipped Date'
FROM dbo.Orders
WHERE ShippedDate > '1998-05-06' or ShippedDate is null
GO