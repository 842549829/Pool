namespace ChinaPay.B3B.Service.Remind.Repository {
    class Factory {
        public static IOrderRemindRepository CreateRemindRepository() {
            return new SqlServer.OrderRemindRepository(ChinaPay.Repository.ConnectionManager.B3BConnectionString);
        }
        public static IOrderRemindSettingRepository CreateRemindSettingRepository() {
            return new SqlServer.OrderRemindSettingRepository(ChinaPay.Repository.ConnectionManager.B3BConnectionString);
        }
    }
}