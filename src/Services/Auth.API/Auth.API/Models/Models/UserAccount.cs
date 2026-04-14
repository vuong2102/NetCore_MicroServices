namespace Auth.Grpc.Models.Models
{
    public class UserAccount : BaseModel
    {
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public required string Role { get; set; }
        public required string UserId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public required string IsActive { get; set; }
    }
}
