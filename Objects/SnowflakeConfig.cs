namespace ci_automation_apitester.Objects
{
    public class SnowflakeConfig
    {
        public string SnowflakeUser { get; set; }
        public string SnowflakePassword { get; set; }
        public string SnowflakeAccount { get; set; }
        public string SnowflakeRole { get; set; }
        public string SnowflakeWareHouse { get; set; }
        public string SnowflakeTestDatabase { get; set; }
        public string TestResultsSchema { get; set; }
        public string SnowflakeHost { get; set; }
        public string SnowflakeDatabaseEnvironment { get; set; }

        public SnowflakeConfig(SecretClient client, string account, string warehouse, string environment)
        {
            KeyVaultSecret secret = client.GetSecret("SnowflakeTRUser-" + account);
            SnowflakeUser = secret.Value;
            secret = client.GetSecret("SnowflakeTRPassword-" + account);
            SnowflakePassword = secret.Value;
            secret = client.GetSecret("SnowflakeAccount-" + account);
            SnowflakeAccount = secret.Value;
            secret = client.GetSecret("SnowflakeRole-" + account);
            SnowflakeRole = secret.Value;
            SnowflakeWareHouse = string.IsNullOrEmpty(warehouse) ? ((KeyVaultSecret)client.GetSecret("SnowflakeWarehouse-" + account)).Value : warehouse;
            secret = client.GetSecret("SnowflakeDatabase-" + account);
            SnowflakeTestDatabase = secret.Value;
            secret = client.GetSecret("TestResultsSchema");
            TestResultsSchema = secret.Value;
            secret = client.GetSecret("SnowflakeHost-" + account);
            SnowflakeHost = secret.Value;
            SnowflakeDatabaseEnvironment = DatabaseEnvironment(environment);
        }
        public string ConnectionString()
        {
            string account = $"account={SnowflakeAccount}";
            string user = $"user={SnowflakeUser}";
            string password = $"password={SnowflakePassword}";
            string warehouse = $"warehouse={SnowflakeWareHouse}";
            string db = $"db={SnowflakeTestDatabase}";
            string host = $"host={SnowflakeHost}";
            string role = $"role={SnowflakeRole}";
            string[] words = { account, user, password, warehouse, db, host, role };
            return string.Join(";", words);
        }
        public string ConnectionString(string tenantRole)
        {
            string account = $"account={SnowflakeAccount}";
            string user = $"user={SnowflakeUser}";
            string password = $"password={SnowflakePassword}";
            string warehouse = $"warehouse={SnowflakeWareHouse}";
            string db = $"db={SnowflakeTestDatabase}";
            string host = $"host={SnowflakeHost}";
            string role = $"role={tenantRole}";
            string[] words = { account, user, password, warehouse, db, host, role };
            return string.Join(";", words);
        }
        public string DatabaseEnvironment(string environment)
        {
            switch (environment.ToLower())
            {
                case "dev":
                    return "_DEV";
                case "test":
                    return "_TEST";
                case "prod":
                    return "";
                default: //Assume Test
                    return "_TEST";
            }
        }
    }
}
