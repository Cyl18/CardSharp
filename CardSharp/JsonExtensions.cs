using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CardSharp
{

    public static class JsonExtensions
    {
        private static readonly SerializeSettings SerializeSettings = new SerializeSettings();

        public static string ToJsonString(this object source) => JsonConvert.SerializeObject(source, SerializeSettings);
        public static T JsonDeserialize<T>(this string source) => JsonConvert.DeserializeObject<T>(source, SerializeSettings);

        public static string ToJsonString(this object source, JsonSerializerSettings settings) => JsonConvert.SerializeObject(source, settings);
        public static T JsonDeserialize<T>(this string source, JsonSerializerSettings settings) => JsonConvert.DeserializeObject<T>(source, settings);

    }

    public class SerializeSettings : JsonSerializerSettings
    {
        public SerializeSettings()
        {
            NullValueHandling = NullValueHandling.Include;
            Formatting = Formatting.Indented;
            MissingMemberHandling = MissingMemberHandling.Ignore;
        }
    }

}
