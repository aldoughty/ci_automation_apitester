namespace ci_automation_apitester.ApiDto.DataTagging
{
    public class Entity : BaseRestApi
    {
        const string Url = "/tagging/api/entity";
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
                Name = "Automation Entity";
                Description = "What What";
                Active = true;
            }
        }
        public override string GetUrl(string action)
        {
            switch (action)
            {
                case "GET":                         //Url - returns all; Url + "/" + CurrentObject.Id - returns specific
                case "POST":
                case "PUT":
                    return Url;
                case "PATCH":
                    return Url + "/" + CurrentObject.Id;
                case "DELETE":
                    return Url + "/" + CurrentObject.Id;
                default: return Url;
            }
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
            string command = "SELECT Id,Name,Description,Active FROM taggingUI.Entity";
            DataTable dt = DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), command);
            return JsonConvert.SerializeObject(dt);
        }
        public override void CleanUp(MessageData messageData, string currentId)
        {
            string command = $@"DELETE FROM taggingUI.Entity WHERE Id ='{currentId}'";
            DataBaseExecuter.ExecuteCommand("SQL", SecretsManager.SQLConnectionString(), command);
        }
    }
}
