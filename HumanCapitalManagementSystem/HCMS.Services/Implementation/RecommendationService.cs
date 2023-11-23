using AutoMapper;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Recommendation;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Services.Implementation
{
    public class RecommendationService : IRecommendationService
    {

        private readonly IRecommendationRepository recommendationRepository;
        private readonly IMapper mapper;

        public RecommendationService(IRecommendationRepository recommendationRepository,IMapper mapper)
        {
            this.recommendationRepository = recommendationRepository;
            this.mapper = mapper;
        }



        public async Task AddAsync(RecommendationDto recommendationDto)
        {
            try
            {
                Recommendation recommendation = mapper.Map<Recommendation>(recommendationDto);
                await recommendationRepository.AddAsync(recommendation);
            } catch (Exception)
            {
                throw new DbUpdateException("Unexpected error occurred while adding the recommendation!");
            }
        }
    }
}
