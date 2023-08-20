using System.Text.Json;
using Inmeta.Test.Data.Models.Enums;
using Inmeta.Test.Startup.Serialization;

namespace Inmeta.Test.Startup.Factories
{
    internal static class JsonSerializerOptionsFactory
    {
        internal static JsonSerializerOptions Create()
        {
            return new JsonSerializerOptions
            {
                Converters = {
					new EnumConverter<Country>(SerializationFormat.String),
					new EnumConverter<OrderStatus>(SerializationFormat.String),
                    new EnumConverter<ZipCode>(SerializationFormat.String),
					new EnumConverter<Service>(SerializationFormat.String)
				},
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
        }
    }
}