using System.Collections.Generic;
using System.Threading.Tasks;

namespace XOProject
{
    public interface IShareRepository : IGenericRepository<HourlyShareRate>
    {
        Task<List<HourlyShareRate>> GetBySymbol(string symbol);
    }
}