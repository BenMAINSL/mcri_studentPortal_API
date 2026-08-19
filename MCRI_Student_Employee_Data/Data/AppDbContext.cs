using MCRI_Student_Employee_Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MCRI_Student_Employee_Data.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Person> People => Set<Person>();

    // Mirrors the People table created by Database/MCRI_People_SqlServer.sql.
    // The table and its rows already exist, so there is no seeding here.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("People");

            entity.HasIndex(e => e.PersonType, "IX_People_PersonType");

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PersonType).HasMaxLength(20);
            entity.Property(e => e.DepartmentOrProgramme).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FunFact).HasMaxLength(500);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Cohort).HasMaxLength(100);
            entity.Property(e => e.Phase).HasMaxLength(100);
            entity.Property(e => e.ImageContentType).HasMaxLength(100);
        });
    }
}
