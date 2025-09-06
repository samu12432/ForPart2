using System.Text.Json;
using System.Text.Json.Serialization;
using ForParts.Models.Customers;

namespace ForParts.Converters
{
    public class DireccionJsonConverter : JsonConverter<Direccion>
    {
        public override Direccion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    return null;
                }
                // Si llega una cadena no vacía, devolver una dirección vacía
                return new Direccion();
            }

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                return null;
            }

            var direccion = new Direccion();
            
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName?.ToLower())
                    {
                        case "calle":
                            direccion.Calle = reader.GetString() ?? string.Empty;
                            break;
                        case "numero":
                            direccion.Numero = reader.GetString() ?? string.Empty;
                            break;
                        case "ciudad":
                            direccion.Ciudad = reader.GetString() ?? string.Empty;
                            break;
                        case "departamento":
                            direccion.Departamento = reader.GetString() ?? string.Empty;
                            break;
                        case "codigopostal":
                            direccion.CodigoPostal = reader.GetString() ?? string.Empty;
                            break;
                        case "pais":
                            direccion.Pais = reader.GetString() ?? "Uruguay";
                            break;
                    }
                }
            }

            return direccion;
        }

        public override void Write(Utf8JsonWriter writer, Direccion value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();
            writer.WriteString("calle", value.Calle);
            writer.WriteString("numero", value.Numero);
            writer.WriteString("ciudad", value.Ciudad);
            writer.WriteString("departamento", value.Departamento);
            writer.WriteString("codigoPostal", value.CodigoPostal);
            writer.WriteString("pais", value.Pais);
            writer.WriteEndObject();
        }
    }
}