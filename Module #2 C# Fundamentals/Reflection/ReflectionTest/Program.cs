using Reflection.Attributes;
using Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container();
            container.AddType(typeof(CustomerBLLFromConstructor));
            container.AddType(typeof(CustomerBLLWithProperty));
            container.AddType(typeof(Logger));
            container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

            var customerBLLFromConstructor = (CustomerBLLFromConstructor)container.CreateInstance(typeof(CustomerBLLFromConstructor));
            var customerBLLWithProperty = container.CreateInstance<CustomerBLLWithProperty>();
        }
    }

    public interface ICustomerDAL { }

    [ImportConstructor]
    public class CustomerBLLFromConstructor
    {
        public CustomerBLLFromConstructor(ICustomerDAL dal, Logger logger) { }
    }

    public class CustomerBLLWithProperty
    {
        [Import]
        public ICustomerDAL CustomerDAL { get; set; }
        [Import]
        public Logger logger { get; set; }
    }

    [Export]
    public class Logger { }

    [Export(typeof(ICustomerDAL))]
    public class CustomerDAL : ICustomerDAL { }
}
