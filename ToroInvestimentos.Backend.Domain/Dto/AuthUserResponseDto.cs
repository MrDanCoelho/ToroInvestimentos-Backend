namespace ToroInvestimentos.Backend.Domain.Dto
{
    public class AuthUserResponseDto
    {
        public string Username { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}