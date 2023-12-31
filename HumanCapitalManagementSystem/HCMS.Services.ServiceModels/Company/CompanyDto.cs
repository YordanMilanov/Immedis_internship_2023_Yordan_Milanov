﻿using HCMS.Common.Structures;

namespace HCMS.Services.ServiceModels.Company
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string IndustryField { get; set; } = null!;

        public LocationStruct Location { get; set; }
        public Guid? LocationId { get; set; }
    }
}