using System.ComponentModel.DataAnnotations;

namespace HRMS.Application.Common.Settings
{
    public record JwtSettings
    {
        public const string SectionName = "Jwt";

        [Required(ErrorMessage = "JWT Signing Key is required.")]
        [MinLength(32, ErrorMessage = "JWT Signing Key must be at least 32 characters long.")]
        public string Key { get; init; } = string.Empty;

        [Required(ErrorMessage = "JWT Issuer is required.")]
        public string Issuer { get; init; } = string.Empty;

        [Required(ErrorMessage = "JWT Audience is required.")]
        public string Audience { get; init; } = string.Empty;

        public int ExpiryMinutes { get; init; } = 60;
    }
}
