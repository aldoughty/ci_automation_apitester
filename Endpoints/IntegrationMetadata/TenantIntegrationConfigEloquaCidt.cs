namespace ci_automation_apitester.ApiDto.IntegrationMetadata
{
    public class TenantIntegrationConfigEloquaCIDT : BaseRestApi
    {
        const string Endpoint = "/api/TenantIntegrationConfigEloquaCidt";
        public Dto CurrentObject = new();

        //GET by TenantIntegrationId, Active/Inactive

        public class Dto
        {

            [RequestTest("PUT", null, 500, "Value cannot be null. (Parameter 'id')", "Omit Id Key")]
            [RequestTest("PUT", "", 500, "Value cannot be null. (Parameter 'id')", "Blank Id Key")]
            [UrlTest("DELETE", "", 405, "", "")]
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling TenantIntegration", "Id Doesn't Exist")]
            [UrlTest("DELETE", "9A4D0ACC-AF46-41A3-AA48-C209C0024528", 500, "Error disabling TenantIntegration", "Id Casing Mismatch")]
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "Id Doesn't Exist")]
            [UrlTest("GET", "9A4D0ACC-AF46-41A3-AA48-C209C0024528", 404, "Sequence contains no elements", "Id Casing Mismatch")]
            public string TenantIntegrationConfigId { get; set; }
            [RequestTest("POST", null, 500, "Required input tenantIntegrationId was empty. (Parameter 'tenantIntegrationId')", "Omit tenantIntegrationId Key")]
            [RequestTest("POST", "", 500, "Required input tenantIntegrationId was empty. (Parameter 'tenantIntegrationId')", "Blank tenantIntegrationId Key")]
            [RequestTest("POST", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "tenantIntegrationId Doesn't Exist")]
            [RequestTest("POST", "52419061-ACD4-4476-9CE3-7B7D78AC0243", 404, "Sequence contains no elements", "tenantIntegrationId Casing Mismatch")]
            [RequestTest("PUT", null, 500, "Error updating TenantIntegrationConfigEloquaCidt", "Omit tenantIntegrationId Key")]
            [RequestTest("PUT", "", 500, "Error updating TenantIntegrationConfigEloquaCidt", "Blank tenantIntegrationId Key")]
            public string TenantIntegrationId { get; set; }
            public string ClientId { get; set; }
            public string UserName { get; set; }
            public string ClientSecret { get; set; }
            public string Password { get; set; }
            public string SiteName { get; set; }
            public bool? Active { get; set; }
            public string StartMessageId { get; set; }
            public Dto()
            {
                TenantIntegrationConfigId = Guid.NewGuid().ToString().ToUpper();
                TenantIntegrationId = "";                                  
                ClientId = "ba0vdd1c-d600-n6GG-ad6d-1ba9b60tAc2n";
                ClientSecret = "RcDA7tAc2n6GGuwMzgEZEba0vz7gHNoYWEKLM";
                Password = "Test@123";
                SiteName = "UofQA";
                UserName = "qa@affinaquest.com";
                Active = true;
                StartMessageId = "2EBBC2F8-3638-4D88-938B-61F03C498F94";
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
                    return Endpoint + "/" + CurrentObject.TenantIntegrationConfigId;
                case "GETQuery":
                    return Endpoint + "/";         //api/TenantOrgs/{id}
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
            CurrentObject.TenantIntegrationConfigId = Guid.NewGuid().ToString().ToUpper();
            CurrentObject.TenantIntegrationId = "";
            CurrentObject.ClientId = "";
            CurrentObject.ClientSecret = "";
            CurrentObject.Password = "";
            CurrentObject.SiteName = "";
            CurrentObject.UserName = "";
            CurrentObject.Active = true;
            CurrentObject.StartMessageId = "";
            return JsonConvert.SerializeObject(CurrentObject);
        }
        public override string GetWorkingId()
        {
            return CurrentObject.TenantIntegrationConfigId;
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

            DataTable allTenantIntegrationConfigEloquaCidtsDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryAllTenantIntegrationConfigEloquaCIDT(SecretsManager.SnowflakeDatabaseEnvironment()));

            //Returns all Tenant Integration Config Eloqua CIDTs in dbo.TenantIntegrationConfig_Eloqua_Cidt (for GET /api/TenantIntegrationConfigEloquaCidt)
            string expectedGetAllResponseBody = JsonConvert.SerializeObject(allTenantIntegrationConfigEloquaCidtsDt);
            TestParams.TestStepName = GetType().Name + "_GET_AllTenantIntegrationConfigEloquaCidts";
            TestParams.Url = GetUrl("GET");
            TestParams.ExpectedResponseBody = expectedGetAllResponseBody;
            testParamsList.Add(TestParams.Copy());

            //Returns specific Tenant Integration Config Eloqua CIDT in dbo.TenantIntegrationConfig_Eloqua_CIDT (for GET /api/TenantIntegrationConfigEloquaCidt/{ID})
            for (int rowIndex = 0; rowIndex < allTenantIntegrationConfigEloquaCidtsDt.Rows.Count; rowIndex++)
            {
                JArray jArray = JArray.FromObject(allTenantIntegrationConfigEloquaCidtsDt, JsonSerializer.CreateDefault());

                JToken rowJToken = jArray[rowIndex];
                string rowJson = rowJToken.ToString(Formatting.None);

                //TestParams.TestStepName = GetType().Name + "_GET_TenantIntegrationConfigEloquaCidtById_" + rowJToken["ID"];
                //TestParams.Url = GetUrl("GETQuery") + rowJToken["ID"].ToString();
                TestParams.TestStepName = GetType().Name + "_GET_TenantIntegrationConfigEloquaCidtById_" + rowJToken["TENANTINTEGRATIONCONFIGID"];
                TestParams.Url = GetUrl("GETQuery") + rowJToken["TENANTINTEGRATIONCONFIGID"].ToString();
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
            string command = $@"DELETE FROM CI_METADATA{SecretsManager.SnowflakeDatabaseEnvironment()}.DBO.TENANTINTEGRATIONCONFIG_ELOQUA_CIDT WHERE TENANTINTEGRATIONCONFIGID = '{currentId}'";
            DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), command);
        }
    }
}
