using Microsoft.Net.Http.Headers;
using ShareThings.FunctionalTest.Security;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ShareThings.FunctionalTest.Controller
{
    public sealed class HttpRequestMessageBuilder
    {
        private readonly HttpMethod _method;
        private readonly string _url;
        private readonly (string fieldValue, string cookieValue) _antiForgeryValues;
        private readonly Dictionary<string, string> _content;

        public HttpRequestMessageBuilder(HttpMethod method, string url, (string fieldValue, string cookieValue) antiForgeryValues)
        {
            this._method = method ?? throw new ArgumentNullException(nameof(method));
            this._url = url ?? throw new ArgumentNullException(nameof(url));
            this._antiForgeryValues = antiForgeryValues;
            this._content = new Dictionary<string, string>()
            {
                { AntiForgeryTokenExtractor.AntiForgeryFieldName, antiForgeryValues.fieldValue }
            };
        }

        public HttpRequestMessageBuilder Set(string key, string value)
        {
            this._content.Add(key, value);

            return this;
        }

        public HttpRequestMessage Build()
        {
            HttpRequestMessage request = new HttpRequestMessage(this._method, this._url)
            {
                Content = new FormUrlEncodedContent(this._content)
            };
            request.Headers.Add("Cookie", 
                new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, this._antiForgeryValues.cookieValue).ToString());

            return request;
        }
    }
}
