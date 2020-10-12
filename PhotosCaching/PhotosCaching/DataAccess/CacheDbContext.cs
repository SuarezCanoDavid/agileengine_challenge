using Microsoft.EntityFrameworkCore;
using PhotosCaching.Domain.PhotosFeed;
using System.Threading.Tasks;

namespace PhotosCaching.DataAccess
{
    public class CacheDbContext : DbContext, ICacheDbContext
    {
        public CacheDbContext(DbContextOptions<CacheDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<PictureDetails> PictureDetails { get; set; }

        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<PictureDetails>();

            entity.ToTable("PICTURES");

            entity.HasKey(x => x.Id).HasName("id");
            entity.Property(x => x.Author).HasColumnName("author");
            entity.Property(x => x.Camera).HasColumnName("camera");
            entity.Property(x => x.Tags).HasColumnName("tags");
            entity.Property(x => x.CroppedPictureUrl).HasColumnName("cropped_picture_url");
            entity.Property(x => x.FullPictureUrl).HasColumnName("full_picture_url");
        }
    }
}
