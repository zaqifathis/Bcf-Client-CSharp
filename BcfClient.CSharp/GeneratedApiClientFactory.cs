using BcfClient.Auth;
using BcfClient.Generated.Client;

namespace BcfClient;

public static class GeneratedApiClientFactory
{
    public static (ApiClient, Configuration) Create(FoundationClient foundation, string bcfBaseUrl)
    {
        var basePath = bcfBaseUrl.TrimEnd('/');
 
        var config = new Configuration
        {
            BasePath = basePath
        };
 
        config.DefaultHeaders["Authorization"] =
            "Bearer " + foundation.GetAccessTokenAsync().GetAwaiter().GetResult();
 
        var apiClient = new ApiClient(basePath);
 
        return (apiClient, config);
    }
}