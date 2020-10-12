using Microsoft.EntityFrameworkCore;
using PhotosCaching.Domain.PhotosFeed;
using System.Threading.Tasks;

namespace PhotosCaching.DataAccess
{
    public interface ICacheDbContext
    {
        DbSet<PictureDetails> PictureDetails { get; set; }
        Task<int> SaveChangesAsync();
    }
}