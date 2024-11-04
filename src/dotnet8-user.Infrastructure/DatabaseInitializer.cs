using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;

namespace dotnet8_user.Infrastructure
{
    public class DatabaseInitializer
    {
        private readonly IUserRepository _userRepository;
        private readonly ICreateVerifyHash _serviceHash;

        public DatabaseInitializer(IUserRepository userRepository, ICreateVerifyHash serviceHash)
        {
            _userRepository = userRepository;
            _serviceHash = serviceHash;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            var email = "masteradmin@example.com";
            var existingUser = await _userRepository.GetByEmail(email, cancellationToken);

            if (existingUser == null)
            {
                var password = "YourSecurePassword";
                _serviceHash.CreateHashPassword(password, out byte[] hashPassword, out byte[] saltPassword);

                var masterAdmin = new User("Master Admin", "masteradmin", email, hashPassword, saltPassword);

                try
                {
                    await _userRepository.CreateAdmin(masterAdmin, new List<string> { "MasterAdmin" }, cancellationToken);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
