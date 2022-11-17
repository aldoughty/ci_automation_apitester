namespace ci_automation_apitester.Objects
{
    public class TestObjects
    {

        public class TestStep         
        {
            public string TestType { get; set; }
            public string TestCaseName { get; set; }
            public string TestStepName { get; set; }
            public string Url { get; set; }
            public string RequestType { get; set; }
            public string ContentType { get; set; }
            public string Body { get; set; }
            public Dictionary<string, string> Headers { get; set; }
            public int ExpectedResponseCode { get; set; }
            public string ExpectedResponseBody { get; set; }
            public List<string> ExpectedReturnBodyParts { get; set; }
            public bool PerformCleanup { get; set; }
            public bool SwapGUID { get; set; }
            public bool GetGUID { get; set; }

            public TestStep Create(string testType, string testStepName, Dictionary<string, string> headers, string url, string requestType, string body, int expectedResponseCode, string expectedResponseBody)
            {
                TestType = testType;
                ContentType = "application/json";
                TestStepName = testStepName;
                Headers = (Dictionary<string, string>)headers;
                Url = url;
                RequestType = requestType;
                Body = body;
                ExpectedResponseCode = expectedResponseCode;
                ExpectedResponseBody = expectedResponseBody;
                return this;
            }
            public TestStep Copy()
            {
                string data = JsonConvert.SerializeObject(this);
                TestStep copy = JsonConvert.DeserializeObject<TestStep>(data);
                return copy;
            }
            public TestStep PerformCleanUp()
            {
                PerformCleanup = true;
                return this;
            }
            public TestStep SwapGuid()
            {
                SwapGUID = true;
                return this;
            }
            public TestStep GetGuid()
            {
                GetGUID = true;
                return this;
            }
        }
        public class TestCase
        {
            public string TestCaseName { get; set; }
            public List<TestStep> TestStep { get; set; }

            public TestCase Create(string testCaseName, List<TestStep> testStep)
            {
                TestCaseName = testCaseName;
                TestStep = testStep;

                return this;
            }
            public TestCase Copy()
            {
                string data = JsonConvert.SerializeObject(this);
                TestCase copy = JsonConvert.DeserializeObject<TestCase>(data);
                return copy;
            }
        }
        public class TestSuite
        {
            public List<TestCase> TestCaseList { get; set; }
            public string EndpointUnderTest { get; set; }

            public TestSuite Create(List<TestCase> testCaseList, string endpointUnderTest)
            {
                TestCaseList = testCaseList;
                EndpointUnderTest = endpointUnderTest;

                return this;
            }
            public TestSuite Copy()
            {
                string data = JsonConvert.SerializeObject(this);
                TestSuite copy = JsonConvert.DeserializeObject<TestSuite>(data);
                return copy;
            }
        }
        public class TestSuiteList
        {
            public List<TestSuite> TestSuite { get; set; }

            public TestSuiteList Create(List<TestSuite> testSuite)
            {
                TestSuite = testSuite;

                return this;
            }
            public TestSuiteList Copy()
            {
                string data = JsonConvert.SerializeObject(this);
                TestSuiteList copy = JsonConvert.DeserializeObject<TestSuiteList>(data);
                return copy;
            }
        }
        public class TestPlan
        {
            public List<TestSuiteList> TestSuiteList { get; set; }

            public TestPlan Create(List<TestSuiteList> testSuiteList)
            {
                TestSuiteList = testSuiteList;

                return this;
            }
        }
    }
}