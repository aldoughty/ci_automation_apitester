namespace ci_automation_apitester.ApiDto.DataTagging
{
    public class Admin : BaseRestApi
    {
        const string Url = "/tagging/api/Admin";
        public Dto CurrentObject = new();

        public class Dto
        {
            public string orgId { get; set; }
            public Dto()
            {
                orgId = "org_KlOYPcLaAzMYiwzx";  //SSB Org ID
            }
        }
        public override string GetUrl(string action)
        {
            switch (action)
            {
                case "GET":
                    return Url;
                default: return Url;
            }
        }
        public override string GetTestType()
        {
            return "ValidateResponseContainsString";
        }
        public override Dictionary<string, string> GetHeaderParameter()
        {
            Dictionary<string, string> headerParams = new()
            {
                {"org_id", CurrentObject.orgId},
            };

            return headerParams;
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

            //TaggingTypes
            DataTable tagDt = DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), query.QueryAdminTagTypes(CurrentObject.orgId));
            foreach (DataRow dataRow in tagDt.Rows)
            {
                TestParams.TestStepName = GetType().Name + "_GET_TagTypes";
                TestParams.ExpectedResponseBody = dataRow["taggingTypes"].ToString().Trim('}');
                testParamsList.Add(TestParams.Copy());
            }

            //Data Models
            DataTable dataDt = DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), query.QueryAdminDataModels(CurrentObject.orgId));
            foreach (DataRow dataRow in dataDt.Rows)
            {
                TestParams.TestStepName = GetType().Name + "_GET_DataModels";
                TestParams.ExpectedResponseBody = dataRow["dataModels"].ToString().Trim('{');
                testParamsList.Add(TestParams.Copy());
            }

            //Configs
            DataTable configDt = DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), query.QueryAdminConfigs(CurrentObject.orgId));
            foreach (DataRow dataRow in configDt.Rows)
            {
                TestParams.TestStepName = GetType().Name + "_GET_Configs";
                TestParams.ExpectedResponseBody = dataRow["configs"].ToString();
                testParamsList.Add(TestParams.Copy());
            }

            //Dimensions
            DataTable dimDt = DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), query.QueryAdminDimensions(CurrentObject.orgId));
            foreach (DataRow dataRow in dimDt.Rows)
            {
                TestParams.TestStepName = GetType().Name + "_GET_Dimensions";
                TestParams.ExpectedResponseBody = dataRow["dimensions"].ToString();
                testParamsList.Add(TestParams.Copy());
            }

            //Columns
            DataTable colDt = DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), query.QueryAdminColumns());
            foreach (DataRow dataRow in colDt.Rows)
            {
                TestParams.TestStepName = GetType().Name + "_GET_Columns";
                TestParams.ExpectedResponseBody = dataRow["columns"].ToString();
                testParamsList.Add(TestParams.Copy());
            }

            return testParamsList;
        }
        public override List<TestStep> GetFreeFormTests(string authValue, IEnvironment environment)
        {
            List<TestStep> result = new();
            result.AddRange(GetInvalidOrgIdHeaderParamTests(authValue, environment));

            return result;
        }
        public List<TestStep> GetInvalidOrgIdHeaderParamTests(string authValue, IEnvironment environment)
        {
            //Validates passing invalid Org Ids via Header for Admin endpoint
            List<TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.Url = GetUrl("GET");
            TestParams.RequestType = "GET";
            TestParams.ExpectedResponseCode = 500;

            //Inactive OrgId
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormHeaderTest_InactiveOrgId";
            TestParams.Headers["org_id"] = "org_YXAl6vQO2ISv51MH";
            TestParams.ExpectedResponseBody = "No tenantid found for org";
            testParamsList.Add(TestParams.Copy());

            //Invalid OrgId (doesn't exist)
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormHeaderTest_InvalidOrgId";
            TestParams.Headers["org_id"] = "org_ss0DHGCZvPZSAMhQ";
            TestParams.ExpectedResponseBody = "No tenantid found for org";
            testParamsList.Add(TestParams.Copy());

            //Blank OrgId
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormHeaderTest_BlankOrgId";
            TestParams.Headers["org_id"] = "";
            TestParams.ExpectedResponseBody = "Please provide the org_id in the header";
            testParamsList.Add(TestParams.Copy());

            //No OrgId
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormHeaderTest_NoOrgId";
            TestParams.Headers.Remove("org_id");
            TestParams.ExpectedResponseBody = "Please provide the org_id in the header";
            testParamsList.Add(TestParams.Copy());

            return testParamsList;
        }
    }
}


