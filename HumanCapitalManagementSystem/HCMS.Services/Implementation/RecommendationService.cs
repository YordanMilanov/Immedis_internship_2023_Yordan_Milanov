using AutoMapper;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.Recommendation;

namespace HCMS.Services.Implementation
{
    public class RecommendationService : IRecommendationService
    {

        private readonly IRecommendationRepository recommendationRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IMapper mapper;

        public RecommendationService(IRecommendationRepository recommendationRepository, ICompanyRepository companyRepository, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            this.recommendationRepository = recommendationRepository;
            this.employeeRepository = employeeRepository;
            this.companyRepository = companyRepository;
            this.mapper = mapper;
        }



        public async Task AddAsync(RecommendationDto recommendationDto)
        {
            try
            {
                Recommendation recommendation = mapper.Map<Recommendation>(recommendationDto);
                Employee recommender = await this.employeeRepository.GetEmployeeByIdAsync(recommendationDto.RecommenderId);
                Employee recommendedEmployee = await this.employeeRepository.GetEmployeeByEmailAsync(recommendationDto.EmployeeEmail.ToString());
                if (recommender.CompanyId != recommendedEmployee.CompanyId)
                {
                    throw new InvalidOperationException("The employee you are trying to recommend is not working in your company!");
                }
                else if (recommendedEmployee.Id == recommender.Id)
                {
                    throw new InvalidOperationException("You cannot recommend yourself!");
                }
                Company receivingCompany = await this.companyRepository.GetCompanyByNameAsync(recommendationDto.CompanyName.ToString());

                recommendation.ForEmployeeId = recommendedEmployee.Id;
                recommendation.ToCompanyId = receivingCompany.Id;

                await recommendationRepository.AddAsync(recommendation);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<QueryDtoResult<RecommendationDto>> GetCurrentPageAsync(QueryDto model, Guid companyId)
        {
            try
            {
                QueryParameterClass parameters = mapper.Map<QueryParameterClass>(model);
                QueryPageWrapClass<Recommendation> result = await this.recommendationRepository.GetCurrentPageAsync(parameters, companyId);

                QueryDtoResult<RecommendationDto> modelToReturn = mapper.Map<QueryDtoResult<RecommendationDto>>(result);
                modelToReturn.CurrentPage = model.CurrentPage;
                modelToReturn.OrderPageEnum = model.OrderPageEnum;
                modelToReturn.ItemsPerPage = model.ItemsPerPage;
                modelToReturn.SearchString = model.SearchString;
                return modelToReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
