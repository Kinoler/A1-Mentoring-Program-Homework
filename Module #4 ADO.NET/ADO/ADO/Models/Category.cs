using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Models
{
    public class Category
    {
        private byte[] picture;

        public Category()
        {
        }

        public int CategoryID { get; private set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get => picture.Skip(78).ToArray(); set => picture = value; }
    }
}
