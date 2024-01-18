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
        if (reader.TokenType == JsonToken.String)
        {
            var timeString = (string)reader.Value;

            // Check if the string contains days
            if (timeString.Contains(":") || timeString.Contains("."))
            {
                char[] separators = { ':', '.' };
                var timeParts = timeString.Split(separators);

                if (timeParts.Length == 3 || timeParts.Length == 4)
                {
                    // Format: "hh:mm:ss" or "hh:mm:ss.fffffff"
                    if (int.TryParse(timeParts[0], out var hours) &&
                        int.TryParse(timeParts[1], out var minutes) &&
                        double.TryParse(timeParts[2], out var seconds))
                    {
                        if (timeParts.Length == 4)
                        {
                            seconds += double.Parse("0." + timeParts[3]);
                        }

                        return new TimeSpan(0, hours, minutes, (int)Math.Floor(seconds), (int)((seconds - Math.Floor(seconds)) * 10000000));
                    }
                }
                else if (timeParts.Length >= 4)
                {
                    // Format: "ddd:d:hh:mm:ss"
                    if (int.TryParse(timeParts[0], out var days) &&
                        int.TryParse(timeParts[2], out var hours) &&
                        int.TryParse(timeParts[3], out var minutes) &&
                        double.TryParse(timeParts[4], out var seconds))
                    {
                        if (timeParts.Length == 6)
                        {
                            seconds += double.Parse("0." + timeParts[5]);
                        }

                        return new TimeSpan(days, hours, minutes, (int)Math.Floor(seconds), (int)((seconds - Math.Floor(seconds)) * 10000000));
                    }
                }
            }
        }

        throw new FormatException("The TimeSpan string is not in a valid format");
    }



    
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }
}