namespace LasPollasHermanas.Client.Data;
using System.Net.Http.Json;
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

    public async Task BuyDildo(int dildoId, string userEmail)
    {
        await httpClient.PostAsJsonAsync($"dildos/buy/{dildoId}", userEmail);
    }

    public async Task<Dildo[]?> GetBoughtDildos(string userEmail)
    {
        return await httpClient.GetFromJsonAsync<Dildo[]>($"dildos/history/{userEmail}");
    }
}
