namespace ci_automation_apitester.Objects
{
    public class ReponseMessage
    {
        public string ApiCall;
        public HttpStatusCode HttpStatus;
        public string HttpStatusMessage;
        public string ApiStatus;
        public string ApiStatusMessage;
        public long ResponseTime;
        public string RepsonseMessage;
        public void PopulateFromHttpResponse(string url, HttpResponseMessage response, long timer)
        {
            ApiCall = url;
            RepsonseMessage = response.Content.ReadAsStringAsync().Result;
            HttpStatus = response.StatusCode;
            HttpStatusMessage = response.ReasonPhrase;
            ResponseTime = timer;
        }
    }
}
