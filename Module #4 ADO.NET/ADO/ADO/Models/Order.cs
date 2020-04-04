using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Models
{
    public class Order
    {
        public Order()
        {
        }

        public int OrderID { get; internal set; }

        public string CustomerID { get; set; }

        public int? EmployeeID { get; set; }

        public DateTime? OrderDate { get; internal set; }

        public DateTime? RequiredDate { get; set; }

        public DateTime? ShippedDate { get; internal set; }

        public int? ShipVia { get; set; }

        public decimal? Freight { get; set; }

        public string ShipName { get; set; }

        public string ShipAddress { get; set; }

        public string ShipCity { get; set; }

        public string ShipRegion { get; set; }

        public string ShipPostalCode { get; set; }

        public string ShipCountry { get; set; }

        public OrderState Status =>
                    OrderDate == null ? OrderState.New :
                    ShippedDate == null ? OrderState.InProgress : OrderState.Complete;
    }
}
