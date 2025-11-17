namespace Application.Common.DTOs
{
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string? Email { get; set; }
        public string Token { get; set; } = "";
    }
}
