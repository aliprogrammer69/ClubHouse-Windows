
using ClubHouse.Domain.Services.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ClubHouse.Business.Services.Common {
    public class NewtonSoftSerializer : ISerializer {
        private readonly JsonSerializerSettings _options;
        private readonly JsonSerializerSettings _globalSetting;
        public NewtonSoftSerializer(JsonSerializerSettings option = null) {
            _options = option;
            _globalSetting = new JsonSerializerSettings() {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
            };
        }

        public string Serialize<T>(T model, object options = null) {
            JsonSerializerSettings newtonOption = options as JsonSerializerSettings;
            return JsonConvert.SerializeObject(model, newtonOption ?? _options ?? _globalSetting);
        }

        public T Deserialize<T>(string json, object options = null) {
            if (string.IsNullOrEmpty(json))
                return default;

            JsonSerializerSettings serializerSettings = options as JsonSerializerSettings;
            return JsonConvert.DeserializeObject<T>(json, serializerSettings ?? _options ?? _globalSetting);
        }

        public T SafeDeserialize<T>(string json, object options = null) {
            try {
                return Deserialize<T>(json, options);
            }
            catch {
                return default;
            }
        }
    }
}
