namespace ci_automation_apitester.Queries
{
    internal class TestSetupStatements
    {
        internal TestSetupStatements()
        {
            
        }
        #region TenantMetadata Setup
        internal static string TenantOrgsSetup(string env)
        {
            //Setup seeded data for /api/TenantOrgs
            //dbo.TenantOrg.TenantId must exist in dbo.Tenant
            //dbo.TenantOrg.TenantId & dbo.TenantOrg.OrgId must be unique pair in dbo.TenantOrg

            var results = $@"

            INSERT INTO ci_metadata{env}.dbo.Tenant(TenantId, TenantName, TenantUrl, TenantType, TenantSubType, ShortName, Nickname, Mascot, Active, OrchardName, OrchardType, IsDiscoveryClient, DiscoveryClientName, Tenant_Key, IsCentralIntelligenceClient, ParentTenantId)
            VALUES('4d4a68cf-6082-4916-8e79-5d8897df7608', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 1, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', '');";
            
            return results;
        }
        internal static string TenantsSetup(string env)
        {
            //Setup seeded data for /api/Tenants
            //dbo.Tenant.TenantUrl must be unique

            var results = $@"

            INSERT INTO ci_metadata{env}.dbo.Tenant(TenantId, TenantName, TenantUrl, TenantType, TenantSubType, ShortName, Nickname, Mascot, Active, OrchardName, OrchardType, IsDiscoveryClient, DiscoveryClientName, Tenant_Key, IsCentralIntelligenceClient, ParentTenantId)
            VALUES('9fad9e91-95b3-44ed-aa42-16b67cc80daf', '{TestSetupAndTeardown.RandomTenantName()}', '/uofqaurl', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 1, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', '');";

            return results;
        }
        #endregion
        #region IntegrationMetadata Setup
        internal static string IntegrationTenantSetup(string env)
        {
            //Setup seeded data for /api/IntegrationTenant
            //dbo.TenantIntegration.IntegrationId must exist in dbo.Integration
            //dbo.TenantIntegration.TenantId must exist in dbo.Tenant
            //dbo.TenantIntegration.IntegrationId and dbo.TenantIntegration.TenantId must be unique pair in dbo.TenantIntegration

            var results = $@"

            INSERT INTO ci_metadata{env}.dbo.Tenant(TenantId, TenantName, TenantUrl, TenantType, TenantSubType, ShortName, Nickname, Mascot, Active, OrchardName, OrchardType, IsDiscoveryClient, DiscoveryClientName, Tenant_Key, IsCentralIntelligenceClient, ParentTenantId)
            VALUES
                ('5951b737-8e86-49dc-aa2c-4527ebc0f065', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 0, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', '');
            
            INSERT INTO ci_metadata{env}.dbo.Integrations(IntegrationId, Integration, IntegrationType, IntegrationDirection, IntegrationDescription, ExternalTable_Refresh_ScriptGroupTenantId, Associated_Model, Sync_Job, Max_Files_Per_Run, Default_Schedule_Id)
            VALUES
                ('02dade94-9d33-4269-b8d6-478b4310515f', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', '');
            
            INSERT INTO ci_metadata{env}.dbo.TenantIntegration(TenantIntegrationId, TenantId, IntegrationId, Active, Sync_Job)
            VALUES('ba7cd234-15d2-4779-953b-e3d5ae6d33e7', '5951b737-8e86-49dc-aa2c-4527ebc0f065', '02dade94-9d33-4269-b8d6-478b4310515f', 1, 'uofqa.Sync.Eloqua.Master');";

            return results;
        }
        internal static string TenantSFTPSetup(string env)
        {
            //Setup seeded data for /api/TenantSFTP

            var results = $@"

            INSERT INTO ci_metadata{env}.dbo.Tenant(TenantId, TenantName, TenantUrl, TenantType, TenantSubType, ShortName, Nickname, Mascot, Active, OrchardName, OrchardType, IsDiscoveryClient, DiscoveryClientName, Tenant_Key, IsCentralIntelligenceClient, ParentTenantId)
            VALUES
                ('fbe2aff9-dfc3-421d-998a-43f3afa8c086', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 1, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', ''),
                ('b7d7fab1-997b-464f-8305-19cd00be33db', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 1, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', '');

            INSERT INTO ci_metadata{env}.dbo.TenantSftp(TenantSftpId, TenantId, Hostname, Username, Password, KeyFilePath, IsActive)
            VALUES
                ('573db342-fc2f-4162-a2dd-f031bb6a2c57', 'b7d7fab1-997b-464f-8305-19cd00be33db', 'UofQAHostname', 'UofQAUsername', 'UofQAPassword', 'UofQAKeyFilePath', '1');
            ";

            return results;
        }
        internal static string TenantGPGSetup(string env)
        {
            //Setup seeded data for /api/TenantGPG

            var results = $@"

            INSERT INTO ci_metadata{env}.dbo.Tenant(TenantId, TenantName, TenantUrl, TenantType, TenantSubType, ShortName, Nickname, Mascot, Active, OrchardName, OrchardType, IsDiscoveryClient, DiscoveryClientName, Tenant_Key, IsCentralIntelligenceClient, ParentTenantId)
            VALUES
                ('8e6c925a-c580-415c-b36f-74e51db87ec6', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 1, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', ''),
                ('d52d1492-d8fc-4beb-8b61-65dbd88bbcd7', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 1, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', '');

            INSERT INTO ci_metadata{env}.dbo.TenantGpg(TenantGpgId, TenantId, GpgKeyName, Key, KeyType, PassPhrase, Active)
            VALUES
                ('ebf8a381-9a37-44d5-af8d-7660492e93db', '8e6c925a-c580-415c-b36f-74e51db87ec6', 'QA Private Key', '-----BEGIN PGP PRIVATE KEY BLOCK-----R18WSf5w/XYtvfg91pxg==\r\n=JINs -----END PGP PRIVATE KEY BLOCK-----', 'private', 'UofQAPassPhrase', '1');
            ";

            return results;
        }
        internal static string TenantIntegrationSetup(string env)
        {
            //Setup seeded data for /api/TenantIntegration
            //dbo.TenantIntegration.IntegrationId must exist in dbo.Integration
            //dbo.TenantIntegration.TenantId must exist in dbo.Tenant
            //dbo.TenantIntegration.IntegrationId and dbo.TenantIntegration.TenantId must be unique pair in dbo.TenantIntegration

            var results = $@"

            INSERT INTO ci_metadata{env}.dbo.Tenant(TenantId, TenantName, TenantUrl, TenantType, TenantSubType, ShortName, Nickname, Mascot, Active, OrchardName, OrchardType, IsDiscoveryClient, DiscoveryClientName, Tenant_Key, IsCentralIntelligenceClient, ParentTenantId)
            VALUES
                ('b313346f-c3f1-4291-9a9c-0ec6c07a3ac3', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 0, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', ''),
                ('ba27a0d7-4952-4279-9edc-7565b8aeb3a9', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 1, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', '');
            
            INSERT INTO ci_metadata{env}.dbo.Integrations(IntegrationId, Integration, IntegrationType, IntegrationDirection, IntegrationDescription, ExternalTable_Refresh_ScriptGroupTenantId, Associated_Model, Sync_Job, Max_Files_Per_Run, Default_Schedule_Id)
            VALUES
                ('e4358cfd-3003-409b-9e41-8664cd158d6b', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', ''),
                ('b2d7c8aa-2d7c-46af-b1c9-3353537ef02a', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', ''),
                ('ddfeb532-154e-4ad5-ba0a-691aad8d504a', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', '');
            
            INSERT INTO ci_metadata{env}.dbo.TenantIntegration(TenantIntegrationId, TenantId, IntegrationId, Active, Sync_Job)
            VALUES('5f1273e4-3cc5-4653-8871-206f33bfa141', 'fe971b24-9572-4005-b22f-351e9c09274d', '179b8861-be7c-465b-9a14-dfa73e0f100f', 1, 'uofqa.Sync.Eloqua.Master');";

            return results;
        }
        internal static string TenantIntegrationConfigSalesforceSetup(string env)
        {
            //Setup seeded data for /api/TenantIntegrationConfigSalesforce
            //TenantIntegrationId must be unique

            var results = $@"
            INSERT INTO ci_metadata{env}.dbo.Tenant(TenantId, TenantName, TenantUrl, TenantType, TenantSubType, ShortName, Nickname, Mascot, Active, OrchardName, OrchardType, IsDiscoveryClient, DiscoveryClientName, Tenant_Key, IsCentralIntelligenceClient, ParentTenantId)
            VALUES
                ('1f69d513-7eb1-4adf-bbed-4c4b4a942bd9', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 0, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', ''),
                ('dbe9f278-9047-43d1-b030-1dbe372c6716', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 1, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', ''),
                ('200b6bac-15ae-4a02-b5e7-32090454c438', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 1, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', '');
                
            INSERT INTO ci_metadata{env}.dbo.Integrations(IntegrationId, Integration, IntegrationType, IntegrationDirection, IntegrationDescription, ExternalTable_Refresh_ScriptGroupTenantId, Associated_Model, Sync_Job, Max_Files_Per_Run, Default_Schedule_Id)
            VALUES
                ('d0782a16-afae-4661-9f41-dd862aea1510', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', 'eb7553dd-3ba9-4f65-810e-3e32ca2df751'),
                ('081e82c1-f8c4-4417-9409-18a1c98b0573', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', 'eb7553dd-3ba9-4f65-810e-3e32ca2df751'),
                ('e3ba49b8-75fa-402b-8779-b378812fe81c', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', 'eb7553dd-3ba9-4f65-810e-3e32ca2df751');
                
            INSERT INTO ci_metadata{env}.dbo.Schedule (ScheduleId, ScheduleName, Description, CronSchedule)
            VALUES 
                ('eb7553dd-3ba9-4f65-810e-3e32ca2df751', 'UofQA Schedule Name', 'UofQA Schedule Description', '*/10 * * * *');            

            INSERT INTO ci_metadata{env}.dbo.TenantIntegration(TenantIntegrationId, TenantId, IntegrationId, Active, Sync_Job)
            VALUES
                ('92b09cb7-bad7-4f3e-b740-f317e1e6f81c', '1f69d513-7eb1-4adf-bbed-4c4b4a942bd9', 'd0782a16-afae-4661-9f41-dd862aea1510', 1, 'uofqa.Sync.Eloqua.Master'),
                ('ec12fa5f-1147-485e-8c1f-15004f29c6de', 'dbe9f278-9047-43d1-b030-1dbe372c6716', '081e82c1-f8c4-4417-9409-18a1c98b0573', 1, 'uofqa.Sync.Eloqua.Master'),
                ('3bbe0917-94e4-485f-a4a4-2192b87e2eb6', '200b6bac-15ae-4a02-b5e7-32090454c438', 'e3ba49b8-75fa-402b-8779-b378812fe81c', 1, 'uofqa.Sync.Eloqua.Master');

            INSERT INTO ci_metadata{env}.dbo.TenantIntegrationConfig_Salesforce(TenantIntegrationConfigId, TenantIntegrationId, Username, Password, Token, Url, Active)
            VALUES
                ('096d7884-de2f-4da6-898b-290ca94a91fb', 'ec12fa5f-1147-485e-8c1f-15004f29c6de', 'UofQAUsername', 'UofQAPassword', 'UofQToken', 'UofQAUrl', '1');
            
            ";

            return results;
        }
        internal static string TenantIntegrationConfigSFTPSetup(string env)
        {
            //Setup seeded data for /api/TenantIntegrationConfigSFTP
            //TenantIntegrationId must be unique,
            //TenantSftpId must correlate to existing record in dbo.TenantSftp where dbo.TenantSftp.Active = True

            var results = $@"
            INSERT INTO ci_metadata{env}.dbo.Tenant(TenantId, TenantName, TenantUrl, TenantType, TenantSubType, ShortName, Nickname, Mascot, Active, OrchardName, OrchardType, IsDiscoveryClient, DiscoveryClientName, Tenant_Key, IsCentralIntelligenceClient, ParentTenantId)
            VALUES
                ('3bec53d4-3438-4c90-9b65-92ed376b60ca', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 0, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', ''),
                ('226c47ff-5fe1-49be-a07a-b5e73d2fddc0', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 1, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', ''),
                ('9cc4b36e-3ff2-47f2-b7d1-f76a3bf5c40b', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 1, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', '');
                
            INSERT INTO ci_metadata{env}.dbo.Integrations(IntegrationId, Integration, IntegrationType, IntegrationDirection, IntegrationDescription, ExternalTable_Refresh_ScriptGroupTenantId, Associated_Model, Sync_Job, Max_Files_Per_Run, Default_Schedule_Id)
            VALUES
                ('77527eb7-4e9f-46d2-ad41-79fd40a83934', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', '4b310156-21e4-43d5-b076-48e72cce42b4'),
                ('e468e0ec-ad34-470e-afdc-1ded9137a263', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', '4b310156-21e4-43d5-b076-48e72cce42b4');
                
            INSERT INTO ci_metadata{env}.dbo.Schedule (ScheduleId, ScheduleName, Description, CronSchedule)
            VALUES 
                ('4b310156-21e4-43d5-b076-48e72cce42b4', 'UofQA Schedule Name', 'UofQA Schedule Description', '*/10 * * * *');            

            INSERT INTO ci_metadata{env}.dbo.TenantIntegration(TenantIntegrationId, TenantId, IntegrationId, Active, Sync_Job)
            VALUES
                ('9c2bfe92-b9ee-4573-bbc1-78358ca4d759', '3bec53d4-3438-4c90-9b65-92ed376b60ca', '77527eb7-4e9f-46d2-ad41-79fd40a83934', 1, 'uofqa.Sync.Eloqua.Master'),
                ('24a2a6ad-964d-4839-894e-cc581cc6333b', '226c47ff-5fe1-49be-a07a-b5e73d2fddc0', 'e468e0ec-ad34-470e-afdc-1ded9137a263', 1, 'uofqa.Sync.Eloqua.Master');
             
            INSERT INTO ci_metadata{env}.dbo.TenantSFTP(TenantSftpId, TenantId, Hostname, Username, Password, KeyFilePath, IsActive)
            VALUES
                ('9473808e-2990-462e-ae59-a6fc04d87495', '3bec53d4-3438-4c90-9b65-92ed376b60ca', 'UofQA Hostname', 'UofQAUsername', 'UofQAPassword', 'UofQAKeyFilePath', '1'),
                ('0994f75b-8af5-415d-8a1e-6e0a938e8811', '226c47ff-5fe1-49be-a07a-b5e73d2fddc0', 'UofQA Hostname', 'UofQAUsername', 'UofQAPassword', 'UofQAKeyFilePath', '1'),
                ('779ffa80-22d1-42e1-b3e1-a417065bb4d8', '9cc4b36e-3ff2-47f2-b7d1-f76a3bf5c40b', 'UofQA Hostname', 'UofQAUsername', 'UofQAPassword', 'UofQAKeyFilePath', '0');

            INSERT INTO ci_metadata{env}.dbo.TenantIntegrationConfig_SFTP(TenantIntegrationConfigId, TenantIntegrationId, TenantSftpId, SFTP_ArchiveDirectory, SFTP_DeleteFromSource, SFTP_LatestOnly, SFTP_Operation, Source_Directory, Source_Filename, Blob_Destination, Active)
            VALUES
                ('73cead83-9df2-4d86-bb50-79199e0d9d11', '9c2bfe92-b9ee-4573-bbc1-78358ca4d759', '9473808e-2990-462e-ae59-a6fc04d87495', 'UofQAArchiveDirectory', 'UofQADeleteFromSource', 'UofQALatestOnly', 'UofQAOperation', 'UofQASourceDirectory', 'UofQASourceFilename', 'UofQABlobDestination', '1');";

            return results;
        }
        internal static string TenantIntegrationConfigTMSetup(string env)
        {
            //Setup seeded data for /api/TenantIntegrationConfigTM
            //TenantIntegrationId must be unique

            var results = $@"
            INSERT INTO ci_metadata{env}.dbo.Tenant(TenantId, TenantName, TenantUrl, TenantType, TenantSubType, ShortName, Nickname, Mascot, Active, OrchardName, OrchardType, IsDiscoveryClient, DiscoveryClientName, Tenant_Key, IsCentralIntelligenceClient, ParentTenantId)
            VALUES
                ('1fc38ced-f8e5-4bcb-8c6e-91ed1e0665e0', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 0, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', ''),
                ('29b1a7d0-c43d-4e4b-acda-aadc160480ac', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 1, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', ''),
                ('389ba239-ea53-4780-96fc-492e9f51a917', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 1, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', '');
                
            INSERT INTO ci_metadata{env}.dbo.Integrations(IntegrationId, Integration, IntegrationType, IntegrationDirection, IntegrationDescription, ExternalTable_Refresh_ScriptGroupTenantId, Associated_Model, Sync_Job, Max_Files_Per_Run, Default_Schedule_Id)
            VALUES
                ('77b81273-ab35-471d-afe5-49c6faeb0e93', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', '2bed04cf-731f-45e3-8e79-6bbc91cf1220'),
                ('d2d9abe1-d1c9-4d18-9843-ab045f4419eb', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', '2bed04cf-731f-45e3-8e79-6bbc91cf1220'),
                ('fa22b776-74fa-4b0e-9cf7-349d38092844', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', '2bed04cf-731f-45e3-8e79-6bbc91cf1220');
                
            INSERT INTO ci_metadata{env}.dbo.Schedule (ScheduleId, ScheduleName, Description, CronSchedule)
            VALUES 
                ('2bed04cf-731f-45e3-8e79-6bbc91cf1220', 'UofQA Schedule Name', 'UofQA Schedule Description', '*/10 * * * *');            

            INSERT INTO ci_metadata{env}.dbo.TenantIntegration(TenantIntegrationId, TenantId, IntegrationId, Active, Sync_Job)
            VALUES
                ('abb76836-e565-4236-8446-54778f37ac7b', '1fc38ced-f8e5-4bcb-8c6e-91ed1e0665e0', '77b81273-ab35-471d-afe5-49c6faeb0e93', 1, 'uofqa.Sync.Eloqua.Master'),
                ('e2690575-95bf-4892-9a2b-7219c2489019', '29b1a7d0-c43d-4e4b-acda-aadc160480ac', 'd2d9abe1-d1c9-4d18-9843-ab045f4419eb', 1, 'uofqa.Sync.Eloqua.Master'),
                ('ee2e654d-e923-48e0-82cd-4d9e1245b25a', '389ba239-ea53-4780-96fc-492e9f51a917', 'fa22b776-74fa-4b0e-9cf7-349d38092844', 0, 'uofqa.Inactive.Tenant.Integration');
             
            INSERT INTO ci_metadata{env}.dbo.TenantIntegrationConfig_TM(TenantIntegrationConfigId, TenantIntegrationId, TM_DSN, TM_TEAM, Active)
            VALUES
                ('edd41f40-70a1-4396-825d-d6eaf7446d58', 'abb76836-e565-4236-8446-54778f37ac7b', 'uofqadsn', 'uofqateam', '1');";

            return results;
        }
        internal static string TenantIntegrationEndpointSetup(string env)
        {
            //Setup seeded data for /api/TenantIntegrationEndpoint
            //dbo.TenantIntegrationEndpoint.IntegrationEndpointId and dbo.TenantIntegrationEndpoint.TenantIntegrationId must be unique pair in dbo.TenantIntegrationEndpoint
            //dbo.TenantIntegration.IntegrationId must equal dbo.IntegrationEndpoints.IntegrationId on dbo.TenantIntegrationEndpoint.TenantIntegrationId

            var results = $@"
            INSERT INTO ci_metadata{env}.dbo.Tenant(TenantId, TenantName, TenantUrl, TenantType, TenantSubType, ShortName, Nickname, Mascot, Active, OrchardName, OrchardType, IsDiscoveryClient, DiscoveryClientName, Tenant_Key, IsCentralIntelligenceClient, ParentTenantId)
            VALUES
                ('f32c547b-bbc4-4949-aaab-b04650f87342', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 0, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', ''),
                ('d3dfc070-033f-4008-a7e2-8b8753a6746d', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 1, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', ''),
                ('26a9423c-be9c-4dec-b00e-56cbbf12bf01', '{TestSetupAndTeardown.RandomTenantName()}', '{TestSetupAndTeardown.RandomTenantUrl()}', 'UofQA Tenant Type', 'UofQA Tenant SubType', 'UofQA ShortName', 'UofQA Nickname', 'UofQA Mascot', 1, 'UofQA Orchard Name', 'UofQA OrchardType', 1, 'UofQA Discovery Client Name', '{TestSetupAndTeardown.RandomTenantKey()}', '1', '');
            
            INSERT INTO ci_metadata{env}.dbo.Integrations(IntegrationId, Integration, IntegrationType, IntegrationDirection, IntegrationDescription, ExternalTable_Refresh_ScriptGroupTenantId, Associated_Model, Sync_Job, Max_Files_Per_Run, Default_Schedule_Id)
            VALUES
                ('2e7c1caf-8160-4451-af62-9bb0baf884b6', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', 'e7af8fe4-c170-498a-be6e-778aa3c77914'),
                ('1b732681-e3f3-4c2d-b161-5040d2ce9dc1', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', 'e7af8fe4-c170-498a-be6e-778aa3c77914'),
                ('82cc055c-cb29-4299-9572-037e2dea3662', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', 'e7af8fe4-c170-498a-be6e-778aa3c77914');

            INSERT INTO ci_metadata{env}.dbo.Schedule (ScheduleId, ScheduleName, Description, CronSchedule)
            VALUES 
                ('e7af8fe4-c170-498a-be6e-778aa3c77914', 'UofQA Schedule Name', 'UofQA Schedule Description', '*/10 * * * *');            

            INSERT INTO ci_metadata{env}.dbo.TenantIntegration(TenantIntegrationId, TenantId, IntegrationId, Active, Sync_Job)
            VALUES
                ('1eee24e6-4db0-492d-86f8-5d08dde943a1', 'f32c547b-bbc4-4949-aaab-b04650f87342', '2e7c1caf-8160-4451-af62-9bb0baf884b6', 1, 'uofqa.Sync.Eloqua.Master'),
                ('6d7e179c-3e86-4692-b576-9ca3c7c2e2aa', 'd3dfc070-033f-4008-a7e2-8b8753a6746d', '1b732681-e3f3-4c2d-b161-5040d2ce9dc1', 1, 'uofqa.Sync.Eloqua.Master'),
                ('3add213c-5b85-4a74-8690-e83a49eeebb1', '26a9423c-be9c-4dec-b00e-56cbbf12bf01', '82cc055c-cb29-4299-9572-037e2dea3662', 1, 'uofqa.Sync.Eloqua.Master');

            INSERT INTO ci_metadata{env}.dbo.IntegrationEndpoints(IntegrationEndpointId, IntegrationId, Name, Description, Endpoint, BlobFolderPath, RequestBody, IsPaginated, ScheduleId, Active)
            VALUES
                ('7b723057-eac1-4718-842e-08f1132300e5', '2e7c1caf-8160-4451-af62-9bb0baf884b6', 'UofQA IntegrationEndpoint Name', 'UofQA IntegrationEndpoint Description', 'endpoint/qa', 'testdata/qabulk/', 'UofQA Request Body', '1', 'e7af8fe4-c170-498a-be6e-778aa3c77914', 1),
                ('bf99d327-40f6-471c-b77b-a24ee9650eae', '1b732681-e3f3-4c2d-b161-5040d2ce9dc1', 'UofQA IntegrationEndpoint Name', 'UofQA IntegrationEndpoint Description', 'endpoint/qa', 'testdata/qabulk/', 'UofQA Request Body', '1', 'e7af8fe4-c170-498a-be6e-778aa3c77914', 1),
                ('8b4eceee-35b1-478b-8b33-7366d8c146e7', '82cc055c-cb29-4299-9572-037e2dea3662', 'UofQA IntegrationEndpoint Name', 'UofQA IntegrationEndpoint Description', 'endpoint/qa', 'testdata/qabulk/', 'UofQA Request Body', '1', 'e7af8fe4-c170-498a-be6e-778aa3c77914', 1);
            
            INSERT INTO ci_metadata{env}.dbo.TenantIntegrationEndpoint(TenantIntegrationEndpointId, IntegrationEndpointId, TenantIntegrationId, Active)
            VALUES
                ('0430169d-9b50-4be8-b949-75b7a19a6fe4', 'bf99d327-40f6-471c-b77b-a24ee9650eae', '6d7e179c-3e86-4692-b576-9ca3c7c2e2aa', '1'),
                ('c528741b-bbb7-42a6-832a-7a2bd3aa3716', '8b4eceee-35b1-478b-8b33-7366d8c146e7', '3add213c-5b85-4a74-8690-e83a49eeebb1', '1');";

            return results;
        }
        internal static string IntegrationEndpointSetup(string env)
        {
            //Setup seeded data for /api/IntegrationEndpoint
            //dbo.TenantIntegrationEndpoint.IntegrationEndpointId and dbo.TenantIntegrationEndpoint.TenantIntegrationId must be unique pair in dbo.TenantIntegrationEndpoint
            //dbo.TenantIntegration.IntegrationId must equal dbo.IntegrationEndpoints.IntegrationId on dbo.TenantIntegrationEndpoint.TenantIntegrationId

            var results = $@"
            INSERT INTO ci_metadata{env}.dbo.Integrations(IntegrationId, Integration, IntegrationType, IntegrationDirection, IntegrationDescription, ExternalTable_Refresh_ScriptGroupTenantId, Associated_Model, Sync_Job, Max_Files_Per_Run, Default_Schedule_Id)
            VALUES
                ('f0108df8-1c04-443f-b067-124da266464d', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', '11ba2791-995a-4f66-a0d2-99a9f917d055'),
                ('eab73437-209c-4cf0-9128-d5a1776e7f9a', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', '11ba2791-995a-4f66-a0d2-99a9f917d055'),
                ('277d6d4b-7947-4608-97ed-fdee920a8562', 'UofQAIntegration', '8cb217a3-9ee5-4527-aa16-2c6e1aa654fe', 'df1159bd-a158-4332-a922-3fce3e52288b', 'UofQA Integration Description', '', 'UofQA Integration Model', 'QA.Sync-Start.QA', '10', '11ba2791-995a-4f66-a0d2-99a9f917d055');

            INSERT INTO ci_metadata{env}.dbo.Schedule (ScheduleId, ScheduleName, Description, CronSchedule)
            VALUES 
                ('11ba2791-995a-4f66-a0d2-99a9f917d055', 'UofQA Schedule Name', 'UofQA Schedule Description', '*/10 * * * *');

            INSERT INTO ci_metadata{env}.dbo.IntegrationEndpoints(IntegrationEndpointId, IntegrationId, Name, Description, Endpoint, BlobFolderPath, RequestBody, IsPaginated, ScheduleId, Active)
            VALUES
                ('b95da86a-1175-449a-8d45-8abdcd59ec59', 'f0108df8-1c04-443f-b067-124da266464d', 'UofQA IntegrationEndpoint Name', 'UofQA IntegrationEndpoint Description', 'endpoint/qa', 'testdata/qabulk/', 'UofQA Request Body', '1', '11ba2791-995a-4f66-a0d2-99a9f917d055', 1);
            
            ";

            return results;
        }
        #endregion
    }
}
