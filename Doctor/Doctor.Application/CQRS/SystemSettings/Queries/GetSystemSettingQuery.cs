using Application.Common;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.SystemSettings.Queries
{
    public class GetSystemSettingQuery : IRequest<Result<SystemSetting>> { }
}
