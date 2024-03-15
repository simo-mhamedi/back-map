using back_map.Auth.Services.DashBoardService;
using back_map.Entity.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_map.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;

        }
        [HttpGet("line-chart")]
        public IActionResult GetLineChartData()
        {
            return Ok(_dashboardService.GetLineChartData());
        }  
        [HttpGet("line-announcement-chart")]
        public IActionResult GetLineAnnouncementChartData()
        {
            return Ok(_dashboardService.GetLineAnnouncementChartData());
        }
        [HttpGet("user-stats")]
        public async Task<ActionResult<UserStatsDto>> GetUserStatistics()
        {
            try
            {
                var totalUsers = await _dashboardService.GetTotalUserCountAsync();
                var newUsersThisMonth = await _dashboardService.GetNewUsersCountThisMonthAsync();

                var userStatsDto = new UserStatsDto
                {
                    TotalUsers = totalUsers,
                    NewUsersThisMonth = newUsersThisMonth
                };

                return Ok(userStatsDto);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it based on your application's requirements
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("announcement-stats")]
        public async Task<ActionResult<UserStatsDto>> GetAnnouncementstatistics()
        {
            try
            {
                var totalAnnouncements = await _dashboardService.GetTotalAnnouncementCountAsync();
                var newAnnouncementsThisMonth = await _dashboardService.GetNewAnnouncementCountThisMonthAsync();

                var userStatsDto = new UserStatsDto
                {
                    TotalUsers = totalAnnouncements,
                    NewUsersThisMonth = newAnnouncementsThisMonth
                };

                return Ok(userStatsDto);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it based on your application's requirements
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
