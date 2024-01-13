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

        // Split the string by ':' to separate days, hours, minutes, and seconds
        var timeParts = timeString.Split(':');

        if (timeParts.Length == 3)
        {
            // Format: "hh:mm:ss"
            if (int.TryParse(timeParts[0], out var hours) &&
                int.TryParse(timeParts[1], out var minutes) &&
                int.TryParse(timeParts[2], out var seconds))
            {
                return new TimeSpan(0, hours, minutes, seconds);
            }
        }
        else if (timeParts.Length == 4)
        {
            // Format: "ddd:hh:mm:ss"
            if (int.TryParse(timeParts[0], out var days) &&
                int.TryParse(timeParts[1], out var hours) &&
                int.TryParse(timeParts[2], out var minutes) &&
                int.TryParse(timeParts[3], out var seconds))
            {
                // Convert days to total hours and construct a TimeSpan object
                return new TimeSpan(days, hours, minutes, seconds);
            }
        }

        throw new FormatException("The TimeSpan string is not in a valid format");
    }
    
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }
}