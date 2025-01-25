using BackendApi.ApiClients;
using Microsoft.Extensions.Options;

public class GeneratedGameClientFactory
{
    private string baseUrl;
    private IHttpClientFactory httpClientFactory;

    public GeneratedGameClientFactory(IOptions<GameClientOptions> options, IHttpClientFactory httpClientFactory)
    {
        this.baseUrl = options.Value.BaseUrl;
        this.httpClientFactory=httpClientFactory;   
    }

    public GeneratedGameClient Generate()
    {
        return new GeneratedGameClient(this.baseUrl, this.httpClientFactory.CreateClient());
    }
}