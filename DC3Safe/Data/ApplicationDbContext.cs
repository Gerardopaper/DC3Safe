using DC3Safe.Models;
using Microsoft.EntityFrameworkCore;

namespace DC3Safe.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {            
        }

        public DbSet<DC3Report> DC3Reports { get; set; }
        public DbSet<ProgramCategory> ProgramCategories { get; set; }
        public DbSet<ProgramInformation> ProgramsInformation { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Occupation> Occupations { get; set; }
        public DbSet<WorkerInformation> WorkersInformation { get; set; }
        public DbSet<Trainer> Trainers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<DC3Report>(report =>
            {
                report.HasKey(x => x.Id);
                report.Property(x => x.Id).HasMaxLength(36);
            });

            builder.Entity<ProgramCategory>(programCategory =>
            {
                programCategory.HasKey(x => x.Id);
                programCategory.Property(x => x.Id).HasMaxLength(36);
            });

            builder.Entity<ProgramCategory>(programInformation =>
            {
                programInformation.HasKey(x => x.Id);
                programInformation.Property(x => x.Id).HasMaxLength(36);
            });

            builder.Entity<Company>(company =>
            {
                company.HasKey(x => x.Id);
                company.Property(x => x.Id).HasMaxLength(36);
            });

            builder.Entity<Occupation>(occupation =>
            {
                occupation.HasKey(x => x.Id);
                occupation.Property(x => x.Id).HasMaxLength(36);
            });

            builder.Entity<WorkerInformation>(workerInformation =>
            {
                workerInformation.HasKey(x => x.Id);
                workerInformation.Property(x => x.Id).HasMaxLength(36);
            });

            builder.Entity<Trainer>(trainer =>
            {
                trainer.HasKey(x => x.Id);
                trainer.Property(x => x.Id).HasMaxLength(36);
            });
        }
    }
}
