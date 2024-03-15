using back_map.Business.Common;

namespace back_map.Entity.Dto
{
    public class AnnoucementDatails
    {
         public int Id { get; set; }
        public int CategoryId { get; set; }
        public int? AddressId { get; set; }
        public string? Phone { get; set; }
        public int MyProperty { get; set; }

        public int Bedrooms { get; set; }
        public int Fairs { get; set; }
        public int Bathrooms { get; set; }
        public int Floors { get; set; }
        public decimal LivingSpace { get; set; }
        public decimal TotalSurface { get; set; }
        public string? Age { get; set; }
        public decimal Price { get; set; }
        public AnnouncementType AnnouncementType { get; set; }
        public string? Description { get; set; }
        public List<string>? MoreDetails { get; set; }
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string FullAddress { get; set; } = string.Empty;
    }
}
