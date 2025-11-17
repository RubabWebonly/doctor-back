using MediatR;

public class DeleteMedicineCommand : IRequest<bool>
{
    public int Id { get; set; }
}
