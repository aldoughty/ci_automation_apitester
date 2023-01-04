namespace ci_automation_apitester.Objects
{
    internal class TestSetupAndTeardown
    {
        internal TestSetupAndTeardown()
        {

        }

        private static Random random = new Random();

        public static string RandomTenantName()
        {
            //min 3, max 100; alpha (upper/lower), numeric 0-9, spaces and only special char '.,-& (' is omitted here bc it breaks the insert statements; also added multiple spaces so they'd be included more often)

            const string chars = "abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 0123456789 .,-& ";
            return new string(Enumerable.Repeat(chars, 100)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomTenantKey()
        {
            //min 3, max 15; alpha (stored as lower), numeric 0-9, no spaces or special char; must start with alpha

            const string firstChar = "abcdefghijklmnopqrstuvwxyz";
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string (
                Enumerable.Repeat(firstChar, 1)
                .Select(s => s[random.Next(s.Length)]
                ).Concat(
                Enumerable.Repeat(chars, 14)
                .Select(s => s[random.Next(s.Length)])
                ).ToArray());
        }
        public static string RandomTenantUrl()
        {
            //no min/max?; alpha (upper/lower), numeric 0-9, no spaces or special char; must start with /

            const string firstChar = "/";
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(
                Enumerable.Repeat(firstChar, 1)
                .Select(s => s[random.Next(s.Length)]
                ).Concat(
                Enumerable.Repeat(chars, 99)
                .Select(s => s[random.Next(s.Length)])
                ).ToArray());
        }
        internal static void RunTestSetup()
        {

            Dictionary<string, Func<string, string>> Setup = new()

            {
                //DataTagging
                //{ "DataTagging.Admin", TestSetupStatements.AdminSetup },                
                //{ "DataTagging.ColumnDefinition", TestSetupStatements.ColumnDefinitionSetup },     
                //{ "DataTagging.ColumnType", TestSetupStatements.ColumnTypeSetup },
                //{ "DataTagging.DataModel", TestSetupStatements.DataModelSetup },
                //{ "DataTagging.Dimension", TestSetupStatements.DimensionSetup },
                //{ "DataTagging.Entity", TestSetupStatements.EntitySetup },
                //{ "DataTagging.Hierarchy", TestSetupStatements.HierarchySetup },
                //{ "DataTagging.Season", TestSetupStatements.SeasonSetup },
                //{ "DataTagging.TagRule", TestSetupStatements.TagRuleSetup },
                //{ "DataTagging.TenantSaveScheme", TestSetupStatements.TenantSaveSchemeSetup },
                
                //IntegrationMetadata
                //{ "IntegrationMetadata.Integration", TestSetupStatements.IntegrationSetup },
                { "IntegrationMetadata.IntegrationEndpoint", TestSetupStatements.IntegrationEndpointSetup },
                { "IntegrationMetadata.IntegrationTenant", TestSetupStatements.IntegrationTenantSetup },
                //{ "IntegrationMetadata.IntegrationType", TestSetupStatements.IntegrationTypeSetup },
                { "IntegrationMetadata.TenantGPG", TestSetupStatements.TenantGPGSetup },
                { "IntegrationMetadata.TenantIntegration", TestSetupStatements.TenantIntegrationSetup },                      
                //{ "IntegrationMetadata.TenantIntegrationConfigEloquaCIDT", TestSetupStatements.TenantIntegrationConfigEloquaCIDTSetup },      
                { "IntegrationMetadata.TenantIntegrationConfigSalesforce", TestSetupStatements.TenantIntegrationConfigSalesforceSetup },
                { "IntegrationMetadata.TenantIntegrationConfigSFTP", TestSetupStatements.TenantIntegrationConfigSFTPSetup },            
                { "IntegrationMetadata.TenantIntegrationConfigTM", TestSetupStatements.TenantIntegrationConfigTMSetup },              
                { "IntegrationMetadata.TenantIntegrationEndpoint", TestSetupStatements.TenantIntegrationEndpointSetup },              
                { "IntegrationMetadata.TenantSFTP", TestSetupStatements.TenantSFTPSetup },                             

                //TenantMetadata
                { "TenantMetadata.TenantOrgs", TestSetupStatements.TenantOrgsSetup },
                //{ "TenantMetadata.TenantRoles", TestSetupStatements.TenantRolesSetup },
                { "TenantMetadata.Tenants", TestSetupStatements.TenantsSetup },

            };

            IEnumerable<string> keys = ActiveTests.GetActiveEndpoints();

            string results = "";
            
            keys.ToList().ForEach
            (
                item =>
                {
                    if (Setup.ContainsKey(item))
                    { results += Setup[item](SecretsManager.SnowflakeDatabaseEnvironment()); }
                }
            );

            //To execute multiple statements, wrap in BEGIN...END;
            if (results != "") { DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), "BEGIN " + results + " END;"); }
        }
        internal static void RunTestTeardown()
        {
            Dictionary<string, Func<string, string>> Setup = new()

            {
                //DataTagging
                //{ "DataTagging.Admin", TestTeardownStatements.AdminTeardown },                
                //{ "DataTagging.ColumnDefinition", TestTeardownStatements.ColumnDefinitionTeardown },     
                //{ "DataTagging.ColumnType", TestTeardownStatements.ColumnTypeTeardown },
                //{ "DataTagging.DataModel", TestTeardownStatements.DataModelTeardown },
                //{ "DataTagging.Dimension", TestTeardownStatements.DimensionTeardown },
                //{ "DataTagging.Entity", TestTeardownStatements.EntityTeardown },
                //{ "DataTagging.Hierarchy", TestTeardownStatements.HierarchyTeardown },
                //{ "DataTagging.Season", TestTeardownStatements.SeasonTeardown },
                //{ "DataTagging.TagRule", TestTeardownStatements.TagRuleTeardown },
                //{ "DataTagging.TenantSaveScheme", TestTeardownStatements.TenantSaveSchemeTeardown },
                
                //IntegrationMetadata
                //{ "IntegrationMetadata.Integration", TestTeardownStatements.IntegrationTeardown },
                { "IntegrationMetadata.IntegrationEndpoint", TestTeardownStatements.IntegrationEndpointTeardown },
                { "IntegrationMetadata.IntegrationTenant", TestTeardownStatements.IntegrationTenantTeardown },
                //{ "IntegrationMetadata.IntegrationType", TestTeardownStatements.IntegrationTypeTeardown },
                { "IntegrationMetadata.TenantGPG", TestTeardownStatements.TenantGPGTeardown },
                { "IntegrationMetadata.TenantIntegration", TestTeardownStatements.TenantIntegrationTeardown },                      
                //{ "IntegrationMetadata.TenantIntegrationConfigEloquaCIDT", TestTeardownStatements.TenantIntegrationConfigEloquaCIDTTeardown },      
                { "IntegrationMetadata.TenantIntegrationConfigSalesforce", TestTeardownStatements.TenantIntegrationConfigSalesforceTeardown },
                { "IntegrationMetadata.TenantIntegrationConfigSFTP", TestTeardownStatements.TenantIntegrationConfigSFTPTeardown },            
                { "IntegrationMetadata.TenantIntegrationConfigTM", TestTeardownStatements.TenantIntegrationConfigTMTeardown },              
                { "IntegrationMetadata.TenantIntegrationEndpoint", TestTeardownStatements.TenantIntegrationEndpointTeardown },              
                { "IntegrationMetadata.TenantSFTP", TestTeardownStatements.TenantSFTPTeardown },                             

                //TenantMetadata
                { "TenantMetadata.TenantOrgs", TestTeardownStatements.TenantOrgsTeardown },
                //{ "TenantMetadata.TenantRoles", TestTeardownStatements.TenantRolesTeardown },
                { "TenantMetadata.Tenants", TestTeardownStatements.TenantsTeardown },

            };

            IEnumerable<string> keys = ActiveTests.GetActiveEndpoints();

            string results = "";

            keys.ToList().ForEach
            (
                item =>
                {
                    if (Setup.ContainsKey(item))
                    { results += Setup[item](SecretsManager.SnowflakeDatabaseEnvironment()); }
                }
            );

            //To execute multiple statements, wrap in BEGIN...END;
            if (results != "") { DataBaseExecuter.ExecuteCommand("Snowflake", SecretsManager.SnowflakeConnectionString(), "BEGIN " + results + " END;"); }
        }

    }
}
