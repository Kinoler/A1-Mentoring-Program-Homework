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
WHERE CONVERT(VARCHAR(26), ShippedDate, 23) >= '1998-05-06' and ShipVia >= 2
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
	END AS ShippedDate
FROM dbo.Orders
WHERE ShippedDate IS NULL
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
		ELSE CONVERT(VARCHAR(25), ShippedDate, 120) 
	END AS 'Shipped Date'
FROM dbo.Orders
WHERE CONVERT(VARCHAR(26), ShippedDate, 23) > '1998-05-06' OR ShippedDate IS NULL
GO