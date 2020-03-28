// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
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
            var products = dataSource.Customers.Where(cus => cus.Orders.Sum(el => el.Total) > X);
            foreach (var p in products)
            {
                ObjectDumper.Write(p);
            }

            X = 10000;
            ObjectDumper.Write("X = 10000");
            foreach (var p in products)
            {
                ObjectDumper.Write(p);
            }
        }

        [Category("LINQ Tasks")]
        [Title("Where - Task 2")]
        [Description("Для каждого клиента составьте список поставщиков, " +
                     "находящихся в той же стране и том же городе. " +
                     "Сделайте задания с использованием группировки и без.")]
        public void Linq2()
        {
            var customers = dataSource.Customers
                .GroupJoin(
                    dataSource.Suppliers, 
                    outer => outer.Country + outer.City, 
                    inner => inner.Country + inner.City, 
                    (customer, suppliers) => 
                        new KeyValuePair<Customer, IEnumerable<Supplier>>(customer, suppliers))
                .ToDictionary(el => el.Key, el => el.Value);
            
            foreach (var c in customers)
            {
                ObjectDumper.Write(c.Key);
                ObjectDumper.Write(c.Value);
            }

            ObjectDumper.Write(" Without group");
            customers = dataSource.Customers
                .Select(cus => new KeyValuePair<Customer, IEnumerable<Supplier>>(
                    cus, dataSource.Suppliers.Where(sup => sup.Country + sup.City == cus.Country + cus.City)))
                .ToDictionary(el => el.Key, el => el.Value);

            foreach (var c in customers)
            {
                ObjectDumper.Write(c.Key);
                ObjectDumper.Write(c.Value);
            }
        }	
        
        [Category("LINQ Tasks")]
        [Title("Where - Task 3")]
        [Description("Найдите всех клиентов, у которых были заказы, превосходящие по сумме величину X")]
        public void Linq3()
        {
            var X = 100000;
            var customers = dataSource.Customers.Where(cus => cus.Orders.Any(el => el.Total > X));

            foreach (var c in customers)
            {
                ObjectDumper.Write(c);
            }
            
        }		

        [Category("LINQ Tasks")]
        [Title("Where - Task 4")]
        [Description("Выдайте список клиентов с указанием, " +
                     "начиная с какого месяца какого года они стали клиентами " +
                     "(принять за таковые месяц и год самого первого заказа)")]
        public void Linq4()
        {
            var customers = dataSource.Customers.Select(cus => 
                new KeyValuePair<Customer, DateTime>(cus, cus.Orders.Min(o => o.OrderDate)));
            
            foreach (var c in customers)
            {
                ObjectDumper.Write($"{c.Key.CustomerID} Day: {c.Value.Day} Year: {c.Value.Year}");
            }
        }	
        
        [Category("LINQ Tasks")]
        [Title("Where - Task 5")]
        [Description("Сделайте предыдущее задание, но выдайте список " +
                     "отсортированным по году, месяцу, оборотам клиента " +
                     "(от максимального к минимальному) и имени клиента")]
        public void Linq5()
        {
            var customers = dataSource.Customers.Select(cus => 
                new KeyValuePair<Customer, DateTime>(cus, cus.Orders.Min(o => o.OrderDate)))
                .OrderBy(pair => pair.Value.Year)
                .ThenBy(pair => pair.Value.Month)
                .ThenByDescending(pair => pair.Key.Orders.Sum(el => el.Total))
                .ThenBy(pair => pair.Key.CompanyName);

            foreach (var c in customers)
            {
                ObjectDumper.Write($"Day: {c.Value.Day} Year: {c.Value.Year} Name: {c.Key.CompanyName}");
            }
        }		

        [Category("LINQ Tasks")]
        [Title("Where - Task 6")]
        [Description("Укажите всех клиентов, у которых указан нецифровой " +
                     "почтовый код или не заполнен регион или в телефоне не указан код " +
                     "оператора (для простоты считаем, что это равнозначно «нет круглых скобочек в начале»).")]
        public void Linq6()
        {
            var customers = dataSource.Customers.Where(cus => 
                int.TryParse(cus.PostalCode, out _) || 
                string.IsNullOrEmpty(cus.Region) ||
                (!string.IsNullOrEmpty(cus.Phone) && cus.Phone[0]=='('));

            foreach (var c in customers)
            {
                ObjectDumper.Write(c);
            }
        }		

        [Category("LINQ Tasks")]
        [Title("Where - Task 7")]
        [Description("Сгруппируйте все продукты по категориям, " +
                     "внутри – по наличию на складе, внутри последней группы отсортируйте по стоимости")]
        public void Linq7()
        {
            var products = dataSource.Products
                .GroupBy(el => el.Category)
                .ToDictionary(
                    el => el.Key,
                    el => el.Select(p => p)
                        .GroupBy(p => p.UnitsInStock != 0)
                        .ToDictionary(
                            p => p.Key, 
                            p => p.Select(ip => ip).OrderBy(ip => ip.UnitPrice)));

            foreach (var p in products)
            {
                foreach (var vp in p.Value)
                {
                    foreach (var vvp in vp.Value)
                    {
                        ObjectDumper.Write($"Category: {p.Key} - ExistsInStore: {vp.Key} - Price: {vvp.UnitPrice}");
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
            var first = 400;
            var second = 700;

            var products = dataSource.Products.GroupBy(p => 
                p.UnitPrice < first ? "Дешевые" : 
                p.UnitPrice > second ? "Дорогие" : 
                "Средняя цена");

            foreach (var p in products)
            {
                foreach (var vp in p)
                {
                    ObjectDumper.Write($"Category: {p.Key} - Price: {vp.UnitPrice} - Name: {vp.ProductName}");
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
            var profitability = dataSource.Customers
                .GroupBy(cus => cus.City)
                .ToDictionary(
                    el => el.Key, 
                    el => el
                        .SelectMany(cus => cus.Orders, (customer, order) => order)
                        .Average(o => o.Total));
            
            foreach (var p in profitability)
            {
                ObjectDumper.Write($"City: {p.Key} - Average price: {p.Value}");
            }

            var intensity = dataSource.Customers
                .GroupBy(cus => cus.City)
                .ToDictionary(
                    el => el.Key, 
                    el => 
                        el.SelectMany(cus => cus.Orders, (customer, order) => order).Count() / 
                        el.Count());

            foreach (var i in intensity)
            {
                ObjectDumper.Write($"City: {i.Key} - Average order: {i.Value}");
            }
        }
        
        [Category("LINQ Tasks")]
        [Title("Where - Task 10")]
        [Description("Сделайте среднегодовую статистику активности " +
                     "клиентов по месяцам (без учета года), статистику по годам, " +
                     "по годам и месяцам (т.е. когда один месяц в разные годы имеет своё значение)")]
        public void Linq10()
        {
            var customerActivity = dataSource.Customers
                .SelectMany(
                    cus => cus.Orders,
                    (customer, order) => new KeyValuePair<Customer, DateTime>(customer, order.OrderDate))
                .ToList();

            var byMonth = customerActivity
                .GroupBy(el => el.Value.Month)
                .ToDictionary(el => el.Key, el => el.Select(c => c.Key))
                .OrderBy(el => el.Key);

            var byYear = customerActivity
                .GroupBy(el => el.Value.Year)
                .ToDictionary(el => el.Key, el => el.Select(c => c.Key))
                .OrderBy(el => el.Key);

            var byYearMonth = customerActivity
                .GroupBy(el => $"{el.Value.Year}:{el.Value.Month}")
                .ToDictionary(el => el.Key, el => el.Select(c => c.Key))
                .OrderBy(el => el.Key);

            
            foreach (var i in byMonth)
            {
                ObjectDumper.Write($"Month: {i.Key} - Count order: {i.Value.Count()}");
            }
            
            foreach (var i in byYear)
            {
                ObjectDumper.Write($"Year: {i.Key} - Count order: {i.Value.Count()}");
            }
            
            foreach (var i in byYearMonth)
            {
                ObjectDumper.Write($"{i.Key} - Count order: {i.Value.Count()}");
            }
        }
    }
}
