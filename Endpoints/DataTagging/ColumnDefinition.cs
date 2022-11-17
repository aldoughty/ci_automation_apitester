namespace ci_automation_apitester.ApiDto.DataTagging
{
    public class ColumnDefinition : BaseRestApi
    {
        const string Endpoint = "/tagging/api/ColumnDefinition";
        public Dto CurrentObject = new();

        public class Dto
        {
            public string Id { get; set; }
            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
            }
        }
        public override string GetUrl(string action)
        {
            switch (action)
            {
                case "GET":                         //Endpoint - returns all; Url + "/Id" - returns specific
                case "POST":
                case "PUT":
                    return Endpoint;
                case "PATCH":
                    return Endpoint + "/" + CurrentObject.Id;
                case "DELETE":
                    return Endpoint + "/" + CurrentObject.Id;
                case "GETQuery":
                    return Endpoint + "/";
                default: return Endpoint;
            }
        }
        public override List<TestObjects.TestStep> GetParameterTests(MessageData messageData, string authValue, IEnvironment environment)
        {
            DataTaggingQueries query = new();
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.Url = GetUrl("GET");
            TestParams.RequestType = "GET";

            //Returns all ColumnDefinitions in taggingUI.ColumnDefinitions (for GET all)
            DataTable allColDefIdDt = DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), query.QueryAllColumnDefinitions());
            foreach (DataRow dataRow in allColDefIdDt.Rows)
            {
                TestParams.TestStepName = GetType().Name + "_GET_AllColumnDefinitions";
                TestParams.ExpectedResponseBody = dataRow["ColumnDefinitions"].ToString();
                testParamsList.Add(TestParams.Copy());
            }

            //Returns each ColumnDefinitionId in taggingUI.ColumnDefinition (for GET of specific ColumnDefinition)
            //NOTE:  Type is returning as NULL only on GET of specific ColumnDefinitionId
            DataTable colDefIdDt = DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), query.QueryAllColumnDefinitions());
            foreach (DataRow dataRow in colDefIdDt.Rows)
            {
                TestParams.TestStepName = GetType().Name + "_GET_ColumnDefinitionId_" + dataRow["columnDefinitionId"].ToString();
                TestParams.Url = GetUrl("GETQuery") + dataRow["columnDefinitionId"].ToString();
                TestParams.ExpectedResponseBody = dataRow["ColumnDefinition"].ToString();
                testParamsList.Add(TestParams.Copy());
            }

            return testParamsList;
        }
        //public override List<TestObjects.TestStep> GetFreeFormTests(string token)
        //{
        //    var result = new List<TestObjects.TestStep>();
        //    result.AddRange(GetInvalidOrgIdHeaderParamTests(token));

        //    return result;
        //}
        //public List<TestObjects.TestStep> GetInvalidOrgIdHeaderParamTests(string token)
        //{
        //    //Validates passing invalid Org Ids via Header for Admin endpoint
        //    var testParamsList = new List<TestObjects.TestStep>();
        //    var testParams = new TestParameters()
        //    {
        //        //TestType = GetTestType(),
        //        //TestName = "",
        //        //Headers = GetHeaders(token),
        //        Url = GetUrl("GET"),
        //        Action = "GET",
        //        //Body = "",
        //        ResponseCode = 500,
        //        //ExpectedResponseBody = ""
        //    };

        //    //Inactive OrgId
        //    TestParams.TestStep = GetType().Name + "_" + TestParams.RequestType + "_FreeFormHeaderTest_InactiveOrgId";
        //    testParams.Headers["org_id"] = "org_YXAl6vQO2ISv51MH";
        //    testParams.ExpectedResponseBody = "No tenantid found for org";
        //    testParamsList.Add(testParams.Copy());

        //    //Invalid OrgId (doesn't exist)
        //    TestParams.TestStep = GetType().Name + "_" + TestParams.RequestType + "_FreeFormHeaderTest_InvalidOrgId";
        //    testParams.Headers["org_id"] = "org_ss0DHGCZvPZSAMhQ";
        //    testParams.ExpectedResponseBody = "No tenantid found for org";
        //    testParamsList.Add(testParams.Copy());

        //    //Blank OrgId
        //    TestParams.TestStep = GetType().Name + "_" + TestParams.RequestType + "_FreeFormHeaderTest_BlankOrgId";
        //    testParams.Headers["org_id"] = "";
        //    testParams.ExpectedResponseBody = "Please provide the org_id in the header";
        //    testParamsList.Add(testParams.Copy());

        //    //No OrgId
        //    TestParams.TestStep = GetType().Name + "_" + TestParams.RequestType + "_FreeFormHeaderTest_NoOrgId";
        //    testParams.Headers.Remove("org_id");
        //    testParams.ExpectedResponseBody = "Please provide the org_id in the header";
        //    testParamsList.Add(testParams.Copy());

        //    return testParamsList;
        //}
    }
}
