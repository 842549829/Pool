using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Announce.Repository {
    static class Factory {
        public static Announce.Reposity.IAnnounceReposity CreateAnnounceReposity() {
            return new Service.Announce.Reposity.SqlServer.AnnounceReposity(ConnectionManager.B3BConnectionString);
        }
    }
}
