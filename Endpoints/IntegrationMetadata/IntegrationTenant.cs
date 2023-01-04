namespace ci_automation_apitester.ApiDto.IntegrationMetadata
{
    public class IntegrationTenant : BaseRestApi
    {
        const string Endpoint = "/api/IntegrationTenant";
        public Dto CurrentObject = new();

        public class Dto
        {
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "IntegrationId Doesn't Exist")]
            [UrlTest("GET", "02DADE94-9D33-4269-B8D6-478B4310515F", 404, "Sequence contains no elements", "IntegrationId Casing Mismatch")]  //seeded, unique
            public string Id { get; set; }
            public string TenantId { get; set; }
            public string TenantName { get; set; }
            public string TenantKey { get; set; }
            public string TenantActive { get; set; }
            public string IntegrationId { get; set; }
            public string TenantIntegrationId { get; set; }
            public string TenantIntegrationActive { get; set; }

            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                TenantId = "";
                TenantName = "";
                TenantKey = "";
                TenantActive = "";
                IntegrationId = "";
                TenantIntegrationId = "";
                TenantIntegrationActive = "";
            }
        }
        public override string GetUrl(string action)
        {
            switch (action)
            {
                case "GET":
                    return Endpoint + "/" + "02dade94-9d33-4269-b8d6-478b4310515f";   //Endpoint has no regular GET, so we need an Url with a seeded Integration for Workflow/Auth tests
                case "GETQuery":
                    return Endpoint + "/";         //api/IntegrationTenant/{IntegrationId}
                case "AttributeUrlTest":
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
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.RequestType = "GET";

            //Returns specific Tenants in dbo.Tenant by correlated IntegrationId in dbo.TenantIntegration (for GET /IntegrationTenant/{IntegrationId})
            DataTable integrationIdDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryAllIntegration(SecretsManager.SnowflakeDatabaseEnvironment()));
            foreach (DataRow row in integrationIdDt.Rows)
            {
                DataTable eachIntegrationIdDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryTenantByIntegrationId(SecretsManager.SnowflakeDatabaseEnvironment(), row.Field<string>("Id")));
                JArray jArray = JArray.FromObject(eachIntegrationIdDt, JsonSerializer.CreateDefault());

                TestParams.TestStepName = GetType().Name + "_GET_TenantsByIntegrationId_" + row.Field<string>("Id");
                TestParams.Url = GetUrl("GETQuery") + row.Field<string>("Id").ToString();
                string rowJson = jArray.ToString(Formatting.None);
                TestParams.ExpectedResponseBody = rowJson;
                testParamsList.Add(TestParams.Copy());
            }

            return testParamsList;
        }
        public override List<UrlAttributeTest> GetUrlAttributeTests()
        {
            UrlAttributeTest attributeTest = new();
            List<UrlAttributeTest> attributeTestList = new();
            JsonSerializerSettings settings = new() { NullValueHandling = NullValueHandling.Ignore };  //if dto property = null, ignore/omit

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
    }
}
