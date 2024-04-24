namespace ECommerceSolution.BLL.Interfaces
{
    public class TokenDTO
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; } = "Bearer";
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}