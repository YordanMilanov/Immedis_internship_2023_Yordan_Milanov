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
        private readonly IMapper mapper;

        public WorkRecordService(IWorkRecordRepository workRecordRepository, IMapper mapper)
        {
            this.workRecordRepository = workRecordRepository;
            this.mapper = mapper;
        }

        public async Task AddWorkRecord(WorkRecordDto model)
        {
            WorkRecord workRecord = mapper.Map<WorkRecord>(model);

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
