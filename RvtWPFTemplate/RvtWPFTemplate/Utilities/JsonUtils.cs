using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace RvtWPFTemplate.Utilities
{
    public class JsonUtils
    {
        public static T Load<T>(string configPath)
      where T : new()
        {
            if (!File.Exists(configPath))
                return CreateDefaultConfigurationFile<T>(configPath);

            string content = File.ReadAllText(configPath, Encoding.UTF8);
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static void Save<T>(string configPath, T configuration)
            where T : new()
        {
            string content = JsonConvert.SerializeObject(configuration);

            File.WriteAllText(configPath, content, Encoding.UTF8);
        }

        private static T CreateDefaultConfigurationFile<T>(string configPath)
            where T : new()
        {
            var config = new T();
            var configData = JsonConvert.SerializeObject(config);

            File.WriteAllText(configPath, configData, Encoding.UTF8);

            return config;
        }
    }
}