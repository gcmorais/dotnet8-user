namespace dotnet8_user.Application.UseCases.UserUseCases.Common
{
    public interface IUserRequest
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
    }
}
