namespace ci_automation_apitester.ApiDto.IntegrationMetadata
{
    public class IntegrationEndpoint : BaseRestApi
    {
        const string Endpoint = "/api/IntegrationEndpoint";
        public Dto CurrentObject = new();

        public class Dto
        {
            //These scenarios involve 2 properties?  Will need known, seeded combos.
            //[RequestTest("PUT", "{\"integrationEndpointId\": \"???\", \"integrationId\": \"???\"}", 500, "IntegrationEndpoint with specified IntegrationEndpointId and integrationId does not exist", "IntegrationEndpointId & IntegrationId Combo Doesn't Exist")]

            [RequestTest("PUT", null, 500, "IntegrationEndpointId is formatted incorrectly", "Omit Id Key")]
            [RequestTest("PUT", "", 500, "IntegrationEndpointId is formatted incorrectly", "Blank Id Key")]
            //[UrlTest("DELETE", "", 405, "", "")]
            //[UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling TenantIntegration", "Id Doesn't Exist")]
            //[UrlTest("DELETE", "B011C622-9A2D-4279-95B2-4fC54755124F", 500, "Error disabling TenantIntegration", "Id Casing Mismatch")]
            //[UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "Id Doesn't Exist")]
            //[UrlTest("GET", "B011C622-9A2D-4279-95B2-4fC54755124F", 404, "Sequence contains no elements", "Id Casing Mismatch")]
            public string IntegrationEndpointId { get; set; }

            [RequestTest("POST", null, 500, "IntegrationId is formatted incorrectly or does not exist", "Omit IntegrationId Key")]
            [RequestTest("POST", "", 500, "IntegrationId is formatted incorrectly or does not exist", "Blank IntegrationId Key")]
            [RequestTest("POST", "B011C622-9A2D-4279-95B2-4fC54755124F", 500, "IntegrationId is formatted incorrectly or does not exist", "IntegrationId Casing Mismatch")]
            [RequestTest("POST", "99999999-9999-9999-9999-999999999999", 500, "IntegrationId is formatted incorrectly or does not exist", "IntegrationId Doesnt' Exist")]
            [RequestTest("PUT", null, 500, "IntegrationId is formatted incorrectly or does not exist", "Omit IntegrationId Key")]
            [RequestTest("PUT", "", 500, "IntegrationId is formatted incorrectly or does not exist", "Blank IntegrationId Key")]
            public string IntegrationId { get; set; }
            [RequestTest("PUT", null, 500, "Value cannot be null. (Parameter 'name')", "Omit Name Key")]
            [RequestTest("PUT", "", 500, "Value cannot be null. (Parameter 'name')", "Blank Name Key")]
            public string Name { get; set; }  //Bug for POST, null'd
            public string Description { get; set; }  //can be null, add 200's?
            [RequestTest("PUT", null, 500, "Value cannot be null. (Parameter 'endpoint')", "Omit Endpoint Key")]
            [RequestTest("PUT", "", 500, "Value cannot be null. (Parameter 'endpoint')", "Blank Endpoint Key")]
            public string Endpoint { get; set; }  //Bug for POST, null'd
            [RequestTest("PUT", null, 500, "Value cannot be null. (Parameter 'blobFolderPath')", "Omit BlobFolderPath Key")]
            [RequestTest("PUT", "", 500, "Value cannot be null. (Parameter 'blobFolderPath')", "Blank BlobFolderPath Key")]
            public string BlobFolderPath { get; set; } //Bug for POST, null'd
            public string RequestBody { get; set; }  //can be null, add 200's?
            public bool IsPaginated { get; set; }
            [RequestTest("PUT", null, 500, "Value cannot be null. (Parameter 'scheduleId')", "Omit ScheduleId Key")]
            [RequestTest("PUT", "", 500, "Value cannot be null. (Parameter 'scheduleId')", "Blank ScheduleId Key")]
            public string ScheduleId { get; set; }  //Bug for POST, null'd
            public bool Active { get; set; }
            public Dto()
            {
                IntegrationEndpointId = Guid.NewGuid().ToString().ToUpper();
                IntegrationId = "179b8861-be7c-465b-9a14-dfa73e0f100f";                             //Change this value
                Name = "QA Inbound Email";
                Description = "QA Inbound Email";
                Endpoint = "activities/qa";
                BlobFolderPath = "cidttestdata/cidt/qabulk/cffb42ef-3670-4230-8b6d-8d45d1506b1a/";
                RequestBody = "{\"name\":\"QA Subscribe Activity Export\",\"fields\":{\"Id\":\"{{Activity.Id}}\",\"Type\":\"{{Activity.Type}}\",\"CreatedAt\":\"{{Activity.CreatedAt}}\",\"EmailRecipientId\":\"{{Activity.Field(EmailRecipientId)}}\",\"ContactId\":\"{{Activity.Contact.Id}}\",\"AssetType\":\"{{Activity.Asset.Type}}\",\"AssetName\":\"{{Activity.Asset.Name}}\",\"AssetId\":\"{{Activity.Asset.Id}}\",\"CampaignId\":\"{{Activity.Campaign.Id}}\",\"CRMCampaignId\":\"{{Activity.Campaign.Field(CRMCampaignId)}}\",\"CampaignName\":\"{{Activity.Campaign.Field(CampaignName)}}\",\"ExternalId\":\"{{Activity.ExternalId}}\",\"EmailAddress\":\"{{Activity.Field(EmailAddress)}}\",\"ContactIdExt\":\"{{Activity.Contact.Field(ContactIDExt)}}\"},\"filter\":\"'{{Activity.Type}}'='Subscribe'\"}";
                IsPaginated = true;
                ScheduleId = "e7af8fe4-c170-498a-be6e-778aa3c77914";
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
                    return Endpoint + "/" + CurrentObject.IntegrationEndpointId;
                case "GETQuery":
                    return Endpoint + "/";         //api/IntegrationEndpoint/{id}
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
            CurrentObject.IntegrationEndpointId = "";
            CurrentObject.IntegrationId = "179b8861-be7c-465b-9a14-dfa73e0f100f";                             //Change this value
            CurrentObject.Name = "QA Inbound Email";
            CurrentObject.Description = "QA Inbound Email";
            CurrentObject.Endpoint = "activities/qa";
            CurrentObject.BlobFolderPath = "cidttestdata/cidt/qabulk/cffb42ef-3670-4230-8b6d-8d45d1506b1a/";
            CurrentObject.RequestBody = "{\"header\":{\"ver\":1,\"src_sys_type\":2,\"src_sys_name\":\"QA\",\"archtics_version\":\"V999\"},\"command1\":{\"cmd\":\"get_crm_audit\",\"uid\":\"QA123\",\"dsn\":\"{{ dsn }}\",\"iTag\":\"SSBCN\",\"iTeam\":\"{{ team }}\",\"iEvent_name\":\"{{ event }}\"}}";
            CurrentObject.IsPaginated = true;
            CurrentObject.ScheduleId = "e7af8fe4-c170-498a-be6e-778aa3c77914";
            CurrentObject.Active = true;
            return JsonConvert.SerializeObject(CurrentObject);
        }
        public override string GetWorkingId()
        {
            return CurrentObject.IntegrationEndpointId;
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

            DataTable allIntegrationEndpointsDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), query.QueryAllIntegrationEndpoints(SecretsManager.SnowflakeDatabaseEnvironment()));

