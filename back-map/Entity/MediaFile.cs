namespace back_map.Entity
{
    public class MediaFile
    {
        public int Id { get; set; }
        public string PulbicId { get; set; }
        public string MediaUrl { get; set; }
        public int? AnnouncementId { get; set; }
        public Announcement? Announcement { get; set; }
    }
}
