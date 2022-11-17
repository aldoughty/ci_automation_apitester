namespace ci_automation_apitester.Libs
{
    public static class ParseArguments
    {
        public static MessageData Execute(string[] args)
        {
            FluentCommandLineParser<MessageData> commandLineParams = new();
            commandLineParams.Setup(arg => arg.TenantId).As('t', "TenantId").Required();
            commandLineParams.Setup(arg => arg.BatchId).As('b', "BatchId").Required();
            commandLineParams.Setup(arg => arg.ApplicationUnderTest).As('u', "ApplicationUnderTest").Required();
            commandLineParams.Setup(arg => arg.EndpointUnderTest).As('x', "EndpointUnderTest").Required();
            commandLineParams.Setup(arg => arg.Environment).As('e', "Environment").SetDefault("Dev");
            commandLineParams.Setup(arg => arg.SnowflakeAccount).As('a', "SnowflakeAccount").SetDefault("af01");
            commandLineParams.Setup(arg => arg.SnowflakeWarehouse).As('w', "SnowflakeWarehouse").SetDefault("AQ_XSMALL");
            commandLineParams.Setup(arg => arg.SnowflakeRole).As('r', "SnowflakeRole").SetDefault("ROLE_SVC_WRITER");
            commandLineParams.Setup(arg => arg.Branch).As('h', "Branch").SetDefault("");
            ICommandLineParserResult parseOutput = commandLineParams.Parse(args);
            return commandLineParams.Object;
        }
        public static MessageData PullFromEnvVariables()
        {
            MessageData results = new();
            IConfiguration settings = SettingsConfig.Get();
            results.ApplicationUnderTest = SettingsHandler.GetSettingString("ApplicationUnderTest", settings);
            results.BatchId = SettingsHandler.GetSettingString("BatchId", settings);
            results.Branch = SettingsHandler.GetSettingString("Branch", settings, "");
            results.EndpointUnderTest = SettingsHandler.GetSettingString("EndpointUnderTest", settings);
            results.Environment = SettingsHandler.GetSettingString("Environment", settings, "Dev");
            results.BaseUrlEnvironment = SettingsHandler.GetSettingString("BaseUrlEnvironment", settings, "");
            results.SnowflakeAccount = SettingsHandler.GetSettingString("SnowflakeAccount", settings, "af01");
            results.SnowflakeRole = SettingsHandler.GetSettingString("SnowflakeRole", settings, "ROLE_SVC_WRITER");
            results.SnowflakeWarehouse = SettingsHandler.GetSettingString("SnowflakeWarehouse", settings, "AQ_XSMALL");
            results.TenantId = SettingsHandler.GetSettingString("TenantId", settings);

            return results;
        }
    }
}
