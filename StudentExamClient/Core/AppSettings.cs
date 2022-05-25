using System.IO;
using System.Text.Json;

namespace StudentExamClient.Core
{
    public static class AppSettings
    {
        private static string path = "config.json";
        
        public static int CurrentUserId { get; set; }

        public static AppConfig GetConfig()
        {
            if (!File.Exists(path))
                SetConfig(new AppConfig() { ApiConnectionString = "https://localhost:7281/api/" });

            var json = File.ReadAllText(path);
            var config = (AppConfig)JsonSerializer.Deserialize(json, typeof(AppConfig))!;
            return config;
        }

        private static void SetConfig(AppConfig config)
        {
            var json = JsonSerializer.Serialize(config, typeof(AppConfig));
            File.WriteAllText(path, json);
        }

        public static void SetApiConnectionString(string connectionString)
        {
            var config = GetConfig();
            config.ApiConnectionString = connectionString;
            SetConfig(config);
            Api.SetServer(connectionString);
        }
    }

    public class AppConfig
    {
        public string ApiConnectionString { get; set; }
    }
}
