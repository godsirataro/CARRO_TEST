namespace CARRO_API.Models
{
    public class AppSettings
    {
        public string Environment { get; set; }
        public string[] AllowedHosts { get; set; }
        public string WebUrl { get; set; }
        public string ApiUrl { get; set; }
    }
}

