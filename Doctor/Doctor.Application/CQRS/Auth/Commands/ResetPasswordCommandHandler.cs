using Application.Common;
using Application.CQRS.Auth.Commands;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Auth.Handlers
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public ResetPasswordCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // 1️⃣ Email ilə istifadəçini tap
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null)
                return Result<string>.Fail("Bu e-poçtla istifadəçi tapılmadı ❌");

            // 2️⃣ Kod uyğunluğunu yoxla
            if (string.IsNullOrEmpty(user.ResetCode))
                return Result<string>.Fail("Təsdiq kodu yoxdur və ya vaxtı bitib!");

            if (user.ResetCode != request.Code)
                return Result<string>.Fail("Təsdiq kodu yanlışdır!");

            // (istəsən, vaxt yoxlamasını da əlavə edə bilərsən)
            if (user.ResetCodeExpireDate.HasValue && user.ResetCodeExpireDate < DateTime.UtcNow)
                return Result<string>.Fail("Kodun vaxtı bitib!");

            // 3️⃣ Yeni şifrəni hash et
            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);

            // 4️⃣ Reset məlumatlarını sıfırla
            user.ResetCode = null;
            user.ResetCodeExpireDate = null;

            // 5️⃣ Yeniləməni yadda saxla
            await _userRepository.UpdateAsync(user, cancellationToken);

            return Result<string>.Ok("Şifrəniz uğurla yeniləndi ✅");
        }
    }
}
