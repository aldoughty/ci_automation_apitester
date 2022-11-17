namespace ci_automation_apitester.ApiDto.TenantMetadata
{
    public class TenantRoles : BaseRestApi
    {
        const string Endpoint = "/api/TenantRoles";
        public Dto CurrentObject = new();

        public class Dto
        {
            [UrlTest("GET", "QA WAS HERE", 500, "Unable to locate tenant id for Tenant with key: QA WAS HERE", "TenantKey Doesn't Exist")]  //Casing mismatch?  
            public string TenantId { get; set; }  
            public string SnowflakeRole { get; set; }
            public Dto()
            {
                TenantId = Guid.NewGuid().ToString();
                SnowflakeRole = "ROLE_CLIENT_READER_SOONERS";
            }
        }
        public override string GetUrl(string action)
        {
            switch (action)
            {
                case "GET":                         //Endpoint - returns all
                case "POST":
                    return Endpoint;
                case "GETQuery":
                    return Endpoint + "/";          //GET /api/TenantRoles/{tenantKey}
                case "AttributeUrlTest":
                    return Endpoint + "/";
                default: return Endpoint;           //TenantKey on UI:  Required; Min 3 Max 50; AlphaNumeric Allowed; No Spec Char or Spaces
            }
        }
        public override List<string> GetRequestTypes()
        {
            List<string> actions = new()
            { "GET" };

            return actions;
        }
        public override string GetPostBody()
        {
            return JsonConvert.SerializeObject(CurrentObject);  
        }
        public override List<TestObjects.TestStep> GetParameterTests(MessageData messageData, string authValue, IEnvironment environment)
        {
            TenantMetadataQueries query = new();
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.Url = GetUrl("GET");
            TestParams.RequestType = "GET";

            //Returns all Tenant Roles in dbo.Tenant (for GET /api/TenantRoles)
            DataTable allTenantRolesDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), query.QueryAllTenantRoles(SecretsManager.SnowflakeDatabaseEnvironment()));
            string expectedGetAllResponseBody = JsonConvert.SerializeObject(allTenantRolesDt);
            TestParams.TestStepName = GetType().Name + "_GET_AllTenantRoles";
            TestParams.ExpectedResponseBody = expectedGetAllResponseBody;
            testParamsList.Add(TestParams.Copy());

            //Queries TenantKeys in dbo.Tenant HAVING records in dbo.TenantRoles for GET /api/TenantRoles/{tenantKey} (returns 200 with role(s))
            DataTable allTenantKeysHavingRolesDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), query.QueryAllTenantKeysHavingTenantRoles(SecretsManager.SnowflakeDatabaseEnvironment()));
            foreach (DataRow dataRow in allTenantKeysHavingRolesDt.Rows)
            {
                DataTable allTenantRolesByTenantIdDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), query.QueryTenantRolesByTenantId(dataRow["TENANTID"].ToString(), SecretsManager.SnowflakeDatabaseEnvironment()));

                string expectedGetRolesByIdResponseBody = JsonConvert.SerializeObject(allTenantRolesByTenantIdDt);

                TestParams.TestStepName = GetType().Name + "_GET_TenantRolesByTenantKeyHavingRoles_" + dataRow["TENANT_KEY"].ToString();
                TestParams.Url = GetUrl("GETQuery") + dataRow["TENANT_KEY"].ToString();
                TestParams.ExpectedResponseBody = expectedGetRolesByIdResponseBody;
                testParamsList.Add(TestParams.Copy());
            }

            //Queries TenantKeys in dbo.Tenant NOT HAVING records in dbo.TenantRoles for GET /api/TenantRoles/{tenantKey} (returns 500 Unable to find role for tenant)
            DataTable allTenantKeysNotHavingRolesDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), query.QueryAllTenantKeysNotHavingTenantRoles(SecretsManager.SnowflakeDatabaseEnvironment()));
            foreach (DataRow dataRow in allTenantKeysNotHavingRolesDt.Rows)
            {
                TestParams.TestType = "ValidateResponseContainsString";
                TestParams.TestStepName = GetType().Name + "_GET_TenantRolesByTenantKeyNotHavingRoles_" + dataRow["TENANT_KEY"].ToString();
                TestParams.Url = GetUrl("GETQuery") + dataRow["TENANT_KEY"].ToString();
                TestParams.ExpectedResponseCode = 500;
                TestParams.ExpectedResponseBody = "Unable to find role for tenant";
                testParamsList.Add(TestParams.Copy());
            }

            return testParamsList;
        }
        public override List<UrlAttributeTest> GetUrlAttributeTests()
        {
            UrlAttributeTest attributeTest = new();
            List<UrlAttributeTest> attributeTestList = new();
            JsonSerializerSettings settings = new() { NullValueHandling = NullValueHandling.Ignore };  //if dto property = null, ignore

            var urlProperties = typeof(Dto).GetProperties()
                             .Where(p => p.IsDefined(typeof(UrlTest), false));

            foreach (PropertyInfo property in urlProperties)
            {
                var attrs = property.GetCustomAttributes(typeof(UrlTest), false);
                foreach (UrlTest attr in attrs)
                {
                    attributeTestList.Add(attributeTest.Create(property.Name, attr.Action, attr.Url, attr.ResponseCode, attr.Message, attr.TestName).Copy());
                }
            }
            return attributeTestList;
        }
        public override void CleanUp(MessageData messageData, string currentId)
        {
            string command = $@"DELETE FROM CI_METADATA{SecretsManager.SnowflakeDatabaseEnvironment()}.dbo.TenantRoles WHERE Id = '{currentId}'";
            DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), command);
        }
    }
}
