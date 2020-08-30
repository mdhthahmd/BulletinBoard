using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Api.Data
{
    public interface IBulletinRepository 
    {
        Task<IEnumerable<Bulletin>> GetBulletins();
        Task<Bulletin> GetBulletin(Guid id);
        Task<Bulletin> AddBulletin(Bulletin bulletin);
        Task<Bulletin> UpdateBulletin(Bulletin bulletin);
        Task<Bulletin> DeleteBulletin(Guid id);
    }
}