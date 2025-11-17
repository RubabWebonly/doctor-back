using MediatR;

namespace Doctor.Application.CQRS.PatientPrescriptions.Queries
{
    public class GetAllPatientPrescriptionsQuery : IRequest<object>
    {
    }
}
