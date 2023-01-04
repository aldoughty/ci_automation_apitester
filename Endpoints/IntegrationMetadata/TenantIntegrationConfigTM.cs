namespace ci_automation_apitester.ApiDto.IntegrationMetadata
{
    public class TenantIntegrationConfigTM : BaseRestApi
    {
        const string Endpoint = "/api/TenantIntegrationConfigTm";
        public Dto CurrentObject = new();

        public class Dto
        { 
            //TenantIntegrationId must be unique

            [UrlTest("DELETE", "", 405, "", "")]
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 404, "Not Found", "TenantIntegrationConfigId Doesn't Exist")]
            //[UrlTest("DELETE", "EDD41F40-70A1-4396-825D-D6EAF7446D58", 500, "Error disabling TenantIntegrationConfigTM", "TenantIntegrationConfigId Casing Mismatch")]  returns 204
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "TenantIntegrationConfigId Doesn't Exist")]
            //[UrlTest("GET", "EDD41F40-70A1-4396-825D-D6EAF7446D58", 404, "Sequence contains no elements", "TenantIntegrationConfigId Casing Mismatch")]  returns 200
            [RequestTest("PUT", null, 404, "Not Found", "Omit TenantIntegrationConfigId Key")]
            [RequestTest("PUT", "", 404, "Not Found", "Blank TenantIntegrationConfigId Key")]
            public string Id { get; set; }
            [RequestTest("POST", null, 500, "Required input TenantIntegrationId was empty", "Omit TenantIntegrationId Key")]
            [RequestTest("POST", "", 500, "Required input TenantIntegrationId was empty", "Blank TenantIntegrationId Key")]
            [RequestTest("POST", "abb76836-e565-4236-8446-54778f37ac7b", 500, "TenantIntegration is not unique in TenantIntegrationConfigTm", "Duplicate TenantIntegrationId")]
            [RequestTest("POST", "99999999-9999-9999-9999-999999999999", 500, "TenantIntegration doesn\\u0027t exist or is inactive", "TenantIntegrationId Doesn't Exist")]
            [RequestTest("POST", "ee2e654d-e923-48e0-82cd-4d9e1245b25a", 500, "TenantIntegration doesn\\u0027t exist or is inactive", "TenantIntegrationId Is Inactive")] //returns 201
            [RequestTest("PUT", null, 404, "Not Found", "Omit TenantIntegrationId Key")]
            [RequestTest("PUT", "", 404, "Not Found", "Blank TenantIntegrationId Key")]
            public string TenantIntegrationId { get; set; }
            [RequestTest("POST", null, 500, "Required input TmDsn was empty", "Omit TmDsn Key")]
            [RequestTest("POST", "", 500, "Required input TmDsn was empty", "Blank TmDsn Key")]
            [RequestTest("PUT", null, 404, "Not Found", "Omit TmDsn Key")]
            [RequestTest("PUT", "", 404, "Not Found", "Blank TmDsn Key")]
            public string TmDsn { get; set; }
            [RequestTest("POST", null, 500, "Required input TmTeam was empty", "Omit TmTeam Key")]
            [RequestTest("POST", "", 500, "Required input TmTeam was empty", "Blank TmTeam Key")]
            [RequestTest("PUT", null, 404, "Not Found", "Omit TmTeam Key")]
            [RequestTest("PUT", "", 404, "Not Found", "Blank TmTeam Key")]
            public string TmTeam { get; set; }
            public bool? Active { get; set; }
            public Dto()
            {
                string timestamp = DateTime.Now.ToString("MMddyyHHmmss");

                Id = Guid.NewGuid().ToString().ToUpper();
                TenantIntegrationId = "e2690575-95bf-4892-9a2b-7219c2489019";  //Seeded, unique                          
                TmDsn = "uofqa" + timestamp;                                  
                TmTeam = "uofqa" + timestamp;
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
            string timestamp = DateTime.Now.ToString("MMddyyHHmmss");

            CurrentObject.Id = Guid.NewGuid().ToString().ToUpper();
            CurrentObject.TenantIntegrationId = "e2690575-95bf-4892-9a2b-7219c2489019";  //Seeded, unique
            CurrentObject.TmDsn = "uofqa" + timestamp;
            CurrentObject.TmTeam = "uofqa" + timestamp;
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

            DataTable allTenantIntegrationConfigTMsDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryAllTenantIntegrationConfigTM(SecretsManager.SnowflakeDatabaseEnvironment()));

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
