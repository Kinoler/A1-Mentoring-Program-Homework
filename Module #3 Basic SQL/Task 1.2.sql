/*
1.	Выбрать из таблицы Customers всех заказчиков, проживающих в USA и Canada. 
	Запрос сделать с только помощью оператора IN. Возвращать колонки с именем 
	пользователя и названием страны в результатах запроса. Упорядочить результаты 
	запроса по имени заказчиков и по месту проживания.
*/
SELECT 
	ContactName,
	Country
FROM dbo.Customers
WHERE Country IN('USA', 'Canada')
ORDER BY ContactName, Country
GO

/*
2.	Выбрать из таблицы Customers всех заказчиков, не проживающих в USA и Canada. 
	Запрос сделать с помощью оператора IN. Возвращать колонки с именем 
	пользователя и названием страны в результатах запроса. 
	Упорядочить результаты запроса по имени заказчиков.
*/
SELECT 
	ContactName,
	Country
FROM dbo.Customers
WHERE Country NOT IN('USA', 'Canada')
ORDER BY ContactName
GO

/*
3.	Выбрать из таблицы Customers все страны, в которых проживают заказчики. 
	Страна должна быть упомянута только один раз и список отсортирован по убыванию. 
	Не использовать предложение GROUP BY. 
	Возвращать только одну колонку в результатах запроса. 
*/
SELECT DISTINCT
	Country
FROM dbo.Customers
ORDER BY Country DESC
GO