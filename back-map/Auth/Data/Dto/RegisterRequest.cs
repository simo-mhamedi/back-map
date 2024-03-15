namespace back_map.Auth.Data.Dto
{
    public class RegisterRequest
    {
        public int? Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public IFormFile? ProfileImage { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }

    }
}
