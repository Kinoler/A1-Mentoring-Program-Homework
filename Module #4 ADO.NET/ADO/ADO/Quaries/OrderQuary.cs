namespace ADO.Quaries
{
    internal abstract class OrderQuary
    {
        protected OrderQuary()
        {
        }

        public static OrderQuary OrderQuaryFactory(string providerName)
        {
            switch (providerName)
            {
                case "System.Data.SqlClient":
                    return new OrderSQL();
                default:
                    throw new System.NotSupportedException($"The {providerName} provider does not support");
            }
        }

        public abstract string AddQuery { get; }

        public abstract string SelectAllQuery { get; }

        public abstract string SelectOneQuery { get; }

        public abstract string SelectOneDetailQuery { get; }

        public abstract string DeleteOneQuery { get; }

        public abstract string UpdateQuery { get; }
    }
}
