﻿namespace ci_automation_apitester.ApiDto.IntegrationMetadata
{
    public class TenantSFTP : BaseRestApi
    {
        const string Endpoint = "/api/TenantSftp";
        public Dto CurrentObject = new();
        
        //GET by TenantId, Active/Inactive

        public class Dto
        {
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "TenantSftpId Doesn't Exist")]
            //[UrlTest("GET", "573DB342-FC2F-4162-A2DD-F031BB6A2C57", 404, "Sequence contains no elements", "TenantSftpId Casing Mismatch")]  //returns 201?
            [RequestTest("PUT", null, 500, "Required input Id was empty", "Omit Id Key")]
            [RequestTest("PUT", "", 500, "Required input Id was empty", "Blank Id Key")]
            //[RequestTest("PUT", "573DB342-FC2F-4162-A2DD-F031BB6A2C57", 500, "Error updating TenantSftp", "TenantSftpId Casing Mismatch")]
            [RequestTest("PUT", "99999999-9999-9999-9999-999999999999", 500, "Error updating TenantSftp", "TenantSftpId Doesn't Exist")]
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling tenant", "TenantSftpId Doesn't Exist")]
            //[UrlTest("DELETE", "573DB342-FC2F-4162-A2DD-F031BB6A2C57", 500, "Error disabling tenant", "TenantSftpId Casing Mismatch")]  //returns 201?
            [UrlTest("DELETE", "", 405, "", "TenantSftpId Method Not Allowed")]
            public string Id { get; set; }

            [RequestTest("POST", null, 500, "Required input TenantId was empty", "Omit TenantId Key")]
            [RequestTest("POST", "", 500, "Required input TenantId was empty", "Blank TenantId Key")]
            [RequestTest("POST", "99999999-9999-9999-9999-999999999999", 500, "TenantId is formatted incorrectly or does not exist", "TenantId Doesn't Exist")]
            [RequestTest("PUT", null, 500, "Required input TenantId was empty", "Omit TenantId Key")]
            [RequestTest("PUT", "", 500, "Required input TenantId was empty", "Blank TenantId Key")]
            [RequestTest("PUT", "99999999-9999-9999-9999-999999999999", 500, "TenantId is formatted incorrectly or does not exist", "TenantId Doesn't Exist")]
            public string TenantId { get; set; }

            [RequestTest("POST", null, 500, "Required input Hostname was empty", "Omit Hostname Key")]
            [RequestTest("POST", "", 500, "Required input Hostname was empty", "Blank Hostname Key")]
            [RequestTest("PUT", null, 500, "Required input Hostname was empty", "Omit Hostname Key")]
            [RequestTest("PUT", "", 500, "Required input Hostname was empty", "Omit Hostname Key")]
            public string Hostname { get; set; }

            [RequestTest("POST", null, 500, "Required input Username was empty", "Omit Username Key")]
            [RequestTest("POST", "", 500, "Required input Username was empty", "Blank Username Key")]
            [RequestTest("PUT", null, 500, "Required input Username was empty", "Omit Username Key")]
            [RequestTest("PUT", "", 500, "Required input Username was empty", "Blank Username Key")]
            public string Username { get; set; }
            public string Password { get; set; }
            public string KeyFilePath { get; set; }

            public bool? Active { get; set; }

            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                TenantId = "fbe2aff9-dfc3-421d-998a-43f3afa8c086";          //Seeded, unique
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
            CurrentObject.TenantId = "fbe2aff9-dfc3-421d-998a-43f3afa8c086";          //Seeded, unique
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
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.RequestType = "GET";

            DataTable allTenantSFTPsDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryAllTenantSFTP(SecretsManager.SnowflakeDatabaseEnvironment()));

            //If dbo.TenantSftp.Password = NULL, replace with false
            foreach (DataRow row in allTenantSFTPsDt.Rows)
            {
                if (string.IsNullOrEmpty(row["Password"].ToString()))
                {
                    row["Password"] = "";
                }
            }

            //If dbo.TenantSFTP.KeyFilePath = NULL, replace with ""
            foreach (DataRow row in allTenantSFTPsDt.Rows)
            {
                if (string.IsNullOrEmpty(row["KeyFilePath"].ToString()))
                {
                    row["KeyFilePath"] = "";
                }
            }

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
