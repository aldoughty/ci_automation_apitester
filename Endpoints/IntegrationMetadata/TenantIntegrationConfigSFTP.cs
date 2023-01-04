namespace ci_automation_apitester.ApiDto.IntegrationMetadata
{
    public class TenantIntegrationConfigSFTP : BaseRestApi
    {
        const string Endpoint = "/api/TenantIntegrationConfigSftp";
        public Dto CurrentObject = new();

        public class Dto
        {
            //TenantIntegrationId must be unique,
            //TenantSftpId must correlate to existing record in dbo.TenantSftp where dbo.TenantSftp.Active = True
            //Negative validation GETS for values that aren't a part of the DTO?  /api/TenantIntegrationConfigSftp/integrationid/{integrationId}
            //Negative validation GETS for /api/TenantIntegrationConfigSftp/tenantintegrationid/{tenantintegrationId}

            [UrlTest("DELETE", "", 405, "", "")]
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling TenantIntegrationConfigSftp", "TenantIntegrationConfigId Doesn't Exist")]
            [UrlTest("DELETE", "73CEADd83-9DF2-4D86-BB50-79199E0D9D11", 500, "Error disabling TenantIntegrationConfigSftp", "TenantIntegrationConfigId Casing Mismatch")]
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "TenantIntegrationConfigId Doesn't Exist")]
            [UrlTest("GET", "73CEADd83-9DF2-4D86-BB50-79199E0D9D11", 404, "Sequence contains no elements", "TenantIntegrationConfigId Casing Mismatch")]
            [RequestTest("PUT", null, 500, "Value cannot be null", "Omit TenantIntegrationConfigId Key")]
            [RequestTest("PUT", "", 500, "Value cannot be null", "Blank TenantIntegrationConfigId Key")]
            public string Id { get; set; }
            [RequestTest("POST", null, 500, "Value cannot be null", "Omit TenantIntegrationId Key")]
            [RequestTest("POST", "", 500, "Required input TenantIntegrationId was empty", "Blank TenantIntegrationId Key")]
            [RequestTest("POST", "9c2bfe92-b9ee-4573-bbc1-78358ca4d759", 500, "TenantIntegration is not unique in TenantIntegrationConfigSftp", "Duplicate TenantIntegrationId")]
            [RequestTest("PUT", null, 500, "Error updating TenantIntegrationConfigSftp", "Omit TenantIntegrationId Key")]
            [RequestTest("PUT", "", 500, "Error updating TenantIntegrationConfigSftp", "Blank TenantIntegrationId Key")]
            public string TenantIntegrationId { get; set; }
            [RequestTest("POST", null, 500, "Value cannot be null", "Omit TenantSftpId Key")]
            [RequestTest("POST", "", 500, "Required input TenantSftpId was empty", "Blank TenantSftpId Key")]
            [RequestTest("POST", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "TenantSftpId Doesn't Exist")]
            [RequestTest("POST", "779ffa80-22d1-42e1-b3e1-a417065bb4d8", 500, "TenantSftp doesn\\u0027t exist or is inactive", "Inactive TenantSftpId")]
            [RequestTest("PUT", null, 500, "Error updating TenantIntegrationConfigSftp", "Omit TenantSftpId Key")]
            [RequestTest("PUT", "", 500, "Error updating TenantIntegrationConfigSftp", "Blank TenantSftpId Key")]
            public string TenantSftpId { get; set; }
            public string SftpArchiveDirectory { get; set; }
            public string SftpDeleteFromSource { get; set; }
            public string SftpLatestOnly { get; set; }
            public string SftpOperation { get; set; }
            public string SourceDirectory { get; set; }
            public string SourceFilename { get; set; }
            public string BlobDestination { get; set; }
            public bool? Active { get; set; }
            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                TenantIntegrationId = "24a2a6ad-964d-4839-894e-cc581cc6333b";                           
                TenantSftpId = "0994f75b-8af5-415d-8a1e-6e0a938e8811";                                  
                SftpArchiveDirectory = "/paciolan-sftp/qawashere/QA/Eloqua/Archive/";
                SftpDeleteFromSource = "FALSE";
                SftpLatestOnly = "FALSE";
                SftpOperation = "GET";
                SourceDirectory = "/paciolan-sftp/qawashere/QA/Eloqua/";
                SourceFilename = "qa_eloqua_*.csv";
                BlobDestination = "qawashere/snowflake/in/Eloqua/uofqa/";
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
                    return Endpoint + "/";                              //api/TenantIntegrationConfigSftp/{id}
                case "GETTenantIntegrationIdQuery":
                    return Endpoint + "/tenantintegrationid/";          //api/TenantIntegrationConfigSftp/tenantintegrationid/{tenantIntegrationId}
                case "GETIntegrationIdQuery":
                    return Endpoint + "/integrationid/";                //api/TenantIntegrationConfigSftp/integrationid/{integrationId}
                case "GETTenantIdQuery":
                    return Endpoint + "/Tenant/";                       //api/TenantIntegrationConfigSftp/tenant/{tenantId}
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
            CurrentObject.TenantIntegrationId = "24a2a6ad-964d-4839-894e-cc581cc6333b";           
            CurrentObject.TenantSftpId = "0994f75b-8af5-415d-8a1e-6e0a938e8811";                                  
            CurrentObject.SftpArchiveDirectory = "/paciolan-sftp/qawashere/QA/Eloqua/Archive/";
            CurrentObject.SftpDeleteFromSource = "FALSE";
            CurrentObject.SftpLatestOnly = "FALSE";
            CurrentObject.SftpOperation = "GET";
            CurrentObject.SourceDirectory = "/paciolan-sftp/qawashere/QA/Eloqua/";
            CurrentObject.SourceFilename = "qa_eloqua_*.csv";
            CurrentObject.BlobDestination = "qawashere/snowflake/in/Eloqua/uofqa/";
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

            DataTable allTenantIntegrationConfigSFTPsDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryAllTenantIntegrationConfigSFTP(SecretsManager.SnowflakeDatabaseEnvironment()));

            //Returns all Tenant Integration Config SFTPs in dbo.TenantIntegrationConfig_SFTP (for GET /api/TenantIntegrationConfigSFTP)
            string expectedGetAllResponseBody = JsonConvert.SerializeObject(allTenantIntegrationConfigSFTPsDt);
            TestParams.TestStepName = GetType().Name + "_GET_AllTenantIntegrationConfigSFTPs";
            TestParams.Url = GetUrl("GET");
            TestParams.ExpectedResponseBody = expectedGetAllResponseBody;
            testParamsList.Add(TestParams.Copy());

            //Returns specific Tenant Integration Config SFTP in dbo.TenantIntegrationConfig_SFTP (for GET /api/TenantIntegrationConfigSFTP/{Id})
            for (int rowIndex = 0; rowIndex < allTenantIntegrationConfigSFTPsDt.Rows.Count; rowIndex++)
            {
                JArray jArray = JArray.FromObject(allTenantIntegrationConfigSFTPsDt, JsonSerializer.CreateDefault()); 

                JToken rowJToken = jArray[rowIndex];
                string rowJson = rowJToken.ToString(Formatting.None);

                TestParams.TestStepName = GetType().Name + "_GET_TenantIntegrationConfigSFTPById_" + rowJToken["ID"];
                TestParams.Url = GetUrl("GETQuery") + rowJToken["ID"].ToString();
                TestParams.ExpectedResponseBody = rowJson;
                testParamsList.Add(TestParams.Copy());
            }

            //Returns specific Tenant Integration Config SFTP in dbo.TenantIntegrationConfig_SFTP (for GET /api/TenantIntegrationConfigSFTP/{TenantIntegrationId})
            for (int rowIndex = 0; rowIndex < allTenantIntegrationConfigSFTPsDt.Rows.Count; rowIndex++)
            {
                JArray jArray = JArray.FromObject(allTenantIntegrationConfigSFTPsDt, JsonSerializer.CreateDefault()); 

                JToken rowJToken = jArray[rowIndex];
                string rowJson = rowJToken.ToString(Formatting.None); 

                TestParams.TestStepName = GetType().Name + "_GET_TenantIntegrationConfigSFTPByTenantIntegrationId_" + rowJToken["TENANTINTEGRATIONID"];
                TestParams.Url = GetUrl("GETTenantIntegrationIdQuery") + rowJToken["TENANTINTEGRATIONID"].ToString();
                TestParams.ExpectedResponseBody = rowJson;
                testParamsList.Add(TestParams.Copy());
            }

            //Returns specific Tenant Integration Config SFTP in dbo.TenantIntegrationConfig_SFTP by correlated IntegrationId in dbo.TenantIntegration (for GET /TenantIntegrationConfigSFTP/{IntegrationId})
            DataTable integrationIdDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryAllIntegration(SecretsManager.SnowflakeDatabaseEnvironment()));
            foreach (DataRow row in integrationIdDt.Rows)
            {
                DataTable eachIntegrationIdDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryTenantIntegrationConfigSFTPByIntegrationId(SecretsManager.SnowflakeDatabaseEnvironment(), row.Field<string>("Id")));
                JArray jArray = JArray.FromObject(eachIntegrationIdDt, JsonSerializer.CreateDefault());

                TestParams.TestStepName = GetType().Name + "_GET_TenantIntegrationConfigSFTPByIntegrationId_" + row.Field<string>("Id");
                TestParams.Url = GetUrl("GETIntegrationIdQuery") + row.Field<string>("Id").ToString();
                string rowJson = jArray.ToString(Formatting.None);
                TestParams.ExpectedResponseBody = rowJson;
                testParamsList.Add(TestParams.Copy());
            }

            //Returns specific Tenant Integration Config SFTP in dbo.TenantIntegrationConfig_SFTP by correlated TenantId in dbo.TenantSftp (for GET /TenantIntegrationConfigSFTP/Tenant/{TenantId})
            DataTable tenantIdDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryDistinctTenantSftpTenantId(SecretsManager.SnowflakeDatabaseEnvironment()));
            foreach (DataRow row in tenantIdDt.Rows)
            {
                DataTable eachTenantIdDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryTenantIntegrationConfigSftpByTenantId(SecretsManager.SnowflakeDatabaseEnvironment(), row.Field<string>("TenantId")));
                JArray jArray = JArray.FromObject(eachTenantIdDt, JsonSerializer.CreateDefault());

                TestParams.TestStepName = GetType().Name + "_GET_TenantIntegrationConfigSFTPByTenantId_" + row.Field<string>("TenantId");
                TestParams.Url = GetUrl("GETTenantIdQuery") + row.Field<string>("TenantId").ToString();
                string rowJson = jArray.ToString(Formatting.None);
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
            string command = $@"DELETE FROM CI_METADATA{SecretsManager.SnowflakeDatabaseEnvironment()}.DBO.TENANTINTEGRATIONCONFIG_SFTP WHERE TENANTINTEGRATIONCONFIGID = '{currentId}'";
            DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), command);
        }
    }
}
