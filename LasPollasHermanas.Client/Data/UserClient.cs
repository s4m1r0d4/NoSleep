using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LasPollasHermanas.Shared;
using System.Net.Http.Json;
using System.Text.Json;


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
}