namespace ci_automation_apitester.ApiDto.TenantMetadata
{
    public class TenantOrgs : BaseRestApi
    {
        const string Endpoint = "/api/TenantOrgs";
        public Dto CurrentObject = new();

        public class Dto
        {
            [RequestTest("PUT", null, 400, "The Id field is required", "Omit Id Key")]
            [RequestTest("PUT", "", 500, "Value cannot be null. (Parameter 'id')", "Blank Id Key")]
            [UrlTest("DELETE", "", 405, "", "")]
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling tenant org mapping", "OrgId Doesn't Exist")]
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "OrgId Doesn't Exist")]
            public string Id { get; set; }
            [RequestTest("POST", null, 400, "The TenantId field is required", "Omit TenantId Key")]
            [RequestTest("POST", "", 422, "'Tenant Id' must not be empty", "Blank TenantId Key")]
            [RequestTest("PUT", null, 400, "The TenantId field is required", "Omit TenantId Key")]
            [RequestTest("PUT", "", 422, "'Tenant Id' must not be empty", "Blank TenantId Key")]
            public string TenantId { get; set; }
            [RequestTest("POST", null, 400, "The OrgId field is required", "Omit OrgId Key")]
            [RequestTest("POST", "", 422, "'Org Id' must not be empty", "Blank OrgId Key")]
            [RequestTest("PUT", "", 422, "'Org Id' must not be empty", "Blank OrgId Key")]
            [RequestTest("PUT", null, 400, "The OrgId field is required", "Omit OrgId Key")]
            public string OrgId { get; set; }
            public bool? Active { get; set; }
            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                TenantId = "9D6B9214-23E0-479E-846C-F6048C0C0058";          //SSB TenantId
                OrgId = "org_KlOYPcLaAzMYiwzx";                             //SSB OrgId
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
            CurrentObject.TenantId = "9D6B9214-23E0-479E-846C-F6048C0C0058";        //SSB TenantId
            CurrentObject.OrgId = "org_KlOYPcLaAzMYiwzx";                           //SSB OrgId
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
            TenantMetadataQueries query = new();
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.Url = GetUrl("GET");
            TestParams.RequestType = "GET";

            //Returns all Tenant Orgs in dbo.TenantOrgs (for GET /api/TenantOrgs)
            DataTable allTenantOrgsDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), query.QueryAllTenantOrgs(SecretsManager.SnowflakeDatabaseEnvironment()));
            
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
