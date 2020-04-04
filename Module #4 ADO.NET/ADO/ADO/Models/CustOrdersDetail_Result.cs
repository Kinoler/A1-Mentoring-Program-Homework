using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Models
{
    public partial class CustOrdersDetail_Result
    {
        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public int? Discount { get; set; }

        public decimal? ExtendedPrice { get; set; }
    }
}
