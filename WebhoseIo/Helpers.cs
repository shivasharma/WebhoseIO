using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Webhoseio
{
    public static class Helpers
    {
        private static void EnsureSuccessStatusCode(HttpWebResponse response)
        {
            if (response.StatusCode >= (HttpStatusCode) 300)
                throw new Exception();
        }

        public static string GetResponseString(Uri requestUri)
        {
            var request = (HttpWebRequest) WebRequest.Create(requestUri);
            var response = (HttpWebResponse) request.GetResponse();
            EnsureSuccessStatusCode(response);
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }

        public static async Task<string> GetResponseStringAsync(Uri requestUri)
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(requestUri);
                var response = (HttpWebResponse) request.GetResponse();
                EnsureSuccessStatusCode(response);
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    return await sr.ReadToEndAsync();
                }
            }
            catch (Exception e)
            {
            }
            return string.Empty;
        }

        public static string GetOrganizationName(this List<string> organizations)
        {
            var subscribedOrganization = new List<string>();
            foreach (var organization in organizations)
                subscribedOrganization.Add(Uri.EscapeUriString(organization));
            return string.Join(",", subscribedOrganization.Select(x => $"(organization:\"{x}\"" + ")").ToList());
        }

        public static string GetSiteTypes(this List<string> siteTypes)
        {
            var selectedSitetypes = new List<string>();
            foreach (var organization in siteTypes)
                selectedSitetypes.Add(Uri.EscapeDataString(organization));
            return string.Join(",", selectedSitetypes.Select(x => $"(site_type:\"{x}\"" + ")").ToList());
        }

        public static string GetNumberofDays(this int numDays)
        {
            var dateParameter = new Dictionary<string, double>();
            var numberofDays = (long) GetUnixTimeStamp(DateTime.UtcNow.AddDays(-numDays));
            dateParameter.Add("ts", numberofDays);
            var query = string.Concat("&",
                string.Join(" ", dateParameter.Select(kv => $"{kv.Key}={kv.Value}").ToArray()));
            return query;
        }

        public static string ExcludeSiteCategory(this string siteCategory)
        {
            var sitecategory = new Dictionary<string, string> {{"exclude_site_category", siteCategory}};
            var query = string.Concat("&", string.Join("", sitecategory.Select(kv => $"{kv.Key}={kv.Value}").ToArray()));
            return query;
        }

        public static double GetUnixTimeStamp(DateTime utcDate)
        {
            return (utcDate - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }


        public static string GetDateByRange(this DateTime startDate, DateTime endDate)
        {
            var sDate = GetUnixTimeStamp(startDate);
            var eDate = GetUnixTimeStamp(endDate);
            var begindate = string.Concat("(published:>") + sDate + ")";
            var operation = string.Concat("-");
            var enddate = string.Concat("(published:>") + eDate + ")";
            var rangeDate = begindate + operation + enddate;
            return string.Concat(" ", rangeDate);
        }
    }
}