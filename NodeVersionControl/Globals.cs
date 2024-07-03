using Newtonsoft.Json.Linq;

namespace NodeVersionControl
{

    public static class Globals
    {

        public static string NPM_GLOBALS_STORAGE_FOLDER_NAME = "NPM_GLOBALS";

        
        public static string NODE_DIRECTORY
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(ConfigManager.GetConfig("NODE_DIRECTORY"));
            }
        }
        
        public static string NODE_VERSIONS_DIRECTORY
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(ConfigManager.GetConfig("NODE_VERSIONS_DIRECTORY"));
            }
        }

        public static string WINDOWS_ARCITECTURE
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(ConfigManager.GetConfig("WINDOWS_ARCITECTURE"));
            }
        }

        public static string TEMP_FOLDER
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(ConfigManager.GetConfig("TEMP_FOLDER"));
            }
        }

        public static string NPM_GLOBALS_DIRECTORY
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(ConfigManager.GetConfig("NPM_GLOBALS_DIRECTORY"));
            }
        }
    }

    internal static class ConfigManager
    {
        private static Dictionary<string, string> Configs;

        public static string GetConfig(string key)
        {
            if (Configs == null)
                ParseConfigs();

            if (Configs.ContainsKey(key))
                return Configs[key];
            else
                return null;
        }

        private static void ParseConfigs()
        {
            Configs = new Dictionary<string, string>();
            string configJson = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json"));
            JObject jsonObject = JObject.Parse(configJson);

            foreach(JProperty jp in jsonObject.Properties())
            {
                Configs.Add(jp.Name, jp.Value.ToString());
            }
        }
    }
}
