using BcfClient.Generated.Api;
using BcfClient.Generated.Model;
 
namespace BcfClient.Client;

public class ProjectClient
{
    private readonly ProjectApi _api;
    private readonly string  _version;
    
    public ProjectClient(ProjectApi api, string version)
    {
        _api = api;
        _version = version;
    }
}