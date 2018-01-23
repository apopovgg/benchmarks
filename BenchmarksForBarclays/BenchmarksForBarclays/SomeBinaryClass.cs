using Apache.Ignite.Core.Binary;

namespace BenchmarksForBarclays
{
    public class SomeBinaryClass : SomeNativeClass, IBinarizable
    {
        public SomeBinaryClass() : base()
        {
            // No-op
        }

        public void WriteBinary(IBinaryWriter writer)
        {
            writer.WriteDoubleArray("data", Data);
        }

        public void ReadBinary(IBinaryReader reader)
        {
            Data = reader.ReadDoubleArray("data");
        }
    }
}
