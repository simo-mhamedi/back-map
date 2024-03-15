using AutoMapper;
using back_map.Business.Cloud;
using back_map.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using static back_map.Auth.Services.DashBoardService.DashboardService;

namespace back_map.Auth.Services.DashBoardService
{

    public class DashboardService : IDashboardService
    {
     
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ICloudService _cloudService;

        public DashboardService(ICloudService cloudService, IMapper mapper,
                        AppDbContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _cloudService = cloudService;

        }
        public class MonthlyUserData
        {
            public List<string> Months { get; set; }
            public List<UserData> UserDatas { get; set; }

        }
        public class UserData
        {
            public string Month { get; set; }
            public int UserCount { get; set; }
        }

        private MonthlyUserData GetMonthlyUserCounts()
        {
            var currentDate = DateTime.UtcNow;

            var monthlyUserCounts = _dbContext.Users
                .Where(u => u.RegistrationDate != null) // Filter out null dates if applicable
                .GroupBy(u => new { u.RegistrationDate.Year, u.RegistrationDate.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => new UserData
                {
                    Month = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month)}",
                    UserCount = g.Count()
                })
                .ToList();

            var nextMonths = new List<MonthlyUserData>();

            for (int i = 1; i <= 6; i++)
            {
                var nextMonth = currentDate.AddMonths(i);
                nextMonths.Add(new MonthlyUserData
                {
                    Months = new List<string> { CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(nextMonth.Month) },
                    UserDatas = new List<UserData>()
                });
            }

            var result = new MonthlyUserData
            {
                Months = monthlyUserCounts.Select(x => x.Month).ToList(),
                UserDatas = monthlyUserCounts.ToList()
            };

            result.UserDatas.AddRange(nextMonths.SelectMany(x => x.UserDatas));
            result.Months.AddRange(nextMonths.SelectMany(x => x.Months));

            return result;
        }
        public dynamic GetLineChartData()
        {
            var monthlyUserCounts = GetMonthlyUserCounts();

            var lineData = new
            {
                labels = monthlyUserCounts.Months.ToArray(),
                datasets =
                    new
                    {
                        label = "Users",
                        data = monthlyUserCounts.UserDatas.Select(entry => entry.UserCount).ToArray(),
                        tension = 0.4
                    }
            };


            return lineData;
        }



        private MonthlyUserData GetMonthlyAnnouncementCounts()
        {
            var currentDate = DateTime.UtcNow;

            var monthlyUserCounts = _dbContext.Announcements
                .Where(u => u.CreationDateDate != null) // Filter out null dates if applicable
                .GroupBy(u => new { u.CreationDateDate.Year, u.CreationDateDate.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => new UserData
                {
                    Month = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month)}",
                    UserCount = g.Count()
                })
                .ToList();

            var nextMonths = new List<MonthlyUserData>();

            for (int i = 1; i <= 6; i++)
            {
                var nextMonth = currentDate.AddMonths(i);
                nextMonths.Add(new MonthlyUserData
                {
                    Months = new List<string> { CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(nextMonth.Month) },
                    UserDatas = new List<UserData>()
                });
            }

            var result = new MonthlyUserData
            {
                Months = monthlyUserCounts.Select(x => x.Month).ToList(),
                UserDatas = monthlyUserCounts.ToList()
            };

            result.UserDatas.AddRange(nextMonths.SelectMany(x => x.UserDatas));
            result.Months.AddRange(nextMonths.SelectMany(x => x.Months));

            return result;
        }
        public dynamic GetLineAnnouncementChartData()
        {
            var monthlyUserCounts = GetMonthlyAnnouncementCounts();

            var lineData = new
            {
                labels = monthlyUserCounts.Months.ToArray(),
                datasets =
                    new
                    {
                        label = "Announcements",
                        data = monthlyUserCounts.UserDatas.Select(entry => entry.UserCount).ToArray(),
                        tension = 0.4
                    }
            };
            return lineData;
        }
        public async Task<int> GetTotalUserCountAsync()
        {
            return await _dbContext.Users.CountAsync();
        }

        public async Task<int> GetNewUsersCountThisMonthAsync()
        {
            var currentDate = DateTime.Now;

            return await _dbContext.Users
                .Where(u => u.RegistrationDate.Date.Month == currentDate.Date.Month)
                .CountAsync();
        }
        

        public async Task<int> GetTotalAnnouncementCountAsync()
        {
            return await _dbContext.Announcements.CountAsync();
        }

        public async Task<int> GetNewAnnouncementCountThisMonthAsync()
        {
            var currentDate = DateTime.Now;

            return await _dbContext.Announcements
                .Where(u => u.CreationDateDate.Date.Month == currentDate.Date.Month)
                .CountAsync();
        }
    }
}
