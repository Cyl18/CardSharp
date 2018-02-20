using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CardSharp.I18N
{
    public class Localization
    {
        public static List<Language> Languages { get; } = GetNames()
            .Select(name => new Language(name)).ToList();

        public static dynamic Normal { get; } = Languages.First(lang => lang.Name == "normal").Content;

        private static IEnumerable<string> GetNames()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceNames();
        }
    }

    public class Language
    {
        public string Name;
        public string CultureName;
        public JObject Content;

        public Language(string filePath)
        {
            Content = JObject.Parse(EmbedResourceReader.Read(filePath));
            SetName(filePath);
        }

        private void SetName(string filePath)
        {
            var count = typeof(Language).ToString().Split('.').Length;
            var sp = filePath.Split('.').Skip(count).ToArray();
            CultureName = sp[0];
            Name = sp[1];
        }
    }

}
