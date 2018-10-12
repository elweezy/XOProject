using System.Linq;

namespace XOProject
{
    public interface IPortfolioRepository : IGenericRepository<Portfolio>
    {
        IQueryable<Portfolio> GetAll();
    }
}