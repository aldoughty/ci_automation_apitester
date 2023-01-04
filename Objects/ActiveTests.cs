
namespace ci_automation_apitester.Objects
{
    public class ActiveTests
    {
        public Dictionary<string, bool> Active { get; set; }

        public ActiveTests()
        {
            Active = new Dictionary<string, bool>()

            {
                //DataTagging  ***Turned off until we go back to it
                { "DataTagging.Admin", false },                //NOTE: Auth is turned completely off for this endpoint, so Auth tests expecting 401 will fail with 200s
                { "DataTagging.ColumnDefinition", false },     //NOTE: Endpoint is turned off because Type is returning as NULL only on GET of specific ColumnDefinitionId in GetParameterTests
                { "DataTagging.ColumnType", false },
                { "DataTagging.DataModel", false },
                { "DataTagging.Dimension", false },
                { "DataTagging.Entity", false },
                { "DataTagging.Hierarchy", false },
                { "DataTagging.Season", false },
                { "DataTagging.TagRule", false },
                { "DataTagging.TenantSaveScheme", false },
                
                //IntegrationMetadata
                { "IntegrationMetadata.Integration", false },  
                { "IntegrationMetadata.IntegrationEndpoint", false },                    
                { "IntegrationMetadata.IntegrationTenant", false },
                { "IntegrationMetadata.IntegrationType", false },
                { "IntegrationMetadata.TenantGPG", false },
                { "IntegrationMetadata.TenantIntegration", false },
                { "IntegrationMetadata.TenantIntegrationConfigEloquaCIDT", false },      //turned off bc we're reworking to just have one CIDT endpoint
                { "IntegrationMetadata.TenantIntegrationConfigSalesforce", false },      //PUTs are failing for existing TenantIntegrationId
                { "IntegrationMetadata.TenantIntegrationConfigSFTP", false },            
                { "IntegrationMetadata.TenantIntegrationConfigTM", false },
                { "IntegrationMetadata.TenantIntegrationEndpoint", true },              //BUGS?
                { "IntegrationMetadata.TenantSFTP", false },                                                    

                //TenantMetadata
                { "TenantMetadata.TenantOrgs", false }, 
                { "TenantMetadata.TenantRoles", false },
                { "TenantMetadata.Tenants", false },                                     
            };
        }
        public bool GetEndpointIsActive(string endpoints)
        {
            if (!Active.ContainsKey(endpoints)) return true;

            return Active[endpoints];
        }
        public static IEnumerable<string> GetActiveEndpoints()
        {
            ActiveTests ActiveTests = new ActiveTests();
            
            IEnumerable<string> keys = ActiveTests.Active.Where(x => x.Value == true).Select(x => x.Key);

            return keys;
        }
    }
}
