namespace ci_automation_apitester.ApiDto.IntegrationMetadata
{
    public class TenantIntegration : BaseRestApi
    {
        const string Endpoint = "/api/TenantIntegration";
        public Dto CurrentObject = new();

        public class Dto
        {
            //Integration and Tenant has to exist and Tenant/Integration combo has to be unique; both Active/Inactive bc both are included in exists

            //This scenario involves 2 properties?
            //[RequestTest("POST", "{\"tenantId\": \"b011c622-9a2d-4279-95b2-4fc54755124f\", \"integrationId\": \"9e4dd5e0-669c-4e42-ae66-fb441d047ab2\"}", 500, "TenantIntegrationId already exists for this tenant and integration combination", "Duplicate Tenant & Integration Combo")]

            [RequestTest("PUT", null, 500, "Required input Id was empty", "Omit Id Key")]
            [RequestTest("PUT", "", 500, "Required input Id was empty", "Blank Id Key")]
            [UrlTest("DELETE", "", 405, "", "")]
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling TenantIntegration", "Id Doesn't Exist")]
            [UrlTest("DELETE", "5F1273E4-3CC5-4653-8871-206F33BFA141", 500, "Error disabling TenantIntegration", "Id Casing Mismatch")]
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "Id Doesn't Exist")]
            [UrlTest("GET", "5F1273E4-3CC5-4653-8871-206F33BFA141", 404, "Sequence contains no elements", "Id Casing Mismatch")]
            public string Id { get; set; }
            
            //Able to create duplicate tenantId/integrationId pair with casing mismatch of tenantId; no validation for tenantId doesn't exist?  BUG 287
            [RequestTest("POST", null, 500, "Required input TenantId was empty", "Omit TenantId Key")]
            [RequestTest("POST", "", 500, "Required input TenantId was empty", "Blank TenantId Key")]
            [RequestTest("PUT", null, 500, "Required input TenantId was empty", "Omit TenantId Key")]
            [RequestTest("PUT", "", 500, "Required input TenantId was empty", "Blank TenantId Key")]
            public string TenantId { get; set; }
            
            [RequestTest("POST", null, 500, "Required input IntegrationId was empty", "Omit IntegrationId Key")]
            [RequestTest("POST", "", 500, "Required input IntegrationId was empty", "Blank IntegrationId Key")]
            [RequestTest("POST", "DDFEB532-154E-4AD5-BA0A-691AAD8D504A", 404, "Sequence contains no elements", "IntegrationId Casing Mismatch")]
            [RequestTest("POST", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "IntegrationId Doesnt' Exist")]
            [RequestTest("PUT", null, 500, "Required input IntegrationId was empty", "Omit IntegrationId Key")]
            [RequestTest("PUT", "", 500, "Required input IntegrationId was empty", "Blank IntegrationId Key")]
            public string IntegrationId { get; set; }
            public string SyncJob { get; set; }
            public bool Active { get; set; }
            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                TenantId = "ba27a0d7-4952-4279-9edc-7565b8aeb3a9";              //Must be seeded TenantId/IntegrationId pair that doesn't exist in dbo.TenantIntegration to POST
                IntegrationId = "b2d7c8aa-2d7c-46af-b1c9-3353537ef02a";         //Must be seeded TenantId/IntegrationId pair that doesn't exist in dbo.TenantIntegration to POST
                SyncJob = "QA.Sync.Testing";
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
                    return Endpoint + "/";         //api/TenantIntegration/{id}
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
            CurrentObject.Id = "5f1273e4-3cc5-4653-8871-206f33bfa141";                                          //Seeded
            CurrentObject.TenantId = "ba27a0d7-4952-4279-9edc-7565b8aeb3a9";                                    //Seeded
            CurrentObject.IntegrationId = "b2d7c8aa-2d7c-46af-b1c9-3353537ef02a";                               //Seeded
            CurrentObject.SyncJob = "UofQA.Sync.Update.PUT";
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

            DataTable allTenantIntegrationDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryAllTenantIntegration(SecretsManager.SnowflakeDatabaseEnvironment()));

            //Returns all Tenant Integration in dbo.TenantIntegration (for GET /api/TenantIntegration)
            string expectedGetAllResponseBody = JsonConvert.SerializeObject(allTenantIntegrationDt);
            TestParams.TestStepName = GetType().Name + "_GET_AllTenantIntegration";
            TestParams.Url = GetUrl("GET");
            TestParams.ExpectedResponseBody = expectedGetAllResponseBody;
            testParamsList.Add(TestParams.Copy());

            //Returns specific Tenant Integration in dbo.TenantIntegration (for GET /api/TenantIntegration/{TENANTINTEGRATIONID})
            for (int rowIndex = 0; rowIndex < allTenantIntegrationDt.Rows.Count; rowIndex++)
            {
                JArray jArray = JArray.FromObject(allTenantIntegrationDt, JsonSerializer.CreateDefault());

                JToken rowJToken = jArray[rowIndex];
                string rowJson = rowJToken.ToString(Formatting.None);

                TestParams.TestStepName = GetType().Name + "_GET_TenantIntegrationById_" + rowJToken["ID"];
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
            JsonSerializerSettings settings = new() { NullValueHandling = NullValueHandling.Ignore };  //if dto property = null, ignore

            IEnumerable<PropertyInfo> requestProperties = typeof(Dto).GetProperties()
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
            string command = $@"DELETE FROM CI_METADATA{SecretsManager.SnowflakeDatabaseEnvironment()}.DBO.TENANTINTEGRATION WHERE TENANTINTEGRATIONID = '{currentId}'";
            DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), command);
        }
    }
}
