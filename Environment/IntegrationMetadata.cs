namespace ci_automation_apitester.Environment
{
    public class IntegrationMetadata : IEnvironment
    {
        public string GetBaseUrl(string environment)
        {
            switch (environment.ToLower())
            {
                case "production":
                case "stage":
                case "staging":
                case "test":
                    return "https://as-test-integrationmetadataservice.azurewebsites.net";
                case "dev":
                    return "https://as-dev-integrationmetadataservice.azurewebsites.net";
                default: //Assume Test
                    return "https://as-test-integrationmetadataservice.azurewebsites.net";
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
        public List<string> GetAuthTypes()
        {
            List<string> authTypes = new List<string>
            { "BearerApiKey" };  //Note:  Integrations endpoints allow both Bearer Token and ApiKey so it will always auth as long as one is valid.

            return authTypes;
        }
        public string GetCurrentAuth()
        {
            return "apikey";
        }
    }
}
