namespace ci_automation_apitester.ApiDto.DataTagging
{
    //TEST CONSIDERATIONS
    //End Year is passed as null from UI

    public class TenantSaveScheme : BaseRestApi
    {
        const string Endpoint = "/tagging/api/TenantSaveScheme";
        public Dto CurrentObject = new();

        public class Dto

        {
            public string Id { get; set; }                          //passed in url, taggingUI.TenantSaveScheme.Id (aka config id) 
            public string OrgId { get; set; }                       //passed in headers, dbo.TenantOrgMapping.OrgId
            public bool Active { get; set; }
            //[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
            public int? StartYear { get; set; }
            public int? EndYear { get; set; }
            public string DataTaggingTypeId { get; set; }
            public string DataModelId { get; set; }
            //public string DimensionId { get; set; }
            //public string HierarchyId { get; set; }
            //public string HierarchyLabel { get; set; }
            public object ColumnIds { get; set; }

            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                OrgId = "org_ss0DHGCZvPZSAMhz";                                          //U of QA - Internal Admin 
                Active = true;
                StartYear = Int32.Parse(DateTime.Now.AddYears(-1).ToString("yyyy"));
                EndYear = Int32.Parse(DateTime.Now.AddYears(1).ToString("yyyy"));        //Int32.Parse(DateTime.Now.AddYears(2).ToString("yyyy"));
                DataTaggingTypeId = "F45B2296-59D1-48C7-BE04-9D4987CE276E";             //taggingUI.DataTaggingType.Name = Ticketing
                DataModelId = "48068947-F09D-42DE-8A41-A0E88FD824AF";                   //taggingui.DataModel.Name = Pac
                //DimensionId = "";
                //HierarchyId = "";
                //ColumnIds = "[]";
                ColumnIds = new JArray();
            }
        }
        public override Dictionary<string, string> GetHeaderParameter()
        {
            Dictionary<string, string> headerParams = new()
            {
                {"org_id", CurrentObject.OrgId},
            };

            return headerParams;
        }
        public override string GetUrl(string action)
        {
            switch (action)
            {
                case "GET":                         //Endpoint - returns all; Url + "/SchemeId" - returns specific; Url + OrgId (Header) - returns Org Schemes
                case "POST":
                case "PUT":
                    return Endpoint;
                case "PATCH":
                    return Endpoint + "/" + CurrentObject.Id;
                case "DELETE":
                    return Endpoint + "/" + CurrentObject.Id;
                case "GETQuery":
                    return Endpoint + "/";
                default: return Endpoint;
            }
        }
        public override List<string> GetRequestTypes()
        {
            List<string> actions = new()
            { "GET"};

            return actions;
        }
        public override string GetTestType()
        {
            return "ValidateResponseContainsString";
        }
        public override void CleanUp(MessageData messageData, string currentId)
        {
            string command = $@"DELETE FROM taggingUI.TenantSaveScheme WHERE Id ='{currentId}'";
            DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), command);
        }
        public override List<TestObjects.TestStep> GetParameterTests(MessageData messageData, string authValue, IEnvironment environment)
        {
            DataTaggingQueries query = new();
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);
            TestParams.TestType = "CompareResponseToString";
            TestParams.Url = GetUrl("GET");
            TestParams.RequestType = "GET";


            //Returns Orgs with dbo.TenantOrgMapping.Active = 1, dbo.Tenant.Active = 1, and having records in taggingUI.TenantSaveScheme
            //where taggingUI.TenantSaveScheme.Active = 1 (for GET by specific orgId)
            DataTable orgIdDt = DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), query.QueryTenantSchemesByOrgId());
            foreach (DataRow dataRow in orgIdDt.Rows)
            {
                TestParams.TestStepName = GetType().Name + "_GET_OrgId_" + dataRow["OrgId"].ToString();
                TestParams.Headers["org_id"] = dataRow["OrgId"].ToString();
                TestParams.ExpectedResponseBody = dataRow["Schemes"].ToString();
                testParamsList.Add(TestParams.Copy());
            }

            TestParams.Headers.Remove("org_id");   //removing for subsequent iterations (bc it can't be included in the GET all schemes)

            //Returns all Schemes in taggingUI.TenantSaveScheme (for GET all)
            DataTable allSchemesIdDt = DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), query.QueryAllTenantSchemes());
            foreach (DataRow dataRow in allSchemesIdDt.Rows)
            {
                TestParams.TestStepName = GetType().Name + "_GET_AllSchemes";
                TestParams.ExpectedResponseBody = dataRow["Schemes"].ToString();
                testParamsList.Add(TestParams.Copy());
            }

            //Returns each SchemeId in taggingUI.TenantSaveScheme (for GET of specific scheme)
            DataTable schemeIdDt = DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), query.QueryTenantSchemeById());
            foreach (DataRow dataRow in schemeIdDt.Rows)
            {
                TestParams.TestStepName = GetType().Name + "_GET_SchemeId_" + dataRow["SchemeId"].ToString();
                TestParams.Url = GetUrl("GETQuery") + dataRow["SchemeId"].ToString();
                TestParams.ExpectedResponseBody = dataRow["Scheme"].ToString();
                testParamsList.Add(TestParams.Copy());
            }

            return testParamsList;
        }
        public override string GetWorkingId()
        {
            return CurrentObject.Id;
        }
        public override string GetPostBody()
        {
            string results = JsonConvert.SerializeObject(CurrentObject);

            return results;
        }
        public override string GetPutBody()
        {
            return JsonConvert.SerializeObject(CurrentObject);
        }
        //public override string GetPatchBody()
        //{
        //    CurrentObject.StartYear = 2000;
        //    return "[{\"op\": \"replace\", \"path\": \"StartYear\", \"value\":\"2000\"}]";
        //}
        public override string GetDeleteBody()
        {
            CurrentObject.Active = false;
            return JsonConvert.SerializeObject(CurrentObject);
        }
        public override List<TestObjects.TestStep> GetFreeFormTests(string authValue, IEnvironment environment)
        {
            List<TestObjects.TestStep> result = new();
            result.AddRange(GetHeaderParamTests(authValue, environment));
            result.AddRange(GetInvalidSchemeParamTests(authValue, environment));
            result.AddRange(PostParamTests(authValue, environment));

            return result;
        }
        public List<TestObjects.TestStep> GetHeaderParamTests(string authValue, IEnvironment environment)
        {
            //Validates passing invalid Org Ids via Header for TenantSaveScheme endpoint
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.Url = GetUrl("GETQuery");
            TestParams.RequestType = "GET";

            //Inactive OrgId (dbo.TenantOrgMapping.Active = 0, dbo.Tenant.Active = 0, Having 0 Schemes in taggingUI.TenantSaveScheme)
            TestParams.Headers["org_id"] = "org_WOFA4Z9l52iB027P";
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_InvalidOrgIdHeaderParam_InactiveOrgTenantNoSchemes_" + TestParams.Headers["org_id"];
            TestParams.ExpectedResponseCode = 500;
            TestParams.ExpectedResponseBody = "No tenantid found for org";
            testParamsList.Add(TestParams.Copy());

            //Inactive OrgId (dbo.TenantOrgMapping.Active = 0, dbo.Tenant.Active = 1, Having 0 Schemes in taggingUI.TenantSaveScheme)
            TestParams.Headers["org_id"] = "org_sHv7KXqj7iM7mxkq";
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_InvalidOrgIdHeaderParam_InactiveOrgActiveTenantNoSchemes_" + TestParams.Headers["org_id"];
            TestParams.ExpectedResponseCode = 500;
            TestParams.ExpectedResponseBody = "No tenantid found for org";
            testParamsList.Add(TestParams.Copy());

            //Inactive OrgId (dbo.TenantOrgMapping.Active = 1, dbo.Tenant.Active = 0, Having 0 Schemes in taggingUI.TenantSaveScheme)
            TestParams.Headers["org_id"] = "org_0s4zr2KkVqWXWa8S";
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_InvalidOrgIdHeaderParam_ActiveOrgInactiveTenantNoSchemes_" + TestParams.Headers["org_id"];
            TestParams.ExpectedResponseCode = 404;
            TestParams.ExpectedResponseBody = "Not Found";
            testParamsList.Add(TestParams.Copy());

            //OrgId doesn't exist in dbo.TenantOrgMapping
            TestParams.Headers["org_id"] = "org_ss0DHGCZvPZSAMhQ";
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_InvalidOrgIdHeaderParam_OrgIdDoesNotExist_" + TestParams.Headers["org_id"];
            TestParams.ExpectedResponseCode = 500;
            TestParams.ExpectedResponseBody = "No tenantid found for org";
            testParamsList.Add(TestParams.Copy());

            //Tenant/OrgId Has 0 Schemes in taggingUI.TenantSaveScheme
            TestParams.Headers["org_id"] = "org_uER4G0blpM14QGA9";
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_InvalidOrgIdHeaderParam_OrgIdHasNoSchemes_" + TestParams.Headers["org_id"];
            TestParams.ExpectedResponseCode = 404;
            TestParams.ExpectedResponseBody = "Not Found";
            testParamsList.Add(TestParams.Copy());

            //dbo.TenantOrgMapping.Active = 0, dbo.Tenant.Active = 1, having Schemes in taggingUI.TenantSaveScheme              200
            //dbo.TenantOrgMapping.Active = 1, dbo.Tenant.Active = 0, having Schemes in taggingUI.TenantSaveScheme              200

            return testParamsList;
        }

        public List<TestObjects.TestStep> GetInvalidSchemeParamTests(string authValue, IEnvironment environment)
        {
            //Validates passing invalid Scheme Ids via Url for TenantSaveScheme endpoint
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.Url = GetUrl("GETQuery");
            TestParams.RequestType = "GET";

            TestParams.Headers.Remove("org_id");   //OrgId is included in GetHeaders base.  It doesn't currently affect the response when Url contains SchemeId, but I'm removing it for the scenario.

            //SchemeID Doesn't Exist (no record in taggingUI.TenantSaveScheme)
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_InvalidSchemeParamTest_SchemeIdDoesNotExist";
            TestParams.Url = GetUrl("GETQuery") + "CC1CB8C4-A17E-4E96-805C-CB6DC1CDC99B";
            TestParams.ExpectedResponseCode = 404;
            TestParams.ExpectedResponseBody = "Not Found";
            testParamsList.Add(TestParams.Copy());

            //Other scenarios?  Mismatch between schemeId and orgId in Headers?

            return testParamsList;
        }

        public List<TestObjects.TestStep> PostParamTests(string authValue, IEnvironment environment)
        {
            //Validates POST for TenantSaveScheme endpoint
            Dto postDto = new();
            JsonSerializerSettings settings = new() { NullValueHandling = NullValueHandling.Ignore };  //if dto property = null, ignore
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.Url = GetUrl("POST");
            TestParams.RequestType = "POST";

            //Omit Header OrgId
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_PostParamTests_OmitHeaderOrgId";
            TestParams.Headers.Remove("org_id");
            TestParams.Body = JsonConvert.SerializeObject(postDto, settings);
            TestParams.ExpectedResponseCode = 500;
            testParamsList.Add(TestParams.Copy());

            TestParams.Headers = GetHeaders(authValue, environment);  //resetting Headers for the rest of the tests

            //Duplicate SchemeID
            postDto.Id = "A9E9A694-7E21-47D9-9FC3-AB0330BFF449";
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_PostParamTests_DuplicateSchemeId_" + postDto.Id;
            TestParams.Body = JsonConvert.SerializeObject(postDto, settings);
            TestParams.ExpectedResponseCode = 500;
            TestParams.ExpectedResponseBody = "Cannot insert duplicate key in object 'taggingUI.TenantSaveScheme";
            testParamsList.Add(TestParams.Copy());

            ////Omit StartYear Key       Only validating ExpectedResponseBody contains "startYear": 0
            //postDto.StartYear = null;
            //TestParams.TestStep = GetType().Name + "_" + TestParams.RequestType + "_PostParamTests_OmitStartYearKey_" + postDto.Id;
            //TestParams.Body = JsonConvert.SerializeObject(postDto, settings);
            //TestParams.ExpectedResponseCode = 201;
            //TestParams.ExpectedResponseBody = "startYear\":0";
            //testParamsList.Add(TestParams.Copy());

            //postDto.StartYear = CurrentObject.StartYear;  //resetting request body StartYear for the rest of the tests

            ////Omit EndYear Key Only validating ExpectedResponseBody contains "endYear": null
            //postDto.EndYear = null;
            //TestParams.TestStep = GetType().Name + "_" + TestParams.RequestType + "_PostParamTests_OmitEndYearKey_" + postDto.Id;
            //TestParams.Body = JsonConvert.SerializeObject(postDto, settings);
            //TestParams.ExpectedResponseCode = 201;
            //TestParams.ExpectedResponseBody = "endYear\":null";
            //testParamsList.Add(TestParams.Copy());

            //postDto.EndYear = CurrentObject.EndYear;  //resetting request body EndYear for the rest of the tests

            ////Omit DataTaggingTypeId Key
            //postDto.DataTaggingTypeId = null;
            //TestParams.TestStep = GetType().Name + "_" + TestParams.RequestType + "_PostParamTests_OmitDataTaggingTypeIdKey_" + postDto.Id;
            //TestParams.Body = JsonConvert.SerializeObject(postDto, settings);
            //TestParams.ExpectedResponseCode = 500;
            //TestParams.ExpectedResponseBody = @"The INSERT statement conflicted with the FOREIGN KEY constraint \""FK_TenantSaveScheme_DataTaggingType\""";
            //testParamsList.Add(TestParams.Copy());

            //postDto.DataTaggingTypeId = CurrentObject.DataTaggingTypeId;  //resetting request body DataTaggingTypeId for the rest of the tests

            ////Omit DataModelId Key
            //postDto.DataModelId = null;
            //TestParams.TestStep = GetType().Name + "_" + TestParams.RequestType + "_PostParamTests_OmitDataModelIdKey_" + postDto.Id;
            //TestParams.Body = JsonConvert.SerializeObject(postDto, settings);
            //TestParams.ExpectedResponseCode = 500;
            //TestParams.ExpectedResponseBody = @"The INSERT statement conflicted with the FOREIGN KEY constraint \""FK_TenantSaveScheme_DataModel\""";
            //testParamsList.Add(TestParams.Copy());

            //postDto.DataModelId = CurrentObject.DataModelId;  //resetting request body DataModelId for the rest of the tests

            //Omit Scheme Id     Only validating ExpectedResponseBody contains orgId
            postDto.Id = null;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_PostParamTests_OmitSchemeId";
            TestParams.Body = JsonConvert.SerializeObject(postDto, settings);
            TestParams.ExpectedResponseCode = 201;
            TestParams.ExpectedResponseBody = "orgId\":\"org_ss0DHGCZvPZSAMhz";
            testParamsList.Add(TestParams.Copy());

            postDto.Id = Guid.NewGuid().ToString().ToUpper(); ;  //resetting request body Id for the rest of the tests

            //Other scenarios?
            //ID mismatch between Header OrgId and RequestBody OrgId
            //No Active = 201
            //

            return testParamsList;
        }
    }
}

