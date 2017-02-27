using System.Configuration;

namespace Webhoseio
{
    public class WebhoseOptions
    {
        public string Token { get; set; } = ConfigurationManager.AppSettings["webhoseio:token"];
        public string Format => "json";
    }
}