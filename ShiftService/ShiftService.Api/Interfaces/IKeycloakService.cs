namespace ShiftService.Api.Interfaces
{
    public interface IKeycloakService
    {
        Task<string> GetTemporaryTokenAsync();
        Task<string> CreateUserAsync(string username, string email, string firstName, string lastName);
        Task<bool> UserExistsAsync(string username);
        Task<KeycloakUser> GetUserByUsernameAsync(string username);
    }

    public class KeycloakUser
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Enabled { get; set; }
    }
}
