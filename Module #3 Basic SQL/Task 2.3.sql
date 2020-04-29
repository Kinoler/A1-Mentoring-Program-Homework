/*
1.	Îïðåäåëèòü ïðîäàâöîâ, êîòîðûå îáñëóæèâàþò ðåãèîí 'Western' (òàáëèöà Region). 
*/
SELECT DISTINCT
	employee.FirstName
FROM dbo.Employees AS employee
JOIN dbo.EmployeeTerritories AS employeeTerritories
	ON employeeTerritories.EmployeeID = employee.EmployeeID
JOIN dbo.Territories AS territory
	ON territory.TerritoryID = employeeTerritories.TerritoryID
JOIN dbo.Region AS region
	ON region.RegionID = territory.RegionID
WHERE region.RegionDescription = 'Western'
GO

/*
2.	Âûäàòü â ðåçóëüòàòàõ çàïðîñà èìåíà âñåõ çàêàç÷èêîâ èç òàáëèöû Customers è 
	ñóììàðíîå êîëè÷åñòâî èõ çàêàçîâ èç òàáëèöû Orders. 
	Ïðèíÿòü âî âíèìàíèå, ÷òî ó íåêîòîðûõ çàêàç÷èêîâ íåò çàêàçîâ, íî îíè òàêæå 
	äîëæíû áûòü âûâåäåíû â ðåçóëüòàòàõ çàïðîñà. 
	Óïîðÿäî÷èòü ðåçóëüòàòû çàïðîñà ïî âîçðàñòàíèþ êîëè÷åñòâà çàêàçîâ.
*/
SELECT
	customer.ContactName,
	COUNT(OrderID) AS CountOrders
FROM dbo.Customers AS customer
LEFT JOIN dbo.Orders AS orders
	ON orders.CustomerID = customer.CustomerID
GROUP BY customer.CustomerID, customer.ContactName
ORDER BY CountOrders