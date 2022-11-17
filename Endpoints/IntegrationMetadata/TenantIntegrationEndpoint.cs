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
            //[RequestTest("PUT", "{\"integrationEndpointId\": \"???\", \"tenantIntegrationId\": \"???\"}", 500, "Invalid integrationEndpoint tenantIntegration combination", "dbo.TenantIntegration.IntegrationId != dbo.IntegrationEndpoint.IntegrationId")]

            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "TenantIntegrationEndpintID Doesn't Exist")]
            //[UrlTest("GET", "009C01DA-F9E5-4691-A622-33CF84958320", 404, "Sequence contains no elements", "TenantIntegrationEndpintID Casing Mismatch")] //returns 200?
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling TenantIntegrationEndpoint", "TenantIntegrationEndpintID Doesn't Exist")]
            //[UrlTest("DELETE", "009C01DA-F9E5-4691-A622-33CF84958320", 500, "Error disabling tenant", "TenantIntegrationEndpintID Casing Mismatch")]  //returns 204?
            [UrlTest("DELETE", "", 405, "", "TenantIntegrationEndpintI Method Not Allowed")]
            [RequestTest("PUT", null, 400, "The Id field is required", "Omit ID Key")]
            [RequestTest("PUT", "", 500, "Required input Id was empty. (Parameter 'Id')", "Blank ID Key")]
            public string Id { get; set; }

            [RequestTest("POST", null, 500, "Value cannot be null. (Parameter 'IntegrationEndpointId')", "Omit IntegrationEndpointId Key")]
            [RequestTest("POST", "", 500, "IntegrationEndpointId or TenantIntegrationId is not valid", "Blank IntegrationEndpointId Key")]
            [RequestTest("POST", "B011C622-9A2D-4279-95B2-4fC54755124F", 500, "Invalid integrationEndpoint tenantIntegration combination", "IntegrationEndpointId Key Casing Mismatch")] //casing mismatch or doesn't exist
            [RequestTest("PUT", null, 500, "Value cannot be null. (Parameter 'IntegrationEndpointId')", "Omit IntegrationEndpointId Key")]
            [RequestTest("PUT", "", 500, "IntegrationEndpointId or TenantIntegrationId is formatted incorrectly", "Blank IntegrationEndpointId Key")]
            public string IntegrationEndpointId { get; set; }
            
            [RequestTest("POST", null, 500, "Value cannot be null. (Parameter 'TenantIntegrationId')", "Omit IntegrationEndpointId Key")]
            [RequestTest("POST", "", 500, "IntegrationEndpointId or TenantIntegrationId is not valid", "Blank IntegrationEndpointId Key")]
            [RequestTest("POST", "9e4dd5e0-669c-4e42-ae66-fb441d047ab2", 500, "Invalid integrationEndpoint tenantIntegration combination", "TenantEndpointId Key Casing Mismatch")] //casing mismatch or doesn't exist
            [RequestTest("PUT", null, 500, "Value cannot be null. (Parameter 'TenantIntegrationId')", "Omit TenantIntegrationId Key")]
            [RequestTest("PUT", "", 500, "IntegrationEndpointId or TenantIntegrationId is formatted incorrectly", "Blank TenantIntegrationId Key")]
            public string TenantIntegrationId { get; set; }

            public bool Active { get; set; }
            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                IntegrationEndpointId = "";                                 //Change these values (GUID?)
                TenantIntegrationId = "";                                   //Change these values (GUID?)
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
            CurrentObject.IntegrationEndpointId = "";                               //Change these values (GUID?, Known Seeded)
            CurrentObject.TenantIntegrationId = "";                                 //Change these values (GUID?, Known Seeded)
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

            DataTable allTenantIntegrationEndpointDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), query.QueryAllTenantIntegrationEndpoint(SecretsManager.SnowflakeDatabaseEnvironment()));

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
