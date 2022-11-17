namespace ci_automation_apitester.Objects
{
    public class MessageData
    {
        public StandardTestParameters StandardTestParameters { get; set; }
        //Need this because Fluent Command Line Parser will not do nested objects so we need to hold them in both places
        //Since we pass StandardTestParameters around as arguments to other functions
        public string ApplicationUnderTest { get; set; }
        public string BatchId { get; set; }
        public string Branch { get; set; }
        public string EndpointUnderTest { get; set; }
        public string Environment { get; set; }
        public string BaseUrlEnvironment { get; set; }
        public string SnowflakeAccount { get; set; }
        public string SnowflakeRole { get; set; }
        public string SnowflakeWarehouse { get; set; }
        public string TenantId { get; set; }
        public string CurrentId { get; set; }

        public MessageData()
        {
            IConfiguration settings = SettingsConfig.Get();
            StandardTestParameters = new StandardTestParameters();
            StandardTestParameters.AutomationMiroService = GetType().Namespace;
            StandardTestParameters.LocalLogs = Convert.ToBoolean(SettingsHandler.GetSettingString("LocalLogs", settings));
        }
        public void ValidateContent()
        {
            StandardTestParameters.BatchId = BatchId;
            StandardTestParameters.ApplicationUnderTest = ApplicationUnderTest;
            StandardTestParameters.Environment = Environment;
            TenantId.Should().NotBeNullOrEmpty("TenantId is Empty");
            StandardTestParameters.BatchId.Should().NotBeNullOrEmpty("BatchId Is Empty");
            StandardTestParameters.Environment.Should().NotBeNullOrEmpty("Environment is EmptyCollection");
            StandardTestParameters.ApplicationUnderTest.Should().NotBeNullOrEmpty("Application Under Test is Empty");
            EndpointUnderTest.Should().NotBeNullOrEmpty("Endpoint Under Test is Empty");
            SecretsManager.Setup(Environment, SnowflakeAccount, SnowflakeWarehouse);
            string cs = SecretsManager.SQLConnectionString();
            StandardTestParameters.TenantName = string.IsNullOrEmpty(TenantId) ? "Multi-Tenant" : TenantData.GetTenantName(SecretsManager.SQLConnectionString(), TenantId);
        }
    }
}
