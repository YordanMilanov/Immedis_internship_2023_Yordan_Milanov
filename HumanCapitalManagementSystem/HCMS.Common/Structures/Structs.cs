using HCMS.Common.JsonConverter;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HCMS.Common.Structures
{

    public readonly struct Description
    {
        private readonly string description;

        [JsonConstructor]
        public Description(string description)
        {
            this.description = description;
        }

        public override string ToString()
        {
            return this.description;
        }
    }

    [JsonConverter(typeof(NameConverter))]
    public readonly struct Name
    {
        private readonly string value;
      
        [JsonConstructor]
        public Name(string name)
        {
            this.value = name;
        }

        public override string ToString()
        {
            return this.value;
        }
    }




    [JsonConverter(typeof(PasswordConverter))]
    public readonly struct Password
    {
        private readonly string password;

        [JsonConstructor]
        public Password(string password)
        {
            this.password = password;
        }

        public override string ToString()
        {
            return this.password;
        }
    }


    [JsonConverter(typeof(EmailConverter))]
    public readonly struct Email
    {
        public readonly string value;
        [JsonConstructor]
        public Email([EmailAddress] string emailAddress)
        {
            this.value = emailAddress;
        }

        public override string ToString()
        {
            return this.value;
        }

        public bool CompareTo(string toCompare)
        {
            return value == toCompare;
        }
    }

    [JsonConverter(typeof(RoleConverter))]
    public readonly struct RoleStruct
    {
        public readonly string value;
        [JsonConstructor]
        public RoleStruct(string role)
        {
            this.value = role;
        }

        public override string ToString()
        {
            return this.value;
        }
    }

    //specific

    //diploma
    public readonly struct Phone
    {
        private readonly string phone;

        public Phone(string phone)
        {
            this.phone = phone;
        }

        public string GetPhoneNumber()
        {
            return this.phone;
        }
    }

    public readonly struct Photo
    {
        private readonly string? photo;

        public Photo(string? photo)
        {
            this.photo = photo;
        }

        public string GetPhotoUrl()
        {
            return this.photo ?? "No photo Link";
        }
    }

    public readonly struct Position
    {
        private readonly string position;

        public Position(string position)
        {
            this.position = position;
        }

        public string GetValue()
        {
            return this.position;
        }
    }

    public readonly struct Department
    {
        private readonly string department;

        public Department(string department)
        {
            this.department = department;
        }

        public string GetValue()
        {
            return this.department;
        }
    }

    public readonly struct Salary
    {
        private readonly decimal salary;

        public Salary(decimal salary)
        {
            this.salary = salary;
        }

        public override string ToString()
        {
            return this.salary.ToString();
        }

        public decimal GetValue()
        {
            return this.salary;
        }
    }

    public readonly struct CoverLetter
    {
        private readonly string coverLetter;

        public CoverLetter(string coverLetter)
        {
            this.coverLetter = coverLetter;
        }

        public string GetValue()
        {
            return this.coverLetter;
        }
    }

    public readonly struct Degree
    {
        private readonly string degree;

        public Degree(string degree)
        {
            this.degree = degree;
        }

        public string GetValue()
        {
            return this.degree;
        }
    }

    public readonly struct FieldOfEducation
    {
        private readonly string fieldOfEducation;

        public FieldOfEducation(string fieldOfEducation)
        {
            this.fieldOfEducation = fieldOfEducation;
        }

        public string GetValueg()
        {
            return this.fieldOfEducation;
        }
    }

    public readonly struct IndustryField
    {
        private readonly string industryField;

        public IndustryField(string industryField)
        {
            this.industryField = industryField;
        }

        public string GetValue()
        {
            return this.industryField;
        }
    }

    public readonly struct IndustryCategory
    {
        private readonly string industryCategory;

        public IndustryCategory(string industryCategory)
        {
            this.industryCategory = industryCategory;
        }

        public string GetValue()
        {
            return this.industryCategory;
        }
    }


    [JsonConverter(typeof(LocationConverter))]
    public readonly struct LocationStruct
    {
        private readonly string? address;
        private readonly string state;
        private readonly string country;

        [JsonConstructor]
        public LocationStruct(string? address, string state, string country)
        {
            this.address = address;
            this.state = state;
            this.country = country;
        }

        public string GetAddress()
        {
            return this.address ?? "No Address";
        }

        public string GetState()
        {
            return this.state;
        }

        public string GetCountry()
        {
            return this.country;
        }
    }
}
