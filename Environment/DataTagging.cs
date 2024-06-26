﻿namespace ci_automation_apitester.Environment
{
    public class DataTagging : IEnvironment
    {
        public string GetBaseUrl(string environment)
        {
            switch (environment.ToLower())
            {
                case "production":
                    return "https://api-shareddev-mgmt.azure-api.net";
                case "stage":
                case "staging":
                    return "https://api-shareddev-mgmt.azure-api.net";
                case "test":
                    return "https://api-shareddev-mgmt.azure-api.net";
                default: //Assume Dev
                    return "https://api-shareddev-mgmt.azure-api.net";
            }
        }
        public string GetAuthenticationToken(string environment)
        {
            return Authentication.GetAuthenticationToken(environment);
        }
        public string GetApiKey(string environment)
        {
            return Authentication.GetApiKey(environment);
        }
        public List<string> GetAuthTypes()
        {
            List<string> authTypes = new List<string>
            { "Bearer" };

            return authTypes;
        }
        public string GetCurrentAuth()
        {
            return "bearer";
        }
        public string GetAuthentication(string environment)
        {
            string authType = GetCurrentAuth();
            string authValue = "";

            switch (authType.ToLower())
            {
                case "apikey":
                    authValue = Authentication.GetApiKey(environment);
                    break;
                case "bearer":
                    authValue = Authentication.GetAuthenticationToken(environment);
                    break;
            }
            return authValue;
        }
    }
}
