using Doctor.Domain.Entities;
using Doctor.Application.Interfaces.Repositories;
using MediatR;

namespace Doctor.Application.CQRS.UserProfiles.Queries
{
    public class GetUserProfileQuery : IRequest<UserProfile?>
    {
        public string Email { get; set; }
    }

    public class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, UserProfile?>
    {
        private readonly IUserProfileRepository _repo;

        public GetUserProfileHandler(IUserProfileRepository repo)
        {
            _repo = repo;
        }

        public async Task<UserProfile?> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetByEmailAsync(request.Email);
        }
    }
}
