using Application.Common;
using Application.CQRS.Auth.Commands;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Auth.Handlers
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var input = request.PhoneNumber.Trim();
            User? user = null;

            if (input.Contains("@"))
            {
                user = await _userRepository.GetByEmailAsync(input, cancellationToken);
                if (user == null)
                    return Result<string>.Fail("Bu e-poçtla istifadəçi tapılmadı ❌");
            }
            else
            {
                if (!input.StartsWith("+994"))
                    input = "+994" + input;

                user = await _userRepository.GetByPhoneAsync(input, cancellationToken);
                if (user == null)
                    return Result<string>.Fail("Bu nömrə ilə istifadəçi tapılmadı ❌");
            }

            if (string.IsNullOrWhiteSpace(user.Email))
                return Result<string>.Fail("İstifadəçinin e-poçt ünvanı tapılmadı ❌");

            // ✅ Təsdiq kodunu yaradıb yadda saxlayırıq
            var code = new Random().Next(100000, 999999).ToString();
            user.ResetCode = code;
            user.ResetCodeExpireDate = DateTime.UtcNow.AddMinutes(5);

            await _userRepository.UpdateAsync(user, cancellationToken); // ✅ indi işləyəcək

            // ✉️ Email göndər
            var subject = "Şifrə bərpası üçün təsdiq kodu";
            var body = $@"
                <p>Salam, {user.FullName ?? "Hörmətli istifadəçi"}!</p>
                <p>Şifrəni sıfırlamaq üçün təsdiq kodunuz:</p>
                <h2 style='color:#264299'>{code}</h2>
                <p>Kod 5 dəqiqə ərzində etibarlıdır.</p>";

            await _emailService.SendAsync(user.Email, subject, body);

            return Result<string>.Ok("Təsdiq kodu e-poçt ünvanınıza göndərildi ✅");
        }
    }
}
