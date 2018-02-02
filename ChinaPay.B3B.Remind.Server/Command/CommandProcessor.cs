namespace ChinaPay.B3B.Remind.Server.Command {
    abstract class CommandProcessor {
        public System.Net.Sockets.TcpClient Connection {
            get;
            private set;
        }
        public string Command {
            get;
            private set;
        }
        public string Request {
            get;
            private set;
        }
        public virtual bool DisposeConnection {
            get { return true; }
        }

        public string Execute() {
            var response = string.Empty;
            try {
                response = ExecuteCore();
            } catch {
                response = "99";
            }
            return this.Command + "/" + response;
        }
        protected abstract string ExecuteCore();

        public static CommandProcessor GetCommandProcessor(string request, System.Net.Sockets.TcpClient connection) {
            CommandProcessor result = null;
            switch(getCommand(request)) {
                case "LOGON":
                    result = new Logon();
                    break;
                case "LOGOFF":
                    result = new Logoff();
                    break;
                case "SAVECARRIER":
                    result = new SaveCarrier();
                    break;
                case "SAVESTATUS":
                    result = new SaveStatus();
                    break;
                case "QUERYCARRIER":
                    result = new QueryCarrier();
                    break;
                case "QUERYSTATUS":
                    result = new QueryStatus();
                    break;
                case "QUERYALLCARRIERS":
                    result = new QueryAllCarriers();
                    break;
                case "QUERYALLSTATUS":
                    result = new QueryAllStatus();
                    break;
            }
            if(result != null) {
                result.Command = getCommand(request);
                result.Request = getRequestContent(request);
                result.Connection = connection;
            }
            return result;
        }
        private static string getCommand(string request) {
            return string.IsNullOrWhiteSpace(request) ? string.Empty : request.Substring(0, request.IndexOf('/'));
        }
        private static string getRequestContent(string request) {
            return request.Substring(request.IndexOf('/') + 1);
        }
    }
}