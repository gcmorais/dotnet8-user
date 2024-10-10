using System.ComponentModel.DataAnnotations;

namespace dotnet_user.WebApi.DTOs
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}
