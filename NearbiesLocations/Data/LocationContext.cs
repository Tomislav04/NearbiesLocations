using Microsoft.EntityFrameworkCore;
using NearbiesLocations.Models;

namespace NearbiesLocations.Data
{
    public class LocationContext : DbContext
    {
        public LocationContext(DbContextOptions<LocationContext> options) : base(options) { }

        public DbSet<Location> Locations { get; set; }
        public DbSet<SearchRequest> SearchRequests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FavoriteLocation> FavoriteLocations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<LocationCategory> LocationCategory { get; set; }
        public DbSet<RequestResponse> RequestResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapiranje za entitet Location
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.LocationID);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Latitude).IsRequired(false);
                entity.Property(e => e.Longitude).IsRequired(false);
                entity.Property(e => e.Address).IsRequired(false).HasMaxLength(200);
                entity.Property(e => e.ExternalID).IsRequired().HasMaxLength(200);
                entity.HasIndex(l => new { l.ExternalID })
                        .IsUnique();
            });

            // Mapiranje za entitet User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserID);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.ApiKey).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.ApiKey).IsUnique(); // Unikatni indeks za API ključ
            });

            // Mapiranje za entitet SearchRequest
            modelBuilder.Entity<SearchRequest>(entity =>
            {
                entity.HasKey(e => e.SearchRequestID);
                entity.Property(e => e.Latitude).IsRequired();
                entity.Property(e => e.Longitude).IsRequired();
                entity.Property(e => e.SearchTime).IsRequired();
                entity.HasOne(e => e.User)
                      .WithMany(u => u.SearchRequests)
                      .HasForeignKey(e => e.UserID);
                entity.HasOne(e => e.Category)
                      .WithMany(u => u.SearchRequests)
                      .HasForeignKey(e => e.CategoryID);
            });

            // Mapiranje za entitet SearchRequest
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryID);
                entity.Property(e => e.Name).IsRequired();
            });

            // Mapiranje za entitet FavouriteLocation
            modelBuilder.Entity<FavoriteLocation>(entity =>
            {
                entity.HasKey(fl => new { fl.FavoriteLocationID });
                entity.Property(e => e.AddedDate).IsRequired();

                entity.HasOne(fl => fl.User)
                      .WithMany(u => u.FavouriteLocations)
                      .HasForeignKey(fl => fl.UserID);
            });

            // Mapiranje za entitet FavouriteLocation
            modelBuilder.Entity<LocationCategory>(entity =>
            {
                entity.HasKey(lc => new { lc.LocationCategoryID }); 

                entity.HasOne(lc => lc.Location)
                      .WithMany(l => l.LocationCategories)
                      .HasForeignKey(lc => lc.LocationID);

                entity.HasOne(lc => lc.Category)
                      .WithMany(c => c.LocationCategories)
                      .HasForeignKey(lc => lc.CategoryID);
            });

            // Mapiranje za entitet RequestResponse
            modelBuilder.Entity<RequestResponse>(entity =>
            {
                entity.HasKey(rr => rr.RequestResponseID);
                entity.HasOne(rr => rr.SearchRequest)
                      .WithMany(sr => sr.RequestResponse)
                      .HasForeignKey(rr => rr.SearchRequestID);
                entity.Property(rr => rr.LocationsIDs);
            });
        }
    }
}
