namespace ci_automation_apitester.Objects
{
    public class ApiTestResults : TestResults
    {
        public ApiTestResults(StandardTestParameters stp) : base(stp)
        {
        }
        public class RequestData : TestDetails
        {
            public string Url { get; set; }
            public string ContentType { get; set; }
            public string RequestType { get; set; }
            public string Body { get; set; }
            public int ExpectedRepsonseCode { get; set; }
            public string ExpectedRepsonseMessage { get; set; }

            public RequestData()
            { 

            }
            public RequestData(TestCase testCase, TestStep testStep)
            {
                Url = testStep.Url;
                ContentType = testStep.ContentType;
                RequestType = testStep.RequestType;
                TestCaseName = testCase.TestCaseName;
                TestCaseStep = testStep.TestStepName;
                Body = testStep.Body;
                ExpectedRepsonseCode = testStep.ExpectedResponseCode;
                ExpectedRepsonseMessage = testStep.ExpectedResponseBody;
            }
        }
        public class ResponseData : TestDetails
        {
            public int StatusCode { get; set; }
            public HttpContent Content { get; set; }

            public ResponseData()
            {

            }
            public ResponseData(HttpResponseMessage response)
            {
                Content = response.Content;
                StatusCode = (int)response.StatusCode;
            }
        }
    }
}
