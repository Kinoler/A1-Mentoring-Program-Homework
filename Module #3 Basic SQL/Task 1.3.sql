/*
1.	¬ыбрать все заказы (OrderID) из таблицы Order Details (заказы не должны повтор€тьс€), 
	где встречаютс€ продукты с количеством от 3 до 10 включительно Ц это колонка Quantity 
	в таблице Order Details. »спользовать оператор BETWEEN. 
	«апрос должен возвращать только колонку OrderID.
*/
SELECT DISTINCT
	OrderID
FROM dbo.[Order Details]
WHERE Quantity BETWEEN 3 and 10
GO

/*
2.	¬ыбрать всех заказчиков из таблицы Customers, у которых название страны начинаетс€ 
	на буквы из диапазона b и g. »спользовать оператор BETWEEN. 
	ѕроверить, что в результаты запроса попадает Germany. 
	«апрос должен возвращать только колонки CustomerID и Country и отсортирован по Country.
*/
SELECT 
	CustomerID,
	Country
FROM dbo.Customers
WHERE SUBSTRING(Country, 1, 1) BETWEEN 'b' and 'g'
ORDER BY Country
GO

/*
3.	¬ыбрать всех заказчиков из таблицы Customers, у которых название страны начинаетс€ 
	на буквы из диапазона b и g, не использу€ оператор BETWEEN. 
*/
SELECT 
	CustomerID,
	Country
FROM dbo.Customers
WHERE SUBSTRING(Country, 1, 1) >= 'b' and SUBSTRING(Country, 1, 1) <= 'g'
GO