namespace ci_automation_apitester.ApiDto.IntegrationMetadata
{
    public class TenantIntegrationConfigSFTP : BaseRestApi
    {
        const string Endpoint = "/api/TenantIntegrationConfigSftp";
        public Dto CurrentObject = new();

        public class Dto
        {
            //POST need seeded:  
                //TenantIntegrationId must be unique,
                //TenantSftpId must correlate to existing record in dbo.TenantSftp where dbo.TenantSftp.Active = True

            [UrlTest("DELETE", "", 405, "", "")]
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling TenantIntegrationConfigSftp", "TenantIntegrationConfigId Doesn't Exist")]
            [UrlTest("DELETE", "0E670336-BDA1-47fA-BC93-FFD9249BEF92", 500, "Error disabling TenantIntegrationConfigSftp", "TenantIntegrationConfigId Casing Mismatch")]
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "TenantIntegrationConfigId Doesn't Exist")]
            [UrlTest("GET", "0E670336-BDA1-47fA-BC93-FFD9249BEF92", 404, "Sequence contains no elements", "TenantIntegrationConfigId Casing Mismatch")]
            [RequestTest("PUT", null, 400, "The Id field is required", "Omit TenantIntegrationConfigId Key")]
            [RequestTest("PUT", "", 500, "Value cannot be null. (Parameter 'id')", "Blank TenantIntegrationConfigId Key")]
            public string Id { get; set; }
            [RequestTest("POST", null, 500, "Value cannot be null. (Parameter 'TenantIntegrationId')", "Omit TenantIntegrationId Key")]
            [RequestTest("POST", "", 500, "Required input TenantIntegrationId was empty. (Parameter 'TenantIntegrationId')", "Blank TenantIntegrationId Key")]
            [RequestTest("POST", "6e0a5769-4147-4b75-81be-4818cc3edde2", 500, "TenantIntegration is not unique in TenantIntegrationConfigSftp", "Duplicate TenantIntegrationId")]
            [RequestTest("PUT", null, 400, "The TenantIntegrationId field is required", "Omit TenantIntegrationId Key")]
            //[RequestTest("PUT", "", ???, "???", "Blank TenantIntegrationId Key")]  //204
            public string TenantIntegrationId { get; set; }
            [RequestTest("POST", null, 500, "Value cannot be null. (Parameter 'TenantSftpId')", "Omit TenantSftpId Key")]
            [RequestTest("POST", "", 500, "Required input TenantSftpId was empty. (Parameter 'TenantSftpId')", "Blank TenantSftpId Key")]
            //[RequestTest("POST", "99999999-9999-9999-9999-999999999999", 500, "???", "TenantSftpId Doesn't Exist")] //404
            [RequestTest("POST", "73d3a3f0-0edb-11ed-8937-a9e9ce36e4dc", 500, "TenantSftp doesn't exist or is inactive", "Inactive TenantSftpId")]
            [RequestTest("PUT", null, 400, "The TenantSftpId field is required", "Omit TenantSftpId Key")]
            //[RequestTest("PUT", "", ???, "???", "Blank TenantSftpId Key")]  //204
            public string TenantSftpId { get; set; }
            [RequestTest("POST", null, 400, "The SftpArchiveDirectory field is required", "Omit SftpArchiveDirectory Key")]
            //[RequestTest("POST", "", 201, "", "Blank SftpArchiveDirectory Key")]  //201
            [RequestTest("PUT", null, 400, "The SftpArchiveDirectory field is required", "Omit SftpArchiveDirectory Key")]
            //[RequestTest("PUT", "", ???, "???", "Blank SftpArchiveDirectory Key")]  //204
            public string SftpArchiveDirectory { get; set; }
            [RequestTest("POST", null, 400, "The SftpDeleteFromSource field is required", "Omit SftpDeleteFromSource Key")]
            //[RequestTest("POST", "", 201, "???", "Blank SftpDeleteFromSource Key")]  //201
            [RequestTest("PUT", null, 400, "The SftpDeleteFromSource field is required", "Omit SftpDeleteFromSource Key")]
            //[RequestTest("PUT", "", ???, "???", "Blank SftpDeleteFromSource Key")]  //204
            public string SftpDeleteFromSource { get; set; }
            [RequestTest("POST", null, 400, "The SftpLatestOnly field is required", "Omit SftpLatestOnly Key")]
            //[RequestTest("POST", "", 201, "???", "Blank SftpLatestOnly Key")]  //201
            [RequestTest("PUT", null, 400, "The SftpLatestOnly field is required", "Omit SftpLatestOnly Key")]
            //[RequestTest("PUT", "", ???, "???", "Blank SftpLatestOnly Key")]  //204
            public string SftpLatestOnly { get; set; }
            [RequestTest("POST", null, 400, "The SftpOperation field is required", "Omit SftpOperation Key")]
            //[RequestTest("POST", "", 201, "???", "Blank SftpOperation Key")]  //201
            [RequestTest("PUT", null, 400, "The SftpOperation field is required", "Omit SftpOperation Key")]
            //[RequestTest("PUT", "", ???, "???", "Blank SftpOperation Key")]  //204
            public string SftpOperation { get; set; }
            [RequestTest("POST", null, 400, "The SourceDirectory field is required", "Omit SourceDirectory Key")]
            //[RequestTest("POST", "", 201, "???", "Blank SourceDirectory Key")]  //201
            [RequestTest("PUT", null, 400, "The SourceDirectory field is required", "Omit SourceDirectory Key")]
            //[RequestTest("PUT", "", ???, "???", "Blank SourceDirectory Key")]  //204
            public string SourceDirectory { get; set; }
            [RequestTest("POST", null, 400, "The SourceFilename field is required", "Omit SourceFilename Key")]
            //[RequestTest("POST", "", 201, "???", "Blank SourceFilename Key")]  //201
            [RequestTest("PUT", null, 400, "The SourceFilename field is required", "Omit SourceFilename Key")]
            //[RequestTest("PUT", "", ???, "???", "Blank SourceFilename Key")]  //204
            public string SourceFilename { get; set; }
            [RequestTest("POST", null, 400, "The BlobDestination field is required", "Omit BlobDestination Key")]
            //[RequestTest("POST", "", 201, "???", "Blank BlobDestination Key")]  //201
            [RequestTest("PUT", null, 400, "The BlobDestination field is required", "Omit BlobDestination Key")]
            //[RequestTest("PUT", "", ???, "???", "Blank BlobDestination Key")]  //204
            public string BlobDestination { get; set; }
            public bool? Active { get; set; }
            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                TenantIntegrationId = "";                           
                TenantSftpId = "";                                  
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
                    return Endpoint + "/tenantintegrationid/";         //api/TenantIntegrationConfigSftp/tenantintegrationid/{tenantIntegrationId}
                case "GETIntegrationIdQuery":
                    return Endpoint + "/integrationid/";              //api/TenantIntegrationConfigSftp/integrationid/{integrationId}
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
            CurrentObject.TenantIntegrationId = "";           
            CurrentObject.TenantSftpId = "";                                  
            CurrentObject.SftpArchiveDirectory = "/paciolan-sftp/qawashere/QA/Eloqua/Archive/";
            CurrentObject.SftpDeleteFromSource = "FALSE";
            CurrentObject.SftpLatestOnly = "FALSE";
            CurrentObject.SftpOperation = "GET";
            CurrentObject.SourceDirectory = "/paciolan-sftp/qawashere/QA/Eloqua/";
            CurrentObject.SourceFilename = "qa_eloqua_*.csv";
            CurrentObject.BlobDestination = "qawashere/snowflake/in/Eloqua/uofqa/";
            CurrentObject.Active = true;                           
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
            IntegrationMetadataQueries query = new();
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.RequestType = "GET";

            DataTable allTenantIntegrationConfigSFTPsDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), query.QueryAllTenantIntegrationConfigSFTP(SecretsManager.SnowflakeDatabaseEnvironment()));

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
            DataTable integrationIdDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), query.QueryAllIntegration(SecretsManager.SnowflakeDatabaseEnvironment()));
            foreach (DataRow row in integrationIdDt.Rows)
            {
                DataTable eachIntegrationIdDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), query.QueryTenantIntegrationConfigSFTPByIntegrationId(SecretsManager.SnowflakeDatabaseEnvironment(), row.Field<string>("Id")));
                JArray jArray = JArray.FromObject(eachIntegrationIdDt, JsonSerializer.CreateDefault());

                TestParams.TestStepName = GetType().Name + "_GET_TenantIntegrationConfigSFTPByIntegrationId_" + row.Field<string>("Id");
                TestParams.Url = GetUrl("GETIntegrationIdQuery") + row.Field<string>("Id").ToString();
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
