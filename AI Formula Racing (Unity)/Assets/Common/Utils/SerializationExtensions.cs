using System.IO;

namespace Assets.Common.Utils
{
    public static class SerializationExtensions
    {
        public static void Write(this BinaryWriter writer, float[] value)
        {
            writer.Write(value.Length);
            foreach (var v in value)
            {
                writer.Write(v);
            }
        }

        public static float[] ReadFloatArray(this BinaryReader reader)
        {
            int length = reader.ReadInt32();
            float[] result = new float[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = reader.ReadSingle();
            }
            return result;
        }

        public static void Write(this BinaryWriter writer, float? value)
        {
            writer.Write(value.HasValue);
            if (value.HasValue)
            {
                writer.Write(value.Value);
            }
        }

        public static float? ReadNullableFloat(this BinaryReader reader)
        {
            bool hasValue = reader.ReadBoolean();
            if (hasValue)
            {
                return reader.ReadSingle();
            }
            return null;
        }
    }
}
