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

        public ICollection<OrderDetail> OrderDetails { get; internal set; }

        public bool Equals(Order other)
        {
            if (!Equals(this.CustomerID, other.CustomerID)) return false;
            if (!Equals(this.EmployeeID, other.EmployeeID)) return false;
            if (!Equals(this.Freight, other.Freight)) return false;
            if (!Equals(this.OrderDate, other.OrderDate)) return false;
            if (!Equals(this.OrderDetails, other.OrderDetails)) return false;
            if (!Equals(this.OrderID, other.OrderID)) return false;
            if (!Equals(this.RequiredDate, other.RequiredDate)) return false;
            if (!Equals(this.ShipAddress, other.ShipAddress)) return false;
            if (!Equals(this.ShipCity, other.ShipCity)) return false;
            if (!Equals(this.ShipCountry, other.ShipCountry)) return false;
            if (!Equals(this.ShipName, other.ShipName)) return false;
            if (!Equals(this.ShippedDate, other.ShippedDate)) return false;
            if (!Equals(this.ShipPostalCode, other.ShipPostalCode)) return false;
            if (!Equals(this.ShipRegion, other.ShipRegion)) return false;
            if (!Equals(this.ShipVia, other.ShipVia)) return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals((Order)obj);
        }
    }
}
