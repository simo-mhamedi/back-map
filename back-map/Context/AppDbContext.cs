using back_map.Entity;
using JwtWebApiTutorial;
using Microsoft.EntityFrameworkCore;

namespace back_map.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<MoreInfo> MoreInfos { get; set; }
        public DbSet<AnnoucementInfo> AnnoucementInfos { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Favorite> Favorites{ get; set; }
    }
}
