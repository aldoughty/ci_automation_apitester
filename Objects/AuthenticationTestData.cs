namespace ci_automation_apitester.Objects
{
    public class AuthenticationTestData
    {
        public string AuthType { get; set; }
        public int ResponseCode { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string AuthCombo { get; set; }

        public AuthenticationTestData Create(string authType, string authCombo, int responseCode, Dictionary<string, string> headers)
        {
            AuthType = authType;
            AuthCombo = authCombo;
            ResponseCode = responseCode;
            Headers = headers;
            return this;
        }
        public AuthenticationTestData Copy()
        {
            string data = JsonConvert.SerializeObject(this);
            AuthenticationTestData copy = JsonConvert.DeserializeObject<AuthenticationTestData>(data);
            return copy;
        }
    }
}
