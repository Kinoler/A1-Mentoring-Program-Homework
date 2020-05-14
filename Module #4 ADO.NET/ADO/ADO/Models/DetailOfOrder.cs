using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Models
{
    public class DetailOfOrder
    {
        public int OrderID { get; set; }

        public int ProductID { get; set; }

        public int CategoryID { get; set; }

        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

        public string ProductName { get; set; }

        public string QuantityPerUnit { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }
    }
}
