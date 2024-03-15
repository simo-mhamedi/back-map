using back_map.Auth.Data.Dto;
using back_map.Business.Common;
using back_map.Entity.Dto;

namespace JwtWebApiTutorial.Services.UserService
{
    public interface IUserService
    {
        string GetMyName();
        Response Register(RegisterRequest request);
        void Update(RegisterRequest request);
        void UpdateClient(RegisterRequestClient request);
        void Delete(int userId);
        List<RegisterRequest> GetUsers(string? searchText, int pageSize, int skip, string? sortField, int? sortOrder);
        Response AddNewUser(RegisterRequest request);
        void DeleteUsers(string userIds);
        RegisterRequest GetUserById(int userId);
        RegisterRequest GetUserByEmail(string email);

    }
}
