using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MessagingApi.Service.Helpers
{
    public interface ISerializerHelper
    {
        byte[] Serialize<T>(T value);
        byte[] Serialize<T>(T value, JsonSerializerOptions options);
        string SerializeObject(object obj);
        string SerializeObject(object obj, JsonSerializerOptions options);
        T Deserialize<T>(byte[] bytes);
        T DeserializeObject<T>(string value);
        T DeepCopy<T>(T other);
    }

    public class JsonSerializerHelper : ISerializerHelper
    {
        public byte[] Serialize<T>(T value)
        {
            var str = JsonSerializer.Serialize(value);
            return Encoding.UTF8.GetBytes(str);
        }

        public byte[] Serialize<T>(T value, JsonSerializerOptions options)
        {
            var str = JsonSerializer.Serialize(value, options);
            return Encoding.UTF8.GetBytes(str);
        }

        public string SerializeObject(object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public string SerializeObject(object obj, JsonSerializerOptions options)
        {
            return JsonSerializer.Serialize(obj, options);
        }

        public T Deserialize<T>(byte[] bytes)
        {
            var str = Encoding.UTF8.GetString(bytes);
            return DeserializeObject<T>(str);
        }

        public T DeserializeObject<T>(string value)
        {
            return JsonSerializer.Deserialize<T>(value, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public T DeepCopy<T>(T other)
        {
            if (other == null)
            {
                return default;
            }

            var bytes = Serialize(other);
            return Deserialize<T>(bytes);
        }
    }
}
