using BcfClient.Generated.Api;
using BcfClient.Generated.Model;
 
namespace BcfClient.Client;

public class RelatedTopicsClient
{
    private readonly RelatedTopicsApi _api;
    private readonly string  _version;
    
    public RelatedTopicsClient(RelatedTopicsApi api, string version)
    {
        _api = api;
        _version = version;
    }
}