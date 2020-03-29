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

        public int OrderID { get; set; }
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

        public OrderState State() =>
                    OrderDate == null ? OrderState.New :
                    ShippedDate == null ? OrderState.InProgress : OrderState.Complete;

        private bool Equals(Order obj)
        {
            if (!CustomerID.Equals(obj.CustomerID)) return false;
            if (!EmployeeID.Equals(obj.EmployeeID)) return false;
            if (!Freight.Equals(obj.Freight)) return false;
            if (!OrderDate.Equals(obj.OrderDate)) return false;
            if (!RequiredDate.Equals(obj.RequiredDate)) return false;
            if (!OrderID.Equals(obj.OrderID)) return false;
            if (!ShipAddress.Equals(obj.ShipAddress)) return false;
            if (!ShipCity.Equals(obj.ShipCity)) return false;
            if (!ShipCountry.Equals(obj.ShipCountry)) return false;
            if (!ShipName.Equals(obj.ShipName)) return false;
            if (!ShippedDate.Equals(obj.ShippedDate)) return false;
            if (!ShipPostalCode.Equals(obj.ShipPostalCode)) return false;
            if (!ShipRegion.Equals(obj.ShipRegion)) return false;
            if (!ShipVia.Equals(obj.ShipVia)) return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj)) return false;
            return Equals((Order)obj);
        }

        public override int GetHashCode()
        {
            return (typeof(Order).GetHashCode() + OrderID).GetHashCode();
        }

        public override string ToString()
        {
            return OrderID.ToString();
        }
    }
}
