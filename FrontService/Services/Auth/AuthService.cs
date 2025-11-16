using FrontService.Services.Auth.FrontService.Services.Auth;

namespace FrontService.Services.Auth
{
    public class AuthService : IAuthService
    {
        public Task<(bool ok, string role, string token)> Login(string username, string password)
        {
            if (username == "admin" && password == "admin")
                return Task.FromResult((true, "admin", "fake-token"));

            if (username == "worker" && password == "1234")
                return Task.FromResult((true, "picker", "fake-token"));

            return Task.FromResult((false, "", ""));
        }
    }
}