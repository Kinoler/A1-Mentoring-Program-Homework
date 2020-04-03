/*
1.	Ќайти общую сумму всех заказов из таблицы Order Details с учетом количества 
	закупленных товаров и скидок по ним. –езультатом запроса должна быть одна 
	запись с одной колонкой с названием колонки 'Totals'.
*/
SELECT 
	SUM((UnitPrice- Discount) * Quantity) as 'Totals'
FROM dbo.[Order Details]
GO

/*
2.	ѕо таблице Orders найти количество заказов, которые еще не были доставлены 
	(т.е. в колонке ShippedDate нет значени€ даты доставки). 
	»спользовать при этом запросе только оператор COUNT. 
	Ќе использовать предложени€ WHERE и GROUP.
*/
SELECT 
	COUNT(*)
FROM dbo.Orders
WHERE ShippedDate is null
GO

/*
3.	ѕо таблице Orders найти количество различных покупателей (CustomerID), 
	сделавших заказы. 
	»спользовать функцию COUNT и не использовать предложени€ WHERE и GROUP.
*/
SELECT
	COUNT(UnicCustomerIDs.CustomerID)
FROM (SELECT DISTINCT CustomerID FROM dbo.Orders) as UnicCustomerIDs
GO