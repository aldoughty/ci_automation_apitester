namespace ci_automation_apitester.ApiDto.IntegrationMetadata
{
    public class IntegrationType : BaseRestApi
    {
        const string Endpoint = "/api/IntegrationType";
        public Dto CurrentObject = new();

        public class Dto
        {
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "Id Doesn't Exist")]
            [UrlTest("GET", "8CB217A3-9EE5-4527-AA16-2C6E1AA654FE", 404, "Sequence contains no elements", "Id Casing Mismatch")]
            public string Id { get; set; }
            public string Name { get; set; }

            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                Name = "";
            }
        }
        public override string GetUrl(string action)
        {
            switch (action)
            {
                case "GET":                         //Endpoint - returns all; Endpoint + "/Id" - returns specific
                case "GETQuery":
                    return Endpoint + "/";         //api/IntegreationType/{IntegrationTypeId}
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

            DataTable allIntegrationTypeDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryAllIntegrationType(SecretsManager.SnowflakeDatabaseEnvironment()));

            //Returns all IntegrationType in dbo.IntegrationType (for GET /api/IntegrationType)
            string expectedGetAllResponseBody = JsonConvert.SerializeObject(allIntegrationTypeDt);
            TestParams.TestStepName = GetType().Name + "_GET_AllIntegrationType";
            TestParams.Url = GetUrl("GET");
            TestParams.ExpectedResponseBody = expectedGetAllResponseBody;
            testParamsList.Add(TestParams.Copy());

            //Returns specific IntegrationType in dbo.IntegrationType (for GET /api/IntegrationType/{IntegrationTypeId})
            for (int rowIndex = 0; rowIndex < allIntegrationTypeDt.Rows.Count; rowIndex++)
            {
                JArray jArray = JArray.FromObject(allIntegrationTypeDt, JsonSerializer.CreateDefault()); 

                JToken rowJToken = jArray[rowIndex];
                string rowJson = rowJToken.ToString(Formatting.None);

                TestParams.TestStepName = GetType().Name + "_GET_IntegrationTypeById_" + rowJToken["ID"];
                TestParams.Url = GetUrl("GETQuery") + rowJToken["ID"].ToString();
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
