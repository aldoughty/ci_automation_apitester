namespace ci_automation_apitester.Libs
{
    public class TestExecutionType
    {
        public TestObjects.TestStep Request;
        public ApiTestResults.ResponseData Response;
        public ApiTestResults.TestDetails TestResultDetail;
        
        public TestExecutionType(TestObjects.TestStep test)
        {
            Request = test;
        }
        public TestExecutionType(Dictionary<string, object> testParameters)
        {
            Request = testParameters.ContainsKey("Request") ? (TestObjects.TestStep)testParameters["Request"] : new TestObjects.TestStep();
            Response = testParameters.ContainsKey("TestResponseDetail") ? (ApiTestResults.ResponseData)testParameters["TestResponseDetail"] : new ApiTestResults.ResponseData();
            TestResultDetail = testParameters.ContainsKey("TestResultDetail") ? (ApiTestResults.TestDetails)testParameters["TestResultDetail"] : new ApiTestResults.TestDetails();
        }
        public ApiTestResults.TestDetails CompareResponseToString()
        {
            ValidateErrorHandling(TestResultDetail, "ValidateResponseCode", Response.StatusCode, Request.ExpectedResponseCode);

            if (Request.ExpectedResponseBody != "" && TestResultDetail.Status == TestResults.Status.Passed)
            {
                string content = Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                ValidateErrorHandling(TestResultDetail, "ValidateExactResponseBody", content, Request.ExpectedResponseBody);
            }
            return TestResultDetail;
        }
        public ApiTestResults.TestDetails CompareResponseToStringAndResponseCodeRange()
        {
            if (Request.ExpectedResponseCode != 200)
            { ValidateErrorHandling(TestResultDetail, "ValidateResponseCode", Response.StatusCode, Request.ExpectedResponseCode); }
            else
            { ValidateErrorHandling(TestResultDetail, "ValidateResponseCodeRange", Response.StatusCode, Request.ExpectedResponseCode); }

            if (Request.ExpectedResponseBody != "" && TestResultDetail.Status == TestResults.Status.Passed)
            {
               string content = Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
               ValidateErrorHandling(TestResultDetail, "ValidateExactResponseBody", content, Request.ExpectedResponseBody);
            }
                
            return TestResultDetail;
        }
        public ApiTestResults.TestDetails ValidateResponseContainsString()
        {
            ValidateErrorHandling(TestResultDetail, "ValidateResponseCode", Response.StatusCode, Request.ExpectedResponseCode);

            if (Request.ExpectedResponseBody != "" && TestResultDetail.Status == TestResults.Status.Passed)
            {
               var content = Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
               ValidateErrorHandling(TestResultDetail, "ValidateContainsResponseBody", content, Request.ExpectedResponseBody);
            }
                
            return TestResultDetail;
        }
        public ApiTestResults.TestDetails ValidateResponseCode()
        {
            if (Request.ExpectedResponseCode != 200)
            { ValidateErrorHandling(TestResultDetail, "ValidateResponseCode", Response.StatusCode, Request.ExpectedResponseCode); }
            else
            { ValidateErrorHandling(TestResultDetail, "ValidateResponseCodeRange", Response.StatusCode, Request.ExpectedResponseCode); }
                
            return TestResultDetail;
        }
        public ApiTestResults.TestDetails ValidateResponseObjectCounts()
        {
            ValidateErrorHandling(TestResultDetail, "ValidateResponseObjectCounts", Response.StatusCode, Request.ExpectedResponseCode);
            
            return TestResultDetail;
        }
        private void ValidateResponseCode(HttpStatusCode actualResponseCode, HttpStatusCode expectedResponseCode)
        {
            actualResponseCode.Should().Be(expectedResponseCode);
        }
        private void ValidateResponseCodeRange(HttpStatusCode actualResponseCode)
        {
            IComparable<int> responseCode = (int)actualResponseCode;
            responseCode.Should().BeInRange(200, 204);
        }
        private void ValidateContainsResponseBody(string actualResponseBody, string expectedResponseBody)
        {
            actualResponseBody.Should().ContainEquivalentOf(expectedResponseBody);
        }
        private void ValidateExactResponseBody(string actualResponseBody, string expectedResponseBody)
        {
            actualResponseBody.Should().BeEquivalentTo(expectedResponseBody);
        }
        private void ValidateResponseObjectCounts(string actualResponseBody, string expectedResponseBody)
        {
            //TODO
        }
        private void ValidateErrorHandling(ApiTestResults.TestDetails testDetail, string test, object actualResult, object expectedResult)
        {
            try
            {
                switch (test)
                {
                    case "ValidateResponseCode":
                        ValidateResponseCode((HttpStatusCode)actualResult, (HttpStatusCode)expectedResult);
                        break;
                    case "ValidateResponseCodeRange":
                        ValidateResponseCodeRange((HttpStatusCode)actualResult);
                        break;
                    case "ValidateExactResponseBody":
                        ValidateExactResponseBody((string)actualResult, (string)expectedResult);
                        break;
                    case "ValidateEquivalentResponseBody":
                        ValidateContainsResponseBody((string)actualResult, (string)expectedResult);
                        break;
                    case "ValidateResponseObjectCounts":
                        ValidateResponseObjectCounts((string)actualResult, (string)expectedResult);
                        break;
                }
                TestResultDetail.Status = TestResults.Status.Passed;
            }
            catch (Exception e)
            {
                TestResultDetail.Status = TestResults.Status.Failed;
                TestResultDetail.FailureMessage = e.Message;
            }
        }
    }
}
