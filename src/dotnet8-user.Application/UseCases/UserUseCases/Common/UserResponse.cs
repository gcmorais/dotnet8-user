using dotnet8_user.Application.UseCases.CategoryUseCases.Common;

namespace dotnet8_user.Application.UseCases.UserUseCases.Common
{
    public sealed record UserResponse
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public byte[] HashPassword { get; set; }
        public byte[] SaltPassword { get; set; }
        public List<CategoryShortResponse> Categories { get; set; }
    }
}
