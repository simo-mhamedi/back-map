using back_map.Auth.Data.Dto;
using back_map.Business.Common;
using back_map.Context;
using back_map.Entity;
using back_map.Entity.Dto;
using back_map.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_map.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAnnouncementService _announcementService;
        private readonly AppDbContext _dbContext;

        public AnnouncementController(AppDbContext dbContext, IConfiguration configuration, IAnnouncementService announcementService)
        {
            _configuration = configuration;
            _announcementService = announcementService;
            _dbContext = dbContext;

        }
        [HttpPost("add-announcement")]
        public IActionResult CreateAnnouncement([FromForm] AnnouncementDto announcementDto)
        {
            _announcementService.CreateAnnouncement(announcementDto);
            return Ok();
        }
        [HttpGet("get-announcement-by-position")]
        public ActionResult<AnnouncementDto> GetAnnouncementByPosition(int id)
        {
            return Ok(_announcementService.GetAnnouncementByPosition(id));
        }

        [HttpPost("update-announcement")]
        public IActionResult UpdateAnnouncement(AnnouncementDto updatedAnnouncementDto)
        {
            _announcementService.UpdateAnnouncement(updatedAnnouncementDto);
            return Ok();
        }

        [HttpPost("delete-announcements")]
        public IActionResult DeleteAnnouncements(string announcementIds)
        {
            _announcementService.DeleteAnnouncements(announcementIds);
            return Ok();
        }

        [HttpPost("delete-announcement")]
        public IActionResult DeleteAnnouncement(int announcementId)
        {
            _announcementService.DeleteAnnouncement(announcementId);
            return Ok();
        }

        [HttpGet("get-announcements")]
        public ActionResult<List<AnnouncementDto>> Getannouncements(string? searchText, int pageSize, int skip, string? sortField, int? sortOrder)
        {
            return Ok(_announcementService.GetAnnouncements(searchText, pageSize, skip, sortField, sortOrder));
        }

        [HttpGet("get-announcement")]
        public ActionResult<AnnouncementDto> CreateAnnouncement(int id)
        {
            return Ok(_announcementService.GetAnnouncementById(id));
        }

        [HttpPost("create-category")]
        public IActionResult CreateCategory([FromForm] string category)
        {
            return Ok(_announcementService.CreateCategory(category));
        }
        [HttpPost("update-category")]
        public IActionResult UpdateCategory([FromForm] int categoryId, [FromForm] string category)
        {
            _announcementService.UpdateCategory(categoryId, category);
            return Ok();
        }
        [HttpGet("get-categorys")]
        public ActionResult<List<CategoryDto>> GetCategories(string? searchText, int pageSize, int skip, string? sortField, int? sortOrder)
        {
            return Ok(_announcementService.GetCategories(searchText, pageSize, skip, sortField, sortOrder));
        }
        [HttpGet("get-category")]
        public ActionResult<CategoryDto> GetCategoryById(int id)
        {
            return Ok(_announcementService.GetCategoryById(id));
        }
        [HttpPost("delete-category")]
        public ActionResult<CategoryDto> DeleteCategory(int categoryId)
        {
            _announcementService.DeleteCategory(categoryId);
            return Ok();
        }

        [HttpPost("delete-categorys")]
        public ActionResult<CategoryDto> DeleteCategorys(string categoryIds)
        {
            _announcementService.DeleteCategorys(categoryIds);
            return Ok();
        }
        [HttpGet("get-announcement-by-params")]
        public ActionResult<AnnouncementDto> GetAnnouncementByLanAndLon(decimal lat, decimal lon)
        {
            return Ok(_announcementService.GetAnnouncementByLanAndLon(lat, lon));
        }
        [HttpGet("get-Announcements-by-Address")]
        public ActionResult<List<AnnouncementDto>> GetAnnouncementsByAddress(string address)
        {
            return Ok(_announcementService.GetAnnouncementsByAddress(address));
        }
        [HttpGet("get-Announcements-by-filter")]
        public ActionResult<List<AnnouncementDto>> GetAnnouncementsByFilter([FromHeader] AnnouncementFilter GetAnnouncementsFilter)
        {
            return Ok(_announcementService.GetAnnouncementsFilter(GetAnnouncementsFilter));
        }

        [HttpPost("add-favorite")]
        public IActionResult AddFavorite(int userId, int announcementId)
        {
            _announcementService.AddFavoriteAsync(userId, announcementId);
            return Ok();
        }

        [HttpPost("toggle-favorite")]
        public IActionResult ToggleFavoriteAsync([FromBody] ToggleFavoriteRequest request)
        {
            _announcementService.ToggleFavoriteAsync(request);
            return Ok();
        }
        [HttpGet("isFavoriteActive")]
        public ActionResult<bool> IsFavoriteActive(int userId, int announcementId)
        {
            return Ok(_announcementService.IsFavoriteActive(userId, announcementId));
        }
        [HttpGet("getFavorAnnouncements")]
        public ActionResult<List<AnnouncementDto>> GetFavorAnnouncements(int userId)
        {
            return Ok(_announcementService.GetFavorAnnouncements(userId));
        }
        [HttpGet("getMesAnnouncements")]
        public ActionResult<List<AnnouncementDto>> GetMesAnnouncements(int userId)
        {
            return Ok(_announcementService.GetMesAnnouncements(userId));
        }

        [HttpPost("create-moreInfo")]
        public ActionResult<Response> CreateMoreInfo([FromForm] string moreInfoDto)
        {
            return Ok(_announcementService.CreateMoreInfo(moreInfoDto));
        }

        [HttpPost("update-moreInfo")]
        public IActionResult UpdateMoreInfo([FromForm] int id, [FromForm] string info)
        {
            _announcementService.UpdateMoreInfo(id, info);
            return Ok();
        }

        [HttpPost("delete-moreInfo")]
        public IActionResult DeletemoreInfo(int moreInfoId)
        {
            _announcementService.DeletemoreInfo(moreInfoId);
            return Ok();
        }
        [HttpGet("get-moreInfos")]
        public ActionResult<List<MoreInfoDto>> GetMoreInfos(string? searchText, int pageSize, int skip, string? sortField, int? sortOrder)
        {
            return Ok(_announcementService.GetMoreInfos(searchText,pageSize,skip,sortField,sortOrder));
        }

        [HttpPost("delete-moreInfos")]
        public IActionResult DeleteMoreInfos(string moreInfosIds)
        {
            _announcementService.DeleteMoreInfos(moreInfosIds);
            return Ok();
        } 
        [HttpGet("get-moreInfo-byId")]
        public ActionResult<MoreInfoDto> GetMoreInfoById(int Id)
        {
            return Ok(_announcementService.GetMoreInfoById(Id));
        }
    }
}
    