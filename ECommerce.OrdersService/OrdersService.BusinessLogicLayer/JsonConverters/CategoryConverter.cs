using OrdersService.BusinessLogicLayer.DTOs.Enums;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace OrdersService.BusinessLogicLayer.JsonConverters
{
    public class CategoryConverter : JsonConverter<Category>
    {
        public override Category Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Console.WriteLine($"TokenType: {reader.TokenType}");

            if (reader.TokenType == JsonTokenType.String)
            {
                string categoryString = reader.GetString()?.Trim() ?? throw new JsonException("Category value is null.");
                Console.WriteLine($"Category string: {categoryString}");

                if (Enum.TryParse(categoryString, ignoreCase: true, out Category category))
                {
                    Console.WriteLine($"Parsed category: {category}");
                    return category;
                }

                throw new JsonException($"Invalid category value: {categoryString}");
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                int categoryValue = reader.GetInt32();
                Console.WriteLine($"Category number: {categoryValue}");

                if (Enum.IsDefined(typeof(Category), categoryValue))
                {
                    return (Category)categoryValue;
                }

                throw new JsonException($"Invalid category value: {categoryValue}");
            }

            throw new JsonException($"Unexpected token type for Category: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, Category value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
