using HCMS.Data.Models;

namespace HCMS.Repository.Interfaces
{
    public interface IRecommendationRepository
    {
        Task AddAsync(Recommendation recommendation);
    }
}
