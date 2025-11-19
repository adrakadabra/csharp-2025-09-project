namespace FrontService.Services.Auth
{
    namespace FrontService.Services.Auth
    {
        public interface IAuthService
        {
            Task<(bool ok, string role, string token)> Login(string username, string password);
        }
    }
}