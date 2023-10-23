using System.Security.Claims;
using Blazored.SessionStorage;
using LasPollasHermanas.Shared;
using Microsoft.AspNetCore.Components.Authorization;

namespace LasPollasHermanas.Client.Extensions;

public class AuthExtension : AuthenticationStateProvider
{
    private readonly ISessionStorageService _sessionStorage;
    private ClaimsPrincipal _anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

    public AuthExtension(ISessionStorageService sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public async Task UpdateAuthStatus(SessionDTO? sessionDTO)
    {
        ClaimsPrincipal claimsPrincipal;
        if (sessionDTO != null)
        {
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Name, sessionDTO.Name),
                new(ClaimTypes.Email, sessionDTO.Email),
                new(ClaimTypes.Role, sessionDTO.Role),
            }, "JwtAuth"));

            await _sessionStorage.SaveStorage("userSession", sessionDTO);
        }
        else
        {
            claimsPrincipal = _anonymousUser;
            await _sessionStorage.RemoveItemAsync("userSession");
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var sessionDTO = await _sessionStorage.GetStorage<SessionDTO>("userSession");
        if (sessionDTO == null)
        {
            return await Task.FromResult(new AuthenticationState(_anonymousUser));
        }
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.Name, sessionDTO.Name),
            new(ClaimTypes.Email, sessionDTO.Email),
            new(ClaimTypes.Role, sessionDTO.Role)
        }, "JwtAuth"));

        return await Task.FromResult(new AuthenticationState(claimsPrincipal));
    }
}