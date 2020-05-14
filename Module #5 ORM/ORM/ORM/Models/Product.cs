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
                product.Supplier = supplier;
            }
            return Product.Build(product, category);
        }
    }

}
