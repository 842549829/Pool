using System.Web;

namespace ChinaPay.B3B.MaintenanceWeb {
    /// <summary>
    /// Logout 的摘要说明
    /// </summary>
    public class Logout : IHttpHandler, System.Web.SessionState.IRequiresSessionState {

        public void ProcessRequest(HttpContext context) {
            LogonUtility.Logout();
        }

        public bool IsReusable {
            get {
                return false;
            }
        }
    }
}