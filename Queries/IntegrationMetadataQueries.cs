namespace ci_automation_apitester.Queries
{
    public class IntegrationMetadataQueries
    {
        internal static string QueryAllTenantSFTP(string env)
        {
            //Returns all TenantSFTP in dbo.TenantSFTP for GET /api/TenantSFTP and GET /api/TenantSFTP/{TenantSftpId}
            var results = $@"SELECT TenantSFTPId AS id, TenantId, Hostname, Username, Password, KeyFilePath, IsActive AS active
                             FROM CI_MetaData{env}.dbo.TENANTSFTP";

            return results;
        }
        internal static string QueryAllTenantGPG(string env)
        {
            //Returns all TenantGPG in dbo.TenantSFTP for GET /api/TenantGPG and GET /api/TenantGPG/{TenantGPGId}
            var results = $@"SELECT TenantGPGId AS id, TenantId, GPGKeyName, Key, KeyType, PassPhrase, Active
                             FROM CI_MetaData{env}.dbo.TENANTGPG";

            return results;
        }
        internal static string QueryAllTenantIntegrationConfigSFTP(string env)
        {
            //Returns all TenantIntegrationConfigSFTP in dbo.TenantIntegrationConfig_SFTP for GET /api/TenantIntegrationConfigSFTP and GET /api/TenantIntegrationConfigSFTP/{TenantIntegrationConfigId}
            var results = $@"SELECT TenantIntegrationConfigId AS id, TenantIntegrationId, TenantSftpId, 
                            SFTP_ArchiveDirectory AS sftpArchiveDirectory, SFTP_DeleteFromSource AS sftpDeleteFromSource, SFTP_LatestOnly AS sftpLatestOnly, 
                            SFTP_Operation AS sftpOperation, Source_Directory AS sourceDirectory, Source_Filename AS sourceFilename, 
                            Blob_Destination AS blobDestination, Active FROM CI_MetaData{env}.dbo.TenantIntegrationConfig_SFTP";

            return results;
        }
        internal static string QueryTenantIntegrationConfigSFTPByIntegrationId(string env, string id)
        {
            //Returns specific Tenant Integration Config SFTP in dbo.TenantIntegrationConfig_SFTP with correlated IntegrationId in dbo.TenantIntegration (for GET /TenantIntegrationConfigSFTP/{IntegrationId})
            var results = $@"SELECT A.TenantIntegrationConfigId AS id, A.TenantIntegrationId, A.TenantSftpId,
                            A.SFTP_ArchiveDirectory AS sftpArchiveDirectory, A.SFTP_DeleteFromSource AS sftpDeleteFromSource, A.SFTP_LatestOnly AS sftpLatestOnly, 
                            A.SFTP_Operation AS sftpOperation, A.Source_Directory AS sourceDirectory, A.Source_Filename AS sourceFilename, 
                            A.Blob_Destination AS BlobDestination, A.Active FROM CI_MetaData{env}.dbo.TenantIntegrationConfig_SFTP A
                            JOIN CI_MetaData{env}.dbo.TenantIntegration B
                            ON A.TenantIntegrationId = B.TenantIntegrationId
                            WHERE B.IntegrationId = '{id}'";

            return results;
        }
        internal static string QueryAllTenantIntegration(string env)
        {
            //Returns all TenantIntegration in dbo.TenantIntegration for GET /api/TenantIntegration and GET /api/TenantIntegration/{TenantIntegrationId}
            var results = $@"SELECT TenantIntegrationId AS id, Tenantid, IntegrationId, Active, Sync_Job AS syncJob                            
                            FROM CI_MetaData{env}.dbo.TenantIntegration";

            return results;
        }
        internal static string QueryAllTenantIntegrationEndpoint(string env)
        {
            //Returns all TenantIntegrationEndpoint in dbo.TenantIntegrationEndpoint for GET /api/TenantIntegrationEndpoint and GET /api/TenantIntegrationEndpoint/{TenantIntegrationEndpointId}
            var results = $@"SELECT TenantIntegrationEndpointId AS id, IntegrationEndpointId, TenantIntegrationId, Active
                             FROM CI_MetaData{env}.dbo.TenantIntegrationEndpoint";

            return results;
        }
        internal static string QueryAllIntegration(string env)
        {
            //Returns all Integration in dbo.Integrations for GET /api/Integration and GET /api/Integration/{IntegrationId}
            var results = $@"SELECT A.IntegrationId AS id, A.IntegrationType AS integrationTypeId, B.IntegrationType AS integrationTypeName, A.IntegrationDirection AS directionId, 
                          C.IntegrationType AS directionName, A.Integration AS name, A.IntegrationDescription AS description, A.Max_Files_Per_Run AS MaxFilesPerRun, A.Default_Schedule_Id AS DefaultScheduleId
                          FROM ci_metadata{env}.dbo.Integrations A
                          JOIN ci_metadata{env}.dbo.IntegrationType B
                          ON A.IntegrationType = B.IntegrationTypeId
                          JOIN ci_metadata{env}.dbo.IntegrationType C
                          ON A.IntegrationDirection = C.IntegrationTypeId";

            return results;
        }
        internal static string QueryAllIntegrationType(string env)
        {
            //Returns all Integration Type in dbo.IntegrationType for GET /api/IntegrationType and GET /api/IntegrationType/{IntegrationTypeId}
            var results = $@"SELECT IntegrationTypeId AS id, IntegrationType AS name 
                             FROM CI_MetaData{env}.dbo.IntegrationType";

            return results;
        }
        internal static string QueryAllIntegrationEndpoints(string env)
        {
            //Returns all Integration Endpoints in dbo.IntegrationEndpoints for GET /api/IntegrationEndpoints and GET /api/IntegrationEndpoints/{IntegrationEndpointId}
            var results = $@"SELECT INTEGRATIONENDPOINTID AS ID, INTEGRATIONID, NAME, DESCRIPTION, ENDPOINT, BLOBFOLDERPATH, REQUESTBODY, ISPAGINATED, SCHEDULEID, ACTIVE 
                             FROM CI_METADATA{env}.DBO.INTEGRATIONENDPOINTS";

            return results;
        }
        internal static string QueryAllTenantIntegrationConfigEloquaCIDT(string env)
        {
            //Returns all Tenant Integration Config Eloqua CIDT in dbo.TenantIntegrationConfig_Eloqua_Cidt; for GET /api/TenantIntegrationConfigEloquaCidt and GET /api/TenantIntegrationConfigEloquaCidt/{TenantIntegrationConfigId}
            var results = $@"SELECT TenantIntegrationConfigId, TenantIntegrationId, ClientId, ClientSecret, Password, SiteName, Username, Active, Start_MessageId AS StartMessageId 
                             FROM CI_MetaData{env}.dbo.TenantIntegrationConfig_Eloqua_CIDT";

            //PRIMARY KEY = ID
            //var results = $@"SELECT TENANTINTEGRATIONCONFIGID AS ID, TENANTINTEGRATIONID, CLIENTID, CLIENTSECRET, PASSWORD, SITENAME, USERNAME, ACTIVE, START_MESSAGEID AS STARTMESSAGEID 
            //                 FROM CI_METADATA{env}.DBO.TENANTINTEGRATIONCONFIG_ELOQUA_CIDT";

            return results;
        }
        internal static string QueryAllTenantIntegrationConfigSalesforce(string env)
        {
            //Returns all Tenant Integration Config Salesforce in dbo.TenantIntegrationConfig_Salesforce; for GET /api/TenantIntegrationConfigSalesforce and GET /api/TenantIntegrationConfigSalesforce/{TenantIntegrationConfigId}
            var results = $@"SELECT TenantIntegrationConfigId AS id, TenantIntegrationId, Username, Password, Token, Url, Active
                             FROM CI_MetaData{env}.dbo.TenantIntegrationConfig_Salesforce";

            return results;
        }
        internal static string QueryTenantIntegrationConfigSalesforceByIntegrationId(string env, string id)
        {
            //Returns specific Tenant Integration Config SFTP in dbo.TenantIntegrationConfig_SFTP with correlated IntegrationId in dbo.TenantIntegration (for GET /TenantIntegrationConfigSFTP/{IntegrationId})
            var results = $@"SELECT A.TenantIntegrationConfigId AS id, A.TenantIntegrationId, A.Username,
                            A.Password, A.Token, A.Url, A.Active FROM CI_MetaData{env}.dbo.TenantIntegrationConfig_Salesforce A
                            JOIN CI_MetaData{env}.dbo.TenantIntegration B
                            ON A.TenantIntegrationId = B.TenantIntegrationId
                            WHERE B.IntegrationId = '{id}'";

            return results;
        }
        internal static string QueryAllTenantIntegrationConfigTM(string env)
        {
            //Returns all Tenant Integration Config Salesforce in dbo.TenantIntegrationConfig_TM; for GET /api/TenantIntegrationConfigTM and GET /api/TenantIntegrationConfigTM/{TenantIntegrationConfigId}
            var results = $@"SELECT TenantIntegrationConfigId AS id, TenantIntegrationId, TM_DSN AS tmDsn, TM_TEAM AS tmTeam, Active
                             FROM CI_MetaData{env}.dbo.TenantIntegrationConfig_TM";

            return results;
        }
        internal static string QueryTenantByIntegrationId(string env, string id)
        {
            //Returns specific Tenants in dbo.Tenant by correlated IntegrationId in dbo.TenantIntegration (for GET /IntegrationTenant/{IntegrationId})
            var results = $@"SELECT B.TenantId, B.TenantName, B.Tenant_Key AS TenantKey, B.Active AS TenantActive,
                            A.IntegrationId, A.TenantIntegrationId, A.Active AS TenantIntegrationActive FROM CI_MetaData{env}.dbo.TenantIntegration A
                            JOIN CI_MetaData{env}.dbo.Tenant B
                            ON A.TenantId = B.TenantId
                            WHERE A.IntegrationId = '{id}'";

            return results;
        }
        internal static string QueryIntegrationByTenantId(string env, string id)
        {
            //Returns specific Integrations in dbo.Integrations by correlated TenantId in dbo.TenantIntegration (for GET /api/Integration/tenantid/{TenantId})
            var results = $@"SELECT A.IntegrationId, A.Integration AS IntegrationName, A.IntegrationDescription, A.IntegrationType AS IntegrationTypeId, 
                                B.IntegrationType AS IntegrationTypeName, C.IntegrationTypeId AS DirectionID, C.IntegrationType AS DirectionName, 
                                D.TenantIntegrationId, D.Active AS TenantIntegrationActive
                                FROM  ci_metadata{env}.dbo.Integrations A
                                INNER JOIN ci_metadata{env}.dbo.IntegrationType B
                                ON A.IntegrationType = B.IntegrationTypeId
                                INNER JOIN ci_metadata{env}.dbo.IntegrationType C
                                ON A.IntegrationDirection = C.IntegrationTypeId
                                INNER JOIN ci_metadata{env}.dbo.tenantIntegration D
                                ON A.IntegrationId = D.IntegrationId
                                WHERE D.TenantId = '{id}'";

            return results;
        }
        internal static string QueryDistinctTenantIntegrationTenantId(string env)
        {
            //Returns distinct TenantIds in dbo.TenantIntegration
            var results = $@"SELECT DISTINCT TenantId FROM ci_metadata{env}.dbo.TenantIntegration;";

            return results;
        }
        internal static string QueryDistinctTenantIntegrationIntegrationId(string env)
        {
            //Returns distinct IntegrationIds in dbo.TenantIntegration
            var results = $@"SELECT DISTINCT IntegrationId FROM ci_metadata{env}.dbo.TenantIntegration;";

            return results;
        }
        internal static string QueryTenantIntegrationConfigSftpByTenantId(string env, string id)
        {
            //Returns specific Tenant Integration Config SFTP in dbo.TenantIntegrationConfig_SFTP by correlated TenantId in dbo.TenantSftp (for GET /TenantIntegrationConfigSFTP/Tenant/{TenantId})
            var results = $@"SELECT A.TenantIntegrationConfigId AS Id, A.TenantIntegrationId, A.TenantSftpId, A.SFTP_ArchiveDirectory AS sftpArchiveDirectory, 
                            A.SFTP_DeleteFromSource AS sftpDeleteFromSource, A.SFTP_LatestOnly AS sftpLatestOnly, A.SFTP_Operation AS sftpOperation, 
                            A.Source_Directory AS sourceDirectory, A.Source_Filename AS sourceFilename, A.Blob_Destination AS BlobDestination, A.Active
                            FROM  ci_metadata{env}.dbo.TenantIntegrationConfig_SFTP A
                            INNER JOIN ci_metadata{env}.dbo.tenantSftp B
                            ON A.TenantSftpId = B.TenantSftpId
                            WHERE B.TenantId = '{id}'";

            return results;
        }
        internal static string QueryDistinctTenantSftpTenantId(string env)
        {
            //Returns distinct TenantIds in dbo.TenantSftp
            var results = $@"SELECT DISTINCT TenantId FROM ci_metadata{env}.dbo.TenantSftp;";

            return results;
        }
    }
}

