namespace ci_automation_apitester.ApiDto.IntegrationMetadata
{
    public class TenantIntegrationConfigSalesforce: BaseRestApi
    {
        const string Endpoint = "/api/TenantIntegrationConfigSalesforce";
        public Dto CurrentObject = new();

        public class Dto
        {
            //TenantIntegrationId must be unique
            //Negative validation GETS for values that aren't a part of the DTO?  /api/TenantIntegrationConfigSalesforce/integrationid/{integrationId}

            [UrlTest("DELETE", "", 405, "", "")]
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling TenantIntegrationConfigSalesforce", "TenantIntegrationConfigSalesforce Doesn't Exist")]
            //[UrlTest("DELETE", "A4C1AE8D-0E63-45B6-B727-91CBDB357CFA", 204?, "Error disabling TenantIntegrationConfigSalesforce", "TenantIntegrationConfigSalesforce Casing Mismatch")]
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "TenantIntegrationConfigSalesforce Doesn't Exist")]
            //[UrlTest("GET", "A4C1AE8D-0E63-45B6-B727-91CBDB357CFA", 200?, "Sequence contains no elements", "TenantIntegrationConfigSalesforce Casing Mismatch")]
            [RequestTest("PUT", null, 500, "Required input Id was empty", "Omit TenantIntegrationConfigId Key")]
            [RequestTest("PUT", "", 500, "Required input Id was empty", "Blank TenantIntegrationConfigId Key")]
            public string Id { get; set; }
            [RequestTest("POST", null, 500, "Required input TenantIntegrationId was empty", "Omit TenantIntegrationId Key")]
            [RequestTest("POST", "", 500, "Required input TenantIntegrationId was empty", "Blank TenantIntegrationId Key")]
            [RequestTest("POST", "ec12fa5f-1147-485e-8c1f-15004f29c6de", 500, "TenantIntegrationConfigSalesforce already exists for TenantIntegration", "Duplicate TenantIntegrationId")]
            [RequestTest("PUT", null, 500, "Required input TenantIntegrationId was empty", "Omit TenantIntegrationId Key")]
            [RequestTest("PUT", "", 500, "Required input TenantIntegrationId was empty", "Blank TenantIntegrationId Key")]
            [RequestTest("PUT", "ec12fa5f-1147-485e-8c1f-15004f29c6de", 500, "Another TenantIntegrationConfigSalesforce already exists for TenantIntegrationId", "Duplicate TenantIntegrationId")]
            public string TenantIntegrationId { get; set; }
            [RequestTest("POST", null, 500, "Required input Username was empty", "Omit Username Key")]
            [RequestTest("POST", "", 500, "Required input Username was empty", "Blank Username Key")]
            [RequestTest("PUT", null, 500, "Required input Username was empty", "Omit Username Key")]
            [RequestTest("PUT", "", 500, "Required input Username was empty", "Blank Username Key")]
            public string Username { get; set; }
            [RequestTest("POST", null, 500, "Required input Password was empty", "Omit Password Key")]
            [RequestTest("POST", "", 500, "Required input Password was empty", "Blank Password Key")]
            [RequestTest("PUT", null, 500, "Required input Password was empty", "Omit Password Key")]
            [RequestTest("PUT", "", 500, "Required input Password was empty", "Blank Password Key")]
            public string Password { get; set; }
            [RequestTest("POST", null, 500, "Required input Token was empty", "Omit Token Key")]
            [RequestTest("POST", "", 500, "Required input Token was empty", "Blank Token Key")]
            [RequestTest("PUT", null, 500, "Required input Token was empty", "Omit Token Key")]
            [RequestTest("PUT", "", 500, "Required input Token was empty", "Blank Token Key")]
            public string Token { get; set; }
            [RequestTest("POST", null, 500, "Required input Url was empty", "Omit Url Key")]
            [RequestTest("POST", "", 500, "Required input Url was empty", "Blank Url Key")]
            [RequestTest("PUT", null, 500, "Required input Url was empty", "Omit Url Key")]
            [RequestTest("PUT", "", 500, "Required input Url was empty", "Blank Url Key")]
            public string Url { get; set; }
            public bool? Active { get; set; }
            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                TenantIntegrationId = "92b09cb7-bad7-4f3e-b740-f317e1e6f81c";      //Seeded, unique   
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
                case "GETIntegrationIdQuery":
                    return Endpoint + "/integrationid/";              //api/TenantIntegrationConfigSalesforce/integrationid/{integrationId}
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
            CurrentObject.TenantIntegrationId = "3bbe0917-94e4-485f-a4a4-2192b87e2eb6";           //Seeded, unique   
            CurrentObject.Username = "salesforceqa@affinaquest.com";
            CurrentObject.Password = "Test@123";
            CurrentObject.Token = "cNBkq8UJg0Qs1KiAXpA1e5cSR4";
            CurrentObject.Url = "https://qa.login.salesforce.com";
            CurrentObject.Active = false;
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
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.RequestType = "GET";

            DataTable allTenantIntegrationConfigSalesforceDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryAllTenantIntegrationConfigSalesforce(SecretsManager.SnowflakeDatabaseEnvironment()));

            //If dbo.TenantIntegrationConfig_Salesforce.Token = NULL, replace with ""
            foreach (DataRow row in allTenantIntegrationConfigSalesforceDt.Rows)
            {
                if (string.IsNullOrEmpty(row["Token"].ToString()))
                {
                    row["Token"] = "";
                }
            }

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

            //Returns specific Tenant Integration Config Salesforce in dbo.TenantIntegrationConfig_Salesforce by correlated IntegrationId in dbo.TenantIntegration (for GET /TenantIntegrationConfigSalesforce/{IntegrationId})
            DataTable integrationIdDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryDistinctTenantIntegrationIntegrationId(SecretsManager.SnowflakeDatabaseEnvironment()));
            foreach (DataRow row in integrationIdDt.Rows)
            {
                DataTable eachIntegrationIdDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryTenantIntegrationConfigSalesforceByIntegrationId(SecretsManager.SnowflakeDatabaseEnvironment(), row.Field<string>("IntegrationId")));
                JArray jArray = JArray.FromObject(eachIntegrationIdDt, JsonSerializer.CreateDefault());

                TestParams.TestStepName = GetType().Name + "_GET_TenantIntegrationConfigSalesforceByIntegrationId_" + row.Field<string>("IntegrationId");
                TestParams.Url = GetUrl("GETIntegrationIdQuery") + row.Field<string>("IntegrationId").ToString();
                string rowJson = jArray.ToString(Formatting.None);
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

