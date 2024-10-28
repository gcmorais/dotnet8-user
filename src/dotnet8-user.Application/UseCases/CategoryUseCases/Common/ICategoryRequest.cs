namespace dotnet8_user.Application.UseCases.CategoryUseCases.Common
{
    public interface ICategoryRequest
    {
        Guid UserId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }
}
