using DataModel;
using LinqToDB.Mapping;

namespace DataModel
{
    public partial class Product
    {
        public static Product Build(Product product, Category category)
        {
            if (product != null)
            {
                product.Category = category;
            }
            return product;
        }

        public static Product Build(Product product, Category category, Supplier supplier)
        {
            if (product != null)
            {
                product.Category = category;
            }
            return Product.Build(product, category);
        }
    }

    public partial class OrderDetail
    {
        public static DataModel.OrderDetail Build(DataModel.OrderDetail orderDetail, DataModel.Product product)
        {
            if (orderDetail != null)
            {
                orderDetail.Product = product;
            }
            return orderDetail;
        }

        public static OrderDetail Build(OrderDetail orderDetail, Order order)
        {
            if (orderDetail != null)
            {
                orderDetail.Order = order;
            }
            return orderDetail;
        }
    }
}
