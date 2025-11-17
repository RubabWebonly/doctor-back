using Doctor.Domain.Entities;

namespace Doctor.Domain.Entities
{
    public class UserProfile : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Speciality { get; set; }
        public string MobileNumber { get; set; }
        public string WorkNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
