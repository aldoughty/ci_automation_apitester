namespace ci_automation_apitester.ApiDto.IntegrationMetadata
{
    public class TenantIntegrationConfigTM : BaseRestApi
    {
        const string Endpoint = "/api/TenantIntegrationConfigTM";
        public Dto CurrentObject = new();

        public class Dto
        {
            //POST need seeded:  
            //TenantIntegrationId must be unique

            [UrlTest("DELETE", "", 405, "", "")]
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling TenantIntegrationConfigTM", "TenantIntegrationConfigId Doesn't Exist")]
            [UrlTest("DELETE", "0E670336-BDA1-47fA-BC93-FFD9249BEF92", 500, "Error disabling TenantIntegrationConfigTM", "TenantIntegrationConfigId Casing Mismatch")]
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "TenantIntegrationConfigId Doesn't Exist")]
            [UrlTest("GET", "0E670336-BDA1-47fA-BC93-FFD9249BEF92", 404, "Sequence contains no elements", "TenantIntegrationConfigId Casing Mismatch")]
            [RequestTest("PUT", null, 400, "The Id field is required", "Omit TenantIntegrationConfigId Key")]
            [RequestTest("PUT", "", 500, "Value cannot be null. (Parameter 'id')", "Blank TenantIntegrationConfigId Key")]
            public string Id { get; set; }
            [RequestTest("POST", null, 500, "Value cannot be null. (Parameter 'TenantIntegrationId')", "Omit TenantIntegrationId Key")]
            [RequestTest("POST", "", 500, "Required input TenantIntegrationId was empty. (Parameter 'TenantIntegrationId')", "Blank TenantIntegrationId Key")]
            [RequestTest("POST", "6e0a5769-4147-4b75-81be-4818cc3edde2", 500, "TenantIntegration is not unique in TenantIntegrationConfigTM", "Duplicate TenantIntegrationId")]
            [RequestTest("PUT", null, 400, "The TenantIntegrationId field is required", "Omit TenantIntegrationId Key")]
            //[RequestTest("PUT", "", ???, "???", "Blank TenantIntegrationId Key")]  //204
            public string TenantIntegrationId { get; set; }
            public string TMDSN { get; set; }
            public string TMTEAM { get; set; }
            public bool? Active { get; set; }
            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                TenantIntegrationId = "";                          
                TMDSN = "";                                  
                TMTEAM = "";
                Active = true;
            }
        }
        public override string GetUrl(string action)
        {
            switch (action)
            {
                case "GET":                         //Endpoint - returns all; Endpoint + "/Id" - returns specific
                case "POST":
                case "PUT":
                    return Endpoint;
                case "DELETE":
                    return Endpoint + "/" + CurrentObject.Id;
                case "GETQuery":
                    return Endpoint + "/";                              //api/TenantIntegrationConfigTm/{id}
                case "AttributeUrlTest":
                    return Endpoint + "/";
                default: return Endpoint;
            }
        }
        public override List<string> GetRequestTypes()
        {
            List<string> actions = new()
            { "GET", "POST", "PUT", "DELETE" };

            return actions;
        }
        public override string GetPostBody()
        {
            return JsonConvert.SerializeObject(CurrentObject);
        }
        public override string GetPutBody()
        {
            CurrentObject.Id = Guid.NewGuid().ToString().ToUpper();
            CurrentObject.TenantIntegrationId = "";
            CurrentObject.TMDSN = "";
            CurrentObject.TMTEAM = "";
            CurrentObject.Active = true;
            return JsonConvert.SerializeObject(CurrentObject);
        }
        public override string GetWorkingId()
        {
            return CurrentObject.Id;
        }

        public override string GetWorkingBody()
        {
            return JsonConvert.SerializeObject(CurrentObject);

        }
        public override List<TestObjects.TestStep> GetParameterTests(MessageData messageData, string authValue, IEnvironment environment)
        {
            IntegrationMetadataQueries query = new();
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.RequestType = "GET";

            DataTable allTenantIntegrationConfigTMsDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), query.QueryAllTenantIntegrationConfigTM(SecretsManager.SnowflakeDatabaseEnvironment()));

            //Returns all Tenant Integration Config TMs in dbo.TenantIntegrationConfig_TM (for GET /api/TenantIntegrationConfigTM)
            string expectedGetAllResponseBody = JsonConvert.SerializeObject(allTenantIntegrationConfigTMsDt);
            TestParams.TestStepName = GetType().Name + "_GET_AllTenantIntegrationConfigTMs";
            TestParams.Url = GetUrl("GET");
            TestParams.ExpectedResponseBody = expectedGetAllResponseBody;
            testParamsList.Add(TestParams.Copy());

            //Returns specific Tenant Integration Config TM in dbo.TenantIntegrationConfig_TM (for GET /api/TenantIntegrationConfigTM/{Id})
            for (int rowIndex = 0; rowIndex < allTenantIntegrationConfigTMsDt.Rows.Count; rowIndex++)
            {
                JArray jArray = JArray.FromObject(allTenantIntegrationConfigTMsDt, JsonSerializer.CreateDefault());

                JToken rowJToken = jArray[rowIndex];
                string rowJson = rowJToken.ToString(Formatting.None);

                TestParams.TestStepName = GetType().Name + "_GET_TenantIntegrationConfigTMById_" + rowJToken["ID"];
                TestParams.Url = GetUrl("GETQuery") + rowJToken["ID"].ToString();
                TestParams.ExpectedResponseBody = rowJson;
                testParamsList.Add(TestParams.Copy());
            }

            return testParamsList;
        }
        public override List<RequestAttributeTest> GetRequestAttributeTests()
        {
            RequestAttributeTest attributeTest = new();
            List<RequestAttributeTest> attributeTestList = new();
            JsonSerializerSettings settings = new() { NullValueHandling = NullValueHandling.Ignore };  //if dto property = null, ignore/omit

            var requestProperties = typeof(Dto).GetProperties()
                             .Where(p => p.IsDefined(typeof(RequestTest), false));

            foreach (PropertyInfo property in requestProperties)
            {
                var attrs = property.GetCustomAttributes(typeof(RequestTest), false);
                foreach (RequestTest attr in attrs)
                {
                    //set DTO property
                    Dto requesDto = new();
                    string key = property.Name;
                    PropertyInfo prop = requesDto.GetType().GetProperty(key);
                    prop.GetValue(requesDto);
                    prop.SetValue(requesDto, attr.Request);
                    attr.Request = JsonConvert.SerializeObject(requesDto, settings);

                    attributeTestList.Add(attributeTest.Create(property.Name, attr.Action, attr.Request, attr.ResponseCode, attr.Message, attr.TestName).Copy());
                }
            }

            return attributeTestList;
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
        public override void CleanUp(MessageData messageData, string currentId)
        {
            string command = $@"DELETE FROM CI_METADATA{SecretsManager.SnowflakeDatabaseEnvironment()}.DBO.TENANTINTEGRATIONCONFIG_TM WHERE TENANTINTEGRATIONCONFIGID = '{currentId}'";
            DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), command);
        }
    }
}
