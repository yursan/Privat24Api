using System;
using System.Buffers.Text;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Privat24
{
    public class DateTimeConverterForUkrainianFormat : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringVal = reader.GetString();
            if(DateTime.TryParseExact(stringVal, @"dd.MM.yyyy", new CultureInfo("uk-UA"), DateTimeStyles.None, out DateTime date))
                return date;

            return DateTime.MinValue;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // The "R" standard format will always be 29 bytes.
            Span<byte> utf8Date = new byte[25];

            bool result = Utf8Formatter.TryFormat(value, utf8Date, out _);
            writer.WriteStringValue(utf8Date);
        }
    }
}