using BcfClient.Generated.Api;
using BcfClient.Generated.Model;
 
namespace BcfClient.Client;

public class CommentsClient
{
    private readonly CommentsApi _api;
    private readonly string  _version;
 
    public CommentsClient(CommentsApi api, string version)
    {
        _api = api;
        _version = version;
    }


}