using LasPollasHermanas.Shared;
using LasPollasHermanas.Shared.Models;
using System.Net.Http.Json;


namespace LasPollasHermanas.Client.Data;

public class UserClient
{
    private readonly HttpClient httpClient;

    public UserClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<SessionDTO?> TryLogin(LoginDTO loginDTO)
    {
        var response = await httpClient.PostAsJsonAsync("auth/login", loginDTO);
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<SessionDTO>();
        }
        else
        {
            return null;
        }
    }

    public async Task<MortalUserDTO?> TryRegister(MortalUserDTO registerDTO)
    {
        var response = await httpClient.PostAsJsonAsync("auth/register/mortaluser", registerDTO);
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<MortalUserDTO>();
        }
        else
        {
            return null;
        }
    }

    public async Task<AdminUserDTO?> TryRegisterAdmin(AdminUserDTO registerDTO)
    {
        var response = await httpClient.PostAsJsonAsync("auth/register/adminuser", registerDTO);
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AdminUserDTO>();
        }
        else
        {
            return null;
        }
    }

    public async Task<MortalUser?> GetMortalUserAsync(string email)
    {
        return await httpClient.GetFromJsonAsync<MortalUser?>($"auth/user/{email}");
    }
}