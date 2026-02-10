using Microsoft.EntityFrameworkCore;
using TimeScale.Application.Entities;

namespace TimeScale.DataAccess
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Result> Results { get; set; }
        public DbSet<ValueRecord> ValueRecords { get; set; }
        public DbSet<UploadedFile> UploadedFiles  { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
