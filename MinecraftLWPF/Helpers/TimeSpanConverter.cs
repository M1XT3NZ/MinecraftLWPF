using System;
using Newtonsoft.Json;

public class TimeSpanConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(TimeSpan);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var timeString = (string)reader.Value;
        // Split the string by ':' to separate hours, minutes, and seconds
        var parts = timeString.Split(':');
        if (parts.Length != 3) throw new FormatException("The TimeSpan string is not in a valid format");

        if (int.TryParse(parts[0], out var hours) &&
            int.TryParse(parts[1], out var minutes) &&
            int.TryParse(parts[2], out var seconds))
            // Construct a TimeSpan object from the parts
            return new TimeSpan(hours, minutes, seconds);
        throw new FormatException("The TimeSpan string contains invalid numbers");
    }


    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }
}