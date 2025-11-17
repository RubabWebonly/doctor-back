using Doctor.Domain.Entities;
using Doctor.Application.Interfaces.Repositories;
using MediatR;

namespace Doctor.Application.CQRS.UserProfiles.Commands
{
    public class UpsertUserProfileCommand : IRequest<int>
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Speciality { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string WorkNumber { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
    }

    public class UpsertUserProfileHandler : IRequestHandler<UpsertUserProfileCommand, int>
    {
        private readonly IUserProfileRepository _repo;
        private readonly IUnitOfWork _uow;

        public UpsertUserProfileHandler(IUserProfileRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<int> Handle(UpsertUserProfileCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByEmailAsync(request.Email);

            if (existing is not null)
            {
                // 🔹 Mövcud profili yenilə
                existing.FullName = request.FullName;
                existing.Email = request.Email;
                existing.Speciality = request.Speciality;
                existing.MobileNumber = request.MobileNumber;
                existing.WorkNumber = request.WorkNumber;
                existing.ProfileImageUrl = request.ProfileImageUrl;

                _repo.Update(existing); // ✅ Async deyil
            }
            else
            {
                // 🔹 Yeni profil yarat
                var newProfile = new UserProfile
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Speciality = request.Speciality,
                    MobileNumber = request.MobileNumber,
                    WorkNumber = request.WorkNumber,
                    ProfileImageUrl = request.ProfileImageUrl
                };

                await _repo.AddAsync(newProfile);
            }

            await _uow.SaveChangesAsync();
            return 1;
        }
    }
}
