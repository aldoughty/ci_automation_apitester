namespace ci_automation_apitester.ApiDto.IntegrationMetadata
{
    public class TenantIntegrationConfigSalesforce: BaseRestApi
    {
        const string Endpoint = "/api/TenantIntegrationConfigSalesforce";
        public Dto CurrentObject = new();

        public class Dto
        {
            //POST need seeded:  
            //TenantIntegrationId must be unique

            [UrlTest("DELETE", "", 405, "", "")]
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling TenantIntegrationConfigSftp", "TenantIntegrationConfigId Doesn't Exist")]
            [UrlTest("DELETE", "0E670336-BDA1-47fA-BC93-FFD9249BEF92", 500, "Error disabling TenantIntegrationConfigSftp", "TenantIntegrationConfigId Casing Mismatch")]
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "TenantIntegrationConfigId Doesn't Exist")]
            [UrlTest("GET", "0E670336-BDA1-47fA-BC93-FFD9249BEF92", 404, "Sequence contains no elements", "TenantIntegrationConfigId Casing Mismatch")]
            [RequestTest("PUT", null, 400, "The Id field is required", "Omit TenantIntegrationConfigId Key")]
            [RequestTest("PUT", "", 500, "Value cannot be null. (Parameter 'id')", "Blank TenantIntegrationConfigId Key")]
            public string Id { get; set; }
            [RequestTest("POST", null, 500, "Value cannot be null. (Parameter 'TenantIntegrationId')", "Omit TenantIntegrationId Key")]
            [RequestTest("POST", "", 500, "Required input TenantIntegrationId was empty. (Parameter 'TenantIntegrationId')", "Blank TenantIntegrationId Key")]
            [RequestTest("POST", "6e0a5769-4147-4b75-81be-4818cc3edde2", 500, "TenantIntegration is not unique in TenantIntegrationConfigSftp", "Duplicate TenantIntegrationId")]
            [RequestTest("PUT", null, 400, "The TenantIntegrationId field is required", "Omit TenantIntegrationId Key")]
            //[RequestTest("PUT", "", ???, "???", "Blank TenantIntegrationId Key")]  //204
            public string TenantIntegrationId { get; set; }
            [RequestTest("POST", null, 500, "Value cannot be null. (Parameter 'Username')", "Omit Username Key")]
            [RequestTest("POST", "", 500, "Required input Username was empty. (Parameter 'Username')", "Blank Username Key")]
            //[RequestTest("POST", "99999999-9999-9999-9999-999999999999", 500, "???", "Username Doesn't Exist")] //404
            [RequestTest("POST", "73d3a3f0-0edb-11ed-8937-a9e9ce36e4dc", 500, "Username doesn't exist or is inactive", "Inactive Username")]
            [RequestTest("PUT", null, 400, "The Username field is required", "Omit Username Key")]
            //[RequestTest("PUT", "", ???, "???", "Blank Username Key")]  //204
            public string Username { get; set; }
            [RequestTest("POST", null, 400, "The Password field is required", "Omit Password Key")]
            //[RequestTest("POST", "", 201, "", "Blank Password Key")]  //201
            [RequestTest("PUT", null, 400, "The Password field is required", "Omit Password Key")]
            //[RequestTest("PUT", "", ???, "???", "Blank Password Key")]  //204
            public string Password { get; set; }
            [RequestTest("POST", null, 400, "The Token field is required", "Omit Token Key")]
            //[RequestTest("POST", "", 201, "???", "Blank Token Key")]  //201
            [RequestTest("PUT", null, 400, "The Token field is required", "Omit Token Key")]
            //[RequestTest("PUT", "", ???, "???", "Blank Token Key")]  //204
            public string Token { get; set; }
            [RequestTest("POST", null, 400, "The Url field is required", "Omit Url Key")]
            //[RequestTest("POST", "", 201, "???", "Blank Url Key")]  //201
            [RequestTest("PUT", null, 400, "The Url field is required", "Omit Url Key")]
            //[RequestTest("PUT", "", ???, "???", "Blank Url Key")]  //204
            public string Url { get; set; }
            public bool? Active { get; set; }
            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                TenantIntegrationId = "";                                       //need seeded   
                Username = "salesforceqa@affinaquest.com";                                                  
                Password = "Test@123";
                Token = "cNBkq8UJg0Qs1KiAXpA1e5cSR4";
                Url = "https://qa.login.salesforce.com";
                Active = true;
            }
        }
        public override string GetUrl(string action)
        {
            switch (action)
            {
                case "GET":                                             //Endpoint - returns all; Endpoint + "/Id" - returns specific
                case "POST":
                case "PUT":
                    return Endpoint;
                case "DELETE":
                    return Endpoint + "/" + CurrentObject.Id;
                case "GETQuery":
                    return Endpoint + "/";                              //api/TenantIntegrationConfigSalesforce/{id}
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
            CurrentObject.TenantIntegrationId = "";                                       //need seeded   
            CurrentObject.Username = "salesforceqa@affinaquest.com";
            CurrentObject.Password = "Test@123";
            CurrentObject.Token = "cNBkq8UJg0Qs1KiAXpA1e5cSR4";
            CurrentObject.Url = "https://qa.login.salesforce.com";
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

            DataTable allTenantIntegrationConfigSalesforceDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), query.QueryAllTenantIntegrationConfigSalesforce(SecretsManager.SnowflakeDatabaseEnvironment()));

            //Returns all Tenant Integration Config Salesforce in dbo.TenantIntegrationConfig_Salesforce (for GET /api/TenantIntegrationConfigSalesforce)
            string expectedGetAllResponseBody = JsonConvert.SerializeObject(allTenantIntegrationConfigSalesforceDt);
            TestParams.TestStepName = GetType().Name + "_GET_AllTenantIntegrationConfigSalesforce";
            TestParams.Url = GetUrl("GET");
            TestParams.ExpectedResponseBody = expectedGetAllResponseBody;
            testParamsList.Add(TestParams.Copy());

            //Returns specific Tenant Integration Config Salesforce in dbo.TenantIntegrationConfig_Salesforce (for GET /api/TenantIntegrationConfigSalesforce/{ID})
            for (int rowIndex = 0; rowIndex < allTenantIntegrationConfigSalesforceDt.Rows.Count; rowIndex++)
            {
                JArray jArray = JArray.FromObject(allTenantIntegrationConfigSalesforceDt, JsonSerializer.CreateDefault());

                JToken rowJToken = jArray[rowIndex];
                string rowJson = rowJToken.ToString(Formatting.None);

                TestParams.TestStepName = GetType().Name + "_GET_TenantIntegrationConfigSalesforceById_" + rowJToken["ID"];
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
            string command = $@"DELETE FROM CI_METADATA{SecretsManager.SnowflakeDatabaseEnvironment()}.DBO.TENANTINTEGRATIONCONFIG_SALESFORCE WHERE TENANTINTEGRATIONCONFIGID = '{currentId}'";
            DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), command);
        }
    }
}