            //Returns all Integration Endpoints in dbo.IntegrationEndpoints (for GET /api/IntegrationEndpoints)
            string expectedGetAllResponseBody = JsonConvert.SerializeObject(allIntegrationEndpointsDt);
            TestParams.TestStepName = GetType().Name + "_GET_AllIntegrationEndpoints";
            TestParams.Url = GetUrl("GET");
            TestParams.ExpectedResponseBody = expectedGetAllResponseBody;
            testParamsList.Add(TestParams.Copy());

            //Returns specific Integration Endpoint in dbo.IntegrationEndpoints (for GET /api/IntegrationEndpoints/{INTEGRATIONENDPOINTID})
            for (int rowIndex = 0; rowIndex < allIntegrationEndpointsDt.Rows.Count; rowIndex++)
            {
                JArray jArray = JArray.FromObject(allIntegrationEndpointsDt, JsonSerializer.CreateDefault());

                JToken rowJToken = jArray[rowIndex];
                string rowJson = rowJToken.ToString(Formatting.None);

                //TestParams.TestStepName = GetType().Name + "_GET_IntegrationEndpointById_" + rowJToken["ID"];
                //TestParams.Url = GetUrl("GETQuery") + rowJToken["ID"].ToString();
                TestParams.TestStepName = GetType().Name + "_GET_IntegrationEndpointById_" + rowJToken["INTEGRATIONENDPOINTID"];
                TestParams.Url = GetUrl("GETQuery") + rowJToken["INTEGRATIONENDPOINTID"].ToString();
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
            string command = $@"DELETE FROM CI_METADATA{SecretsManager.SnowflakeDatabaseEnvironment()}.DBO.INTEGRATIONENDPOINTS WHERE INTEGRATIONENDPOINTID = '{currentId}'";
            DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), command);
        }
    }
}
