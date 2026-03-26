using BcfClient.Generated.Api;
using BcfClient.Generated.Model;
 
namespace BcfClient.Client;

public class EventsClient
{
    private readonly EventsApi _api;
    private readonly string  _version;
 
    public EventsClient(EventsApi api, string version)
    {
        _api = api;
        _version = version;
    }

}