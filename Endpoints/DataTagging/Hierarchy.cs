namespace ci_automation_apitester.ApiDto.DataTagging
{
    public class Hierarchy : BaseRestApi
    {
        const string Url = "/tagging/api/Hierarchy";
        public Dto CurrentObject = new();

        public class Dto
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public bool Active { get; set; }

            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                Name = "QA Hierarchy";
                Description = "QA Hierarchy";
                Active = true;
            }
        }
        public override string GetUrl(string action)
        {
            switch (action)
            {
                case "GET":                         //Url - returns all; Url + "/" + CurrentObject.Id - returns specific
                case "POST":
                case "PUT":
                    return Url;
                case "PATCH":
                    return Url + "/" + CurrentObject.Id;
                case "DELETE":
                    return Url + "/" + CurrentObject.Id;
                default: return Url;
            }
        }
        public override string GetTestType()
        {
            return "ValidateResponseContainsString";
        }
        public override Dictionary<string, string> GetHeaders(string authValue, IEnvironment environment)
        {
            Dictionary<string, string> headers = new()
                {
                    {"Ocp-Apim-Subscription-Key", "31b292424f3c47aea201d71c6e17c60d"},
                    {"ApiVersion", "1.0"},
                    {"Authorization", authValue },
                };

            return headers;
        }
        public override List<string> GetRequestTypes()
        {
            List<string> actions = new()
            { "GET" };

            return actions;
        }
        public override List<TestStep> GetParameterTests(MessageData messageData, string authValue, IEnvironment environment)
        {
            DataTaggingQueries query = new();
            List<TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.Url = GetUrl("GET");
            TestParams.RequestType = "GET";

            //Returns all Hierarchies in taggingUI.Hierarchy (for GET All Hierarchy)
            DataTable dataTable = DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), query.QueryHierarchies());
            foreach (DataRow dataRow in dataTable.Rows)
            {
                TestParams.TestStepName = GetType().Name + "_GET_Hierarchies";
                TestParams.ExpectedResponseBody = dataRow["Hierarchies"].ToString();
                testParamsList.Add(TestParams.Copy());
            }

            return testParamsList;
        }
    }
}
