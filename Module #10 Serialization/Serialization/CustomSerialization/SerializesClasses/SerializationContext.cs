using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace Task.SerializesClasses
{
    public class SerializationContext
    {
        public SerializationContext(ObjectContext objectContext)
        {
            ObjectContext = objectContext;
        }

        public ObjectContext ObjectContext { get; }
    }
}
