using back_map.Business.Common;
using back_map.Entity.Dto;

namespace back_map.Services
{
    public interface IAnnouncementService
    {
        void CreateAnnouncement(AnnouncementDto announcementDto);
        void DeleteAnnouncement(int announcementId);
        AnnouncementDto GetAnnouncementByPosition(int id);
        void UpdateAnnouncement(AnnouncementDto updatedAnnouncementDto);
        void DeleteAnnouncements(string announcementIds);
        AnnouncementDto GetAnnouncementById(int announcementId);
        List<AnnouncementDto> GetAnnouncements(string? searchText, int pageSize, int skip, string? sortField, int? sortOrder);
        CategoryDto GetCategoryById(int categoryId);
        Response CreateCategory(string categoryDto);
        void UpdateCategory(int categoryId, string updatedCategoryDto);
        void DeleteCategory(int categoryId);
        List<CategoryDto> GetCategories(string? searchText, int pageSize, int skip, string? sortField, int? sortOrder);
        void DeleteCategorys(string categoryIds);
        AnnouncementDto GetAnnouncementByLanAndLon(decimal lat, decimal lon);
        List<AnnouncementDto> GetAnnouncementsByAddress(string address);
        List<AnnouncementDto> GetAnnouncementsFilter(AnnouncementFilter announcementFilter);
        void AddFavoriteAsync(int userId, int announcementId);
        void ToggleFavoriteAsync(ToggleFavoriteRequest request);
        bool IsFavoriteActive(int userId, int announcementId);
        List<AnnouncementDto> GetFavorAnnouncements(int userId);
        List<AnnouncementDto> GetMesAnnouncements(int userId);
        List<MoreInfoDto> GetMoreInfos();
        Response CreateMoreInfo(string moreInfoDto);
        void UpdateMoreInfo(int moreInfoId, string updatedMoreInfo);
        void DeletemoreInfo(int moreInfoId);
        List<MoreInfoDto> GetMoreInfos(string? searchText, int pageSize, int skip, string? sortField, int? sortOrder);
        void DeleteMoreInfos(string moreInfosIds);
        MoreInfoDto GetMoreInfoById(int Id);
    }
}
