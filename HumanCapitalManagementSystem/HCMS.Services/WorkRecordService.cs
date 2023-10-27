using AutoMapper;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.WorkRecord;
using System.Reflection.Metadata.Ecma335;

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

        public async Task AddWorkRecord(WorkRecordDto model)
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
               await workRecordRepository.AddWorkRecord(workRecord);
            } 
            catch(Exception)
            {
                throw new Exception("Unexpected error occured while adding the work record!");
            }

        }
    }
}
