namespace ChinaPay.B3B.Remind.Client {
    class Settings {
        private static string m_serverHost;
        public static string ServerHost {
            get {
                if(m_serverHost == null) {
                    m_serverHost = System.Configuration.ConfigurationManager.AppSettings["serverhost"];
                }
                return m_serverHost;
            }
        }

        private static int? m_serverPort = null;
        public static int ServerPort {
            get {
                if(!m_serverPort.HasValue) {
                    m_serverPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["serverport"]);
                }
                return m_serverPort.Value;
            }
        }
    }
}