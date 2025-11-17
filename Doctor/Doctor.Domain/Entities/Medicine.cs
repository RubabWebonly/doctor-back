using Doctor.Domain.Entities;

public class Medicine : BaseEntity
{
    public string Name { get; set; }
    public bool IsActive { get; set; } = true;
}
