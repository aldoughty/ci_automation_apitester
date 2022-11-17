namespace ci_automation_apitester.ApiDto.IntegrationMetadata   
{
    public class TenantSFTP : BaseRestApi
    {
        const string Endpoint = "/api/TenantSftp";
        public Dto CurrentObject = new();
        
        //GET by TenantId, Active/Inactive

        public class Dto
        {
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "IntegrationId Doesn't Exist")]
            //[UrlTest("GET", "0F62F8B5-79C2-44FF-B734-A08B5E8E1E2F", 404, "Sequence contains no elements", "IntegrationId Casing Mismatch")]  //returns 201?
            [RequestTest("PUT", null, 500, "Value cannot be null. (Parameter 'Id')", "Omit Id Key")]
            [RequestTest("PUT", "", 500, "Value cannot be null. (Parameter 'Id')", "Blank Id Key")]
            [RequestTest("PUT", "B011C622-9A2D-4279-95B2-4fC54755124F", 500, "Error updating TenantSftp", "IntegrationId Doesn't Exist")] //casing mismatch or doesn't exist
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling tenant", "IntegrationId Doesn't Exist")]
            //[UrlTest("DELETE", "0F62F8B5-79C2-44FF-B734-A08B5E8E1E2F", 500, "Error disabling tenant", "IntegrationId Casing Mismatch")]  //returns 201?
            [UrlTest("DELETE", "", 405, "", "IntegrationId Method Not Allowed")]
            public string Id { get; set; }

            [RequestTest("POST", null, 400, "The TenantId field is required", "Omit TenantId Key")]
            [RequestTest("PUT", null, 500, "Missing type map configuration or unsupported mapping", "Omit TenantId Key_Bug 250")]  //Bug #250
            [RequestTest("PUT", "", 500, "Missing type map configuration or unsupported mapping.", "Blank TenantId Key_Bug 250")]  //Bug #250
            public string TenantId { get; set; }

            [RequestTest("POST", null, 400, "The Hostname field is required", "Omit Hostname Key")]
            [RequestTest("PUT", null, 400, "The Hostname field is required", "Omit Hostname Key")]
            [RequestTest("PUT", "", 500, "Missing type map configuration or unsupported mapping.", "Omit Hostname Key_Bug 250")]  //Bug #250
            public string Hostname { get; set; }

            [RequestTest("POST", null, 400, "The Username field is required", "Omit Username Key")]
            [RequestTest("PUT", null, 400, "The Username field is required", "Omit Username Key")]
            [RequestTest("PUT", "", 500, "Missing type map configuration or unsupported mapping.", "Omit Username Key_Bug 250")]  //Bug #250
            public string Username { get; set; }

            [RequestTest("POST", null, 400, "The Password field is required", "Omit Password Key")]
            [RequestTest("PUT", null, 400, "The Password field is required", "Omit Password Key")]
            [RequestTest("PUT", "", 500, "Missing type map configuration or unsupported mapping.", "Omit Password Key_Bug 250")]  //Bug #250
            public string Password { get; set; }

            [RequestTest("POST", null, 400, "The KeyFilePath field is required", "Omit KeyFilePath Key")]
            [RequestTest("PUT", null, 400, "The KeyFilePath field is required", "Omit KeyFilePath Key")]
            [RequestTest("PUT", "", 500, "Missing type map configuration or unsupported mapping.", "Omit KeyFilePath Key_Bug 250")]  //Bug #250
            public string KeyFilePath { get; set; }

            public bool? Active { get; set; }

            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                TenantId = "9d6b9214-23e0-479e-846c-f6048c0c0058";          //SSB TenantId
                Hostname = "QAWasHere";
                Username = "QAUser";
                Password = "Testing@123";
                KeyFilePath = "QAWasHere";
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
                    return Endpoint + "/";         //+ CurrentObject.Id;
                case "GETQuery":
                    return Endpoint + "/";         //api/TenantSftp/{id}
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
            CurrentObject.TenantId = "9D6B9214-23E0-479E-846C-F6048C0C0058";          //SSB TenantId
            CurrentObject.Hostname = "QAWasHereToo";
            CurrentObject.Username = "QAUser2";
            CurrentObject.Password = "Testing@456";
            CurrentObject.KeyFilePath = "QAWasHereToo";
            CurrentObject.Active = true;
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
            IntegrationMetadataQueries query = new();
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.RequestType = "GET";

            DataTable allTenantSFTPsDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), query.QueryAllTenantSFTP(SecretsManager.SnowflakeDatabaseEnvironment()));

            //Returns all Tenant SFTP in dbo.TenantSFTP (for GET /api/TenantSFTP)
            string expectedGetAllResponseBody = JsonConvert.SerializeObject(allTenantSFTPsDt);
            TestParams.TestStepName = GetType().Name + "_GET_AllTenantSFTPs";
            TestParams.Url = GetUrl("GET");
            TestParams.ExpectedResponseBody = expectedGetAllResponseBody;
            testParamsList.Add(TestParams.Copy());

            //Returns specific Tenant SFTP in dbo.TenantSFTP (for GET /api/TenantSFTP/{TENANTSFTPID})
            for (int rowIndex = 0; rowIndex < allTenantSFTPsDt.Rows.Count; rowIndex++)
            {
                JArray jArray = JArray.FromObject(allTenantSFTPsDt, JsonSerializer.CreateDefault());

                JToken rowJToken = jArray[rowIndex];
                string rowJson = rowJToken.ToString(Formatting.None);

                TestParams.TestStepName = GetType().Name + "_GET_TenantSFTPByTenantSFTPId_" + rowJToken["ID"];
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
            string command = $@"DELETE FROM CI_METADATA{SecretsManager.SnowflakeDatabaseEnvironment()}.dbo.TenantSFTP WHERE TENANTSFTPID = '{currentId}'";
            DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), command);
        }
    }
}
