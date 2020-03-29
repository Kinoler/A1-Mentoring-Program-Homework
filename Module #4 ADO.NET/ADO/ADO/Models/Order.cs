using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO
{
    public class Order
    {
        private string customerID;
        private int? employeeID;
        private DateTime? requiredDate;
        private int? shipVia;
        private decimal? freight;
        private string shipName;
        private string shipAddress;
        private string shipCity;
        private string shipRegion;
        private string shipPostalCode;
        private string shipCountry;

        public Order()
        {
        }

        public int OrderID { get; set; }
        public string CustomerID { get => customerID; set { if (State() == OrderState.New) customerID = value; } }
        public Nullable<int> EmployeeID { get => employeeID; set { if (State() == OrderState.New) employeeID = value; } }
        public Nullable<System.DateTime> OrderDate { get; internal set; }
        public Nullable<System.DateTime> RequiredDate { get => requiredDate; set { if (State() == OrderState.New) requiredDate = value; } }
        public Nullable<System.DateTime> ShippedDate { get; internal set; }
        public Nullable<int> ShipVia { get => shipVia; set { if (State() == OrderState.New) shipVia = value; } }
        public Nullable<decimal> Freight { get => freight; set { if (State() == OrderState.New) freight = value; } }
        public string ShipName { get => shipName; set { if (State() == OrderState.New) shipName = value; } }
        public string ShipAddress { get => shipAddress; set { if (State() == OrderState.New) shipAddress = value; } }
        public string ShipCity { get => shipCity; set { if (State() == OrderState.New) shipCity = value; } }
        public string ShipRegion { get => shipRegion; set { if (State() == OrderState.New) shipRegion = value; } }
        public string ShipPostalCode { get => shipPostalCode; set { if (State() == OrderState.New) shipPostalCode = value; } }
        public string ShipCountry { get => shipCountry; set { if (State() == OrderState.New) shipCountry = value; } }

        public OrderState State() =>
                    OrderDate == null ? OrderState.New :
                    ShippedDate == null ? OrderState.InProgress : OrderState.Complete;
    }
}
