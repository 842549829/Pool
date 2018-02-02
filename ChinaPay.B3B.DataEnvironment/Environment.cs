
namespace ChinaPay.B3B.Data {
    public static class Environment {
        private static string databaseProvider = "System.Data.SqlClient";
        public static string DatabaseProvider {
            get { return databaseProvider; }
        }

        private static string databaseConnectionString = Repository.ConnectionManager.B3BConnectionString;
        public static string DatabaseConnectionString {
            get { return databaseConnectionString; }
        }

        //private static string defaultPassword = "88888888";
        //public static string DefaultPassword { get { return defaultPassword; } }

        public static void Refresh(){}
    }
}