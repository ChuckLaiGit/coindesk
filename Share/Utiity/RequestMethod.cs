using Microsoft.AspNetCore.WebUtilities;

namespace Share.Utiity
{
    public class RequestMethod : IRequestMethod
    {
        private readonly HttpClient _client = new HttpClient();
        public RequestMethod(Dictionary<string, string>? headers = null, int timeoutMinute = 2)
        {
            SetTimeout(timeoutMinute);
            SetHeaders(headers);
        }
        public HttpResponseMessage SendGet(string requestUri, Dictionary<string, string> queryParams, Dictionary<string, string> headers = null)
        {
            // 設定 header
            SetHeaders(headers);

            var url = (queryParams == null) ? requestUri : QueryHelpers.AddQueryString(requestUri, queryParams);
            // 發出 get 並取得結果
            return _client.GetAsync(url).GetAwaiter().GetResult();
        }

        public async Task<HttpResponseMessage> SendGetAsync(string requestUri, Dictionary<string, string> queryParams, Dictionary<string, string> headers = null)
        {
            // 設定 header
            SetHeaders(headers);


            var url = (queryParams == null) ? requestUri : QueryHelpers.AddQueryString(requestUri, queryParams);
            // 發出 get 並取得結果
            return await _client.GetAsync(url);
        }

        /// <summary>
        /// 設定請求逾時
        /// </summary>
        /// <param name="timeoutMinute"></param>
        public void SetTimeout(int timeoutMinute)
        {
            _client.Timeout = TimeSpan.FromMinutes(timeoutMinute);
        }
        /// <summary>
        /// 設定 headers
        /// </summary>
        /// <param name="headers"></param>
        private void SetHeaders(Dictionary<string, string>? headers)
        {
            // 設定 header
            if (headers != null)
            {
                _client.DefaultRequestHeaders.Clear();
                foreach (var header in headers)
                {
                    _client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }
    }
}
