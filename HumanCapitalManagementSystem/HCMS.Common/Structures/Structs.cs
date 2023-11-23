using HCMS.Common.JsonConverter;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using static HCMS.Common.DataModelConstants;

namespace HCMS.Common.Structures
{

    public readonly struct Description
    {

        private readonly string value;

        [JsonConstructor]
        public Description(string description)
        {
            this.value = description;
        }

        public override string ToString()
        {
            return this.value;
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
        private readonly string value;

        [JsonConstructor]
        public Password(string password)
        {
            this.value = password;
        }

        public override string ToString()
        {
            return this.value;
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
        private readonly string? value;

        public Phone(string? phone)
        {
            this.value = phone;
        }

        public override string ToString()
        {
            return this.value!;
        }
    }

    public readonly struct Photo
    {
        private readonly string value;

        public Photo(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return this.value!;
        }
    }

    public readonly struct Position
    {
        private readonly string value;

        public Position(string position)
        {
            this.value = position;
        }

        public override string ToString()
        {
            return this.value;
        }
    }

    public readonly struct Department
    {
        private readonly string value;

        public Department(string department)
        {
            this.value = department;
        }

        public override string ToString()
        {
            return this.value;
        }
    }

    public readonly struct Salary
    {
        private readonly decimal value;

        public Salary(decimal salary)
        {
            this.value = salary;
        }

        public override string ToString()
        {
            return this.value.ToString();
        }

        public decimal GetValue()
        {
            return this.value;
        }
    }

    public readonly struct CoverLetter
    {
        private readonly string value;

        public CoverLetter(string coverLetter)
        {
            this.value = coverLetter;
        }

        public override string ToString()
        {
            return this.value;
        }
    }

    public readonly struct Degree
    {
        private readonly string value;

        public Degree(string degree)
        {
            this.value = degree;
        }

        public override string ToString()
        {
            return this.value;
        }
    }

    public readonly struct FieldOfEducation
    {
        private readonly string value;

        public FieldOfEducation(string fieldOfEducation)
        {
            this.value = fieldOfEducation;
        }

        public override string ToString()
        {
            return this.value;
        }
    }

    public readonly struct IndustryField
    {
        private readonly string value;

        public IndustryField(string industryField)
        {
            this.value = industryField;
        }

        public override string ToString()
        {
            return this.value;
        }
    }

    public readonly struct IndustryCategory
    {
        private readonly string value;

        public IndustryCategory(string industryCategory)
        {
            this.value = industryCategory;
        }

        public override string ToString()
        {
            return this.value;
        }
    }

    public readonly struct LocationStruct
    {
        [StringLength(Location.AddressMaxLength, MinimumLength = Location.AddressMaxLength, ErrorMessage = "Address must be between {2} and {1} characters long")]
        private readonly string? address;

        [StringLength(Location.StateMaxLength,MinimumLength = Location.StateMinLength, ErrorMessage = "State must be between {2} and {1} characters long")]
        private readonly string state;

        [StringLength(Location.CountryMaxLength, MinimumLength = Location.CountryMinLength, ErrorMessage = "Country must be between {2} and {1} characters long")]
        private readonly string country;

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
