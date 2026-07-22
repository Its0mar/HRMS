using HRMS.Domain.Entities.Employees.Enums;

namespace HRMS.Domain.Entities.Employees
{
    public sealed class PersonalInformation
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateOnly DateOfBirth { get; private set; }
        public Gender Gender { get; private set; }
        public string NationalId { get; private set; }
        public string Nationality { get; private set; }
        public MaritalStatus MaritalStatus { get; private set; }
        public string Phone { get; private set;  }
        public string Email { get; private set;  }
        public string Address { get; private set; }
        public string? ProfilePictureUrl { get; private set; }

        public PersonalInformation(
            string firstName,
            string lastName,
            DateOnly dateOfBirth,
            Gender gender,
            string nationalId,
            string nationality,
            MaritalStatus maritalStatus,
            string phone,
            string email,
            string address,
            string? profilePictureUrl)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            NationalId = nationalId;
            Nationality = nationality;
            MaritalStatus = maritalStatus;
            Phone = phone;
            Email = email;
            Address = address;
            ProfilePictureUrl = profilePictureUrl;
        }
    }
}
