using HCMS.Services.ServiceModels.Recommendation;

namespace HCMS.Services.Interfaces
{
    public interface IRecommendationService
    {
        Task AddAsync(RecommendationDto recommendationDto);
    }
}
