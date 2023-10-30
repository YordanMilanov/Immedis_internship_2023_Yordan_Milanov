using AutoMapper;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.WorkRecord;

namespace HCMS.Services
{
    public class WorkRecordService : IWorkRecordService
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
                } catch { }  
            }
            try
            {
               await workRecordRepository.AddWorkRecordAsync(workRecord);
            } 
            catch(Exception)
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

        public async Task<List<WorkRecordDto>> GetAllWorkRecordsDtosByEmployeeId(Guid id)
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

        public async Task<List<WorkRecordDto>> FilteredBySearchAndOrdered()
        {


            throw new NotImplementedException();
        }
    }
}
