using System;

using System.Globalization;
using Newtonsoft.Json;
using SixLabors.ImageSharp;

namespace WeChat.Official.Services.TemplateMessage.Json
{
    /// <summary>
    /// 针对于 <see cref="Color"/> 类型定义的 Json 转换器，用于将颜色实例转换为 16 进制字符串。
    /// 并且实现了从十六进制字符串，转换为 <see cref="Color"/> 实例的功能。
    /// </summary>
    public class ColorJsonConverter : JsonConverter<Color>
    {
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is string valueStr)
            {
                return Color.ParseHex(valueStr);
            }

            return Color.Black;
        }
    }
}