/*
1.	Выбрать все заказы (OrderID) из таблицы Order Details (заказы не должны повторяться), 
	где встречаются продукты с количеством от 3 до 10 включительно – это колонка Quantity 
	в таблице Order Details. Использовать оператор BETWEEN. 
	Запрос должен возвращать только колонку OrderID.
*/
SELECT DISTINCT
	OrderID
FROM dbo.[Order Details]
WHERE Quantity BETWEEN 3 AND 10
GO

/*
2.	Выбрать всех заказчиков из таблицы Customers, у которых название страны начинается 
	на буквы из диапазона b и g. Использовать оператор BETWEEN. 
	Проверить, что в результаты запроса попадает Germany. 
	Запрос должен возвращать только колонки CustomerID и Country и отсортирован по Country.
*/
SELECT 
	CustomerID,
	Country
FROM dbo.Customers
WHERE SUBSTRING(Country, 1, 1) BETWEEN 'b' AND 'g'
ORDER BY Country
GO

/*
3.	Выбрать всех заказчиков из таблицы Customers, у которых название страны начинается 
	на буквы из диапазона b и g, не используя оператор BETWEEN. 
*/
SELECT 
	CustomerID,
	Country
FROM dbo.Customers
WHERE SUBSTRING(Country, 1, 1) >= 'b' AND SUBSTRING(Country, 1, 1) <= 'g'
GO