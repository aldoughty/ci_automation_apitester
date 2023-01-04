namespace ci_automation_apitester.ApiDto.TenantMetadata
{
    public class Tenants : BaseRestApi
    {
        const string Endpoint = "/api/Tenants";
        public Dto CurrentObject = new();

        public class Dto
        {
            [RequestTest("PUT", null, 400, "The Id field is required", "Omit Id Key")]
            [RequestTest("PUT", "", 500, "Required input Id was empty", "Blank Id Key")]
            [UrlTest("DELETE", "", 405, "", "")]
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling tenant", "Id Doesn't Exist")]
            [UrlTest("DELETE", "23A81509-909C-4037-AAE5-0E70C75ff14B", 500, "Error disabling tenant", "Id Casing Mismatch")]
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "Id Doesn't Exist")]
            [UrlTest("GET", "23A81509-909C-4037-AAE5-0E70C75ff14B", 404, "Sequence contains no elements", "Id Casing Mismatch")]
            public string Id { get; set; }
            [RequestTest("POST", null, 400, "The Key field is required", "Omit (Tenant)Key Key")]
            [RequestTest("POST", "", 422, "\\u0027Key\\u0027 must not be empty", "Blank (Tenant)Key Key")]
            [RequestTest("POST", "AB", 422, "The length of \\u0027Key\\u0027 must be at least 3 characters", "(Tenant)Key Minimum 3 Chars")]
            [RequestTest("POST", "abcdefghijklmnop", 422, "The length of \\u0027Key\\u0027 must be 15 characters or fewer", "(Tenant)Key Maximum 15 Chars")]
            [RequestTest("POST", "1ab", 422, "Must meet the tenant key requirements", "(Tenant)Key Must Start With Letter")]
            [RequestTest("POST", "ab!@#", 422, "Must meet the tenant key requirements", "(Tenant)Key No Special Char")]
            [RequestTest("POST", "abc def", 422, "Must meet the tenant key requirements", "(Tenant)Key No Spaces")]
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "Key Doesn't Exist")]
            [UrlTest("GET", "23A81509-909C-4037-AAE5-0E70C75ff14B", 404, "Sequence contains no elements", "Key Casing Mismatch")]
            //Auth0 Requirements:  Not Empty, Min 3 Char, Max 50 Char, Only Upper/Lower Alpha, Num 0-9, dash (-) and underscore(_) (no spaces or any other character)
            public string Key { get; set; }
            [RequestTest("POST", null, 400, "The Name field is required", "Omit Name Key")]
            [RequestTest("POST", "", 422, "Name: \\u0027Name\\u0027 must not be empty", "Blank Name Key")]
            [RequestTest("POST", "AB", 422, "The length of \\u0027Name\\u0027 must be at least 3 characters", "Name Minimum 3 Chars")]
            [RequestTest("POST", "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ", 422, "The length of \\u0027Name\\u0027 must be 100 characters or fewer", "Name Maximum 100 Chars")]
            [RequestTest("POST", "`~!@#$%^*()=_+[]{}|;:,./<>?", 422, "Name: Must meet the tenant name requirements", "Name Not Allowed Chars")]
            [RequestTest("PUT", null, 400, "The Name field is required", "Omit Name Key")]
            [RequestTest("PUT", "AB", 422, "The length of \\u0027Name\\u0027 must be at least 3 characters", "Name Minimum 3 Chars")]
            [RequestTest("PUT", "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ", 422, "The length of \\u0027Name\\u0027 must be 100 characters or fewer", "Name Maximum 100 Chars")]
            [RequestTest("PUT", "`~!@#$%^*()=_+[]{}|;:,./<>?", 422, "Name: Must meet the tenant name requirements", "Name Not Allowed Chars")]
            public string Name { get; set; }
            //[RequestTest("POST", null, 400, "The Url field is required", "Omit Url Key")]
            //[RequestTest("POST", "", 422, "Must meet the tenant url requirements", "Blank Url Key")]
            [RequestTest("POST", "myurl", 422, "Must meet the tenant url requirements", "Url Must Start With /")]
            [RequestTest("POST", "/myurl!@#", 422, "Must meet the tenant url requirements", "Url No Special Char")]
            [RequestTest("POST", "/myurl url", 422, "Must meet the tenant url requirements", "Url No Spaces")]
            [RequestTest("POST", "/uofqaurl", 500, "Tenant with url already exists", "Duplicate Url")]
            //[RequestTest("PUT", null, 400, "The Url field is required", "Omit Url Key")]
            //[RequestTest("PUT", "", 422, "Must meet the tenant url requirements", "Blank Url Key")]
            [RequestTest("PUT", "myurl", 422, "Must meet the tenant url requirements", "Url Must Start With /")]
            [RequestTest("PUT", "/myurl!@#", 422, "Must meet the tenant url requirements", "Url No Special Char")]
            [RequestTest("PUT", "/myurl url", 422, "Must meet the tenant url requirements", "Url No Spaces")]
            [RequestTest("PUT", "/uofqaurl", 500, "Tenant with url already exists", "Duplicate Url")]
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
                string timestamp = DateTime.Now.ToString("MMddyyHHmmss");

                Id = Guid.NewGuid().ToString().ToUpper();
                Key = "QA" + timestamp;                      //TenantKey on UI:  Required; Min 3 Max 50; AlphaNumeric Allowed; No Spec Char or Spaces
                Name = "QA" + timestamp;
                Url = "/QA" + timestamp;
                Type = "University";
                SubType = "SEC";
                ShortName = "QA" + timestamp;
                Nickname = "QA" + timestamp;
                Mascot = "QA" + timestamp;
                Active = true;
                OrchardName = "QA" + timestamp;
                OrchardType = "CI";
                IsDiscoveryClient = true;
                DiscoveryClientName = "QA" + timestamp;
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
            string timestamp = DateTime.Now.ToString("MMddyyyy");

            CurrentObject.Key = "QA" + timestamp;
            CurrentObject.Name = "QA" + timestamp;
            CurrentObject.Url = "/QA" + timestamp;
            CurrentObject.ShortName = "QA" + timestamp;
            CurrentObject.Nickname = "QA" + timestamp;
            CurrentObject.Mascot = "QA" + timestamp;
            CurrentObject.OrchardName = "QA" + timestamp;
            CurrentObject.DiscoveryClientName = "QA" + timestamp;

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

            DataTable allTenantsDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), TenantMetadataQueries.QueryAllTenants(SecretsManager.SnowflakeDatabaseEnvironment()));

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
