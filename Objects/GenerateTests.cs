namespace ci_automation_apitester.Objects
{
    public class GenerateTests
    {
        private MessageData MessageData;

        public GenerateTests(Dictionary<string, object> testParameters)
        {
            MessageData = testParameters.ContainsKey("MessageData") ? (MessageData)testParameters["MessageData"] : new MessageData();
        }
        public TestPlan GenerateApiTests(Dictionary<string, object> testParameters)
        {
            TestSuiteList TestSuiteList = new();
            List<TestSuiteList> testSuiteList = new();
            TestPlan TestPlan = new();
            List<string> apiUnderTest = MessageData.EndpointUnderTest.Split(',').Select(p => p.Trim()).ToList();

            foreach (string api in apiUnderTest)
            {
                Assembly assembly = Assembly.Load("ci_automation_apitester");
                var envType = assembly.GetType("ci_automation_apitester.Environment." + api);
                var myEnvironment = Activator.CreateInstance(envType);
                //Find all the objects to build the tests for
                List<Type> apiObjects = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace == "ci_automation_apitester.ApiDto." + api && !t.FullName.Contains("+"))
                .ToList();

                foreach (Type apiObject in apiObjects)
                {
                    IRestApi myObject = (IRestApi)Activator.CreateInstance(apiObject);
                    testParameters["ApiDto"] = myObject;
                    testParameters["ApiEnvironment"] = myEnvironment;
                    CreateTests createTests = new(testParameters);
                    testSuiteList.Add(TestSuiteList.Create(createTests.GetTests()).Copy());
                }
                TestPlan.Create(testSuiteList);
            }
            
            return TestPlan;
        }
    }
}
