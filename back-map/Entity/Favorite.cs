using JwtWebApiTutorial;

namespace back_map.Entity
{
    public class Favorite
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int AnnouncementId { get; set; }
        public Announcement Announcement { get; set; }
        public bool IsActive { get; set; }
    }
}
