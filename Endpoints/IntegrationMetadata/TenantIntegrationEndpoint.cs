namespace ci_automation_apitester.ApiDto.IntegrationMetadata
{
    public class TenantIntegrationEndpoint : BaseRestApi
    {
        const string Endpoint = "/api/TenantIntegrationEndpoint";
        public Dto CurrentObject = new();

        public class Dto
        {
            //These scenarios involve 2 properties?  Will need known, seeded combos.
            //[RequestTest("POST", "{\"integrationEndpointId\": \"???\", \"tenantIntegrationId\": \"???\"}", 500, "TenantIntegrationEndpoint already exists for this tenantIntegration and integrationEndpoint combination", "TenantIntegration & IntegrationEndpoint Combo Exists")]
            //[RequestTest("PUT", "{\"integrationEndpointId\": \"???\", \"tenantIntegrationId\": \"???\"}", 500, "TenantIntegrationEndpoint already exists for this tenantIntegration and integrationEndpoint combination", "TenantIntegration & IntegrationEndpoint Combo Exists")]
            //[RequestTest("PUT", "{\"integrationEndpointId\": \"???\", \"tenantIntegrationId\": \"???\"}", 500, "Invalid integrationEndpoint tenantIntegration combination", "dbo.TenantIntegration.IntegrationId != dbo.IntegrationEndpoints.IntegrationId")]

            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "TenantIntegrationEndpintId Doesn't Exist")]
            //[UrlTest("GET", "C528741B-BBB7-42A6-832A-7A2BD3AA3716", 404, "Sequence contains no elements", "TenantIntegrationEndpintID Casing Mismatch")] //returns 200? Bug 326
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling TenantIntegrationEndpoint", "TenantIntegrationEndpintId Doesn't Exist")]
            //[UrlTest("DELETE", "C528741B-BBB7-42A6-832A-7A2BD3AA3716", 500, "Error disabling tenant", "TenantIntegrationEndpintID Casing Mismatch")]  //returns 204? Bug 326
            [UrlTest("DELETE", "", 405, "", "TenantIntegrationEndpintId Method Not Allowed")]
            [RequestTest("PUT", null, 500, "Required input Id was empty", "Omit ID Key")]
            [RequestTest("PUT", "", 500, "Required input Id was empty", "Blank ID Key")]
            public string Id { get; set; }

            [RequestTest("POST", null, 500, "Value cannot be null", "Omit IntegrationEndpointId Key")]
            [RequestTest("POST", "", 500, "Required input IntegrationEndpointId was empty", "Blank IntegrationEndpointId Key")]
            //[RequestTest("POST", "7B723057-EAC1-4718-842E-08F1132300E5", 500, "Invalid integrationEndpoint tenantIntegration combination", "IntegrationEndpointId Casing Mismatch")]  //returns 201? Bug 326
            [RequestTest("POST", "99999999-9999-9999-9999-999999999999", 500, "Invalid integrationEndpoint tenantIntegration combination", "IntegrationEndpointId Doesn't Exist")]
            [RequestTest("PUT", null, 500, "Value cannot be null", "Omit IntegrationEndpointId Key")]
            [RequestTest("PUT", "", 500, "Required input IntegrationEndpointId was empty", "Blank IntegrationEndpointId Key")]
            public string IntegrationEndpointId { get; set; }
            
            [RequestTest("POST", null, 500, "Value cannot be null", "Omit TenantIntegrationId Key")]
            [RequestTest("POST", "", 500, "Required input TenantIntegrationId was empty", "Blank TenantIntegrationId Key")]
            [RequestTest("POST", "1EEE24E6-4DB0-492D-86F8-5D08DDE943A1", 500, "Invalid integrationEndpoint tenantIntegration combination", "TenantIntegrationId Casing Mismatch")]
            [RequestTest("POST", "99999999-9999-9999-9999-999999999999", 500, "Invalid integrationEndpoint tenantIntegration combination", "TenantIntegrationId Doesn't Exist")]
            [RequestTest("PUT", null, 500, "Value cannot be null", "Omit TenantIntegrationId Key")]
            [RequestTest("PUT", "", 500, "Required input TenantIntegrationId was empty", "Blank TenantIntegrationId Key")]
            public string TenantIntegrationId { get; set; }

            public bool Active { get; set; }
            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                IntegrationEndpointId = "7b723057-eac1-4718-842e-08f1132300e5";         //Must be seeded TenantIntegrationId/IntegrationEndpointId pair that doesn't exist in dbo.TenantIntegrationEndpoint to POST
                TenantIntegrationId = "1eee24e6-4db0-492d-86f8-5d08dde943a1";           //Must be seeded TenantIntegrationId/IntegrationEndpointId pair that doesn't exist in dbo.TenantIntegrationEndpoint to POST
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
                    return Endpoint + "/";         //api/TenantIntegrationEndpoint/{id}
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
            CurrentObject.Id = "0430169d-9b50-4be8-b949-75b7a19a6fe4";                          //Seeded
            CurrentObject.IntegrationEndpointId = "7b723057-eac1-4718-842e-08f1132300e5";       //Seeded
            CurrentObject.TenantIntegrationId = "1eee24e6-4db0-492d-86f8-5d08dde943a1";         //Seeded
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

            DataTable allTenantIntegrationEndpointDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryAllTenantIntegrationEndpoint(SecretsManager.SnowflakeDatabaseEnvironment()));

            //Returns all Tenant Integration Endpoint in dbo.TenantIntegrationEndpoint (for GET /api/TenantIntegrationEndpoint)
            string expectedGetAllResponseBody = JsonConvert.SerializeObject(allTenantIntegrationEndpointDt);
            TestParams.TestStepName = GetType().Name + "_GET_AllTenantIntegrationEndpoint";
            TestParams.Url = GetUrl("GET");
            TestParams.ExpectedResponseBody = expectedGetAllResponseBody;
            testParamsList.Add(TestParams.Copy());

            //Returns specific Tenant Integration Endpoint in dbo.TenantIntegrationEndpoint (for GET /api/TenantIntegrationEndpoint/{TenantIntegrationEndpointId})
            for (int rowIndex = 0; rowIndex < allTenantIntegrationEndpointDt.Rows.Count; rowIndex++)
            {
                JArray jArray = JArray.FromObject(allTenantIntegrationEndpointDt, JsonSerializer.CreateDefault()); 

                JToken rowJToken = jArray[rowIndex];
                string rowJson = rowJToken.ToString(Formatting.None);

                TestParams.TestStepName = GetType().Name + "_GET_TenantIntegrationEndpointById_" + rowJToken["ID"];
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
            string command = $@"DELETE FROM CI_METADATA{SecretsManager.SnowflakeDatabaseEnvironment()}.DBO.TENANTINTEGRATIONENDPOINT WHERE TENANTINTEGRATIONENDPOINTID = '{currentId}'";
            DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), command);
        }
    }
}
