// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
	[Title("LINQ Module")]
	[Prefix("Linq")]
	public class LinqSamples : SampleHarness
	{

		private DataSource dataSource = new DataSource();

        [Category("LINQ Tasks")]
        [Title("Where - Task 1")]
        [Description("Выдайте список всех клиентов, чей суммарный оборот (сумма всех заказов) " +
                     "превосходит некоторую величину X. Продемонстрируйте выполнение запроса с различными X ")]
        public void Linq1()
        {
            var X = 100000;
            var products = dataSource.Customers.Where(customer => customer.Orders.Sum(order => order.Total) > X);

            foreach (var product in products)
            {
                ObjectDumper.Write(product);
            }

            X = 10000;
            ObjectDumper.Write("X = 10000");
            foreach (var product in products)
            {
                ObjectDumper.Write(product);
            }
        }

        [Category("LINQ Tasks")]
        [Title("Where - Task 2")]
        [Description("Для каждого клиента составьте список поставщиков, " +
                     "находящихся в той же стране и том же городе. " +
                     "Сделайте задания с использованием группировки и без.")]
        public void Linq2()
        {
            ObjectDumper.Write("With group");
            var customersWithSuppliersInTheSameCity = dataSource.Customers
                .GroupJoin(
                    dataSource.Suppliers,
                    customer => customer.Country + customer.City,
                    supplier => supplier.Country + supplier.City,
                    (customer, suppliers) =>
                        new { Customer = customer, Suppliers = suppliers });

            foreach (var customerWithSuppliersInTheSameCity in customersWithSuppliersInTheSameCity)
            {
                ObjectDumper.Write(customerWithSuppliersInTheSameCity.Customer);
                ObjectDumper.Write(customerWithSuppliersInTheSameCity.Suppliers);
            }

            ObjectDumper.Write("Without group");
            customersWithSuppliersInTheSameCity = dataSource.Customers
                .Select(customer => 
                    new { 
                        Customer = customer, 
                        Suppliers = dataSource.Suppliers.Where(supplier => 
                            $"{supplier.Country} {supplier.City}" == $"{customer.Country} {customer.City}")
                    });

            foreach (var customerWithSuppliersInTheSameCity in customersWithSuppliersInTheSameCity)
            {
                ObjectDumper.Write(customerWithSuppliersInTheSameCity.Customer);
                ObjectDumper.Write(customerWithSuppliersInTheSameCity.Suppliers);
            }
        }	
        
        [Category("LINQ Tasks")]
        [Title("Where - Task 3")]
        [Description("Найдите всех клиентов, у которых были заказы, превосходящие по сумме величину X")]
        public void Linq3()
        {
            var X = 100000;
            var customersWithOrderExpensiveThenX = dataSource.Customers.Where(customer => customer.Orders.Any(order => order.Total > X));

            foreach (var customerWithOrderExpensiveThenX in customersWithOrderExpensiveThenX)
            {
                ObjectDumper.Write(customerWithOrderExpensiveThenX);
            }
        }		

        [Category("LINQ Tasks")]
        [Title("Where - Task 4")]
        [Description("Выдайте список клиентов с указанием, " +
                     "начиная с какого месяца какого года они стали клиентами " +
                     "(принять за таковые месяц и год самого первого заказа)")]
        public void Linq4()
        {
            var customersWithMinOrderDate = dataSource.Customers
                .Select(customer => 
                    new {
                        Customer = customer, 
                        MinOrderDate = customer.Orders?.Min(order => order.OrderDate)
                    });

            foreach (var customerWithMinOrderDate in customersWithMinOrderDate)
            {
                ObjectDumper.Write(customerWithMinOrderDate.MinOrderDate.HasValue
                    ? $"{customerWithMinOrderDate.Customer.CustomerID} Day: {customerWithMinOrderDate.MinOrderDate?.Day} Year: {customerWithMinOrderDate.MinOrderDate?.Year}"
                    : $"{customerWithMinOrderDate.Customer.CustomerID} Have no some order");
            }
        }	
        
        [Category("LINQ Tasks")]
        [Title("Where - Task 5")]
        [Description("Сделайте предыдущее задание, но выдайте список " +
                     "отсортированным по году, месяцу, оборотам клиента " +
                     "(от максимального к минимальному) и имени клиента")]
        public void Linq5()
        {
            var customersWithMinOrderDateOrderedByYearMonthTurnoverAndCompanyName = dataSource.Customers                
                .Select(customer => 
                    new {
                        Customer = customer, 
                        MinOrderDate = customer.Orders?.Min(order => order.OrderDate)
                    })
                .OrderBy(customerWithMinOrderDate => customerWithMinOrderDate.MinOrderDate?.Year)
                .ThenBy(customerWithMinOrderDateOrderedByYear => 
                    customerWithMinOrderDateOrderedByYear.MinOrderDate?.Month)
                .ThenByDescending(customerWithMinOrderDateOrderedByYearByMonth => 
                    customerWithMinOrderDateOrderedByYearByMonth.Customer.Orders.Sum(order => order.Total))
                .ThenBy(customerWithMinOrderDateOrderedByYearByMonthByTurnover => 
                    customerWithMinOrderDateOrderedByYearByMonthByTurnover.Customer.CompanyName);

            foreach (var customerWithMinOrderDate in customersWithMinOrderDateOrderedByYearMonthTurnoverAndCompanyName)
            {
                ObjectDumper.Write($"Year: {customerWithMinOrderDate.MinOrderDate?.Year} Month: {customerWithMinOrderDate.MinOrderDate?.Month} Name: {customerWithMinOrderDate.Customer.CompanyName}");
            }
        }		

        [Category("LINQ Tasks")]
        [Title("Where - Task 6")]
        [Description("Укажите всех клиентов, у которых указан нецифровой " +
                     "почтовый код или не заполнен регион или в телефоне не указан код " +
                     "оператора (для простоты считаем, что это равнозначно «нет круглых скобочек в начале»).")]
        public void Linq6()
        {
            var customersWithSameInvalidData = dataSource.Customers.Where(customer => 
                int.TryParse(customer.PostalCode, out _) || 
                string.IsNullOrEmpty(customer.Region) ||
                (!string.IsNullOrEmpty(customer.Phone) && customer.Phone[0]=='('));

            foreach (var customerWithSameInvalidData in customersWithSameInvalidData)
            {
                ObjectDumper.Write(customerWithSameInvalidData);
            }
        }		

        [Category("LINQ Tasks")]
        [Title("Where - Task 7")]
        [Description("Сгруппируйте все продукты по категориям, " +
                     "внутри – по наличию на складе, внутри последней группы отсортируйте по стоимости")]
        public void Linq7()
        {
            var productsGroupedByCategoryAndUnitsInStock = dataSource.Products
                .GroupBy(product => product.Category)
                .Select( 
                    productGroupedByCategory => 
                        new
                        {
                            Category = productGroupedByCategory.Key, 
                            ProductsGroupedByUnitsInStock = productGroupedByCategory
                                .Select(p => p)
                                .GroupBy(p => p.UnitsInStock != 0)
                                .Select(
                                    productGroupedByUnitsInStock => 
                                        new
                                        {
                                            UnitsInStock = productGroupedByUnitsInStock.Key,
                                            Products = productGroupedByUnitsInStock
                                                .Select(product => product)
                                                .OrderBy(product => product.UnitPrice)
                                        }
                                    )
                        }
                    );

            foreach (var productGroupedByCategoryAndUnitsInStock in productsGroupedByCategoryAndUnitsInStock)
            {
                foreach (var productGroupedByUnitsInStock in productGroupedByCategoryAndUnitsInStock.ProductsGroupedByUnitsInStock)
                {
                    foreach (var product in productGroupedByUnitsInStock.Products)
                    {
                        ObjectDumper.Write($"Category: {product.Category} - ExistsInStore: {product.UnitsInStock} - Price: {product.UnitPrice}");
                    }
                }
            }
        }		

        [Category("LINQ Tasks")]
        [Title("Where - Task 8")]
        [Description("Сгруппируйте товары по группам «дешевые», " +
                     "«средняя цена», «дорогие». Границы каждой группы задайте сами")]
        public void Linq8()
        {
            const int cheapBorder = 400;
            const int averagePriceBorder = 700;

            var productsGroupedByPrice = dataSource.Products.GroupBy(product => 
                product.UnitPrice < cheapBorder ? "Cheap" : 
                product.UnitPrice > averagePriceBorder ? "Expensive" : 
                "Average price");

            foreach (var productGroupedByPrice in productsGroupedByPrice)
            {
                foreach (var product in productGroupedByPrice)
                {
                    ObjectDumper.Write($"Category: {productGroupedByPrice.Key} - Price: {product.UnitPrice} - Name: {product.ProductName}");
                }
            }
        }

        [Category("LINQ Tasks")]
        [Title("Where - Task 9")]
        [Description("Рассчитайте среднюю прибыльность каждого города " +
                     "(среднюю сумму заказа по всем клиентам из данного города) и " +
                     "среднюю интенсивность (среднее количество заказов, " +
                     "приходящееся на клиента из каждого города)")]
        public void Linq9()
        {
            var profitabilities = dataSource.Customers
                .GroupBy(customer => customer.City)
                .Select(
                    customerGroupedByCity => 
                        new
                        {
                            City = customerGroupedByCity.Key,
                            AverageOrderPrice = customerGroupedByCity
                                .SelectMany(customer => customer.Orders, (customer, order) => order)
                                .Average(order => order.Total)
                        } 
                    );
            
            foreach (var profitability in profitabilities)
            {
                ObjectDumper.Write($"City: {profitability.City} - Average price: {profitability.AverageOrderPrice}");
            }

            var intensities = dataSource.Customers
                .GroupBy(customer => customer.City)
                .Select(
                    customerGroupedByCity => 
                        new
                        {
                            City = customerGroupedByCity.Key,
                            AverageOrderCount = 
                                customerGroupedByCity
                                    .SelectMany(customer => customer.Orders, (customer, order) => order)
                                    .Count() 
                                / 
                                customerGroupedByCity.Count()
                        } 
                );

            foreach (var intensity in intensities)
            {
                ObjectDumper.Write($"City: {intensity.City} - Average order: {intensity.AverageOrderCount}");
            }
        }
        
        [Category("LINQ Tasks")]
        [Title("Where - Task 10")]
        [Description("Сделайте среднегодовую статистику активности " +
                     "клиентов по месяцам (без учета года), статистику по годам, " +
                     "по годам и месяцам (т.е. когда один месяц в разные годы имеет своё значение)")]
        public void Linq10()
        {
            var activityCustomers = dataSource.Customers
                .SelectMany(
                    customer => customer.Orders,
                    (customer, order) => new { Customer = customer, OrderDate = order.OrderDate })
                .ToList();

            var activitiesByMonth = activityCustomers
                .GroupBy(customerActivity => customerActivity.OrderDate.Month)
                .Select(
                    activityByMonth => 
                        new
                        {
                            Month = activityByMonth.Key, 
                            Customers = activityByMonth.Select(c => c.Customer)
                        })
                .OrderBy(el => el.Month);
            
            var activitiesByYear = activityCustomers
                .GroupBy(customerActivity => customerActivity.OrderDate.Year)
                .Select(
                    activityByYear => 
                        new
                        {
                            Year = activityByYear.Key, 
                            Customers = activityByYear.Select(c => c.Customer)
                        })
                .OrderBy(el => el.Year);
            
            var activitiesByYearMonth = activityCustomers
                .GroupBy(customerActivity => $"{customerActivity.OrderDate.Year}:{customerActivity.OrderDate.Month}")
                .Select(
                    activityByYearMonth => 
                        new
                        {
                            YearMonth = activityByYearMonth.Key, 
                            Customers = activityByYearMonth.Select(c => c.Customer)
                        })
                .OrderBy(el => el.YearMonth);

            
            foreach (var activity in activitiesByMonth)
            {
                ObjectDumper.Write($"Month: {activity.Month} - Count order: {activity.Customers.Count()}");
            }
            
            foreach (var activity in activitiesByYear)
            {
                ObjectDumper.Write($"Year: {activity.Year} - Count order: {activity.Customers.Count()}");
            }
            
            foreach (var activity in activitiesByYearMonth)
            {
                ObjectDumper.Write($"{activity.YearMonth} - Count order: {activity.Customers.Count()}");
            }
        }
    }
}
