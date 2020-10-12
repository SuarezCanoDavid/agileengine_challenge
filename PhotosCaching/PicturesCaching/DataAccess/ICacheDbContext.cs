using Microsoft.EntityFrameworkCore;
using PicturesCaching.Domain.PhotosFeed;
using System.Threading.Tasks;

namespace PicturesCaching.DataAccess
{
    public interface ICacheDbContext
    {
        DbSet<PictureDetails> PictureDetails { get; set; }
        Task<int> SaveChangesAsync();
    }
}