using BcfClient.Generated.Api;
using BcfClient.Generated.Model;
 
namespace BcfClient.Client;

public class DocumentReferencesClient
{
    private readonly DocumentReferencesApi _api;
    private readonly string  _version;
 
    public DocumentReferencesClient(DocumentReferencesApi api, string version)
    {
        _api = api;
        _version = version;
    }
}