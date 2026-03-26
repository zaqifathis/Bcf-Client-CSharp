using BcfClient.Generated.Api;
using BcfClient.Generated.Model;
 
namespace BcfClient.Client;

public class DocumentsClient
{
    private readonly DocumentsApi _api;
    private readonly string  _version;
 
    public DocumentsClient(DocumentsApi api, string version)
    {
        _api = api;
        _version = version;
    }
}