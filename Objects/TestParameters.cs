namespace ci_automation_apitester.Objects
{
    public class TestParameters
    {
        public Dictionary<string, object> CreateTestParameters(MessageData messageData)
        {
            Dictionary<string, object> testParameters = new();
            {
                testParameters.Add("ApiDto", null);
                testParameters.Add("ApiEnvironment", null);
                testParameters.Add("Request", null);
                testParameters.Add("MessageData", messageData);
                testParameters.Add("TestResultDetail", new ApiTestResults.TestDetails());
            }
            return testParameters;
        }
    }
}
