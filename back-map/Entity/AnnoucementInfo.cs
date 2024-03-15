namespace back_map.Entity
{
    public class AnnoucementInfo
    {
        public  int Id { get; set; }
        public int MoreInfoId { get; set; }
        public MoreInfo MoreInfo { get; set; }
        public int AnnouncementId { get; set; }
        public Announcement Announcement { get; set; }
    }
}
