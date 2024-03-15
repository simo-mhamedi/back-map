using back_map.Business.Common;
using JwtWebApiTutorial;

namespace back_map.Entity
{
    public class Announcement
    {
        public int Id { get; set; }

        public int CategoryId { get; set; } // Corrected property name to follow C# conventions

        public Category Category { get; set; } // Assuming "Category" is the correct class name

        public int AddressId { get; set; }

        public Address Address { get; set; }

        public int? UserId { get; set; }

        public User? User { get; set; }

        public string Phone { get; set; }

        public List<int>? MediaFileIds { get; set; } // Corrected property name to follow C# conventions

        public List<MediaFile> MediaFiles { get; set; } // Corrected property name to follow C# conventions
        public int Bedrooms { get; set; }
        public int Fairs { get; set; }
        public int Bathrooms { get; set; }
        public AnnouncementType AnnouncementType { get; set; }
        public int Floors { get; set; }
        public decimal LivingSpace { get; set; }
        public decimal TotalSurface { get; set; }
        public string Age { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public string MoreDetails { get; set; }
        public DateTime CreationDateDate { get; set; }
    }
}
