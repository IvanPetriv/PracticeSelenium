using Newtonsoft.Json;

namespace Locators.Tests {
    public record LoginInfo(string Login, string Password);

    public class Credentials
    {
        public const string CREDENTIALS_DIR = @"..\..\..\TestData\TestCredentials.json";

        public List<LoginInfo> Google { get; set; }
        public List<LoginInfo> Microsoft { get; set; }

        public static Credentials DeserializeJson(string filename)
        {
            string jsonData = File.ReadAllText(filename);

            Credentials credentials = JsonConvert.DeserializeObject<Credentials>(jsonData);
            return credentials;
        }
    }
}
