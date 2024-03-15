using back_map.auth.Data.Dto;
using back_map.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using back_map.Auth.Data.Dto;
using AutoMapper;
using back_map.Business.Common;
using back_map.Business.Cloud;
using CloudinaryDotNet.Actions;
using static back_map.Auth.Services.DashBoardService.DashboardService;
using back_map.Entity.Dto;

namespace JwtWebApiTutorial.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ICloudService _cloudService;

        public UserService(ICloudService cloudService,IMapper mapper,
                        AppDbContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _cloudService = cloudService;

        }

        public string GetMyName()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }
            return result;
        }

        public Response Register(RegisterRequest request)
        {
            try
            {
                // Check if the email already exists
                if (_dbContext.Users.Any(u => u.Email == request.Email))
                {
                    return Response.EmailAlreadyRegistered;
                }

                // Create password hash and salt
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                // Use AutoMapper to map the properties
                User user = _mapper.Map<User>(request);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.RegistrationDate = DateTime.Now;
                // Add the user to the context and save changes
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                return Response.Success;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return Response.Error;
            }
        }

        public Response AddNewUser(RegisterRequest request)
        {
            try
            {
                // Check if the email already exists
                if (_dbContext.Users.Any(u => u.Email == request.Email))
                {
                    return Response.EmailAlreadyRegistered;
                }

                // Create password hash and salt
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                // Use AutoMapper to map the properties
                User user = _mapper.Map<User>(request);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                if(request.ProfileImage!= null)
                {
                    var media =_cloudService.UploadImage(request.ProfileImage);
                    user.MediaFile=media;
                }
                // Add the user to the context and save changes
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                return Response.Success;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return Response.Error;
            }
        }


        public void Update(RegisterRequest request)
        {
           
            // Assuming you have a method to get the existing user by ID
            var existingUser = _dbContext.Users.FirstOrDefault(u=>u.Id==request.Id);
            _mapper.Map(request, existingUser);
            if (request.Password != null)
            {
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                // Update other properties as needed
                existingUser.PasswordHash = passwordHash;
                existingUser.PasswordSalt = passwordSalt;
            }
            if (request.ProfileImage != null)
            {
                var media = _cloudService.UploadImage(request.ProfileImage);
                existingUser.MediaFile = media;
            }
            _dbContext.Users.Update(existingUser);
            // Save changes
            _dbContext.SaveChanges();

        }

        public void UpdateClient(RegisterRequestClient request)
        {

            // Assuming you have a method to get the existing user by ID
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Id == request.Id);
            _mapper.Map(request, existingUser);
            if (request.ProfileImage != null)
            {
                var media = _cloudService.UploadImage(request.ProfileImage);
                existingUser.MediaFile = media;
            }
            _dbContext.Users.Update(existingUser);
            // Save changes
            _dbContext.SaveChanges();

        }
        public void Delete(int userId)
        {
            var existingUser = _dbContext.Users.Find(userId);

            if (existingUser == null)
            {
                throw new Exception("not exist");
            }
            _dbContext.Users.Remove(existingUser);
            // Save changes
            _dbContext.SaveChanges();

        }
        public void DeleteUsers(string userIds)
        {
            var userIdsArray = userIds.Split(',').Select(int.Parse).ToList();

            // Delete users by IDs
            var usersToDelete = _dbContext.Users.Where(u => userIdsArray.Contains(u.Id)).ToList();
            _dbContext.Users.RemoveRange(usersToDelete);

            // Save changes after all deletions
            _dbContext.SaveChanges();
        }
        public RegisterRequest GetUserById(int userId)
        {
            // Assuming you have a method to get the user by ID
            User user = _dbContext.Users
                .Include(u => u.MediaFile)
                .FirstOrDefault(u=>u.Id==userId);
            // Map the User entity to a DTO using AutoMapper
            RegisterRequest userDto = _mapper.Map<RegisterRequest>(user);
            // Now, userDto contains the user information
            return userDto;
        }

        public List<RegisterRequest> GetUsers(string? searchText, int pageSize, int skip, string? sortField, int? sortOrder)
        {

            // Calculate the number of items to skip
            // Retrieve a paginated list of users from the database, ordering by a date or ID in descending order
            var users = _dbContext.Users
               .Include(u => u.MediaFile)
               .AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                users = users.Where(u => EF.Functions.ILike(u.Username, $"%{searchText}%") || EF.Functions.ILike(u.Email, $"%{searchText}%")
                         || EF.Functions.ILike(u.Email, $"%{searchText}%"));
            }

            switch (sortField)
            {
                case "userName":
                    users = sortOrder > 0 ? users.OrderByDescending(u => u.Username) : users.OrderBy(u => u.Username);
                    break;
                case "email":
                    users = sortOrder > 0 ? users.OrderByDescending(u => u.Email) : users.OrderBy(u => u.Email);
                    break;
                case "phone":
                    users = sortOrder > 0 ? users.OrderByDescending(u => u.Phone) : users.OrderBy(u => u.Phone);
                    break;
                case "role":
                    users = sortOrder > 0 ? users.OrderByDescending(u => u.Role) : users.OrderBy(u => u.Role);
                    break;
                default:
                    users = users.OrderByDescending(u => u.Id);
                    break;
            }
 
            users = users.Skip(skip).Take(pageSize);
            // Map the list of User entities to a list of DTOs using AutoMapper
            List<RegisterRequest> userDtos = _mapper.Map<List<RegisterRequest>>(users);
            return userDtos;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public RegisterRequest GetUserByEmail(string email)
        {
            return _mapper.Map<RegisterRequest>(_dbContext.Users
                .Include(u => u.MediaFile)
                .FirstOrDefault(e=>e.Email==email));
        }
    }

}
