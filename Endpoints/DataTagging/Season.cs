namespace ci_automation_apitester.ApiDto.DataTagging
{
    public class Season : BaseRestApi
    {
        const string Endpoint = "/tagging/api/season";
        public Dto CurrentObject = new();

        public class Dto
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public bool Active { get; set; }
            public Dto()
            {
                Id = Guid.NewGuid().ToString().ToUpper();
                Name = "Default Dto Season Description";
                Description = "Default Dto Season Description";
                Active = true;
            }
        }
        public override string GetUrl(string action)
        {
            switch (action)
            {
                case "GET":                         //Url - returns all; Url + "?entityId=" + CurrentObject.Id - returns specific
                case "POST":
                case "PUT":
                    return Endpoint;
                case "PATCH":
                    return Endpoint + "/" + CurrentObject.Id;
                case "DELETE":
                    return Endpoint + "/" + CurrentObject.Id;
                case "GETQuery":
                    return Endpoint + "?EntityId=";
                default: return Endpoint;
            }
        }
        public override List<TestObjects.TestStep> GetParameterTests(MessageData messageData, string authValue, IEnvironment environment)
        {
            DataTaggingQueries query = new();
            List<TestObjects.TestStep> testParamsList = new();
            SetTestParams(authValue, environment);

            TestParams.Url = GetUrl("GET");
            TestParams.RequestType = "GET";

            DataTable seasonDt = DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), query.QuerySeasonByEntityId());
            foreach (DataRow dataRow in seasonDt.Rows)
            {
                TestParams.TestStepName = GetType().Name + "_GET_EntityID=" + dataRow["entityId"].ToString();
                TestParams.Url = GetUrl("GETQuery") + dataRow["entityId"].ToString();
                TestParams.ExpectedResponseBody = dataRow["seasons"].ToString();

                testParamsList.Add(TestParams.Copy());
            }

            return testParamsList;
        }
        public override string GetPostBody()
        {
            return JsonConvert.SerializeObject(CurrentObject);
        }
        public override string GetPutBody()
        {
            CurrentObject.Name = "Automation Change My Name";
            CurrentObject.Description = "So Cool";
            return JsonConvert.SerializeObject(CurrentObject);
        }
        public override string GetPatchBody()
        {
            CurrentObject.Name = "Patch Name Update";
            CurrentObject.Description = "Patch Description Update";
            return "[{\"op\": \"replace\", \"path\": \"name\", \"value\":\"Patch Name Update\"},{\"op\": \"replace\", \"path\": \"description\", \"value\":\"Patch Description Update\"}]";
        }
        public override string GetDeleteBody()
        {
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
        public override object GetResults(MessageData messageData, Dictionary<string, string> parameters)
        {
            string whereClause = parameters.Keys.Aggregate("", (current, key) => $@"{current} a.{key} ='{parameters[key]}' AND");
            whereClause = string.IsNullOrEmpty(whereClause) ? "" : "WHERE " + whereClause.Remove(whereClause.Length - 3, 3);

            string command = "";

            if (whereClause == "")

                //URL:  baseUrl + Season
                //Returns all seasons in taggingUI.Season
                command = $@"SELECT Id,Name,Description,Active 
                            FROM taggingUI.Season
                            {whereClause}";

            else
                //URL:  baseUrl + Season?entityId=[entityId]
                //Entity must have entry in taggingUI.EntitySeason for a season
                //Could return multiple
                command = $@"SELECT s.Id,s.Name,s.Description,s.Active
                            FROM taggingUI.EntitySeason a
                            JOIN taggingUi.Entity e ON e.Id = a.EntityId
                            JOIN taggingUI.Season s ON s.Id = a.SeasonId
                            {whereClause}";

            DataTable dt = DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), command);
            return JsonConvert.SerializeObject(dt);
        }
        public override List<TestObjects.TestStep> GetFreeFormTests(string authValue, IEnvironment environment)
        {
            List<TestObjects.TestStep> result = new();
            result.AddRange(GetFreeFormPostTests(authValue, environment));
            result.AddRange(GetFreeFormPatchTests(authValue, environment));
            result.AddRange(GetFreeFormPutTests(authValue, environment));
            result.AddRange(GetFreeFormDeleteTests(authValue, environment));
            result.AddRange(GetFreeFormGetTests(authValue, environment));

            return result;
        }
        public List<TestObjects.TestStep> GetFreeFormPostTests(string authValue, IEnvironment environment)
        {
            List<TestObjects.TestStep> testParamsList = new();
            Dto freeFormBodyDto = new();
            JsonSerializerSettings settings = new() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore };  //if dto property = null, ignore
            SetTestParams(authValue, environment);

            TestParams.Url = GetUrl("POST");
            TestParams.RequestType = "POST";


            //No Id key returns 201
            freeFormBodyDto.Id = null;
            freeFormBodyDto.Name = "No Id Key";
            freeFormBodyDto.Description = "No Id Key";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_No Id Key";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 201;
            testParamsList.Add(TestParams.Copy());

            //Duplicate ID returns 500
            freeFormBodyDto.Id = "4203E26D-BE88-47B5-8726-6635F2E6C180";
            freeFormBodyDto.Name = "Duplicate Id";
            freeFormBodyDto.Description = "Duplicate Id";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Duplicate Id";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 500;
            testParamsList.Add(TestParams.Copy());

            //No Name key returns 500
            freeFormBodyDto.Id = Guid.NewGuid().ToString().ToUpper();
            freeFormBodyDto.Name = null;
            freeFormBodyDto.Description = "No Name Key";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_No Name Key";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 500;
            testParamsList.Add(TestParams.Copy());

            //No Description key defaults Description = NULL
            freeFormBodyDto.Id = Guid.NewGuid().ToString().ToUpper();
            freeFormBodyDto.Name = "No Description Key";
            freeFormBodyDto.Description = null;
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_No Description Key";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 201;
            testParamsList.Add(TestParams.Copy());

            //Blank Name key returns 500
            freeFormBodyDto.Id = Guid.NewGuid().ToString().ToUpper();
            freeFormBodyDto.Name = "";
            freeFormBodyDto.Description = "Blank Name Key";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Blank Name Key";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 500;
            testParamsList.Add(TestParams.Copy());

            //Blank Description key defaults Description = NULL
            freeFormBodyDto.Id = Guid.NewGuid().ToString().ToUpper();
            freeFormBodyDto.Name = "Blank Description Key";
            freeFormBodyDto.Description = "";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Blank Description Key";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 201;
            testParamsList.Add(TestParams.Copy());

            //No Active Key defaults Active = 0 (false)  Unable to verify bc we can't null bool.
            //freeFormBodyDto.Id = Guid.NewGuid().ToString().ToUpper();
            //freeFormBodyDto.Name = "No Active Key";
            //freeFormBodyDto.Description = "No Active Key";
            //freeFormBodyDto.Active = null;
            //results.Add(new FreeFormBody("No Active Key", GetUrl("POST"), JsonConvert.SerializeObject(freeFormBodyDto, settings), 201, "POST"));

            //Duplicate Name returns 201
            freeFormBodyDto.Id = Guid.NewGuid().ToString().ToUpper();
            freeFormBodyDto.Name = "Do Not Delete";
            freeFormBodyDto.Description = "Duplicate Name Key";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Duplicate Name";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 201;
            testParamsList.Add(TestParams.Copy());

            //Duplicate Description returns 201
            freeFormBodyDto.Id = Guid.NewGuid().ToString().ToUpper();
            freeFormBodyDto.Name = "Duplicate Description Key";
            freeFormBodyDto.Description = "Do Not Delete";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Duplicate Description";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 201;
            testParamsList.Add(TestParams.Copy());

            //Active = false returns 201
            freeFormBodyDto.Id = Guid.NewGuid().ToString().ToUpper();
            freeFormBodyDto.Name = "Inactive Season";
            freeFormBodyDto.Description = "Inactive Season";
            freeFormBodyDto.Active = false;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Inactive Season";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 201;
            testParamsList.Add(TestParams.Copy());

            //Special Char in Name, Description returns 201
            freeFormBodyDto.Id = Guid.NewGuid().ToString().ToUpper();
            freeFormBodyDto.Name = "Special Char:  `~!@#$%^&*()_+-={}|[]:;'<>?,./";
            freeFormBodyDto.Description = "Special Char:  `~!@#$%^&*()_+-={}|[]:;'<>?,./";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Special Char Name, Description";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 201;
            testParamsList.Add(TestParams.Copy());

            //NOTES:  ID validation?  free form posts for specific ID in url, Active = 1, 2, yes, no, string, blank

            return testParamsList;
        }
        public List<TestObjects.TestStep> GetFreeFormPatchTests(string authValue, IEnvironment environment)
        {
            List<TestObjects.TestStep> testParamsList = new();
            Dto freeFormBodyDto = new();
            JsonPatchDocument seasonPatchBody = new();

            SetTestParams(authValue, environment);
            TestParams.RequestType = "PATCH";

            //Single Patch Object - ID Not Found
            freeFormBodyDto.Id = "99999999-9999-9999-9999-999999999999";
            seasonPatchBody.Replace("name", "ID Not Found 404");
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Single Object_ID Not Found 404";
            TestParams.Url = GetUrl("default") + "/" + freeFormBodyDto.Id;
            TestParams.Body = JsonConvert.SerializeObject(seasonPatchBody);
            TestParams.ExpectedResponseCode = 404;
            testParamsList.Add(TestParams.Copy());

            //Single Patch Object - ID Not Found
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FZZ";
            seasonPatchBody.Replace("name", "ID Not Found 500");
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Single Object_ID Not Found 500";
            TestParams.Url = GetUrl("default") + "/" + freeFormBodyDto.Id;
            TestParams.Body = JsonConvert.SerializeObject(seasonPatchBody);
            TestParams.ExpectedResponseCode = 500;
            testParamsList.Add(TestParams.Copy());

            //Single Patch Object - Replace Name
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            seasonPatchBody.Replace("name", "Replace Name");
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Single Object_Replace Name";
            TestParams.Url = GetUrl("default") + "/" + freeFormBodyDto.Id;
            TestParams.Body = JsonConvert.SerializeObject(seasonPatchBody);
            TestParams.ExpectedResponseCode = 204;
            testParamsList.Add(TestParams.Copy());

            //Single Patch Object  - Replace Description
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            seasonPatchBody.Replace("description", "Replace Description");
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Single Object_Replace Description";
            TestParams.Url = GetUrl("default") + "/" + freeFormBodyDto.Id;
            TestParams.Body = JsonConvert.SerializeObject(seasonPatchBody);
            TestParams.ExpectedResponseCode = 204;
            testParamsList.Add(TestParams.Copy());

            //Single Patch Object  - Replace Active True  (bool true, false, 1, 0)
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            seasonPatchBody.Replace("active", "true");
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Single Object_Replace Active True";
            TestParams.Url = GetUrl("default") + "/" + freeFormBodyDto.Id;
            TestParams.Body = JsonConvert.SerializeObject(seasonPatchBody);
            TestParams.ExpectedResponseCode = 204;
            testParamsList.Add(TestParams.Copy());

            //Single Patch Object - Replace Name - Special Char
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            seasonPatchBody.Replace("name", "`~!@#$%^&*()_+-={}|[]:;'<>?,./");
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Single Object_Replace Name_Special Char";
            TestParams.Url = GetUrl("default") + "/" + freeFormBodyDto.Id;
            TestParams.Body = JsonConvert.SerializeObject(seasonPatchBody);
            TestParams.ExpectedResponseCode = 204;
            testParamsList.Add(TestParams.Copy());

            //Single Patch Object  - Replace Description - Special Char
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            seasonPatchBody.Replace("description", "`~!@#$%^&*()_+-={}|[]:;'<>?,./");
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Single Object_Replace Description_Special Char";
            TestParams.Url = GetUrl("default") + "/" + freeFormBodyDto.Id;
            TestParams.Body = JsonConvert.SerializeObject(seasonPatchBody);
            TestParams.ExpectedResponseCode = 204;
            testParamsList.Add(TestParams.Copy());

            //Single Patch Object - Replace Duplicate Name
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            seasonPatchBody.Replace("name", "Replace Duplicate Name");
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Single Object_Replace Duplicate Name";
            TestParams.Url = GetUrl("default") + "/" + freeFormBodyDto.Id;
            TestParams.Body = JsonConvert.SerializeObject(seasonPatchBody);
            TestParams.ExpectedResponseCode = 204;
            testParamsList.Add(TestParams.Copy());

            //Single Patch Object  - Replace Duplicate Description
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            seasonPatchBody.Replace("description", "Replace Duplicate Description");
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Single Object_Replace Duplicate Description";
            TestParams.Url = GetUrl("default") + "/" + freeFormBodyDto.Id;
            TestParams.Body = JsonConvert.SerializeObject(seasonPatchBody);
            TestParams.ExpectedResponseCode = 204;
            testParamsList.Add(TestParams.Copy());

            //Single Patch Object  - Replace Description NULL
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            seasonPatchBody.Replace("description", "");
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Single Object_Replace Description NULL";
            TestParams.Url = GetUrl("default") + "/" + freeFormBodyDto.Id;
            TestParams.Body = JsonConvert.SerializeObject(seasonPatchBody);
            TestParams.ExpectedResponseCode = 204;
            testParamsList.Add(TestParams.Copy());

            //Multiple Patch Object - Replace All
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            seasonPatchBody.Replace("name", "U of QA Season");
            seasonPatchBody.Replace("description", "U of QA Season");
            seasonPatchBody.Replace("active", "false");
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Single Object_Replace All";
            TestParams.Url = GetUrl("default") + "/" + freeFormBodyDto.Id;
            TestParams.Body = JsonConvert.SerializeObject(seasonPatchBody);
            TestParams.ExpectedResponseCode = 204;
            testParamsList.Add(TestParams.Copy());

            //**ISSUE** Should not be able to null Name.  A GET of all seasons or any other request on season with Name = null returns 500.
            //Single Patch Object  - Replace Name NULL
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            seasonPatchBody.Replace("name", "");
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Single Object_Replace Name NULL_EXPECTED FAILURE";
            TestParams.Url = GetUrl("default") + "/" + freeFormBodyDto.Id;
            TestParams.Body = JsonConvert.SerializeObject(seasonPatchBody);
            TestParams.ExpectedResponseCode = 500;
            testParamsList.Add(TestParams.Copy());

            //NOTES:  no id in url? other operations (justin looking into disabling)

            return testParamsList;

        }
        public List<TestObjects.TestStep> GetFreeFormPutTests(string authValue, IEnvironment environment)
        {
            List<TestObjects.TestStep> testParamsList = new();
            Dto freeFormBodyDto = new();
            JsonSerializerSettings settings = new() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore };  //if dto property = null, ignore
            SetTestParams(authValue, environment);

            TestParams.Url = GetUrl("PUT");
            TestParams.RequestType = "PUT";

            //No Id key returns 500
            freeFormBodyDto.Id = null;
            freeFormBodyDto.Name = "No Id Key";
            freeFormBodyDto.Description = "No Id Key";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_No Id Key";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 500;
            testParamsList.Add(TestParams.Copy());

            //Blank Id key returns 400 "error converting value to system.guid"
            freeFormBodyDto.Id = "";
            freeFormBodyDto.Name = "Blank Id Key";
            freeFormBodyDto.Description = "Blank Id Key";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Blank Id Key";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 400;
            testParamsList.Add(TestParams.Copy());

            //Id doesn't exist returns 500 "unable to update null object"
            //freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FF0";
            freeFormBodyDto.Id = CurrentObject.Id;
            freeFormBodyDto.Name = "Id Doesn't Exist";
            freeFormBodyDto.Description = "Id Doesn't Exist";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Id Doesn't Exist";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 500;
            testParamsList.Add(TestParams.Copy());

            //No Name Key
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            freeFormBodyDto.Name = null;
            freeFormBodyDto.Description = "No Name Key";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_No Name Key";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 500;
            testParamsList.Add(TestParams.Copy());

            //Blank Name Key
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            freeFormBodyDto.Name = "";
            freeFormBodyDto.Description = "Blank Name Key";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Blank Name Key";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 500;
            testParamsList.Add(TestParams.Copy());

            //No Description Key
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            freeFormBodyDto.Name = "U of QA Season";
            freeFormBodyDto.Description = null;
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_No Description Key";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 204;
            testParamsList.Add(TestParams.Copy());

            //Blank Description Key
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            freeFormBodyDto.Name = "U of QA Season";
            freeFormBodyDto.Description = "";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Blank Description Key";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 204;
            testParamsList.Add(TestParams.Copy());

            ////No Active Key defaults Active = 0 (false)  Unable to verify bc we can't null bool.
            //freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            //freeFormBodyDto.Name = "U of QA Season";
            //freeFormBodyDto.Description = "U of QA Season";
            //freeFormBodyDto.Active = null;
            //results.Add(new FreeFormBody("No Active Key", GetUrl("PUT"), JsonConvert.SerializeObject(freeFormBodyDto, settings), 204, "PUT"));

            ////Blank Active Key returns 400 "error converting value to null"  Unable to verify bc we can't null bool
            //freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            //freeFormBodyDto.Name = "U of QA Season";
            //freeFormBodyDto.Description = "U of QA Season";
            //freeFormBodyDto.Active = "";
            //results.Add(new FreeFormBody("Blank Active Key", GetUrl("PUT"), JsonConvert.SerializeObject(freeFormBodyDto, settings), 400, "PUT"));

            //Name Special Char
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            freeFormBodyDto.Name = "`~!@#$%^&*()_+-={}|[]:;'<>?,./";
            freeFormBodyDto.Description = "U of QA Season";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Name Special Char";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 204;
            testParamsList.Add(TestParams.Copy());

            //Description Special Char
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            freeFormBodyDto.Name = "U of QA Season";
            freeFormBodyDto.Description = "`~!@#$%^&*()_+-={}|[]:;'<>?,./";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Description Special Char";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 204;
            testParamsList.Add(TestParams.Copy());

            //Duplicate Name
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            freeFormBodyDto.Name = "Duplicate Name";
            freeFormBodyDto.Description = "U of QA Season";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Duplicate Name";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 204;
            testParamsList.Add(TestParams.Copy());

            //Duplicate Description
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            freeFormBodyDto.Name = "U of QA Season";
            freeFormBodyDto.Description = "Duplicate Description";
            freeFormBodyDto.Active = true;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Duplicate Description";
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto, settings);
            TestParams.ExpectedResponseCode = 204;
            testParamsList.Add(TestParams.Copy());

            //NOTES:  Active = 1, 2, yes, no, string, blank

            return testParamsList;

        }
        public List<TestObjects.TestStep> GetFreeFormDeleteTests(string authValue, IEnvironment environment)
        {
            List<TestObjects.TestStep> testParamsList = new();
            Dto freeFormBodyDto = new();
            SetTestParams(authValue, environment);
            TestParams.RequestType = "DELETE";

            //Delete ID Not Found "attempted to deactivate an entity that wasn't found"
            freeFormBodyDto.Id = "99999999-9999-9999-9999-999999999999";
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_ID Not Found 500";
            TestParams.Url = GetUrl("DELETE") + freeFormBodyDto.Id;
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto);
            TestParams.ExpectedResponseCode = 500;
            testParamsList.Add(TestParams.Copy());

            //Delete ID Not Converted to GUID "id couldn't be converted to guid"
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FZZ";
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_ID Not Converted to GUID 500";
            TestParams.Url = GetUrl("default") + "/" + freeFormBodyDto.Id;
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto);
            TestParams.ExpectedResponseCode = 500;
            testParamsList.Add(TestParams.Copy());

            //Delete No ID
            freeFormBodyDto.Id = null;
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_No ID Provided in URL 404";
            TestParams.Url = GetUrl(default) + "/" + freeFormBodyDto.Id;
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto);
            TestParams.ExpectedResponseCode = 404;
            testParamsList.Add(TestParams.Copy());

            //Delete Success (Active Updated to false/0)
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FFD";
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_Delete Success";
            TestParams.Url = GetUrl("default") + "/" + freeFormBodyDto.Id;
            TestParams.Body = JsonConvert.SerializeObject(freeFormBodyDto);
            TestParams.ExpectedResponseCode = 204;
            testParamsList.Add(TestParams.Copy());

            return testParamsList;
        }
        public List<TestObjects.TestStep> GetFreeFormGetTests(string authValue, IEnvironment environment)
        {
            List<TestObjects.TestStep> testParamsList = new();
            Dto freeFormBodyDto = new();
            SetTestParams(authValue, environment);
            TestParams.RequestType = "GET";

            //Get ID Doesn't Exist
            freeFormBodyDto.Id = "99999999-9999-9999-9999-999999999999";
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_ID Doesn't Exist 200";
            TestParams.Url = GetUrl("GETQuery") + freeFormBodyDto.Id;
            TestParams.ExpectedResponseCode = 200;
            TestParams.ExpectedResponseBody = "[]";
            testParamsList.Add(TestParams.Copy());

            //Get ID Not Converted to GUID "id couldn't be converted to guid"
            freeFormBodyDto.Id = "6AC80966-8281-41A1-8206-92BC8E910FZZ";
            TestParams.TestType = "ValidateResponseContainsString";
            TestParams.TestStepName = GetType().Name + "_" + TestParams.RequestType + "_FreeFormBodyTest_ID Not Converted to GUID 500";
            TestParams.Url = GetUrl("GETQuery") + freeFormBodyDto.Id;
            TestParams.ExpectedResponseCode = 500;
            TestParams.ExpectedResponseBody = "id couldn't be converted to guid";
            testParamsList.Add(TestParams.Copy());

            //get all when there are none?

            return testParamsList;
        }
        public override void CleanUp(MessageData messageData, string currentId)
        {
            string command = $@"DELETE FROM taggingUI.Season WHERE Id ='{currentId}'";
            DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), command);
        }
    }
}

