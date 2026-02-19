using Microsoft.EntityFrameworkCore;
using URLSortner.Model;
public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options){}
    public DbSet<URLShort> URLShort { get; set; }
    protected override void OnModelCreating(ModelBuilder modBuilder)
    {
       var entityBuilder= modBuilder.Entity<URLShort>();
        entityBuilder.HasKey(x=>x.ID);
        entityBuilder.Property(x => x.ID).ValueGeneratedOnAdd();
        entityBuilder.Property(x=> x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        entityBuilder.Property(x => x.Password);
        entityBuilder.Property(x => x.OriginalURL).IsRequired();
        entityBuilder.HasIndex(x => x.ShortID);
    }
}