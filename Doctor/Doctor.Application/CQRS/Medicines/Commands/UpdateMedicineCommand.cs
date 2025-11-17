using MediatR;

public class UpdateMedicineCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
}
