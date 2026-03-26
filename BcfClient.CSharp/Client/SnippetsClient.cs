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

    public Task<FileParameter> GetTopicSnippet(string projectId, string topicId)
        => _api.GetTopicSnippetAsync(_version, projectId, topicId);

    public Task UpdateTopicSnippet(string projectId, string topicId, FileParameter payload)
        => _api.UpdateTopicSnippetAsync(_version, projectId, topicId, payload);
        
}