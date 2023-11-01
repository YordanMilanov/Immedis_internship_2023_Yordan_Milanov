using AutoMapper;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.WorkRecord;

namespace HCMS.Services.Implementation
{
    internal class WorkRecordService : IWorkRecordService
    {
        private readonly IWorkRecordRepository workRecordRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;

        public WorkRecordService(IWorkRecordRepository workRecordRepository, IMapper mapper, ICompanyRepository companyRepository)
        {
            this.workRecordRepository = workRecordRepository;
            this.mapper = mapper;
            this.companyRepository = companyRepository;
        }

        public async Task AddWorkRecordAsync(WorkRecordDto model)
        {
            WorkRecord workRecord = mapper.Map<WorkRecord>(model);
            workRecord.Id = Guid.NewGuid();
            workRecord.EmployeeId = model.EmployeeId;

            if (model.CompanyName != null)
            {
                try
                {
                    Company company = await companyRepository.GetCompanyByNameAsync(model.CompanyName);
                    workRecord.Company = company;
                    workRecord.CompanyId = company.Id;
                }
                catch { }
            }
            try
            {
                await workRecordRepository.AddWorkRecordAsync(workRecord);
            }
            catch (Exception)
            {
                throw new Exception("Unexpected error occured while adding the work record!");
            }

        }


        public async Task<List<WorkRecordDto>> GetAllWorkRecordsDtosAsync()
        {
            List<WorkRecord> allWorkRecords = await workRecordRepository.AllWorkRecordsAsync();
            List<WorkRecordDto> AllWorkRecordsDtos = allWorkRecords
                       .Select(workRecord => mapper.Map<WorkRecordDto>(workRecord))
                       .ToList();

            return AllWorkRecordsDtos;
        }

        public async Task<List<WorkRecordDto>> GetAllWorkRecordsDtosByEmployeeIdAsync(Guid id)
        {
            try
            {
                List<WorkRecord> AllWorkRecords = await workRecordRepository.AllWorkRecordsByEmployeeIdAsync(id);
                List<WorkRecordDto> AllWorkRecordsDto = AllWorkRecords
                .Select(workRecord => mapper.Map<WorkRecordDto>(workRecord))
                .ToList();

                return AllWorkRecordsDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<WorkRecordDto>> GetWorkRecordsPageAsync(WorkRecordQueryDto searchModel)
        {
            try
            {
                List<WorkRecord> workRecords = await workRecordRepository.GetWorkRecordsPageAsync(searchModel);
                List<WorkRecordDto> workRecordDtos = workRecords.Select(wr => mapper.Map<WorkRecordDto>(wr)).ToList();
                return workRecordDtos;
            }
            catch (Exception)
            {
                throw new Exception("Unexpected error occurred while trying to load the page!");
            }
        }

        public async Task<int> GetWorkRecordsCountByEmployeeIdAsync(Guid employeeId)
        {
            try
            {
                return await workRecordRepository.WorkRecordsCountByEmployeeIdAsync(employeeId);
            }
            catch (Exception)
            {
                throw new Exception("No work records was found");
            }
        }
    }
}
