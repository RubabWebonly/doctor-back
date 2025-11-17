using Doctor.Domain.Entities;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; } 
    public string PasswordHash { get; set; }

    public string? Email { get; set; }
    public string? ResetCode { get; set; }

    public DateTime? ResetCodeExpireDate { get; set; }

}
