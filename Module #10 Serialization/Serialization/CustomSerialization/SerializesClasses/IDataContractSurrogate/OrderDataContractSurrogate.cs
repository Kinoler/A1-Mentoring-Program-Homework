using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
using System.Reflection;
using System.Runtime.Serialization;
using Task.DB;

namespace Task.SerializesClasses
{
    public class OrderDataContractSurrogate : IDataContractSurrogate
    {
        public Type GetDataContractType(Type type)
        {
            if (type == typeof(Order))
            {
                return typeof(Order);
            }
            return type;
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            if (targetType == typeof(Order))
            {
                var order = (Order)obj;
                var newOrder = new Order
                {
                    Customer = order.Customer,
                    CustomerID = order.CustomerID,
                    Employee = order.Employee,
                    EmployeeID = order.EmployeeID,
                    Freight = order.Freight,
                    OrderDate = order.OrderDate,
                    OrderID = order.OrderID,
                    Order_Details = order.Order_Details,
                    RequiredDate = order.RequiredDate,
                    ShipAddress = order.ShipAddress,
                    ShipCity = order.ShipCity,
                    ShipCountry = order.ShipCountry,
                    ShipName = order.ShipName,
                    ShippedDate = order.ShippedDate,
                    Shipper = order.Shipper,
                    ShipPostalCode = order.ShipPostalCode,
                    ShipRegion = order.ShipRegion,
                    ShipVia = order.ShipVia
                };

                return newOrder;
            }

            if (targetType == typeof(Customer))
            {
                var customer = (Customer)obj;
                var newCustomer = new Customer
                {
                    CustomerID = customer.CustomerID,
                    Address = customer.Address,
                    City = customer.City,
                    CompanyName = customer.CompanyName,
                    ContactName = customer.ContactName,
                    ContactTitle = customer.ContactTitle,
                    Country = customer.Country,
                    CustomerDemographics = customer.CustomerDemographics,
                    Fax = customer.Fax,
                    Orders = null,
                    Phone = customer.Phone,
                    PostalCode = customer.PostalCode,
                    Region = customer.Region,
                };

                return newCustomer;
            }

            if (targetType == typeof(Employee))
            {
                var employee = (Employee)obj;
                var newEmployee = new Employee
                {
                    BirthDate = employee.BirthDate,
                    Address = employee.Address,
                    City = employee.City,
                    Employee1 = null,
                    EmployeeID = employee.EmployeeID,
                    Employees1 = null,
                    Country = employee.Country,
                    Extension = employee.Extension,
                    FirstName = employee.FirstName,
                    Orders = null,
                    HireDate = employee.HireDate,
                    PostalCode = employee.PostalCode,
                    Region = null,
                    HomePhone = employee.HomePhone,
                    Notes = employee.Notes,
                    LastName = employee.LastName,
                    Photo = employee.Photo,
                    PhotoPath = employee.PhotoPath,
                    ReportsTo = employee.ReportsTo,
                    Territories = null,
                    Title = employee.Title,
                    TitleOfCourtesy = employee.TitleOfCourtesy,
                };

                return newEmployee;
            }

            if (targetType == typeof(Order_Detail))
            {
                var orderDetail = (Order_Detail)obj;
                var newOrderDetail = new Order_Detail
                {
                    Discount = orderDetail.Discount,
                    Order = null,
                    OrderID = orderDetail.OrderID,
                    Product = null,
                    ProductID = orderDetail.ProductID,
                    Quantity = orderDetail.Quantity,
                    UnitPrice = orderDetail.UnitPrice,
                };

                return newOrderDetail;
            }

            if (targetType == typeof(Shipper))
            {
                var shipper = (Shipper)obj;
                var newShipper = new Shipper
                {
                    CompanyName = shipper.CompanyName,
                    Orders = null,
                    Phone = shipper.Phone,
                    ShipperID = shipper.ShipperID,
                };

                return newShipper;
            }

            return obj;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            return obj;
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            return null;
        }

        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            return null;
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            return typeof(Order);
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            return typeDeclaration;
        }
    }
}
