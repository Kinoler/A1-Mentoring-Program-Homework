using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADO.Models;

namespace ADO.Interfaces
{
    public interface IOrdersRepository
    {
        IEnumerable<Order> GetOrders(int windowCapacity, int windowNumber);
        Order GetOrder(int id);
        IEnumerable<DetailOfOrder> GetDetailOfOrder(int id);
        void Add(Order order);
        void Update(Order order);
        void Delete(Order order);
        void SetOrderDate(Order order, DateTime orderDate);
        void SetShippedDate(Order order, DateTime shippedDate);
    }
}
