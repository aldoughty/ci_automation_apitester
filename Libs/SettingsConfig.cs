namespace ci_automation_apitester.Libs
{

    public static class SettingsConfig
    {
        public static IConfiguration Get()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
