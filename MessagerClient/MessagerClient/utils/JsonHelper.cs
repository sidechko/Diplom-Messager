using MessagerClient.models;
using MessagerClient.utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace MessagerClient.Utils
{
    public class JsonHelper
    {
        private readonly JsonSerializerOptions DeserializeOption = new JsonSerializerOptions() {  };

        public static T Deserialize<T>(string json) where T : IModelBase<T> 
        {
            T? result = JsonSerializer.Deserialize<T>(json);
            if (result is null || result.IsNull())
                throw new MessagerDeserializeException("Unable to deserialize json: "+json);
            return result;
        }

        public static ICollection<T> DeserializeList<T>(string json) where T : IModelBase<T>
        {
            ICollection <T> ? result = JsonSerializer.Deserialize<ICollection<T>>(json);
            if (result is null || result.Count == 0)
                throw new MessagerDeserializeException("Unable to deserialize json: "+json);
            return result;
        }

        public static string Serialize<T>(T value)
        {
            return JsonSerializer.Serialize(value);
        }
    }
}
