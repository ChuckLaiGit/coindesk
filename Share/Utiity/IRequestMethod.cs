namespace Share.Utiity
{
    public interface IRequestMethod
    {
        public Task<HttpResponseMessage> SendGetAsync(string requestUri, Dictionary<string, string> queryParams, Dictionary<string, string> headers);
        public void SetTimeout(int timeoutMinute);
        public HttpResponseMessage SendGet(string requestUri, Dictionary<string, string> queryParams, Dictionary<string, string> headers = null);
    }
}
