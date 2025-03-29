using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentalsProject.Models;

namespace RentalsProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUserProfile>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<RentalListing> RentalListings { get; set; }

        public DbSet<FileModel> Files { get; set; }

        public DbSet<Reservation> Reservation { get; set; }
    }
}
