namespace LasPollasHermanas.Client.Data;
using System.Net.Http.Json;
using LasPollasHermanas.Shared;
using LasPollasHermanas.Shared.Models;
public class DildoClient
{
    private readonly HttpClient httpClient;

    public DildoClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Dildo[]?> GetDildosAsync()
    {
        return await httpClient.GetFromJsonAsync<Dildo[]>("dildos");
    }

    public async Task<Dildo[]?> SearchDildosAsync(string query)
    {
        return await httpClient.GetFromJsonAsync<Dildo[]>($"dildos/search/{query}");
    }

    public async Task AddDildoAsync(Dildo dildo)
    {
        await httpClient.PostAsJsonAsync("dildos", dildo);
    }

    public async Task<Dildo> GetDildoAsync(int id)
    {
        return await httpClient.GetFromJsonAsync<Dildo>($"dildos/{id}") ??
        throw new Exception("Could not find dildo");
    }

    public async Task UpdateDildoAsync(Dildo updatedDildo)
    {
        await httpClient.PutAsJsonAsync($"dildos/{updatedDildo.Id}", updatedDildo);
    }

    public async Task DeleteDildoAsync(int id)
    {
        await httpClient.DeleteAsync($"dildos/{id}");
    }

    // Login to the server
    public async Task<SessionDTO?> LoginAsync(LoginDTO data)
    {
        var response = await httpClient.PostAsJsonAsync("auth/login", data);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to login: {response.StatusCode}");
        }

        return await response.Content.ReadFromJsonAsync<SessionDTO>();
    }
}
