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

    public Task<List<RelatedTopicGET>> GetRelatedTopics(string projectId, string topicId)
        => _api.GetRelatedTopicsAsync(_version, projectId, topicId);

    public Task<List<RelatedTopicGET>> UpdateRelatedTopics(string projectId, string topicId, List<RelatedTopicPUT> payload)
        => _api.UpdateRelatedTopicsAsync(_version, projectId, topicId, payload);
        
}