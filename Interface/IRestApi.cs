namespace ci_automation_apitester.Interface
{
    public interface IRestApi
    {
        object SetTestParams(string authValue, IEnvironment environment);
        List<RequestAttributeTest> GetRequestAttributeTests();
        List<UrlAttributeTest> GetUrlAttributeTests();
        string GetUrl(string action);
        List<TestObjects.TestStep> GetParameterTests(MessageData messageData, string authValue, IEnvironment environment);
        string GetPostBody();
        string GetPutBody();
        string GetPatchBody();
        string GetDeleteBody();
        string GetWorkingId();
        string GetWorkingBody();
        string GetTestType();
        Dictionary<string, string> GetHeaders(string authValue, IEnvironment environment);
        List<string> GetRequestTypes();
        object GetResults(MessageData messageData, Dictionary<string, string> parameters);
        void CleanUp(MessageData messageData, string currentId);
        List<TestObjects.TestStep> GetFreeFormTests(string authValue, IEnvironment environment);
        string GetRequestBodyByAction(string action);
        Dictionary<string, string> GetHeaderParameter();
    }
}
