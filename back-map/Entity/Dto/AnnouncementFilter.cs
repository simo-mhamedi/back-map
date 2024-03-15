using back_map.Business.Common;

namespace back_map.Entity.Dto
{
    public class AnnouncementFilter
    {
        public int CategoryId { get; set; }
        public string Address { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
        public AnnouncementType AnnouncementType { get; set; }

    }
}
