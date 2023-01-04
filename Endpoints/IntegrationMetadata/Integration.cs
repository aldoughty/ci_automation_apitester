namespace ci_automation_apitester.ApiDto.IntegrationMetadata
{
    public class Integration : BaseRestApi
    {
        const string Endpoint = "/api/Integration";
        public Dto CurrentObject = new();

        //Negative validation GETS for values that aren't a part of the DTO?

        public class Dto
        {
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "Id Doesn't Exist")]
            [UrlTest("GET", "87130460-06B8-4A4B-9F0E-FEE5D05A7335", 404, "Sequence contains no elements", "Id Casing Mismatch")]
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string IntegrationTypeId { get; set; }
            public string IntegrationDirection { get; set; }
            public int MaxFilesPerRun { get; set; }
            public string DefaultScheduleId { get; set; }

            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                Name = "";
                Description = "";
                IntegrationTypeId = "";
                IntegrationDirection = "";
                MaxFilesPerRun = 10;
                DefaultScheduleId = "";
            }
        }
        public override string GetUrl(string action)
        {
            switch (action)
            {
                case "GET":                         //Endpoint - returns all; Endpoint + "/Id" - returns specific
                    return Endpoint;
                case "GETQuery":
                    return Endpoint + "/";         //api/Integration/{IntegrationId}
                case "GETTenantQuery":
                    return Endpoint + "/tenantid/";         //api/Integration/tenantid/{TenantId}
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

            DataTable allIntegrationDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryAllIntegration(SecretsManager.SnowflakeDatabaseEnvironment()));

            //Returns all Integrations in dbo.Integrations (for GET /api/Integration)
            string expectedGetAllResponseBody = JsonConvert.SerializeObject(allIntegrationDt);
            TestParams.TestStepName = GetType().Name + "_GET_AllIntegration";
            TestParams.Url = GetUrl("GET");
            TestParams.ExpectedResponseBody = expectedGetAllResponseBody;
            testParamsList.Add(TestParams.Copy());

            //Returns specific Integration in dbo.Integrations (for GET /api/Integration/{IntegrationId})
            for (int rowIndex = 0; rowIndex < allIntegrationDt.Rows.Count; rowIndex++)
            {
                JArray jArray = JArray.FromObject(allIntegrationDt, JsonSerializer.CreateDefault());

                JToken rowJToken = jArray[rowIndex];
                string rowJson = rowJToken.ToString(Formatting.None);

                TestParams.TestStepName = GetType().Name + "_GET_IntegrationById_" + rowJToken["ID"];
                TestParams.Url = GetUrl("GETQuery") + rowJToken["ID"].ToString();
                TestParams.ExpectedResponseBody = rowJson;
                testParamsList.Add(TestParams.Copy());
            }

            //Returns specific Integrations in dbo.Integrations by correlated TenantId in dbo.TenantIntegration (for GET /api/Integration/tenantid/{TenantId})
            DataTable tenantIdDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryDistinctTenantIntegrationTenantId(SecretsManager.SnowflakeDatabaseEnvironment()));
            foreach (DataRow row in tenantIdDt.Rows)
            {
                DataTable eachTenantIdDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryIntegrationByTenantId(SecretsManager.SnowflakeDatabaseEnvironment(), row.Field<string>("TenantId")));
                JArray jArray = JArray.FromObject(eachTenantIdDt, JsonSerializer.CreateDefault());

                TestParams.TestStepName = GetType().Name + "_GET_IntegrationByTenantId_" + row.Field<string>("TenantId");
                TestParams.Url = GetUrl("GETTenantQuery") + row.Field<string>("TenantId").ToString();
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
