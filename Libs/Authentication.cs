namespace ci_automation_apitester.Libs
{
    static class Authentication
    {
        static string token;
        static DateTime tokenExpiry = DateTime.MinValue;

        public static string GetAuthenticationToken(string environment)
        {
            bool missingTokenOrExpiry = (tokenExpiry == DateTime.MinValue || String.IsNullOrEmpty(token));
            bool tokenExpired = (String.IsNullOrEmpty(token) && DateTime.UtcNow > tokenExpiry.AddMinutes(10));

            if (missingTokenOrExpiry || tokenExpired)
            {
                string content = "";
                string url = "";
                switch (environment.ToLower())
                {
                    case "production":
                    case "stage":
                    case "staging":
                    case "test":
                    default: //Assume Dev
                        content = "{\"client_id\": \"MNgt8OaxbCjE2io0ejJROi0fQxzX035t\"," +
                                  "\"client_secret\": \"s0bRYFVGNOfymzt1x-mLq5KvycbVHYXj3horOAcPFobGH_JSMkQtFNnSHpH_sWKu\"," +
                                  "\"audience\": \"https://apimgmt-shareddev-hub.azure-api.net\"," +
                                  "\"grant_type\": \"client_credentials\"}";
                        url = "https://ssb-hub-dev.us.auth0.com/oauth/token";
                        break;
                }
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, url);
                message.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = ApiMessageHandler.SendMessageSync(message);
                JObject jsonResponse = JObject.Parse(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());

                token = "Bearer " + jsonResponse["access_token"];
                tokenExpiry = DateTime.UtcNow;
                return token;
            }
            return token;
        }
        public static string GetApiKey(string environment)
        {
            string apiKey = "";

            switch (environment.ToLower())
            {
                case "production":
                case "stage":
                case "staging":
                case "test":
                default: //Assume Dev
                    apiKey = "d9cd293e-46de-4db2-a8ca-b013d24e00a24";
                    break;
            }
            return apiKey;
        }
        public static List<string> GetAuthCombos()
        {
            List<string> authCombos = new()

            //InvalidApiKey, ExpiredBearerToken & NoAuthValue must be last bc they are failures for all actions and
            //we need a 200 POST to update the CurrentId for subsequent calls.

            { "ValidBearerValidApiKey", "ValidBearerInvalidApiKey", "InvalidBearerInvalidApiKey", "InvalidBearerValidApiKey", "InvalidApiKey", "ExpiredBearerToken", "NoAuthValue" };

            return authCombos;
        }
        public static List<AuthenticationTestData> GetAuthHeadersAndResponseCodes(List<string> authTypes, Dictionary<string, string> headerParam, string environment)
        {
            string invalidBearer = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6ImFfN0MybFdKbVpod2ltako4V0lPdCJ9.eyJodHRwczovL3NzYmh1Yi9yb2xlcyI6WyJzc2ItYWRtaW4iXSwiaXNzIjoiaHR0cHM6Ly9zc2ItaHViLWRldi51cy5hdXRoMC5jb20vIiwic3ViIjoiYXV0aDB8NjE0YTM4Nzk5NDdjOWQwMDY4MDBiMzMxIiwiYXVkIjpbImh0dHBzOi8vYXBpbWdtdC1zaGFyZWRkZXYtaHViLmF6dXJlLWFwaS5uZXQiLCJodHRwczovL3NzYi1odWItZGV2LnVzLmF1dGgwLmNvbS91c2VyaW5mbyJdLCJpYXQiOjE2NDkxODEwNDMsImV4cCI6MTY0OTE4MTY0MywiYXpwIjoibkZTZ3BzZWJUY05RVVh2c2FIdVRsVnhpZXEzWFVkVFciLCJzY29wZSI6Im9wZW5pZCBwcm9maWxlIGVtYWlsIG9mZmxpbmVfYWNjZXNzIiwib3JnX2lkIjoib3JnX1lYQWw2dlFPMklTdjUxTUgiLCJwZXJtaXNzaW9ucyI6WyJkZWxldGUiLCJyZWFkIiwid3JpdGUiXX0.GMTpD-J-LyhLGnMmdDJQuhTRXXyzfRHNAQ3eZknDVB5dgsMGFZ8kNUZ64JNb7yuMybaebtVwzMrsbEoMqDeH0RXaiQxYAPKEFW1txPElxN0KQuvRWP-SsTcsvJwtfIHqxCx-ZrkBdZt0GOieLXAfhZ9-fyeLNpwFEONTFhwpUDmNJN1nb0y49ERujl_ZbsrvvBvaJEnfU2aqJrl-ulTYc5f1GIxe-wOWqeTXswTEirHWik9_zihVzhrftr8SUDyqRLSPD_xkb35V43MuovZltlZPGP1rQSDrJtxjUkPKWmZ5JoFWD5iRl_jGnrhvez-hzHK_MSzO7FRtW4GWjMDoOQ";
            string invalidApiKey = "ui2lp2axTNmsyakw9tvNnw";
            List<string> authCombos = GetAuthCombos();
            int responseCode = 0;
            AuthenticationTestData authenticationTestData = new();
            List<AuthenticationTestData> authTestDataList = new();

            foreach (string authType in authTypes) //Bearer, ApiKey, BearerApiKey
            {
                Dictionary<string, string> headers = new()
                {
                    { "Ocp-Apim-Subscription-Key", "31b292424f3c47aea201d71c6e17c60d"},
                    { "ApiVersion", "1.0"},
                };

                headerParam.ToList().ForEach                        //if the endpoint has extra/unique header params, add them 
                (                                                   //(ie Admin & TenantSaveScheme have OrgId in the headers)
                    pair =>
                    {
                        headers[pair.Key] = pair.Value;
                    }
                );

                foreach (string authCombo in authCombos) //"InvalidApiKey", "ExpiredBearerToken", "NoAuthValue", "ValidBearerInvalidApiKey", "InvalidBearerInvalidApiKey", "InvalidBearerValidApiKey", "ValidBearerValidApiKey" 
                {
                    switch (authCombo)
                    {
                        case "InvalidApiKey":
                            responseCode = 401;
                            headers["ApiKey"] = invalidApiKey;
                            break;
                        case "ExpiredBearerToken":
                            responseCode = 401;
                            headers["Authorization"] = invalidBearer;
                            break;
                        case "NoAuthValue":
                            responseCode = 401;
                            break;
                        case "ValidBearerInvalidApiKey":
                            if (authType.Contains("Bearer")) { responseCode = 200; }
                            else { responseCode = 401; }
                            headers["Authorization"] = Authentication.GetAuthenticationToken(environment);
                            headers["ApiKey"] = invalidApiKey;
                            break;
                        case "InvalidBearerInvalidApiKey":
                            responseCode = 401;
                            headers["Authorization"] = invalidBearer;
                            headers["ApiKey"] = invalidApiKey;
                            break;
                        case "InvalidBearerValidApiKey":
                            if (authType.Contains("ApiKey")) { responseCode = 200; }
                            else { responseCode = 401; }
                            headers["Authorization"] = invalidBearer;
                            headers["ApiKey"] = Authentication.GetApiKey(environment);
                            break;
                        case "ValidBearerValidApiKey":
                            responseCode = 200;
                            headers["Authorization"] = Authentication.GetAuthenticationToken(environment);
                            headers["ApiKey"] = Authentication.GetApiKey(environment);
                            break;
                    }
                    authTestDataList.Add(authenticationTestData.Create(authType, authCombo, responseCode, headers).Copy());
                }
            }

            return authTestDataList;
        }
    }
}
