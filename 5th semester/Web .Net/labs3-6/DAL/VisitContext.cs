using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class VisitContext : DbContext
    {
        public VisitContext() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Sanitas;Trusted_Connection=True;MultipleActiveResultSets=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Visit>(entity =>
            {
                entity.HasKey(e => e.Record);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Doctor).IsRequired();
                entity.Property(e => e.Patient);
                entity.Property(e => e.Diagnosis);
            });
        }

        public DbSet<Visit> Visits { get; set; }
    }
}
