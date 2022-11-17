
namespace ci_automation_apitester.Objects
{
    public class ActiveTests
    {
        public Dictionary<string, bool> Active { get; set; }

        public ActiveTests()
        {
            Active = new Dictionary<string, bool>()

            {
                //DataTagging
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
                { "IntegrationMetadata.Integration", true },  
                { "IntegrationMetadata.IntegrationEndpoint", true },                     //BUGS for POST, PUT, GET by ID
                { "IntegrationMetadata.IntegrationType", true },                         
                { "IntegrationMetadata.TenantIntegration", true },                       //requires seeded data script
                { "IntegrationMetadata.TenantIntegrationConfigEloquaCIDT", false },      //BUG #300 POST returns tenantIntegrationConfigId instead of Id.  Breaks swapping ID for subsequent calls.  Also, has other validation issues.
                { "IntegrationMetadata.TenantIntegrationConfigSalesforce", false },      //current execution errors
                { "IntegrationMetadata.TenantIntegrationConfigSFTP", false },            //current execution errors
                { "IntegrationMetadata.TenantIntegrationConfigTM", false },              //current execution errors
                { "IntegrationMetadata.TenantIntegrationEndpoint", false },              //current execution errors due to need seeded data
                { "IntegrationMetadata.TenantSFTP", false },                             //Bug #288 500 on valid PUT (workflow/auth tests failing)

                //TenantMetadata  //true
                { "TenantMetadata.TenantOrgs", true },
                { "TenantMetadata.TenantRoles", true },
                { "TenantMetadata.Tenants", true },
            };
        }
        public bool GetEndpointIsActive(string endpoints)
        {
            if (!Active.ContainsKey(endpoints)) return true;

            return Active[endpoints];
        }
    }
}
