using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Falcon.Utils.Serialization
{
    public class JsonSerializationService : ISerializationService
    {
        private readonly JsonSerializerSettings _settings;

        public JsonSerializationService()
        {
            _settings = new JsonSerializerSettings
            {
                MaxDepth = 8,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Arrays,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffK",
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new JsonConverter[] { new Newtonsoft.Json.Converters.StringEnumConverter() }
            };
        }

        public T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, _settings);
        }

        public string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data, _settings);
        }
    }
}