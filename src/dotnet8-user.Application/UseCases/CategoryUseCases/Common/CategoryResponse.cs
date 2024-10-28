using dotnet8_user.Application.UseCases.UserUseCases.Common;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.Common
{
    public sealed record CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public UserShortResponse User { get; set; }
    }
}
