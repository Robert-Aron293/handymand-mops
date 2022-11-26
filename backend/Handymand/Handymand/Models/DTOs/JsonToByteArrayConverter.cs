using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Handymand.Models.DTOs
{
    internal sealed class JsonToByteArrayConverter : JsonConverter<byte[]?>
    {
        // Converts base64 encoded string to byte[].
        public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            short[] sByteArray = JsonSerializer.Deserialize<short[]>(ref reader);
            byte[] value = new byte[sByteArray.Length];
            for (int i = 0; i < sByteArray.Length; i++)
            {
                value[i] = (byte)sByteArray[i];
            }

            return value;
            /*            if (!reader.TryGetBytesFromBase64(out byte[]? result) || result == default)
                        {

                            throw new Exception("Add your fancy exception message here...");
                        }
                        return result;*/
        }

        // Converts byte[] to base64 encoded string.
        public override void Write(Utf8JsonWriter writer, byte[]? value, JsonSerializerOptions options)
        {
            writer.WriteBase64StringValue(value);
        }
    }
}
