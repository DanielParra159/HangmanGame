using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Services.Web
{
    // Extracted from: https://stackoverflow.com/questions/14517798/append-values-to-query-string
    public static class UriExtension
    {
        public static Uri ExtendQuery(this Uri uri, object values) {
            return ExtendQuery(uri, values.GetType().GetFields().ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo =>
                {
                    var value = propInfo.GetValue(values);
                    return value?.ToString();
                }
            ));
        }
        
        public static Uri ExtendQuery(this Uri uri, IDictionary<string, string> values) {
            var baseUrl = uri.ToString();
            var queryString = string.Empty;
            if (baseUrl.Contains("?")) {
                var urlSplit = baseUrl.Split('?');
                baseUrl = urlSplit[0];
                queryString = urlSplit.Length > 1 ? urlSplit[1] : string.Empty;
            }

            var queryCollection = HttpUtility.ParseQueryString(queryString);
            foreach (var kvp in values ?? new Dictionary<string, string>()) {
                queryCollection[kvp.Key] = kvp.Value;
            }
            var uriKind = uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative;
            return queryCollection.Count == 0 
                ? new Uri(baseUrl, uriKind) 
                : new Uri($"{baseUrl}?{queryCollection}", uriKind);
        }
    }
}