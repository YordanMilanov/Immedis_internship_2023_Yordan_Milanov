using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.Recommendation;

namespace HCMS.Services.Interfaces
{
    public interface IRecommendationService
    {
        Task AddAsync(RecommendationDto recommendationDto);
        Task<QueryDtoResult<RecommendationDto>> GetCurrentPageAsync(QueryDto model, Guid companyId);
    }
}
