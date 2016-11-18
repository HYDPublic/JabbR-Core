﻿using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace JabbR_Core.Infrastructure
{
    public static class Http
    {
        private const string _userAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0; MAAU)";

        public static Task<dynamic> GetJsonAsync(string url)
        {
            var task = GetAsync(url, webRequest =>
            {
                webRequest.Accept = "application/json";
            });

            return task.Then(response =>
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return JsonConvert.DeserializeObject(reader.ReadToEnd());
                }
            });
        }

        public static async Task<HttpWebResponse> GetAsync(Uri uri, Action<HttpWebRequest> init = null)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(uri);
            var webHeaders = new WebHeaderCollection();
            webHeaders[HttpRequestHeader.UserAgent]= _userAgent;
            request.Headers = webHeaders;
            if (init != null)
            {
                init(request);
            }

            return (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(continueOnCapturedContext: false);
        }

        public static Task<HttpWebResponse> GetAsync(string url, Action<HttpWebRequest> init = null)
        {
            return GetAsync(new Uri(url), init);
        }

        public static Task<TResult> GetJsonAsync<TResult>(string url)
        {
            var task = GetAsync(url, webRequest =>
            {
                webRequest.Accept = "application/json";
            });

            return task.Then(response =>
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return JsonConvert.DeserializeObject<TResult>(reader.ReadToEnd());
                }
            });
        }
    }
}