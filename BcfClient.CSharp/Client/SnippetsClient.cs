using BcfClient.Generated.Api;
using BcfClient.Generated.Model;
 
namespace BcfClient.Client;

public class SnippetsClient
{
    private readonly SnippetsApi _api;
    private readonly string  _version;
    
    public SnippetsClient(SnippetsApi api, string version)
    {
        _api = api;
        _version = version;
    }
}