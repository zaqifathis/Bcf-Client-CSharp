using BcfClient.Generated.Api;
using BcfClient.Generated.Model;
 
namespace BcfClient.Client;

public class TopicsClient
{
    private readonly TopicsApi _api;
    private readonly string  _version;
 
    public TopicsClient(TopicsApi api, string version)
    {
        _api = api;
        _version = version;
    }
}