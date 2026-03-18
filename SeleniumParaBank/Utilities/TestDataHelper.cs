using Newtonsoft.Json.Linq;

namespace SeleniumParaBank.Utilities
{
    /// <summary>
    /// Loads test data from TestData/users.json.
    /// </summary>
    public static class TestDataHelper
    {
        private static JObject? _data;

        private static JObject Data
        {
            get
            {
                if (_data == null)
                {
                    // Walk up from bin/Debug/net8.0 to project root
                    var dir  = AppDomain.CurrentDomain.BaseDirectory;
                    var path = Path.Combine(dir, "..", "..", "..", "TestData", "users.json");
                    path     = Path.GetFullPath(path);
                    _data    = JObject.Parse(File.ReadAllText(path));
                }
                return _data;
            }
        }

        public static string BaseUrl       => Data["baseUrl"]!.ToString();
        public static string ExistingUser  => Data["existingUser"]!["username"]!.ToString();
        public static string ExistingPass  => Data["existingUser"]!["password"]!.ToString();

        public static JToken GetUser(string id)
            => Data["users"]!.First(u => u["id"]!.ToString() == id)!;

        public static string Get(string path)
        {
            var parts = path.Split('.');
            JToken? token = Data;
            foreach (var p in parts)
                token = token?[p];
            return token?.ToString() ?? string.Empty;
        }
    }
}
