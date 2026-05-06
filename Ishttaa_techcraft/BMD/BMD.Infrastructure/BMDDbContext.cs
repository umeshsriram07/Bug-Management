using BMD.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BMD.Infrastructure
{
    public class BMDDbContext : DbContext
    {
        public BMDDbContext(DbContextOptions<BMDDbContext> options)
            : base(options)
        {
        }

        public DbSet<Bug> Bugs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BugComment> BugComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bug>()
                .HasOne(b => b.Creator)
                .WithMany(u => u.CreatedBugs)
                .HasForeignKey(b => b.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bug>()
                .HasOne(b => b.Assignee)
                .WithMany()
                .HasForeignKey(b => b.AssignedTo)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}