using MediatR;

public class CreateMedicineCommand : IRequest<int>
{
    public string Name { get; set; }
}
