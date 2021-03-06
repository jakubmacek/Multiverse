namespace NHibernate.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    public class JsonColumnTypeWorker
    {
        public static readonly JsonSerializerSettings Settings;

        static JsonColumnTypeWorker()
        {
            Settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Converters = new List<Newtonsoft.Json.JsonConverter> {new StringEnumConverter()},
                    ObjectCreationHandling = ObjectCreationHandling.Auto,
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                    TypeNameHandling = TypeNameHandling.Auto
                };
        }

        public static JsonSerializerSettings Configure(params Action<JsonSerializerSettings>[] actions)
        {
            actions.ToList().ForEach(x => x(Settings));
            return Settings;
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, Settings);
        }

        public static T? Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        public static void PopulateObject<T>(string json, T obj)
        {
            if (json != null && obj != null)
                JsonConvert.PopulateObject(json, obj, Settings);
        }
    }
}