namespace back_map.Entity.Dto
{
    public class RegisterRequestClient
    {
        public int? Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public IFormFile? ProfileImage { get; set; }
    }
}
