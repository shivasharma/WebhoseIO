using System;
using Newtonsoft.Json.Linq;

namespace Webhoseio
{
#if !NET35 && !NET40
    using System.Threading.Tasks;

#endif

    public class WebhoseJsonResponseMessage
    {
        internal WebhoseJsonResponseMessage(string content)
        {
            Json = JObject.Parse(content);
        }

        public JObject Json { get; }

        public JToken this[string key] => Json[key];

        public WebhoseJsonResponseMessage GetNext()
        {
            var response = Helpers.GetResponseString(GetNextUri(Json));
            return new WebhoseJsonResponseMessage(response);
        }


        public async Task<WebhoseJsonResponseMessage> GetNextAsync()
        {
            var response = await Helpers.GetResponseStringAsync(GetNextUri(Json));
            return new WebhoseJsonResponseMessage(response);
        }


        protected static Uri GetNextUri(JObject json)
        {
            return new Uri(Constants.BaseUri + json["next"].Value<string>());
        }
    }
}