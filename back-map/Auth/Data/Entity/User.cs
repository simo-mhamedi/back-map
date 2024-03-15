using back_map.Entity;

namespace JwtWebApiTutorial
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int? MediaFileId { get; set; }
        public MediaFile MediaFile { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public string Role { get; set; } = string.Empty;
        public DateTime TokenExpires { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
