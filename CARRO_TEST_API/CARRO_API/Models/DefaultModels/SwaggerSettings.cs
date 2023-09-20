namespace CARRO_API.Models
{
    public class SwaggerSettings
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public string RoutePrefix { get; set; }
        public string Endpoint { get; set; }
        public SwaggerContact Contact { get; set; }
    }

    public class SwaggerContact
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
    }
}
