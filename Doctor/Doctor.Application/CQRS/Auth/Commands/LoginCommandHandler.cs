using Application.Common;
using Application.Common.DTOs;
using Application.CQRS.Auth.Commands;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.CQRS.Auth.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LoginCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var phone = request.PhoneNumber?.Trim();

            if (string.IsNullOrWhiteSpace(phone))
                return Result<LoginResponseDto>.Fail("Telefon nömrəsi boş ola bilməz ❌");

            if (!phone.StartsWith("+994"))
                phone = "+994" + phone;

            var user = await _userRepository.GetByPhoneAsync(phone, cancellationToken);
            if (user == null)
                return Result<LoginResponseDto>.Fail("İstifadəçi tapılmadı ❌");

            var password = request.Password?.Trim();
            if (string.IsNullOrWhiteSpace(password))
                return Result<LoginResponseDto>.Fail("Şifrə boş ola bilməz ❌");

            var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (verify == PasswordVerificationResult.Failed)
                return Result<LoginResponseDto>.Fail("Şifrə yanlışdır ❌");

            // 🔥 Token kimi GUID qaytarırıq (1 user üçün kifayətdir)
            var token = Guid.NewGuid().ToString();

            var response = new LoginResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Token = token
            };

            return Result<LoginResponseDto>.Ok(response);
        }
    }

}
