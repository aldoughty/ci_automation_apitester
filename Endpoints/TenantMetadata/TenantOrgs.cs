namespace ci_automation_apitester.ApiDto.TenantMetadata
{
    public class TenantOrgs : BaseRestApi
    {
        const string Endpoint = "/api/TenantOrgs";
        public Dto CurrentObject = new();

        //POST OrgId must meet Auth0 Name requirements if Auth0 Launch Darkly flag is enabled

        //This scenario involves 2 properties?
        //[RequestTest("POST", "{\"tenantId\": \"9d6b9214-23e0-479e-846c-f6048c0c0058\", \"orgId\": \"org_KlOYPcLaAzMYiwzx\"}", 500, "TenantOrg already exists for TenantId 9d6b9214-23e0-479e-846c-f6048c0c0058 and OrgId org_KlOYPcLaAzMYiwzx", "Duplicate Tenant & Org Combo")]
        //[RequestTest("PUT", "{\"tenantId\": \"9d6b9214-23e0-479e-846c-f6048c0c0058\", \"orgId\": \"org_KlOYPcLaAzMYiwzx\"}", 500, "Another TenantOrg already exists for TenantId 9d6b9214-23e0-479e-846c-f6048c0c0058 and OrgId org_KlOYPcLaAzMYiwzx", "Duplicate Tenant & Org Combo")]
        
        public class Dto
        {
            [RequestTest("PUT", null, 500, "Required input Id was empty", "Omit Id Key")]
            [RequestTest("PUT", "", 500, "Required input Id was empty", "Blank Id Key")]
            [RequestTest("PUT", "99999999-9999-9999-9999-999999999999", 500, "TenantOrg with Id 99999999-9999-9999-9999-999999999999 does not exist", "Id Doesn't Exist")]
            [UrlTest("DELETE", "", 405, "", "")]
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling tenant org mapping", "OrgId Doesn't Exist")]
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "OrgId Doesn't Exist")]
            public string Id { get; set; }
            [RequestTest("POST", null, 422, "\\u0027Tenant Id\\u0027 must not be empty", "Omit TenantId Key")]
            [RequestTest("POST", "", 422, "\\u0027Tenant Id\\u0027 must not be empty", "Blank TenantId Key")]
            [RequestTest("POST", "non guid", 500, "TenantId is formatted incorrectly or does not exist", "Non GUID TenantId")]
            [RequestTest("POST", "99999999-9999-9999-9999-999999999999", 500, "TenantId is formatted incorrectly or does not exist", "TenantId Doesn't Exist")]
            [RequestTest("PUT", null, 422, "\\u0027Tenant Id\\u0027 must not be empty", "Omit TenantId Key")]
            [RequestTest("PUT", "", 422, "\\u0027Tenant Id\\u0027 must not be empty", "Blank TenantId Key")]
            [RequestTest("PUT", "non guid", 500, "TenantId is formatted incorrectly or does not exist", "Non GUID TenantId")]
            [RequestTest("PUT", "99999999-9999-9999-9999-999999999999", 500, "TenantId is formatted incorrectly or does not exist", "TenantId Doesn't Exist")]
            public string TenantId { get; set; }
            [RequestTest("POST", null, 422, "\\u0027Org Id\\u0027 must not be empty", "Omit OrgId Key")]
            [RequestTest("POST", "", 422, "\\u0027Org Id\\u0027 must not be empty", "Blank OrgId Key")]
            [RequestTest("POST", "ab", 422, "The length of \\u0027Org Id\\u0027 must be at least 3 characters", "OrgId Minimum 3 Chars")]
            [RequestTest("POST", "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ", 422, "The length of \\u0027Org Id\\u0027 must be 50 characters or fewer", "OrgId Maximum 50 Chars")]
            [RequestTest("POST", "`~!@#$%^*()=+[]{}|;:,./<>?", 422, "OrgId: Must meet the Auth0 Org_name requirements", "OrgId No Special Char Except _-")]
            [RequestTest("POST", "ABC DEF", 422, "OrgId: Must meet the Auth0 Org_name requirements", "OrgId No Spaces")]
            [RequestTest("PUT", "", 422, "\\u0027Org Id\\u0027 must not be empty", "Blank OrgId Key")]
            [RequestTest("PUT", null, 422, "\\u0027Org Id\\u0027 must not be empty", "Omit OrgId Key")]
            [RequestTest("PUT", "ab", 422, "The length of \\u0027Org Id\\u0027 must be at least 3 characters", "OrgId Minimum 3 Chars")]
            [RequestTest("PUT", "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ", 422, "The length of \\u0027Org Id\\u0027 must be 50 characters or fewer", "OrgId Maximum 50 Chars")]
            [RequestTest("PUT", "`~!@#$%^*()=+[]{}|;:,./<>?", 422, "OrgId: Must meet the Auth0 Org_name requirements", "OrgId No Special Char Except _-")]
            [RequestTest("PUT", "ABC DEF", 422, "OrgId: Must meet the Auth0 Org_name requirements", "OrgId No Spaces")]
            public string OrgId { get; set; }
            public bool? Active { get; set; }
            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                TenantId = "4d4a68cf-6082-4916-8e79-5d8897df7608";          //Seeded TenantId
                OrgId = "org_UofQAcLaAzMYiwzx";                             
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
            CurrentObject.Id = Guid.NewGuid().ToString().ToUpper();
            CurrentObject.TenantId = "4d4a68cf-6082-4916-8e79-5d8897df7608";        //Seeded TenantId
            CurrentObject.OrgId = "org_UofQAcLaAzMYiwzx";
            CurrentObject.Active = false;

            return JsonConvert.SerializeObject(CurrentObject);
        }
        public override string GetWorkingId()
        {
            return CurrentObject.TenantId;
        }
        public override string GetWorkingBody()
        {
            return JsonConvert.SerializeObject(CurrentObject);
        }
        public override List<TestObjects.TestStep> GetParameterTests(MessageData messageData, string authValue, IEnvironment environment)
        {
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.Url = GetUrl("GET");
            TestParams.RequestType = "GET";

            //Returns all Tenant Orgs in dbo.TenantOrgs (for GET /api/TenantOrgs)
            DataTable allTenantOrgsDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), TenantMetadataQueries.QueryAllTenantOrgs(SecretsManager.SnowflakeDatabaseEnvironment()));
            
            string expectedGetAllResponseBody = JsonConvert.SerializeObject(allTenantOrgsDt);
            TestParams.TestStepName = GetType().Name + "_GET_AllTenantOrgs";
            TestParams.ExpectedResponseBody = expectedGetAllResponseBody;
            testParamsList.Add(TestParams.Copy());

            //Returns each Id in dbo.TenantOrgs (for GET /api/TenantOrgs/{Id})
            for (int rowIndex = 0; rowIndex < allTenantOrgsDt.Rows.Count; rowIndex++)
            {
                JArray jArray = JArray.FromObject(allTenantOrgsDt, JsonSerializer.CreateDefault());

                JToken rowJToken = jArray[rowIndex];
                string rowJson = rowJToken.ToString(Formatting.None);

                TestParams.TestStepName = GetType().Name + "_GET_TenantOrgId_" + rowJToken["ID"];
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
            string command = $@"DELETE FROM CI_METADATA{SecretsManager.SnowflakeDatabaseEnvironment()}.dbo.TenantOrg WHERE Id = '{currentId}'";
            DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), command);
        }
    }
}
