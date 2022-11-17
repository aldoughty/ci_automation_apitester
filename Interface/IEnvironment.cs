
namespace ci_automation_apitester.Interface
{
    public interface IEnvironment
    {
        string GetBaseUrl(string environment);
        string GetAuthenticationToken(string environment);
        string GetApiKey(string environment);
        List<string> GetAuthTypes();
        string GetCurrentAuth();
        public string GetAuthentication(string environment);
    }
}
