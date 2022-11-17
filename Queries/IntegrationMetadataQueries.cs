namespace ci_automation_apitester.Queries
{
    public class IntegrationMetadataQueries
    {

        public string QueryAllTenantSFTP(string env)
        {
            //Returns all TenantSFTP in dbo.TenantSFTP for GET /api/TenantSFTP and GET /api/TenantSFTP/{TenantSftpId}
            var results = $@"SELECT TENANTSFTPID AS ID, TENANTID, HOSTNAME, USERNAME, PASSWORD, KEYFILEPATH, ISACTIVE AS ACTIVE
                             FROM CI_METADATA{env}.DBO.TENANTSFTP";

            return results;
        }
        public string QueryAllTenantIntegrationConfigSFTP(string env)
        {
            //Returns all TenantIntegrationConfigSFTP in dbo.TenantIntegrationConfig_SFTP for GET /api/TenantIntegrationConfigSFTP and GET /api/TenantIntegrationConfigSFTP/{TenantIntegrationConfigId}
            var results = $@"SELECT TENANTINTEGRATIONCONFIGID AS ID, TENANTINTEGRATIONID, TENANTSFTPID, 
                            SFTP_ARCHIVEDIRECTORY AS SFTPARCHIVEDIRECTORY, SFTP_DELETEFROMSOURCE AS SFTPDELETEFROMSOURCE, SFTP_LATESTONLY AS SFTPLATESTONLY, 
                            SFTP_OPERATION AS SFTPOPERATION, SOURCE_DIRECTORY AS SOURCEDIRECTORY, SOURCE_FILENAME AS SOURCEFILENAME, 
                            BLOB_DESTINATION AS BLOBDESTINATION, ACTIVE FROM CI_METADATA{env}.DBO.TENANTINTEGRATIONCONFIG_SFTP";

            return results;
        }
        public string QueryTenantIntegrationConfigSFTPByIntegrationId(string env, string id)
        {
            //Returns specific Tenant Integration Config SFTP in dbo.TenantIntegrationConfig_SFTP with correlated IntegrationId in dbo.TenantIntegration (for GET /TenantIntegrationConfigSFTP/{IntegrationId})
            var results = $@"SELECT A.TENANTINTEGRATIONCONFIGID AS ID, A.TENANTINTEGRATIONID, A.TENANTSFTPID,
                            A.SFTP_ARCHIVEDIRECTORY AS SFTPARCHIVEDIRECTORY, A.SFTP_DELETEFROMSOURCE AS SFTPDELETEFROMSOURCE, A.SFTP_LATESTONLY AS SFTPLATESTONLY, 
                            A.SFTP_OPERATION AS SFTPOPERATION, A.SOURCE_DIRECTORY AS SOURCEDIRECTORY, A.SOURCE_FILENAME AS SOURCEFILENAME, 
                            A.BLOB_DESTINATION AS BLOBDESTINATION, A.ACTIVE FROM CI_METADATA{env}.DBO.TENANTINTEGRATIONCONFIG_SFTP A
                            JOIN CI_METADATA{env}.DBO.TENANTINTEGRATION B
                            ON A.TENANTINTEGRATIONID = B.TENANTINTEGRATIONID
                            WHERE B.INTEGRATIONID = '{id}'";

            return results;
        }
        public string QueryAllTenantIntegration(string env)
        {
            //Returns all TenantIntegration in dbo.TenantIntegration for GET /api/TenantIntegration and GET /api/TenantIntegration/{TenantIntegrationId}
            var results = $@"SELECT TENANTINTEGRATIONID AS ID, TENANTID, INTEGRATIONID, ACTIVE, SYNC_JOB AS SYNCJOB                            
                            FROM CI_METADATA{env}.DBO.TENANTINTEGRATION";

            return results;
        }
        public string QueryAllTenantIntegrationEndpoint(string env)
        {
            //Returns all TenantIntegrationEndpoint in dbo.TenantIntegrationEndpoint for GET /api/TenantIntegrationEndpoint and GET /api/TenantIntegrationEndpoint/{TenantIntegrationEndpointId}
            var results = $@"SELECT TENANTINTEGRATIONENDPOINTID AS ID, INTEGRATIONENDPOINTID, TENANTINTEGRATIONID, ACTIVE
                             FROM CI_METADATA{env}.DBO.TENANTINTEGRATIONENDPOINT";

            return results;
        }
        public string QueryAllIntegration(string env)
        {
            //Returns all Integration in dbo.Integrations for GET /api/Integration and GET /api/Integration/{IntegrationId}
            var results = $@"SELECT INTEGRATIONID AS ID, INTEGRATIONTYPE AS INTEGRATIONTYPEID, INTEGRATION AS NAME, INTEGRATIONDESCRIPTION AS DESCRIPTION,
                            MAX_FILES_PER_RUN AS MAXFILESPERRUN, DEFAULT_SCHEDULE_ID AS DEFAULTSCHEDULEID
                             FROM CI_METADATA{env}.DBO.INTEGRATIONS";

            return results;
        }
        public string QueryAllIntegrationType(string env)
        {
            //Returns all Integration Type in dbo.IntegrationType for GET /api/IntegrationType and GET /api/IntegrationType/{IntegrationTypeId}
            var results = $@"SELECT INTEGRATIONTYPEID AS ID, INTEGRATIONTYPE AS NAME 
                             FROM CI_METADATA{env}.DBO.INTEGRATIONTYPE";

            return results;
        }
        public string QueryAllIntegrationEndpoints(string env)
        {
            //Returns all Integration Endpoints in dbo.IntegrationEndpoints for GET /api/IntegrationEndpoints and GET /api/IntegrationEndpoints/{IntegrationEndpointId}
            var results = $@"SELECT INTEGRATIONENDPOINTID, INTEGRATIONID, NAME, DESCRIPTION, ENDPOINT, BLOBFOLDERPATH, REQUESTBODY, ISPAGINATED, SCHEDULEID, ACTIVE 
                             FROM CI_METADATA{env}.DBO.INTEGRATIONENDPOINTS";

            //PRIMARY KEY = ID
            //var results = $@"SELECT INTEGRATIONENDPOINTID AS ID, INTEGRATIONID, NAME, DESCRIPTION, ENDPOINT, BLOBFOLDERPATH, REQUESTBODY, ISPAGINATED, SCHEDULEID, ACTIVE 
            //                 FROM CI_METADATA{env}.DBO.INTEGRATIONENDPOINTS";

            return results;
        }
        public string QueryAllTenantIntegrationConfigEloquaCIDT(string env)
        {
            //Returns all Tenant Integration Config Eloqua CIDT in dbo.TenantIntegrationConfig_Eloqua_Cidt; for GET /api/TenantIntegrationConfigEloquaCidt and GET /api/TenantIntegrationConfigEloquaCidt/{TenantIntegrationConfigId}
            var results = $@"SELECT TENANTINTEGRATIONCONFIGID, TENANTINTEGRATIONID, CLIENTID, CLIENTSECRET, PASSWORD, SITENAME, USERNAME, ACTIVE, START_MESSAGEID AS STARTMESSAGEID 
                             FROM CI_METADATA{env}.DBO.TENANTINTEGRATIONCONFIG_ELOQUA_CIDT";

            //PRIMARY KEY = ID
            //var results = $@"SELECT TENANTINTEGRATIONCONFIGID AS ID, TENANTINTEGRATIONID, CLIENTID, CLIENTSECRET, PASSWORD, SITENAME, USERNAME, ACTIVE, START_MESSAGEID AS STARTMESSAGEID 
            //                 FROM CI_METADATA{env}.DBO.TENANTINTEGRATIONCONFIG_ELOQUA_CIDT";

            return results;
        }
        public string QueryAllTenantIntegrationConfigSalesforce(string env)
        {
            //Returns all Tenant Integration Config Salesforce in dbo.TenantIntegrationConfig_Salesforce; for GET /api/TenantIntegrationConfigSalesforce and GET /api/TenantIntegrationConfigSalesforce/{TenantIntegrationConfigId}
            var results = $@"SELECT TENANTINTEGRATIONCONFIGID AS ID, TENANTINTEGRATIONID, USERNAME, PASSWORD, TOKEN, URL, ACTIVE
                             FROM CI_METADATA{env}.DBO.TENANTINTEGRATIONCONFIG_SALESFORCE";

            return results;
        }
        public string QueryAllTenantIntegrationConfigTM(string env)
        {
            //Returns all Tenant Integration Config Salesforce in dbo.TenantIntegrationConfig_TM; for GET /api/TenantIntegrationConfigTM and GET /api/TenantIntegrationConfigTM/{TenantIntegrationConfigId}
            var results = $@"SELECT TENANTINTEGRATIONCONFIGID AS ID, TENANTINTEGRATIONID, TM_DSN AS TMDSN, TM_TEAM AS TMTEAM, ACTIVE
                             FROM CI_METADATA{env}.DBO.TENANTINTEGRATIONCONFIG_TM";

            return results;
        }
    }
}

