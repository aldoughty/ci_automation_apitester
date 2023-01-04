namespace ci_automation_apitester.Queries
{
    internal class TestTeardownStatements
    {

        public Dictionary<string, Func<string, string>> Setup { get; set; }

        internal TestTeardownStatements()
        {

        }
        #region TenantMetadata Teardown
        internal static string TenantOrgsTeardown(string env)
        {
            //Teardown seeded data for /api/TenantOrgs

            var results = $@"
                DELETE FROM ci_metadata{env}.dbo.Tenant WHERE TenantId IN ('4d4a68cf-6082-4916-8e79-5d8897df7608');
                DELETE FROM ci_metadata{env}.dbo.TenantOrg WHERE TenantId IN ('4d4a68cf-6082-4916-8e79-5d8897df7608');
            ";

            return results;
        }
        internal static string TenantsTeardown(string env)
        {
            //Teardown seeded data for /api/Tenant

            var results = $@"
                DELETE FROM ci_metadata{env}.dbo.Tenant WHERE TenantId IN ('9fad9e91-95b3-44ed-aa42-16b67cc80daf');
            ";

            return results;
        }
        #endregion
        #region IntegrationMetadata Teardown
        internal static string IntegrationTenantTeardown(string env)
        {
            //Teardown seeded data for /api/IntegrationTenant

            var results = $@"
                DELETE FROM ci_metadata{env}.dbo.Tenant WHERE TenantId IN ('5951b737-8e86-49dc-aa2c-4527ebc0f065');
                DELETE FROM ci_metadata{env}.dbo.Integrations WHERE IntegrationId IN ('02dade94-9d33-4269-b8d6-478b4310515f');
                DELETE FROM ci_metadata{env}.dbo.TenantIntegration WHERE TenantIntegrationId IN ('ba7cd234-15d2-4779-953b-e3d5ae6d33e7');
            ";

            return results;
        }
        internal static string TenantIntegrationTeardown(string env)
        {
            //Teardown seeded data for /api/TenantIntegration

            var results = $@"
                DELETE FROM ci_metadata{env}.dbo.Tenant WHERE TenantId IN ('b313346f-c3f1-4291-9a9c-0ec6c07a3ac3', 'ba27a0d7-4952-4279-9edc-7565b8aeb3a9');
                DELETE FROM ci_metadata{env}.dbo.Integrations WHERE IntegrationId IN ('e4358cfd-3003-409b-9e41-8664cd158d6b', 'b2d7c8aa-2d7c-46af-b1c9-3353537ef02a', 'ddfeb532-154e-4ad5-ba0a-691aad8d504a');
                DELETE FROM ci_metadata{env}.dbo.TenantIntegration WHERE TenantId = 'b313346f-c3f1-4291-9a9c-0ec6c07a3ac3' AND IntegrationId = 'e4358cfd-3003-409b-9e41-8664cd158d6b';
                DELETE FROM ci_metadata{env}.dbo.TenantIntegration WHERE TenantId = 'ba27a0d7-4952-4279-9edc-7565b8aeb3a9' AND IntegrationId = 'b2d7c8aa-2d7c-46af-b1c9-3353537ef02a';
                DELETE FROM ci_metadata{env}.dbo.TenantIntegration WHERE TenantId = 'fe971b24-9572-4005-b22f-351e9c09274d' AND IntegrationId = '179b8861-be7c-465b-9a14-dfa73e0f100f';
            ";

            return results;
        }
        internal static string TenantSFTPTeardown(string env)
        {
            //Teardown seeded data for /api/TenantSFTP

            var results = $@"
                DELETE FROM ci_metadata{env}.dbo.Tenant WHERE TenantId IN ('fbe2aff9-dfc3-421d-998a-43f3afa8c086', 'b7d7fab1-997b-464f-8305-19cd00be33db');
                DELETE FROM ci_metadata{env}.dbo.TenantSftp WHERE TenantSftpId IN ('573db342-fc2f-4162-a2dd-f031bb6a2c57');
                DELETE FROM ci_metadata{env}.dbo.TenantSftp WHERE TenantId IN ('fbe2aff9-dfc3-421d-998a-43f3afa8c086');
            ";

            return results;
        }
        internal static string TenantGPGTeardown(string env)
        {
            //Teardown seeded data for /api/TenantGPG

            var results = $@"
                DELETE FROM ci_metadata{env}.dbo.Tenant WHERE TenantId IN ('8e6c925a-c580-415c-b36f-74e51db87ec6',  'd52d1492-d8fc-4beb-8b61-65dbd88bbcd7');
                DELETE FROM ci_metadata{env}.dbo.Gpg WHERE TenantSftpId IN ('ebf8a381-9a37-44d5-af8d-7660492e93db');
                DELETE FROM ci_metadata{env}.dbo.TenantGpg WHERE TenantId IN ('d52d1492-d8fc-4beb-8b61-65dbd88bbcd7');
            ";

            return results;
        }
        internal static string TenantIntegrationConfigSalesforceTeardown(string env)
        {
            //Teardown seeded data for /api/TenantIntegrationConfigSalesforce

            var results = $@"
                DELETE FROM ci_metadata{env}.dbo.Tenant WHERE TenantId IN ('1f69d513-7eb1-4adf-bbed-4c4b4a942bd9', 'dbe9f278-9047-43d1-b030-1dbe372c6716', '200b6bac-15ae-4a02-b5e7-32090454c438');
                DELETE FROM ci_metadata{env}.dbo.Integrations WHERE IntegrationId IN ('d0782a16-afae-4661-9f41-dd862aea1510', '081e82c1-f8c4-4417-9409-18a1c98b0573', 'e3ba49b8-75fa-402b-8779-b378812fe81c');
                DELETE FROM ci_metadata{env}.dbo.Schedule WHERE ScheduleId = 'eb7553dd-3ba9-4f65-810e-3e32ca2df751';
                DELETE FROM ci_metadata{env}.dbo.TenantIntegration WHERE TenantIntegrationId IN ('92b09cb7-bad7-4f3e-b740-f317e1e6f81c', 'ec12fa5f-1147-485e-8c1f-15004f29c6de', '3bbe0917-94e4-485f-a4a4-2192b87e2eb6');
                DELETE FROM ci_metadata{env}.dbo.TenantIntegrationConfig_Salesforce WHERE TenantIntegrationConfigId IN ('096d7884-de2f-4da6-898b-290ca94a91fb');
            ";

            return results;
        }
        internal static string TenantIntegrationConfigSFTPTeardown(string env)
        {
            //Teardown seeded data for /api/TenantIntegrationConfigSFTP

            var results = $@"
                DELETE FROM ci_metadata{env}.dbo.Tenant WHERE TenantId IN ('3bec53d4-3438-4c90-9b65-92ed376b60ca', '226c47ff-5fe1-49be-a07a-b5e73d2fddc0', '9cc4b36e-3ff2-47f2-b7d1-f76a3bf5c40b');
                DELETE FROM ci_metadata{env}.dbo.Integrations WHERE IntegrationId IN ('77527eb7-4e9f-46d2-ad41-79fd40a83934', 'e468e0ec-ad34-470e-afdc-1ded9137a263');
                DELETE FROM ci_metadata{env}.dbo.Schedule WHERE ScheduleId = '4b310156-21e4-43d5-b076-48e72cce42b4';
                DELETE FROM ci_metadata{env}.dbo.TenantIntegration WHERE TenantIntegrationId IN ('9c2bfe92-b9ee-4573-bbc1-78358ca4d759', '24a2a6ad-964d-4839-894e-cc581cc6333b');
                DELETE FROM ci_metadata{env}.dbo.TenantSftp WHERE TenantSftpId IN ('9473808e-2990-462e-ae59-a6fc04d87495', '0994f75b-8af5-415d-8a1e-6e0a938e8811', '779ffa80-22d1-42e1-b3e1-a417065bb4d8');
                DELETE FROM ci_metadata{env}.dbo.TenantIntegrationConfig_SFTP WHERE TenantIntegrationConfigId IN ('73cead83-9df2-4d86-bb50-79199e0d9d11'); 
                DELETE FROM ci_metadata{env}.dbo.TenantIntegrationConfig_SFTP WHERE TenantIntegrationId = '24a2a6ad-964d-4839-894e-cc581cc6333b' AND TenantSftpId = '0994f75b-8af5-415d-8a1e-6e0a938e8811';
            ";

            return results;
        }
        internal static string TenantIntegrationConfigTMTeardown(string env)
        {
            //Teardown seeded data for /api/TenantIntegrationConfigTM

            var results = $@"
                DELETE FROM ci_metadata{env}.dbo.Tenant WHERE TenantId IN ('1fc38ced-f8e5-4bcb-8c6e-91ed1e0665e0', '29b1a7d0-c43d-4e4b-acda-aadc160480ac', '389ba239-ea53-4780-96fc-492e9f51a917');
                DELETE FROM ci_metadata{env}.dbo.Integrations WHERE IntegrationId IN ('77b81273-ab35-471d-afe5-49c6faeb0e93', 'd2d9abe1-d1c9-4d18-9843-ab045f4419eb', 'fa22b776-74fa-4b0e-9cf7-349d38092844');
                DELETE FROM ci_metadata{env}.dbo.Schedule WHERE ScheduleId = '2bed04cf-731f-45e3-8e79-6bbc91cf1220';
                DELETE FROM ci_metadata{env}.dbo.TenantIntegration WHERE TenantIntegrationId IN ('abb76836-e565-4236-8446-54778f37ac7b', 'e2690575-95bf-4892-9a2b-7219c2489019', 'ee2e654d-e923-48e0-82cd-4d9e1245b25a');
                DELETE FROM ci_metadata{env}.dbo.TenantIntegrationConfig_TM WHERE TenantIntegrationConfigId IN ('edd41f40-70a1-4396-825d-d6eaf7446d58'); 
                DELETE FROM ci_metadata{env}.dbo.TenantIntegrationConfig_TM WHERE TenantIntegrationId IN ('abb76836-e565-4236-8446-54778f37ac7b', 'e2690575-95bf-4892-9a2b-7219c2489019', 'ee2e654d-e923-48e0-82cd-4d9e1245b25a');
            ";

            return results;
        }
        internal static string TenantIntegrationEndpointTeardown(string env)
        {
            //Teardown seeded data for /api/TenantIntegrationEndpoint

            var results = $@"
                DELETE FROM ci_metadata{env}.dbo.Tenant WHERE TenantId IN ('f32c547b-bbc4-4949-aaab-b04650f87342', 'd3dfc070-033f-4008-a7e2-8b8753a6746d', '26a9423c-be9c-4dec-b00e-56cbbf12bf01');
                DELETE FROM ci_metadata{env}.dbo.Integrations WHERE IntegrationId IN ('2e7c1caf-8160-4451-af62-9bb0baf884b6', '1b732681-e3f3-4c2d-b161-5040d2ce9dc1', '82cc055c-cb29-4299-9572-037e2dea3662');
                DELETE FROM ci_metadata{env}.dbo.Schedule WHERE ScheduleId = 'e7af8fe4-c170-498a-be6e-778aa3c77914';
                DELETE FROM ci_metadata{env}.dbo.TenantIntegration WHERE TenantIntegrationId IN ('1eee24e6-4db0-492d-86f8-5d08dde943a1', '6d7e179c-3e86-4692-b576-9ca3c7c2e2aa', '3add213c-5b85-4a74-8690-e83a49eeebb1');
                DELETE FROM ci_metadata{env}.dbo.IntegrationEndpoints WHERE IntegrationEndpointId IN ('7b723057-eac1-4718-842e-08f1132300e5', 'bf99d327-40f6-471c-b77b-a24ee9650eae', '8b4eceee-35b1-478b-8b33-7366d8c146e7');
                DELETE FROM ci_metadata{env}.dbo.TenantIntegrationEndpoint WHERE TenantIntegrationEndpointId IN ('0430169d-9b50-4be8-b949-75b7a19a6fe4', 'c528741b-bbb7-42a6-832a-7a2bd3aa3716'); 
                DELETE FROM ci_metadata{env}.dbo.TenantIntegrationEndpoint WHERE IntegrationEndpointId = '7b723057-eac1-4718-842e-08f1132300e5' AND TenantIntegrationId = '1eee24e6-4db0-492d-86f8-5d08dde943a1';
            ";

            return results;
        }
        internal static string IntegrationEndpointTeardown(string env)
        {
            //Teardown seeded data for /api/IntegrationEndpoint

            var results = $@"
                DELETE FROM ci_metadata{env}.dbo.Integrations WHERE IntegrationId IN ('f0108df8-1c04-443f-b067-124da266464d', 'eab73437-209c-4cf0-9128-d5a1776e7f9a', '277d6d4b-7947-4608-97ed-fdee920a8562');
                DELETE FROM ci_metadata{env}.dbo.Schedule WHERE ScheduleId = '11ba2791-995a-4f66-a0d2-99a9f917d055';
                DELETE FROM ci_metadata{env}.dbo.IntegrationEndpoints WHERE IntegrationEndpointId IN ('b95da86a-1175-449a-8d45-8abdcd59ec59');
                DELETE FROM ci_metadata{env}.dbo.IntegrationEndpoints WHERE IntegrationId = '277d6d4b-7947-4608-97ed-fdee920a8562';
            ";
                
            return results;
        }
        #endregion
    }
}
