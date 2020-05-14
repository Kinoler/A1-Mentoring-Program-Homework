using System.IO;
using System.Runtime.Serialization;

namespace CachingSolutionsSamples.Helpers
{
    public class Serializer<TSerialize>
    {
        private readonly DataContractSerializer _serializer = new DataContractSerializer(
            typeof(TSerialize));

        public void Serialize(TSerialize obj, out MemoryStream stream)
        {
            stream = new MemoryStream();
            _serializer.WriteObject(stream, obj);
        }

        public TSerialize Deserialize(Stream stream)
        {
            return (TSerialize)_serializer.ReadObject(stream);
        }
    }
}
