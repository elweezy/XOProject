using System.Collections.Generic;
using System.Threading.Tasks;

namespace XOProject
{
    public interface ITradeRepository : IGenericRepository<Trade>
    { 
        Task<List<Trade>> GetAllTradings(int portFolioId);
    }
}