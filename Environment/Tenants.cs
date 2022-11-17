namespace ci_automation_apitester.ApiDto.TenantMetadata
{
    public class Tenants : BaseRestApi
    {
        const string Endpoint = "/api/Tenants";
        public Dto CurrentObject = new();

        public class Dto
        {
            //bugs open for PUT scenarios:
            //min, max, special char, spaces on key
            //casing mismatch on id/key
            //blank/omitted key
            //blank name (nulls db)
            //add GET/PUT scenarios for above when we get above ironed out
            //Per Kristine:  Need to validate uniqueness of Key and Url 10.24.22 (PUT/POST)

            [RequestTest("PUT", null, 400, "The Id field is required", "Omit Id Key")]
            [RequestTest("PUT", "", 500, "Value cannot be null. (Parameter 'id')", "Blank Id Key")]
            [UrlTest("DELETE", "", 405, "", "")]
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling tenant", "Id Doesn't Exist")]
            [UrlTest("DELETE", "23A81509-909C-4037-AAE5-0E70C75ff14B", 500, "Error disabling tenant", "Id Casing Mismatch")]
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "Id Doesn't Exist")]
            [UrlTest("GET", "23A81509-909C-4037-AAE5-0E70C75ff14B", 404, "Sequence contains no elements", "Id Casing Mismatch")]
            public string Id { get; set; }
            [RequestTest("POST", null, 400, "The Key field is required", "Omit (Tenant)Key Key")]
            [RequestTest("POST", "", 422, "'Key' must not be empty", "Blank (Tenant)Key Key")]
            [RequestTest("POST", "AB", 422, "The length of 'Key' must be at least 3 characters", "(Tenant)Key < 3 Chars")]
            [RequestTest("POST", "QWERTYUIOPASDFGHJKLZXCVBNMQWERTYUIOPASDFGHJKLZXCVBNM", 422, "The length of 'Key' must be 50 characters or fewer", "(Tenant)Key > 50 Chars")]
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "Key Doesn't Exist")]
            [UrlTest("GET", "23A81509-909C-4037-AAE5-0E70C75ff14B", 404, "Sequence contains no elements", "Key Casing Mismatch")]
            //Auth0 Requirements:  Not Empty, Min 3 Char, Max 50 Char, Only Upper/Lower Alpha, Num 0-9, dash (-) and underscore(_) (no spaces or any other character)
            public string Key { get; set; }
            [RequestTest("POST", null, 400, "The Name field is required", "Omit Name Key")]
            [RequestTest("POST", "", 422, "'Name' must not be empty", "Blank Name Key")]
            [RequestTest("PUT", null, 400, "The Name field is required", "Omit Name Key")]
            public string Name { get; set; }
            public string Url { get; set; }
            public string Type { get; set; }
            public string SubType { get; set; }
            public string ShortName { get; set; }
            public string Nickname { get; set; }
            public string Mascot { get; set; }
            public bool? Active { get; set; }
            public string OrchardName { get; set; }
            public string OrchardType { get; set; }
            public bool IsDiscoveryClient { get; set; }
            public string DiscoveryClientName { get; set; }
            public bool IsCentralIntelligenceClient { get; set; }
            public string ParentTenantId { get; set; }
            public Dto()
            {
                string timestamp = DateTime.Now.ToString("MMddyyyyHHmmss");

                Id = Guid.NewGuid().ToString().ToUpper();
                Key = "UofQA" + timestamp;                      //TenantKey on UI:  Required; Min 3 Max 50; AlphaNumeric Allowed; No Spec Char or Spaces
                Name = "UofQA" + timestamp;
                Url = "/UofQA" + timestamp;
                Type = "University";
                SubType = "SEC";
                ShortName = "UofQA" + timestamp;
                Nickname = "UofQA" + timestamp;
                Mascot = "UofQA" + timestamp;
                Active = true;
                OrchardName = "UofQA" + timestamp;
                OrchardType = "CI";
                IsDiscoveryClient = true;
                DiscoveryClientName = "UofQA" + timestamp;
                IsCentralIntelligenceClient = true;
                ParentTenantId = Guid.NewGuid().ToString().ToUpper();
            }
        }
        public override string GetUrl(string action)
        {
            switch (action)
            {
                case "GET":                         //Endpoint - returns all; Endpoint + "/{ID or KEY}" - returns specific
                case "POST":
                case "PUT":
                    return Endpoint;
                case "DELETE":
                    return Endpoint + "/" + CurrentObject.Id;
                case "GETIdQuery":
                    return Endpoint + "/";              //api/Tenants/{ID}
                case "GETKeyQuery":
                    return Endpoint + "/key/";         //api/Tenants/key/{KEY}
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
            string timestamp = DateTime.Now.ToString("MMddyyyyHHmmss");

            CurrentObject.Key = "UofQA" + timestamp;
            CurrentObject.Name = "UofQA" + timestamp;
            CurrentObject.Url = "/UofQA" + timestamp;
            CurrentObject.ShortName = "UofQA" + timestamp;
            CurrentObject.Nickname = "UofQA" + timestamp;
            CurrentObject.Mascot = "UofQA" + timestamp;
            CurrentObject.OrchardName = "UofQA" + timestamp;
            CurrentObject.DiscoveryClientName = "UofQA" + timestamp;

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
            TenantMetadataQueries query = new();
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.RequestType = "GET";

            DataTable allTenantsDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), query.QueryAllTenants(SecretsManager.SnowflakeDatabaseEnvironment()));

            //Returns all Tenants in dbo.Tenant (for GET /api/tenants)
            string expectedGetAllResponseBody = JsonConvert.SerializeObject(allTenantsDt);
            TestParams.TestStepName = GetType().Name + "_GET_AllTenants";
            TestParams.Url = GetUrl("GET");
            TestParams.ExpectedResponseBody = expectedGetAllResponseBody;
            testParamsList.Add(TestParams.Copy());

            //Returns specific Tenant in dbo.Tenant by TenantId for GET /api/tenants/{tenantId}
            for (int rowIndex = 0; rowIndex < allTenantsDt.Rows.Count; rowIndex++)
            {
                JArray jArray = JArray.FromObject(allTenantsDt, JsonSerializer.CreateDefault());

                JToken rowJToken = jArray[rowIndex];
                string rowJson = rowJToken.ToString(Formatting.None);

                TestParams.TestStepName = GetType().Name + "_GET_TenantByDistinctTenantId_" + rowJToken["ID"];
                TestParams.Url = GetUrl("GETIdQuery") + rowJToken["ID"].ToString();
                TestParams.ExpectedResponseBody = rowJson;
                testParamsList.Add(TestParams.Copy());
            }

            //Returns specific Tenant in dbo.Tenant by TenantKey for GET /api/tenants/key/{tenantKey}
            for (int rowIndex = 0; rowIndex < allTenantsDt.Rows.Count; rowIndex++)
            {
                JArray jArray = JArray.FromObject(allTenantsDt, JsonSerializer.CreateDefault());

                JToken rowJToken = jArray[rowIndex];
                string rowJson = rowJToken.ToString(Formatting.None);

                TestParams.TestStepName = GetType().Name + "_GET_TenantByDistinctTenantKey_" + rowJToken["KEY"];
                TestParams.Url = GetUrl("GETKeyQuery") + rowJToken["KEY"].ToString();
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
            string command = $@"DELETE FROM CI_METADATA{SecretsManager.SnowflakeDatabaseEnvironment()}.dbo.Tenant WHERE tenantId = '{currentId}'";
            DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), command);
        }
    }
}
