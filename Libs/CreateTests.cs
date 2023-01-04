namespace ci_automation_apitester.Libs
{
    public class CreateTests
    {
        private MessageData MessageData;
        private IRestApi ApiDto;
        private IEnvironment Environment;
        private string BaseUrl;
        private string baseTestName;
        private string EndpointUnderTest;
        private ActiveTests ActiveTest;
        private string AuthValue;
        public CreateTests(Dictionary<string, object> testParameters) 
        {
            MessageData = testParameters.ContainsKey("MessageData") ? (MessageData)testParameters["MessageData"] : new MessageData();
            if (!testParameters.ContainsKey("ApiDto") || !testParameters.ContainsKey("ApiEnvironment")) throw new AutomationFrameworkException();
            ApiDto = (IRestApi)testParameters["ApiDto"];
            ActiveTest = new ActiveTests();
            Environment = (IEnvironment)testParameters["ApiEnvironment"];
            BaseUrl = $"{Environment.GetBaseUrl(MessageData.Environment)}";
            baseTestName = ApiDto.GetType().Name;
            AuthValue = Environment.GetAuthentication(MessageData.Environment);
            EndpointUnderTest = Environment.GetType().Name + "." + ApiDto.GetType().Name;
        }
        public List<TestSuite> GetTests()
        {
            TestSuite testSuite = new();
            List<TestSuite> testSuiteList = new();

            if (!ActiveTest.GetEndpointIsActive(EndpointUnderTest)) return testSuiteList;

            testSuiteList.Add(testSuite.Create(GetGetTests(), EndpointUnderTest).Copy());
            testSuiteList.Add(testSuite.Create(GetWorkFlowTests(), EndpointUnderTest).Copy());
            testSuiteList.Add(testSuite.Create(GetAuthorizationTests(), EndpointUnderTest).Copy());
            testSuiteList.Add(testSuite.Create(GetTestAttributeTests(), EndpointUnderTest).Copy());

            return testSuiteList;
        }
        public List<TestCase> GetGetTests()
        {
            List<TestCase> testCaseList = new();
            List<TestStep> parameterTests = ApiDto.GetParameterTests(MessageData, AuthValue, Environment);
            parameterTests.ForEach(x => x.Url = BaseUrl + x.Url);
            var testStepGroups = parameterTests.GroupBy(x => x.TestStepName);
            
            foreach (var testStepGroup in testStepGroups)
            {
                string testName = testStepGroup.Key;
                TestCase testCase = new();
                List<TestStep> testStepList = testStepGroup.ToList();
                testCase.Create(testName, testStepList);
                testCaseList.Add(testCase);
            }
                    
            return testCaseList;
        }
        private List<TestCase> GetWorkFlowTests()
        {
            TestStep testStep = new();
            List<TestStep> testStepList = new();
            TestCase testCase = new();
            List<TestCase> testCaseList = new();

            foreach (var action in ApiDto.GetRequestTypes())
            {
                var requestBody = ApiDto.GetRequestBodyByAction(action);

                if (action == "DELETE")
                { testStepList.Add(testStep.Create("CompareResponseToStringAndResponseCodeRange", action + "_" + baseTestName, ApiDto.GetHeaders(AuthValue, Environment), BaseUrl + ApiDto.GetUrl(action), action, requestBody, 200, "").Copy().PerformCleanUp().SwapGuid()); }
                else if (action == "PUT")
                { testStepList.Add(testStep.Create("CompareResponseToStringAndResponseCodeRange", action + "_" + baseTestName, ApiDto.GetHeaders(AuthValue, Environment), BaseUrl + ApiDto.GetUrl(action), action, requestBody, 200, "").Copy().SwapGuid()); }
                else if (action == "POST") 
                { testStepList.Add(testStep.Create("CompareResponseToStringAndResponseCodeRange", action + "_" + baseTestName, ApiDto.GetHeaders(AuthValue, Environment), BaseUrl + ApiDto.GetUrl(action), action, requestBody, 200, "").Copy().GetGuid()); }
                else
                { testStepList.Add(testStep.Create("CompareResponseToStringAndResponseCodeRange", action + "_" + baseTestName, ApiDto.GetHeaders(AuthValue, Environment), BaseUrl + ApiDto.GetUrl(action), action, requestBody, 200, "").Copy()); }

            }
            testCaseList.Add(testCase.Create("WorkflowTests", testStepList).Copy());

            return testCaseList;
        }
        private List<TestCase> GetAuthorizationTests()
        {
            TestStep testStep = new();
            List<TestStep> testStepList = new();
            TestCase testCase = new();
            List<TestCase> testCaseList = new();
            List<string> authTypes = Environment.GetAuthTypes();
            Dictionary<string, string> headerParam = ApiDto.GetHeaderParameter();
            List<AuthenticationTestData> authTestData = GetAuthHeadersAndResponseCodes(authTypes, headerParam, MessageData.Environment);

            foreach (var authTest in authTestData)
            {
                foreach (var action in ApiDto.GetRequestTypes())
                {
                    var requestBody = ApiDto.GetRequestBodyByAction(action);

                    if (action == "DELETE")
                    { testStepList.Add(testStep.Create("ValidateResponseCode", action + "_" + baseTestName + "_" + authTest.AuthType + "_" + authTest.AuthCombo, authTest.Headers, BaseUrl + ApiDto.GetUrl(action), action, requestBody, authTest.ResponseCode, "").Copy().PerformCleanUp().SwapGuid()); }
                    else if (action == "PUT")
                    { testStepList.Add(testStep.Create("ValidateResponseCode", action + "_" + baseTestName + "_" + authTest.AuthType + "_" + authTest.AuthCombo, authTest.Headers, BaseUrl + ApiDto.GetUrl(action), action, requestBody, authTest.ResponseCode, "").Copy().SwapGuid()); }
                    else if (action == "POST")
                    { testStepList.Add(testStep.Create("ValidateResponseCode", action + "_" + baseTestName + "_" + authTest.AuthType + "_" + authTest.AuthCombo, authTest.Headers, BaseUrl + ApiDto.GetUrl(action), action, requestBody, authTest.ResponseCode, "").Copy().GetGuid()); }
                    else
                    { testStepList.Add(testStep.Create("ValidateResponseCode", action + "_" + baseTestName + "_" + authTest.AuthType + "_" + authTest.AuthCombo, authTest.Headers, BaseUrl + ApiDto.GetUrl(action), action, requestBody, authTest.ResponseCode, "").Copy()); }
                }
                //thinking about having this as a "CleanupStep" instead of the if stmt above
                //testStepList.Add(testStep.Create("ValidateResponseCode", "CleanupStep_" + baseTestName + "_" + authTest.AuthType + "_" + authTest.AuthCombo, authTest.Headers, BaseUrl + ApiDto.GetUrl("GET"), "GET", "", authTest.ResponseCode, "").Copy().PerformCleanUp());
            }
            testCaseList.Add(testCase.Create("AuthTests", testStepList).Copy());
            return testCaseList;
        }
        private List<TestCase> GetBadPayloadTests()
        {
            TestCase testCase = new();
            List<TestCase> testCaseList = new();
            TestStep testStep = new();
            List<TestStep> testStepList = new();
            List<string> actions = new()
            { "POST", "PUT", "PATCH" };

            foreach (string action in actions)
            {
                //No Payload
                testStepList.Add(testStep.Create("CompareResponseToString", baseTestName + "_NoPayload", ApiDto.GetHeaders(AuthValue, Environment), BaseUrl + ApiDto.GetUrl(action), action, "", 401, "").Copy());

                //Empty Payload
                testStepList.Add(testStep.Create("CompareResponseToString", baseTestName + "_EmptyPayload", ApiDto.GetHeaders(AuthValue, Environment), BaseUrl + ApiDto.GetUrl(action), action, "{}", 401, "").Copy());

                //Payload with invalid object

                //NonJson Payload
                testStepList.Add(testStep.Create("CompareResponseToString", baseTestName + "_NonJsonPayload", ApiDto.GetHeaders(AuthValue, Environment), BaseUrl + ApiDto.GetUrl(action), action, "bad stuff", 401, "").Copy());
            }
            
            testCaseList.Add(testCase.Create("BadPayloadTests", testStepList));

            return testCaseList;
        }
        public List<TestCase> GetTestAttributeTests()
        {
            TestStep testStep = new();
            List<TestStep> testStepList = new();
            TestCase testCase = new();
            List<TestCase> testCaseList = new();
            List<RequestAttributeTest> requestAttributeTestList = ApiDto.GetRequestAttributeTests();
            List<UrlAttributeTest> urlAttributeTestList = ApiDto.GetUrlAttributeTests();

            foreach (var test in requestAttributeTestList)
            {
                //Validates PUT/POST error scenarios and that a record IS NOT created as a result
                testStepList.Add(testStep.Create("ValidateResponseCode", "GET_" + baseTestName + "_RequestAttributeTest_" + test.TestName + "_Get Initial Object Count", ApiDto.GetHeaders(AuthValue, Environment), BaseUrl + ApiDto.GetUrl("GET"), "GET", test.Request, 200, "").Copy().GetExpectedObjectCount(0));
                testStepList.Add(testStep.Create("ValidateResponseContainsString", test.Action + "_" + baseTestName + "_RequestAttributeTest_" + test.TestName + "_" + test.ResponseCode, ApiDto.GetHeaders(AuthValue, Environment), BaseUrl + ApiDto.GetUrl(test.Action), test.Action, test.Request, test.ResponseCode, test.Message).Copy());
                testStepList.Add(testStep.Create("ValidateResponseObjectCounts", "GET_" + baseTestName + "_RequestAttributeTest_" + test.TestName + "_Get Final Object Count", ApiDto.GetHeaders(AuthValue, Environment), BaseUrl + ApiDto.GetUrl("GET"), "GET", test.Request, 200, "").Copy().ValidateActualObjectCount());
            }

            foreach (var test in urlAttributeTestList)
            {
                testStepList.Add(testStep.Create("ValidateResponseContainsString", test.Action + "_" + baseTestName + "_UrlAttributeTest_" + test.TestName + "_" + test.ResponseCode, ApiDto.GetHeaders(AuthValue, Environment), BaseUrl + ApiDto.GetUrl("AttributeUrlTest") + test.Url, test.Action, "", test.ResponseCode, "").Copy());
            }

            testCaseList.Add(testCase.Create("AttributeTests", testStepList).Copy());

            return testCaseList;
        }
    }
}
