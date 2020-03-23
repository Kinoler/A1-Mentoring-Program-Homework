/*
1.	Выбрать в таблице Orders заказы, которые были доставлены после 6 мая 1998 года 
	(колонка ShippedDate) включительно и которые доставлены с ShipVia >= 2. 
	Запрос должен возвращать только колонки OrderID, ShippedDate и ShipVia. 
*/
SELECT 
	OrderID, 
	ShippedDate, 
	ShipVia
FROM dbo.Orders
WHERE ShippedDate > '1998-05-05 23:59:59.999' and ShipVia >= 2
GO

/*
2.	Написать запрос, который выводит только недоставленные заказы из таблицы Orders. 
	В результатах запроса возвращать для колонки ShippedDate вместо значений NULL 
	строку ‘Not Shipped’ (использовать системную функцию CASЕ). 
	Запрос должен возвращать только колонки OrderID и ShippedDate.
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
3.	Выбрать в таблице Orders заказы, которые были доставлены после 6 мая 1998 года 
	(ShippedDate) не включая эту дату или которые еще не доставлены. 
	В запросе должны возвращаться только колонки OrderID (переименовать в Order Number) и 
	ShippedDate (переименовать в Shipped Date). В результатах запроса возвращать для 
	колонки ShippedDate вместо значений NULL строку ‘Not Shipped’, для остальных значений 
	возвращать дату в формате по умолчанию.
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