using ELTBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ELTBackend.Data
{
    public class EmployeeLeaveTrackerDbContext : DbContext
    {
        public EmployeeLeaveTrackerDbContext(
            DbContextOptions<EmployeeLeaveTrackerDbContext> options
        )
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Leave> Leaves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Admin
            modelBuilder
                .Entity<User>()
                .HasData(
                    new User
                    {
                        Id = Guid.Parse("9e6d8c7b-4a4a-4f1e-9a3f-8f6c4c2b1a9d"),
                        Name = "Admin",
                        Email = "admin@gmail.com",
                        Role = "Admin",
                        PasswordHash =
                            "$2a$11$PPDEDA5XLmss8nSmo6Je8O34gVQH6Tuza/nNEIKC8cIca0bSFGQUG",
                        IsDeleted = false,
                    }
                );
        }
    }
}
