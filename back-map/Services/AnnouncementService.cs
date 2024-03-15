using AutoMapper;
using back_map.Auth.Data.Dto;
using back_map.Business.Cloud;
using back_map.Business.Common;
using back_map.Context;
using back_map.Entity;
using back_map.Entity.Dto;
using JwtWebApiTutorial;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace back_map.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ICloudService _cloudService;

        public AnnouncementService(ICloudService cloudService, IMapper mapper,
                        AppDbContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _cloudService = cloudService;

        }

        public void CreateAnnouncement(AnnouncementDto announcementDto)
        {
            var announcement = _mapper.Map<Announcement>(announcementDto);

            var address = new Address
            {
                City = announcementDto.City,
                Country = announcementDto.Country,
                Latitude = announcementDto.Latitude,
                Longitude = announcementDto.Longitude,
                FullAddress = announcementDto.FullAddress
            };
            if (announcement.UserId == null)
            {
                announcement.User = null;
                announcement.UserId = null;
            }


            announcement.CategoryId = announcementDto.CategoryId;
            announcement.Address = address;
            announcement.CreationDateDate = DateTime.Now;

            // Assuming dbContext is an instance of your Entity Framework context
            _dbContext.Address.Add(address);
            _dbContext.Announcements.Add(announcement);
            _dbContext.SaveChanges();
            if (!string.IsNullOrEmpty(announcementDto.MoreDetails))
            {
                List<int> moreDetailsList = announcementDto.MoreDetails
                    .Split(',')
                    .Select(int.Parse)
                    .ToList();
                List<AnnoucementInfo> announcementInfoList = moreDetailsList
                    .Select(item => new AnnoucementInfo
                    {
                        Announcement = announcement,
                        MoreInfoId = item
                    })
                    .ToList();
                _dbContext.AnnoucementInfos.AddRange(announcementInfoList);
                _dbContext.SaveChanges();
            }

            UploadPhotos(announcement, announcementDto.Photo1, announcementDto.Photo2, announcementDto.Photo3, announcementDto.Photo4, announcementDto.Photo5);
        }

        private void UploadPhotos(Announcement announcement, params IFormFile[] files)
        {
            foreach (var file in files.Where(f => f != null))
            {
                var media = _cloudService.UploadImage(file);
                media.AnnouncementId = announcement.Id;
                announcement.MediaFiles.Add(media);
            }

            if (announcement.MediaFiles.Any())
            {
                _dbContext.SaveChanges();
            }
        }

        public void DeleteAnnouncement(int announcementId)
        {
            var announcement = _dbContext.Announcements
                .Include(m => m.MediaFiles)
                .FirstOrDefault(m => m.Id == announcementId);

            if (announcement != null)
            {
                if (announcement.MediaFiles != null)
                {
                    foreach (var mediaFile in announcement.MediaFiles)
                    {  
                        _cloudService.DeleteImage(mediaFile.PulbicId); // Assuming there's a method to delete an image in _cloudService
                    }
                }
                // Delete associated media files

                _dbContext.Announcements.Remove(announcement);
                _dbContext.SaveChanges();
            }
            // Handle the case where the announcement with the specified ID is not found
            else
            {
                // You can throw an exception, log, or handle it in some other way based on your requirements.
                // For example:
                throw new Exception($"Announcement with ID {announcementId} not found.");
            }
        }
        public void UpdateAnnouncement(AnnouncementDto updatedAnnouncementDto)
        {
            // Find the existing announcement in the database
            var existingAnnouncement = _dbContext.Announcements.Find(updatedAnnouncementDto.Id);

            if (existingAnnouncement != null)
            {
                // Update properties from the DTO
                _mapper.Map(updatedAnnouncementDto, existingAnnouncement);


                var address = _dbContext.Address.FirstOrDefault(u => u.Id == updatedAnnouncementDto.AddressId);

                address.City = updatedAnnouncementDto.City;
                address.Country = updatedAnnouncementDto.Country;
                address.Latitude = updatedAnnouncementDto.Latitude; 
                address.Longitude = updatedAnnouncementDto.Longitude;
                address.FullAddress = updatedAnnouncementDto.FullAddress;
                if (existingAnnouncement.UserId == 0)
                {
                    existingAnnouncement.User = null;
                    existingAnnouncement.UserId = null;
                }
                _dbContext.Address.Update(address);

                existingAnnouncement.CategoryId = updatedAnnouncementDto.CategoryId;


                // Save changes to the database
                _dbContext.SaveChanges();
            }
            else
            {
                // Handle the case where the announcement with the specified ID is not found
                throw new Exception($"Announcement with ID {updatedAnnouncementDto.Id} not found.");
            }
        }
        public void DeleteAnnouncements(string announcementIds)
        {
            var announcementIdsArray = announcementIds.Split(',').Select(int.Parse).ToList();

            var announcementsToDelete = _dbContext.Announcements
                .Include(a => a.MediaFiles) // Include related MediaFiles to avoid lazy loading
                .Where(a => announcementIdsArray.Contains(a.Id))
                .ToList();

            foreach (var announcement in announcementsToDelete)
            {
                // Delete associated media files
                foreach (var mediaFile in announcement.MediaFiles)
                {
                    _cloudService.DeleteImage(mediaFile.PulbicId); // Assuming you can get the public ID or URL from MediaFile
                }
            }

            // Remove announcements from the database
            _dbContext.Announcements.RemoveRange(announcementsToDelete);

            // Save changes to the database
            _dbContext.SaveChanges();
        }
        public List<AnnouncementDto> GetAnnouncements(string? searchText, int pageSize, int skip, string? sortField, int? sortOrder)
         {
            // Calculate the number of items to skip
            // Retrieve a paginated list of users from the database, ordering by a date or ID in descending order
            var announcements = _dbContext.Announcements
               .Include(u => u.Address)
               .Include(u => u.Category)                                                                                                                                                                            
               .AsQueryable();       

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                announcements = announcements.Where(u => EF.Functions.ILike(u.Description, $"%{searchText}%"));
            }

            switch (sortField)
            {
                case "Description":
                    announcements = sortOrder > 0 ? announcements.OrderByDescending(u => u.Description) : announcements.OrderBy(u => u.Description);
                    break;
                case "Price":
                    announcements = sortOrder > 0 ? announcements.OrderByDescending(u => u.Price) : announcements.OrderBy(u => u.Price);
                    break;
                case "Age":
                    announcements = sortOrder > 0 ? announcements.OrderByDescending(u => u.Age) : announcements.OrderBy(u => u.Age);
                    break;
                case "LivingSpace":
                    announcements = sortOrder > 0 ? announcements.OrderByDescending(u => u.LivingSpace) : announcements.OrderBy(u => u.LivingSpace);
                    break;
                case "TotalSurface":
                    announcements = sortOrder > 0 ? announcements.OrderByDescending(u => u.TotalSurface) : announcements.OrderBy(u => u.TotalSurface);
                    break;
                default:
                    announcements = announcements.OrderByDescending(u => u.Id);
                    break;
            }

            var announcementsDto = announcements.Skip(skip).Take(pageSize);
            // Map the list of User entities to a list of DTOs using AutoMapper
            var announcementDtos = _mapper.Map<List<AnnouncementDto>>(announcementsDto);

            return announcementDtos;
        }

        public AnnouncementDto GetAnnouncementById(int announcementId)
        {
            var announcement = _dbContext.Announcements
                .Include(u => u.Category)
                .Include(u => u.Address)
                .Include(u => u.MediaFiles)
                .Include(u => u.User)
                .FirstOrDefault(u => u.Id == announcementId);
            var dto = _mapper.Map<AnnouncementDto>(announcement);
            dto.category = _mapper.Map<CategoryDto>(announcement.Category);
            return dto;
        }
        public CategoryDto GetCategoryById(int categoryId)
        {
            var category = _dbContext.Categorys.Find(categoryId);
            return _mapper.Map<CategoryDto>(category);
        }
        public Response CreateCategory(string categoryDto)
        {
            var check = _dbContext.Categorys.FirstOrDefault(c => c.CategorieName.ToLower() == categoryDto);
            if (check != null)
            {
                return Response.CategoryAlreadySaved;
            }
            // Map CategoryDto to Category using AutoMapper
            var category = new Category
            {
                CategorieName = categoryDto,
            };
            // Add the new category to the database
            _dbContext.Categorys.Add(category);
            _dbContext.SaveChanges();
            return Response.Success;
        }
        public void UpdateCategory(int categoryId, string updatedCategory)
        {
            var existingCategory = _dbContext.Categorys.Find(categoryId);

            if (existingCategory != null)
            {
                existingCategory.CategorieName = updatedCategory;
                _dbContext.SaveChanges();
            }
            else
            {
                // Handle the case where the category with the specified ID is not found
                throw new Exception($"Category with ID {categoryId} not found.");
            }
        }
        public void DeleteCategory(int categoryId)
        {
            var category = _dbContext.Categorys.Find(categoryId);

            if (category != null)
            {
                _dbContext.Categorys.Remove(category);
                _dbContext.SaveChanges();
            }
            // Handle the case where the category with the specified ID is not found
            else
            {
                // You can throw an exception, log, or handle it in some other way based on your requirements.
                // For example:
                throw new Exception($"Category with ID {categoryId} not found.");
            }
        }
        public List<CategoryDto> GetCategories(string? searchText, int pageSize, int skip, string? sortField, int? sortOrder)
        {

            // Retrieve a paginated list of users from the database, ordering by a date or ID in descending order
            var categories = _dbContext.Categorys
               .AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                categories = categories.Where(u => EF.Functions.ILike(u.CategorieName, $"%{searchText}%"));
            }

            switch (sortField)
            {
                case "categorieName":
                    categories = sortOrder > 0 ? categories.OrderByDescending(u => u.CategorieName) : categories.OrderBy(u => u.CategorieName);
                    break;
                default:
                    categories = categories.OrderByDescending(u => u.Id);
                    break;
            }

            categories = categories.Skip(skip).Take(pageSize);
            // Map the list of User entities to a list of DTOs using AutoMapper
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public void DeleteCategorys(string categoryIds)
        {
            var userIdsArray = categoryIds.Split(',').Select(int.Parse).ToList();

            // Delete users by IDs
            var usersToDelete = _dbContext.Categorys.Where(u => userIdsArray.Contains(u.Id)).ToList();
            _dbContext.Categorys.RemoveRange(usersToDelete);

            // Save changes after all deletions
            _dbContext.SaveChanges();
        }
        public AnnouncementDto GetAnnouncementByPosition(int id)
        {

            var announcement = _dbContext.Announcements
              .Include(u => u.Category)
              .Include(u => u.Address)
              .Include(u => u.MediaFiles)
              .Include(u => u.User).ThenInclude(u => u.MediaFile)
              .FirstOrDefault(u => u.Id == id);
            var dto = _mapper.Map<AnnouncementDto>(announcement);
            return dto;

        }
        public AnnouncementDto GetAnnouncementByLanAndLon(decimal lat, decimal lon)
        {

            var announcement = _dbContext.Announcements
              .Include(u => u.Category)
              .Include(u => u.Address)
              .Include(u => u.MediaFiles)
              .Include(u => u.User).ThenInclude(u => u.MediaFile)
              .FirstOrDefault(u => u.Address.Latitude == lat && u.Address.Longitude == lon);
            var dto = _mapper.Map<AnnouncementDto>(announcement);
            return dto;

        }

        public List<AnnouncementDto> GetAnnouncementsByAddress(string address)
        {

            var announcement = _dbContext.Announcements
              .Include(u => u.Category)
              .Include(u => u.Address)
              .Include(u => u.MediaFiles)
              .Include(u => u.User).ThenInclude(u => u.MediaFile)
              .Where(u => u.Address.FullAddress.ToLower().Contains(address.ToLower())).ToList();
            var dto = _mapper.Map<List<AnnouncementDto>>(announcement);
            return dto;

        }

        public List<AnnouncementDto> GetAnnouncementsFilter(AnnouncementFilter announcementFilter)
        {

            var announcement = _dbContext.Announcements
              .Include(u => u.Category)
              .Include(u => u.Address)
              .Include(u => u.MediaFiles)
              .Include(u => u.User).ThenInclude(u => u.MediaFile)
              .Where(u => announcementFilter.Address.ToLower().Contains(
              u.Address.FullAddress.ToLower())
              && (u.Price >= announcementFilter.MinPrice || u.Price <= announcementFilter.MaxPrice)
              && u.CategoryId == announcementFilter.CategoryId && u.AnnouncementType == announcementFilter.AnnouncementType
              ).ToList();
            var dto = _mapper.Map<List<AnnouncementDto>>(announcement);
            return dto;

        }

        public void AddFavoriteAsync(int userId, int announcementId)
        {
            // Check if the favorite already exists for the user and announcement
            if (!_dbContext.Favorites.Any(f => f.UserId == userId && f.AnnouncementId == announcementId))
            {
                // If it doesn't exist, create a new favorite
                var favorite = new Favorite
                {
                    UserId = userId,
                    AnnouncementId = announcementId
                };

                _dbContext.Favorites.Add(favorite);
                _dbContext.SaveChangesAsync();
            }
        }
        public MoreInfoDto GetMoreInfoById(int Id)
        {
            var category = _dbContext.MoreInfos.Find(Id);
            return _mapper.Map<MoreInfoDto>(category);
        }
        public void ToggleFavoriteAsync(ToggleFavoriteRequest request)
        {
            // Check if the favorite already exists for the user and announcement
            var existingFavorite = _dbContext.Favorites.FirstOrDefault(f => f.UserId == request.UserId && f.AnnouncementId == request.AnnouncementId);

            if (existingFavorite == null)
            {
                // If it doesn't exist, create a new favorite
                var newFavorite = new Favorite
                {
                    UserId = request.UserId,
                    AnnouncementId = request.AnnouncementId,
                    IsActive = true
                };

                _dbContext.Favorites.Add(newFavorite);
            }
            else
            {
                // If it exists, toggle the IsActive property to deactivate/activate the favorite
                existingFavorite.IsActive = !existingFavorite.IsActive;
            }
            _dbContext.SaveChangesAsync();
        }

        public bool IsFavoriteActive(int userId, int announcementId)
        {
            // Check if the favorite exists for the user and announcement
            var existingFavorite = _dbContext.Favorites.FirstOrDefault(f => f.UserId == userId && f.AnnouncementId == announcementId);

            // If the favorite exists and is active, return true; otherwise, return false
            return existingFavorite != null && existingFavorite.IsActive;
        }

        public List<AnnouncementDto> GetFavorAnnouncements(int userId)
        {

            var announcement = _dbContext.Favorites
              .Include(u => u.Announcement)
              .ThenInclude(u => u.Address)
              .Include(u => u.Announcement)
              .ThenInclude(u => u.MediaFiles).Where(u => u.UserId == userId && u.IsActive == true).ToList();

            var dto = _mapper.Map<List<AnnouncementDto>>(announcement.Select(u => u.Announcement));
            return dto;
        }
        public List<AnnouncementDto> GetMesAnnouncements(int userId)
        {

            var announcement = _dbContext.Announcements
              .Include(u => u.Address)
              .Include(u => u.MediaFiles).Where(u => u.UserId == userId).ToList();
            var dto = _mapper.Map<List<AnnouncementDto>>(announcement);
            return dto;
        }

        public List<MoreInfoDto> GetMoreInfos()
        {
            var moreInfo = _dbContext.MoreInfos.ToList();
            var dto = _mapper.Map<List<MoreInfoDto>>(moreInfo);
            return dto;
        }

        public Response CreateMoreInfo(string moreInfoDto)
        {
            var check = _dbContext.MoreInfos.FirstOrDefault(c => c.Info.ToLower() == moreInfoDto);
            if (check != null)
            {
                return Response.CategoryAlreadySaved;
            }
            // Map CategoryDto to Category using AutoMapper
            var moreInfo = new MoreInfo
            {
                Info = moreInfoDto,
            };
            // Add the new category to the database
            _dbContext.MoreInfos.Add(moreInfo);
            _dbContext.SaveChanges();
            return Response.Success;
        }
        public void UpdateMoreInfo(int moreInfoId, string updatedMoreInfo)
        {
            var existingMoreInfo = _dbContext.MoreInfos.Find(moreInfoId);

            if (existingMoreInfo != null)
            {
                existingMoreInfo.Info = updatedMoreInfo;
                _dbContext.SaveChanges();
            }
            else
            {
                // Handle the case where the category with the specified ID is not found
                throw new Exception($"Category with ID {moreInfoId} not found.");
            }
        }
        public void DeletemoreInfo(int moreInfoId)
        {
            var moreInfo = _dbContext.MoreInfos.Find(moreInfoId);

            if (moreInfo != null)
            {
                _dbContext.MoreInfos.Remove(moreInfo);
                _dbContext.SaveChanges();
            }
            // Handle the case where the category with the specified ID is not found
            else
            {
                // You can throw an exception, log, or handle it in some other way based on your requirements.
                // For example:
                throw new Exception($"Category with ID {moreInfoId} not found.");
            }
        }
        public List<MoreInfoDto> GetMoreInfos(string? searchText, int pageSize, int skip, string? sortField, int? sortOrder)
        {

            // Retrieve a paginated list of users from the database, ordering by a date or ID in descending order
            var moreInfos = _dbContext.MoreInfos
               .AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                moreInfos = moreInfos.Where(u => EF.Functions.ILike(u.Info, $"%{searchText}%"));
            }

            switch (sortField)
            {
                case "info":
                    moreInfos = sortOrder > 0 ? moreInfos.OrderByDescending(u => u.Info) : moreInfos.OrderBy(u => u.Info);
                    break;
                default:
                    moreInfos = moreInfos.OrderByDescending(u => u.Id);
                    break;
            }

            moreInfos = moreInfos.Skip(skip).Take(pageSize);
            // Map the list of User entities to a list of DTOs using AutoMapper
            return _mapper.Map<List<MoreInfoDto>>(moreInfos);
        }

        public void DeleteMoreInfos(string moreInfosIds)
        {
            var userIdsArray = moreInfosIds.Split(',').Select(int.Parse).ToList();

            // Delete users by IDs
            var usersToDelete = _dbContext.MoreInfos.Where(u => userIdsArray.Contains(u.Id)).ToList();
            _dbContext.MoreInfos.RemoveRange(usersToDelete);

            // Save changes after all deletions
            _dbContext.SaveChanges();
        }


    }
}
