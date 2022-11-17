namespace ci_automation_apitester.Queries
{
    public class TenantMetadataQueries
    {  
        public string QueryAllTenants(string env)
        {
            //Returns all Tenants in dbo.Tenant for GET /api/Tenants
            var results = $@"SELECT TENANTID AS ID, TENANT_KEY AS KEY, TENANTNAME AS NAME, TENANTURL AS URL, TENANTTYPE AS TYPE, TENANTSUBTYPE AS SUBTYPE, 
                             SHORTNAME, NICKNAME, MASCOT, ACTIVE, ORCHARDNAME, ORCHARDTYPE, ISDISCOVERYCLIENT, DISCOVERYCLIENTNAME, ISCENTRALINTELLIGENCECLIENT, 
                             PARENTTENANTID FROM CI_METADATA{env}.DBO.TENANT";

            return results;
        }
        public string QueryAllTenantRoles(string env)
        {
            //Returns Tenant Roles in dbo.TenantRoles for GET /api/TenantRoles
            var results = $@"SELECT TENANTID, SNOWFLAKEROLE FROM CI_METADATA{env}.DBO.TENANTROLES";

            return results;
        }
        public string QueryAllTenantKeysHavingTenantRoles(string env)
        {
            //Returns all TenantIds & TenantKeys in dbo.Tenant HAVING records in dbo.TenantRoles for GET /api/TenantRoles/{tenantKey}
            var results = $@"SELECT TENANTID, TENANT_KEY FROM CI_METADATA{env}.DBO.TENANT WHERE TENANTID IN (SELECT TENANTID 
                            FROM CI_METADATA{env}.DBO.TENANTROLES)";

            return results;
        }
        public string QueryAllTenantKeysNotHavingTenantRoles(string env)
        {
            //Returns all TenantIds & TenantKeys in dbo.Tenant NOT HAVING records in dbo.TenantRoles for GET /api/TenantRoles/{tenantKey}
            var results = $@"SELECT TENANTID, TENANT_KEY FROM CI_METADATA{env}.DBO.TENANT WHERE TENANTID NOT IN (SELECT TENANTID 
                            FROM CI_METADATA{env}.DBO.TENANTROLES)";

            return results;
        }
        public string QueryTenantRolesByTenantId(string tenantId, string env)
        {
            //Returns specific Tenant Role in dbo.TenantRoles for GET /api/TenantRoles/{tenantRole}
            var results = $@"SELECT TENANTID, SNOWFLAKEROLE FROM CI_METADATA{env}.DBO.TENANTROLES WHERE TENANTID = '{tenantId}'";

            return results;
        }
        public string QueryAllTenantOrgs(string env)
        {
            //Returns Tenant Orgs in dbo.TenantOrgMapping for GET /api/TenantOrgs
            var results = $@"SELECT ID, TENANTID, ORGID, ACTIVE FROM CI_METADATA{env}.DBO.TENANTORG";

            return results;
        }
    }
}

