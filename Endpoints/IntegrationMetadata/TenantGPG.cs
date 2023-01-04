namespace ci_automation_apitester.Endpoints.IntegrationMetadata
{
    internal class TenantGPG : BaseRestApi
    {
        const string Endpoint = "/api/TenantGpg";
        public Dto CurrentObject = new();

        //GET by TenantId, Active/Inactive

        public class Dto
        {
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "TenantGpgId Doesn't Exist")]
            //[UrlTest("GET", "573DB342-FC2F-4162-A2DD-F031BB6A2C57", 404, "Sequence contains no elements", "TenantGpgId Casing Mismatch")]  //returns 201?
            [RequestTest("PUT", null, 500, "Required input Id was empty", "Omit Id Key")]
            [RequestTest("PUT", "", 500, "Required input Id was empty", "Blank Id Key")]
            //[RequestTest("PUT", "573DB342-FC2F-4162-A2DD-F031BB6A2C57", 500, "Error updating TenantGpg", "TenantSftpId Casing Mismatch")]
            [RequestTest("PUT", "99999999-9999-9999-9999-999999999999", 500, "Error updating TenantGpg", "TenantGpgId Doesn't Exist")]
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling TenantGpg", "TenantGpgId Doesn't Exist")]
            //[UrlTest("DELETE", "573DB342-FC2F-4162-A2DD-F031BB6A2C57", 500, "Error disabling tenant", "TenantGpgId Casing Mismatch")]  //returns 201?
            [UrlTest("DELETE", "", 405, "", "TenantSftpId Method Not Allowed")]
            public string Id { get; set; }

            [RequestTest("POST", null, 500, "Required input TenantId was empty", "Omit TenantId Key")]
            [RequestTest("POST", "", 500, "Required input TenantId was empty", "Blank TenantId Key")]
            [RequestTest("POST", "99999999-9999-9999-9999-999999999999", 500, "TenantId is formatted incorrectly or does not exist", "TenantId Doesn't Exist")]
            [RequestTest("PUT", null, 500, "Required input TenantId was empty", "Omit TenantId Key")]
            [RequestTest("PUT", "", 500, "Required input TenantId was empty", "Blank TenantId Key")]
            [RequestTest("PUT", "99999999-9999-9999-9999-999999999999", 500, "TenantId is formatted incorrectly or does not exist", "TenantId Doesn't Exist")]
            public string TenantId { get; set; }

            [RequestTest("POST", null, 500, "Required input GpgKeyName was empty", "Omit GpgKeyName Key")]
            [RequestTest("POST", "", 500, "Required input GpgKeyName was empty", "Blank GpgKeyName Key")]
            [RequestTest("PUT", null, 500, "Required input GpgKeyName was empty", "Omit GpgKeyName Key")]
            [RequestTest("PUT", "", 500, "Required input GpgKeyName was empty", "Omit GpgKeyName Key")]
            public string GpgKeyName { get; set; }

            [RequestTest("POST", null, 500, "Required input Key was empty", "Omit Key Key")]
            [RequestTest("POST", "", 500, "Required input Key was empty", "Blank Key Key")]
            [RequestTest("PUT", null, 500, "Required input Key was empty", "Omit Key Key")]
            [RequestTest("PUT", "", 500, "Required input Key was empty", "Blank Key Key")]
            public string Key { get; set; }
            public string KeyType { get; set; }
            public string PassPhrase { get; set; }

            public bool? Active { get; set; }

            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                TenantId = "fbe2aff9-dfc3-421d-998a-43f3afa8c086";          //Seeded, unique
                GpgKeyName = "QAWasHere";
                Key = "QAUser";
                KeyType = "Testing@123";
                PassPhrase = "QAWasHere";
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
                    return Endpoint + "/";         //api/TenantGpg/{id}
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
            CurrentObject.TenantId = "fbe2aff9-dfc3-421d-998a-43f3afa8c086";          //Seeded, unique
            CurrentObject.GpgKeyName = "QA Private Key";
            CurrentObject.Key = "-----BEGIN PGP PRIVATE KEY BLOCK-----R18WSf5w/XYtv1O9FmqMHa1hvDd6vSTfG6HOMBXzgHM+ISqr2iYiYqB6F\r\n8ZzMbo2pxij/TYU8IuwOc6PQEe63Jz+XRgYxRlCk0NXpTd90Pc3atvRCPJg+bOaA\r\nkGsaU4SEsOEawx1kXSd0jdifeL4NG8bo\r\nPgv70g/jxYcF1qllXriXnyPEV4607Z1UCwal9XJkJfwzjAjl5eIFvUTGNfUSnXws\r\nD/3YQJ9L9lxIR6IEg/3Lz/EBX1rOgFf1RQCglhKncB4oxMyw4GIp6d2IuOCuTgbd\r\neM/sLdPZxBJOsmw+ogVpqDyacmvUWF8Lt9KNHiVBwt/UACxfg91pxg==\r\n=JINs -----END PGP PRIVATE KEY BLOCK-----";
            CurrentObject.KeyType = "private";
            CurrentObject.PassPhrase = "QAWasHereToo";
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
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.RequestType = "GET";

            DataTable allTenantGPGsDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryAllTenantGPG(SecretsManager.SnowflakeDatabaseEnvironment()));

            //Returns all Tenant GPG in dbo.TenantGPG (for GET /api/TenantGPG)
            string expectedGetAllResponseBody = JsonConvert.SerializeObject(allTenantGPGsDt);
            TestParams.TestStepName = GetType().Name + "_GET_AllTenantGPGs";
            TestParams.Url = GetUrl("GET");
            TestParams.ExpectedResponseBody = expectedGetAllResponseBody;
            testParamsList.Add(TestParams.Copy());

            //Returns specific Tenant GPG in dbo.TenantGPG (for GET /api/TenantGPG/{TENANTGPGID})
            for (int rowIndex = 0; rowIndex < allTenantGPGsDt.Rows.Count; rowIndex++)
            {
                JArray jArray = JArray.FromObject(allTenantGPGsDt, JsonSerializer.CreateDefault());

                JToken rowJToken = jArray[rowIndex];
                string rowJson = rowJToken.ToString(Formatting.None);

                TestParams.TestStepName = GetType().Name + "_GET_TenantSFTPByTenantGPGId_" + rowJToken["ID"];
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
            string command = $@"DELETE FROM CI_METADATA{SecretsManager.SnowflakeDatabaseEnvironment()}.dbo.TenantGPG WHERE TENANTGPGID = '{currentId}'";
            DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), command);
        }
    }
}
