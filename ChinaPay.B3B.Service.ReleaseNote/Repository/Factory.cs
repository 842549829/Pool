using ChinaPay.Repository;
namespace ChinaPay.B3B.Service.ReleaseNote.Repository
{
    static class Factory
    {
        public static IReleaseNoteRepository CreateReportRepository()
        {
            return new SqlServer.ReleaseNoteRepository(ConnectionManager.B3BConnectionString);
        }
    }
}
