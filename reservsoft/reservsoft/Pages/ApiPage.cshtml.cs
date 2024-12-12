using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;

public class ApiPageModel : PageModel
{
    private readonly HttpClient _httpClient;

    public string ApiData { get; set; }

    public ApiPageModel(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task OnGetAsync()
    {
        var response = await _httpClient.GetAsync("https://apihotel-rh4d.onrender.com/api/descuentos");
        response.EnsureSuccessStatusCode();
        ApiData = await response.Content.ReadAsStringAsync();
    }
}