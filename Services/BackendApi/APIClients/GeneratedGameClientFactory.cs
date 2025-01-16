using BackendApi.ApiClients;
using Microsoft.Extensions.Options;

public class GeneratedGameClientFactory
{
    private string baseUrl;
    private HttpClient client;

    public GeneratedGameClientFactory(IOptions<GameClientOptions> options, HttpClient httpClient)
    {
        this.baseUrl = options.Value.BaseUrl;
        this.client = httpClient;   
    }

    public GeneratedGameClient Generate()
    {
        return new GeneratedGameClient(this.baseUrl, this.client);
    }
}