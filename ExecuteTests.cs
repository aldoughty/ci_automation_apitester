namespace ci_automation_apitester
{
    public class ExecuteTests
    {
        //static int Main(string[] args)

        static int Main()
        {
            Stopwatch timer = new();
            MessageData messageData = ParseArguments.PullFromEnvVariables();
            messageData.ValidateContent();
            TestParameters tP= new();
            Dictionary<string, object> testParameters = tP.CreateTestParameters(messageData);
            Serilog.Core.Logger logger = Logger.CreateSystemLogger(messageData.StandardTestParameters);
            ApiTestResults testResults = new(messageData.StandardTestParameters);
            ApiTestResults.TestDetails TestResultDetail = testParameters.ContainsKey("TestResultDetail") ? (ApiTestResults.TestDetails)testParameters["TestResultDetail"] : new ApiTestResults.TestDetails();

            try
            {
                GenerateTests generateTests = new GenerateTests(testParameters);
                TestPlan testPlan = generateTests.GenerateApiTests(testParameters);
                timer.Start();
                logger.Information("Kicking off Automation_Execute_ApiTesterTests.  Message received {@Message}", messageData);

                foreach (TestSuiteList testSuiteList in testPlan.TestSuiteList)
                {
                    foreach (TestSuite testSuite in testSuiteList.TestSuite)
                    {
                        foreach (TestCase testCase in testSuite.TestCaseList)
                        {
                            foreach (TestStep testStep in testCase.TestStep)
                            {
                                testParameters["Request"] = testStep;
                                TestObjects.TestStep Request = testParameters.ContainsKey("Request") ? (TestObjects.TestStep)testParameters["Request"] : new TestObjects.TestStep();
                                testParameters["TestResultDetail"] = new ApiTestResults.RequestData(testCase, testStep);
                                testParameters["EndpointUnderTest"] = "ci_automation_apitester.ApiDto." + testSuite.EndpointUnderTest;
                                IRestApi EndpointUnderTest = (IRestApi)Activator.CreateInstance(Type.GetType(testParameters["EndpointUnderTest"].ToString()));
                                HttpResponseMessage Response = new();
                                bool exceptionOccurred = false;
                                try
                                {
                                    timer.Start();

                                    if (Request.RequestType.ToUpper() != "GET")
                                    {
                                        if (Request.SwapGUID)
                                        {
                                            if (Request.RequestType.ToUpper() == "DELETE")
                                            {
                                                Request.Url = Request.Url.Remove(Request.Url.LastIndexOf('/')) + "/" + messageData.CurrentId;
                                            }

                                            if (Request.RequestType.ToUpper() == "PUT")
                                            {

                                                string primaryKey = JObject.Parse(Request.Body).First.First.Path;  //allows for Primary Keys that may not be "Id" in response
                                                dynamic jsonObj = JsonConvert.DeserializeObject(Request.Body);
                                                jsonObj[primaryKey] = messageData.CurrentId;
                                                //jsonObj["Id"] = messageData.CurrentId;
                                                Request.Body = JsonConvert.SerializeObject(jsonObj, Formatting.None);
                                            }
                                        }
                                    }

                                    HttpRequestMessage message = new(new HttpMethod(Request.RequestType), Request.Url);

                                    foreach (var key in Request.Headers.Keys)
                                    {
                                        message.Headers.Add(key, Request.Headers[key]);
                                    }

                                    message.Content = new StringContent(Request.Body, Encoding.UTF8, Request.ContentType);
                                    Response = ApiMessageHandler.SendMessageSync(message);
                                    testParameters["TestResponseDetail"] = new ApiTestResults.ResponseData(Response);

                                }
                                catch (Exception e)
                                {
                                    exceptionOccurred = true;

                                    TestResultDetail.Status = TestResults.Status.Error;
                                    TestResultDetail.FailureMessage = e.Message;
                                }
                                finally
                                {

                                    TestResultDetail.ExecutionDate = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ssZ");
                                    if (Request.GetGUID && Request.ExpectedResponseCode == 200)
                                    {
                                        string responsebody = Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                        string primaryKey = JObject.Parse(responsebody).First.First.Path;   //allows for Primary Keys that may not be "Id" in response
                                        messageData.CurrentId = JObject.Parse(responsebody)[primaryKey].ToString();
                                        //messageData.CurrentId = JObject.Parse(responsebody)["id"].ToString();
                                    }

                                    if (Request.PerformCleanup) EndpointUnderTest.CleanUp(messageData, messageData.CurrentId);


                                    if (!exceptionOccurred)
                                    {
                                        Assembly assembly = Assembly.Load("ci_automation_apitester");
                                        string typeString = "ci_automation_apitester.Libs.TestExecutionType";
                                        var t = assembly.GetType(typeString);
                                        var testClass = Activator.CreateInstance(t, testParameters);
                                        testResults.LogIndividualResults((ApiTestResults.TestDetails)testClass.GetType().GetMethod(testStep.TestType).Invoke(testClass, null));
                                    }

                                    timer.Stop();
                                    TestResultDetail.MilliSecondsToExecute = Convert.ToInt32(timer.ElapsedMilliseconds);
                                }
                            }
                        }
                    }
                }
                timer.Stop();
                testResults.SecondsToExecute = Convert.ToInt32(timer.ElapsedMilliseconds / 1000);
                testResults.LogFinalResults();
                logger.Verbose("Tests Completed");
                logger.Verbose("Number of Tests Run : {@Num}", testResults.NumberTestsExecuted);
                logger.Verbose("Number of Tests Passed: {@Num}", testResults.NumberTestsPassed);
                logger.Verbose("Number of Tests Failed: {@Num}", testResults.NumberTestsFailed);
                logger.Verbose("Number of Tests Warnings: {@Num}", testResults.NumberTestsWithWarning);
                logger.Verbose("Number of Tests Errors: {@Num}", testResults.NumberTestsWithError);
                return 0;

            }
            catch (Exception e)
            {
                timer.Stop();

                logger.Fatal("Exception Message: {@Message}", e.Message);
                logger.Fatal("Exception StackTrace: {@Exception}", e.StackTrace);
                return 1;
            }
        }
    }
}