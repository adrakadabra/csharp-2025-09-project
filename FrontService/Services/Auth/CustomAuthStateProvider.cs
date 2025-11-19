using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _js;

    public CustomAuthStateProvider(IJSRuntime js)
    {
        _js = js;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _js.InvokeAsync<string>("localStorage.getItem", "token");
        var role = await _js.InvokeAsync<string>("localStorage.getItem", "role");

        if (string.IsNullOrWhiteSpace(token))
        {
            var anonymous = new ClaimsIdentity();
            return new AuthenticationState(new ClaimsPrincipal(anonymous));
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "User"),
            new Claim(ClaimTypes.Role, role)
        };

        var identity = new ClaimsIdentity(claims, "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task SetAuth(string token, string role)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", "token", token);
        await _js.InvokeVoidAsync("localStorage.setItem", "role", role);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task Logout()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", "token");
        await _js.InvokeVoidAsync("localStorage.removeItem", "role");

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}