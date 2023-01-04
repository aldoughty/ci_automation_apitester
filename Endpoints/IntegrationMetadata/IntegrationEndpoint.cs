namespace ci_automation_apitester.ApiDto.IntegrationMetadata
{
    public class IntegrationEndpoint : BaseRestApi
    {
        const string Endpoint = "/api/IntegrationEndpoint";
        public Dto CurrentObject = new();

        public class Dto
        {
            
            [RequestTest("PUT", null, 500, "Required input Id was empty", "Omit IntegrationEndpointId Key")]
            [RequestTest("PUT", "", 500, "Required input Id was empty", "Blank IntegrationEndpointId Key")]
            [RequestTest("PUT", "99999999-9999-9999-9999-999999999999", 500, "IntegrationEndpoint with specified Id and IntegrationId does not exist", "IntegrationEndpointId Doesn't Exist")]
            [UrlTest("DELETE", "", 405, "", "")]
            [UrlTest("DELETE", "99999999-9999-9999-9999-999999999999", 500, "Error disabling IntegrationEndpoint", "Id Doesn't Exist")]
            //[UrlTest("DELETE", "B95DA86A-1175-449A-8D45-8ABDCD59EC59", 204, "Error disabling IntegrationEndpoint", "Id Casing Mismatch")]
            [UrlTest("GET", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "Id Doesn't Exist")]
            //[UrlTest("GET", "B95DA86A-1175-449A-8D45-8ABDCD59EC59", 200, "Sequence contains no elements", "Id Casing Mismatch")]
            public string Id { get; set; }

            [RequestTest("POST", null, 500, "Required input IntegrationId was empty", "Omit IntegrationId Key")]
            [RequestTest("POST", "", 500, "Required input IntegrationId was empty", "Blank IntegrationId Key")]
            [RequestTest("POST", "EAB73437-209C-4CF0-9128-D5A1776E7F9A", 404, "Sequence contains no elements", "IntegrationId Casing Mismatch")]
            [RequestTest("POST", "99999999-9999-9999-9999-999999999999", 404, "Sequence contains no elements", "IntegrationId Doesn't Exist")]
            //[RequestTest("POST", "f0108df8-1c04-443f-b067-124da266464d", 201, "", "Duplicate IntegrationId")]
            [RequestTest("PUT", null, 500, "Required input IntegrationId was empty", "Omit IntegrationId Key")]
            [RequestTest("PUT", "", 500, "Required input IntegrationId was empty", "Blank IntegrationId Key")]
            public string IntegrationId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; } 
            public string Endpoint { get; set; } 
            public string BlobFolderPath { get; set; }
            public string RequestBody { get; set; }
            public bool IsPaginated { get; set; }
            public string ScheduleId { get; set; }
            public bool Active { get; set; }
            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                IntegrationId = "277d6d4b-7947-4608-97ed-fdee920a8562";                             //Seeded, unique
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
                    return Endpoint + "/" + CurrentObject.Id;
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
            CurrentObject.Id = "b95da86a-1175-449a-8d45-8abdcd59ec59";
            CurrentObject.IntegrationId = "277d6d4b-7947-4608-97ed-fdee920a8562";                             //Seeded, unique
            CurrentObject.Name = "QA Inbound Email";
            CurrentObject.Description = "QA Inbound Email";
            CurrentObject.Endpoint = "activities/qa";
            CurrentObject.BlobFolderPath = "cidttestdata/cidt/qabulk/cffb42ef-3670-4230-8b6d-8d45d1506b1a/";
            CurrentObject.RequestBody = "{\"header\":{\"ver\":1,\"src_sys_type\":2,\"src_sys_name\":\"QA\",\"archtics_version\":\"V999\"},\"command1\":{\"cmd\":\"get_crm_audit\",\"uid\":\"QA123\",\"dsn\":\"{{ dsn }}\",\"iTag\":\"SSBCN\",\"iTeam\":\"{{ team }}\",\"iEvent_name\":\"{{ event }}\"}}";
            CurrentObject.IsPaginated = true;
            CurrentObject.ScheduleId = "e7af8fe4-c170-498a-be6e-778aa3c77914";
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

            DataTable allIntegrationEndpointsDt = DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), IntegrationMetadataQueries.QueryAllIntegrationEndpoints(SecretsManager.SnowflakeDatabaseEnvironment()));

            //If dbo.IntegrationEndpoints.IsPaginated = NULL, replace with false
            foreach (DataRow row in allIntegrationEndpointsDt.Rows)
            {
                if (string.IsNullOrEmpty(row["IsPaginated"].ToString()))
                {
                    row["IsPaginated"] = false;
                }
            }

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

                TestParams.TestStepName = GetType().Name + "_GET_IntegrationEndpointById_" + rowJToken["ID"];
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
            string command = $@"DELETE FROM CI_METADATA{SecretsManager.SnowflakeDatabaseEnvironment()}.DBO.INTEGRATIONENDPOINTS WHERE INTEGRATIONENDPOINTID = '{currentId}'";
            DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), command);
        }
    }
}
