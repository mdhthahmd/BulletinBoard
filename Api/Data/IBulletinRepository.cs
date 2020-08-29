using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Data
{
    public interface IBulletinRepository 
    {
        Task<IEnumerable<Bulletin>> GetBulletins();
        Task<Bulletin> GetBulletin(int id);
        Task<Bulletin> AddBulletin(Bulletin bulletin);
        Task<Bulletin> UpdateBulletin(Bulletin bulletin);
        Task<Bulletin> DeleteBulletin(int id);
    }
}