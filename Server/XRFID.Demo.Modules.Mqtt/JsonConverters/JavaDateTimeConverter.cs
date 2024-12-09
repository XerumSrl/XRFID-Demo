﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace XRFID.Demo.Modules.Mqtt.JsonConverters;
/// <summary>
/// converter for System.Text.Json for java generated JSONs
/// </summary>
/// <remarks>
/// <para>this is needed because C# expects a date formatted as follows yyyy-MM-ddThh:mm:ss.ZZZZZZZ+hhmm, but java provides it formatted as yyyy-MM-ddThh:mm:ss.ZZZ+hhmm</para>
/// <para> note the precision difference in the faractions of seconds</para>
/// </remarks>
public class JavaDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.Parse(reader.GetString() ?? DateTime.Now.ToString());
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
