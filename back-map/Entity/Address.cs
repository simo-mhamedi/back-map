namespace back_map.Entity
{
    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal Latitude { get; set; }  // Corrected spelling: decimal, Latitude
        public decimal Longitude { get; set; } // Corrected spelling: decimal, Longitude
        public string FullAddress { get; set; } // Corrected spelling: FullAddress
    }

}
