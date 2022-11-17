namespace ci_automation_apitester.ApiDto.DataTagging
{
    public class ColumnType : BaseRestApi
    {
        const string Endpoint = "/tagging/api/ColumnType";
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
        public override List<string> GetRequestTypes()
        {
            List<string> actions = new()
            { "GET" };

            return actions;
        }
        public override List<TestObjects.TestStep> GetParameterTests(MessageData messageData, string authValue, IEnvironment environment)
        {
            DataTaggingQueries query = new();
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.Url = GetUrl("GET");
            TestParams.RequestType = "GET";

            //Returns all ColumnTypes in taggingUI.ColumnType (for GET All ColumnType)
            var allColTypIdDt = DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), query.QueryAllColumnTypes());
            foreach (DataRow dataRow in allColTypIdDt.Rows)
            {
                TestParams.TestStepName = GetType().Name + "_GET_AllColumnTypes";
                TestParams.ExpectedResponseBody = dataRow["ColumnTypes"].ToString();
                testParamsList.Add(TestParams.Copy());
            }

            //Returns each ColumnTypeId in taggingUI.ColumnType (for GET of specific ColumnType)
            var colTypIdDt = DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), query.QueryEachColumnType());
            foreach (DataRow dataRow in colTypIdDt.Rows)
            {
                TestParams.TestStepName = GetType().Name + "_GET_ColumnTypeId_" + dataRow["columnTypeId"].ToString();
                TestParams.Url = GetUrl("GETQuery") + dataRow["columnTypeId"].ToString();
                TestParams.ExpectedResponseBody = dataRow["ColumnType"].ToString();
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
