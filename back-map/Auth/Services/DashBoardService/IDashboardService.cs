namespace back_map.Auth.Services.DashBoardService
{
    public interface IDashboardService
    {
        dynamic GetLineChartData();
        dynamic GetLineAnnouncementChartData();
        Task<int> GetTotalUserCountAsync();
        Task<int> GetNewUsersCountThisMonthAsync();
        Task<int> GetTotalAnnouncementCountAsync();
        Task<int> GetNewAnnouncementCountThisMonthAsync();
    }
}
