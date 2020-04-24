using DataModel;
using LinqToDB.Mapping;

namespace DataModel
{
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
