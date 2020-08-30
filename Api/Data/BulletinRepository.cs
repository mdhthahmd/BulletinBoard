using Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Api.Data
{
    public class BulletinRepository : IBulletinRepository
    {
        private readonly AppDbContext appDbContext;

        public BulletinRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Bulletin>> GetBulletins()
        {
            IQueryable<Bulletin> query = appDbContext.Bulletins;
            query = query.OrderByDescending(item => item.CreatedAt).Where(e => e.Status == Status.Active);

            return await query.ToListAsync();
            // return await appDbContext.Bulletins.ToListAsync();
        }

        public async Task<Bulletin> GetBulletin(int bulletinId)
        {
            return await appDbContext.Bulletins
                .FirstOrDefaultAsync(e => e.Id == bulletinId);
        }

        public async Task<Bulletin> AddBulletin(Bulletin Bulletin)
        {
            var result = await appDbContext.Bulletins.AddAsync(Bulletin);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Bulletin> UpdateBulletin(Bulletin Bulletin)
        {
            var result = await appDbContext.Bulletins
                .FirstOrDefaultAsync(e => e.Id == Bulletin.Id);

            if (result != null)
            {
                result.Id = Bulletin.Id;
                result.HeadingText = Bulletin.HeadingText;
                result.Status = Bulletin.Status;
                await appDbContext.SaveChangesAsync();
                return result;
            }
            return null;
        }

        
        public async Task<Bulletin> DeleteBulletin(int bulletinId)
        {
            var result = await appDbContext.Bulletins
                .FirstOrDefaultAsync(e => e.Id == bulletinId);
            if (result != null)
            {
                appDbContext.Bulletins.Remove(result);
                await appDbContext.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}