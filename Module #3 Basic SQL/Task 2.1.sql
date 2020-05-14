/*
1.	Найти общую сумму всех заказов из таблицы Order Details с учетом количества 
	закупленных товаров и скидок по ним. Результатом запроса должна быть одна 
	запись с одной колонкой с названием колонки 'Totals'.
*/
SELECT 
	SUM((UnitPrice - Discount) * Quantity) AS 'Totals'
FROM dbo.[Order Details]
GO

/*
2.	По таблице Orders найти количество заказов, которые еще не были доставлены 
	(т.е. в колонке ShippedDate нет значения даты доставки). 
	Использовать при этом запросе только оператор COUNT. 
	Не использовать предложения WHERE и GROUP.
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
3.	По таблице Orders найти количество различных покупателей (CustomerID), 
	сделавших заказы. 
	Использовать функцию COUNT и не использовать предложения WHERE и GROUP.
*/
SELECT
	COUNT(DISTINCT CustomerID)
FROM dbo.Orders
GO