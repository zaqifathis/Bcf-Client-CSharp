using BcfClient.Generated.Api;
using BcfClient.Generated.Model;
 
namespace BcfClient.Client;

public class TopicsClient
{
    private readonly TopicsApi _api;
    private readonly string  _version;
 
    public TopicsClient(TopicsApi api, string version)
    {
        _api = api;
        _version = version;
    }

    public Task<TopicGET> CreateTopic(string projectId)
        => _api.CreateTopicAsync(_version, projectId);

    public Task DeleteTopic(string projectId)
        => _api.DeleteTopicAsync(_version, projectId);

    public Task<TopicGET> GetTopicById(string projectId, string topicId)
        => _api.GetTopicByIdAsync(_version, projectId, topicId);

    public Task<List<TopicGET>> GetTopics(string projectId,
        string? filter = null, string? orderBy = null, string? top = null, string? skip = null)
        => _api.GetTopicsAsync(_version, projectId, filter, orderBy, top, skip);

    public Task<TopicGET> UpdateTopic(string projectId, string topicId, TopicPUT payload)
        => _api.UpdateTopicAsync(_version, projectId, topicId, payload);

}