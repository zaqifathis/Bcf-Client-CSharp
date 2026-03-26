using BcfClient.Generated.Api;
using BcfClient.Generated.Model;
 
namespace BcfClient.Client;

public class ViewpointsClient
{
    private readonly ViewpointsApi _api;
    private readonly string  _version;
 
    public ViewpointsClient(ViewpointsApi api, string version)
    {
        _api = api;
        _version = version;
    }
}