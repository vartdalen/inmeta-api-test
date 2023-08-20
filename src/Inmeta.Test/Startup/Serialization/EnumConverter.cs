using Inmeta.Test.Data.Utilities;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Inmeta.Test.Startup.Serialization
{
	internal class EnumConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        private readonly SerializationFormat _serializationFormat;
        public EnumConverter(SerializationFormat serializationFormat)
        {
            _serializationFormat = serializationFormat;
        }

        //Deserialize to string or hex (JSON to object instance)
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var enumValueString = reader.GetString();
                if ((int.TryParse(enumValueString, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var hexValue) ||
                    int.TryParse(enumValueString, out hexValue)) &&
                    (Enum.IsDefined(typeof(T), hexValue) || EnumUtilities.IsFlagsValid<T>(hexValue)))
                {
                    return (T)Enum.ToObject(typeof(T), hexValue);
                }
                else if (Enum.TryParse<T>(enumValueString, out var enumValue))
                {
                    return enumValue;
                }

                throw new JsonException($"Unable to convert '{enumValueString}' to enum '{typeToConvert.FullName}'");
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                var type = typeof(T).GetEnumUnderlyingType();
                if (type == typeof(byte) &&
                    reader.TryGetByte(out var byteValue) &&
                    (Enum.IsDefined(typeof(T), byteValue) || EnumUtilities.IsFlagsValid<T>(byteValue)))
                {
                    return (T)Enum.ToObject(typeof(T), byteValue);
                }
                if (type == typeof(short) &&
                    reader.TryGetInt16(out var shortValue) &&
                    (Enum.IsDefined(typeof(T), shortValue) || EnumUtilities.IsFlagsValid<T>(shortValue)))
                {
                    return (T)Enum.ToObject(typeof(T), shortValue);
                }
                if (type == typeof(int) &&
                    reader.TryGetInt32(out var intValue) &&
                    (Enum.IsDefined(typeof(T), intValue) || EnumUtilities.IsFlagsValid<T>(intValue)))
                {
                    return (T)Enum.ToObject(typeof(T), intValue);
                }

                throw new JsonException($"Unable to convert '{reader.GetInt32()}' to enum '{typeToConvert.FullName}'");
            }

            throw new JsonException($"Unexpected token type '{reader.TokenType}' when deserializing {typeof(T)}");
        }

        //Serialize to int, hex or string (object instance to JSON)
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var serializedValue = _serializationFormat switch
            {
                SerializationFormat.Int => Convert.ToInt32(value).ToString(),
                SerializationFormat.Hex => Convert.ToInt32(value).ToString("X"),
                SerializationFormat.String => value.ToString(),
                _ => throw new NotSupportedException($"{nameof(SerializationFormat)} {_serializationFormat} not supported")
            };
            writer.WriteStringValue(serializedValue);
        }
    }
}
