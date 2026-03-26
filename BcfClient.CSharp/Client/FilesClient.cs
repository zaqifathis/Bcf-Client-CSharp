using BcfClient.Generated.Api;
using BcfClient.Generated.Model;
 
namespace BcfClient.Client;

public class FilesClient
{
    private readonly FilesApi _api;
    private readonly string  _version;
 
    public FilesClient(FilesApi api, string version)
    {
        _api = api;
        _version = version;
    }
}