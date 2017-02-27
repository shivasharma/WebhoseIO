using System;
using System.Collections.Generic;
using System.Linq;

namespace Webhoseio
{
    public class WebhoseClient
    {
        private readonly WebhoseOptions options;

        public WebhoseClient(string token)
        {
            options = new WebhoseOptions {Token = token};
        }

        public WebhoseClient(WebhoseOptions options = null)
        {
            this.options = options ?? new WebhoseOptions();
        }


        public WebhoseJsonResponseMessage Query(string endpoint, string q, List<string> organizations,
            List<string> siteTypes, int? days, DateTime? begindate, DateTime? endDate)
        {
            var response =
                Helpers.GetResponseString(GetQueryUri(options, endpoint, q, organizations, siteTypes, days, begindate,
                    endDate));
            return new WebhoseJsonResponseMessage(response);
        }


        protected static Uri GetQueryUri(WebhoseOptions options, string endpoint, string q, List<string> organization,
            List<string> siteTypes, int? days, DateTime? begindate, DateTime? endDate)
        {
            var query = Query(options, q);
            if (organization.Count > 0)
                query = query + string.Join(" ", organization.GetOrganizationName());
            if (siteTypes.Count > 0)
                query = query + string.Join(" ", siteTypes.GetSiteTypes());
            if (days.HasValue)
                query = query + string.Join("", days.Value.GetNumberofDays());

            query = query + string.Join(" ", "adult".ExcludeSiteCategory());
            if (begindate.HasValue && endDate.HasValue)
                query = query + string.Join(" ", begindate.Value.GetDateByRange(endDate.Value));
            var tempURL = new Uri(Constants.BaseUri + endpoint + query).AbsoluteUri;
            return new Uri(Constants.BaseUri + endpoint + query);
        }

        private static string Query(WebhoseOptions options, string q)
        {
            try
            {
                var parameters = new Dictionary<string, string>();

                if (!parameters.ContainsKey("token"))
                    parameters.Add("token", options.Token);
                if (!parameters.ContainsKey("format"))
                    parameters.Add("format", options.Format);
                if (!parameters.ContainsKey("q"))
                    parameters.Add("q", string.IsNullOrEmpty(q) ? "" : q);

                var query = string.Join("&",
                    parameters.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}").ToArray());
                query = string.IsNullOrEmpty(query) ? query : "?" + query;
                return query;
            }
            catch (Exception ex)
            {
            }
            return string.Empty;
        }
    }
}