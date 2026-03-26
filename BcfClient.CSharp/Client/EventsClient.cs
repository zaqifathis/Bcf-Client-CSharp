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

    public Task<List<CommentEventGET>> GetCommentEvent(string projectId, string topicId, string commentId, string? top = null, string? skip = null, string? filter = null, string? orderby = null)
        => _api.GetCommentEventAsync(_version, projectId, topicId, commentId, top, skip, filter, orderby);

    public Task<List<CommentEventGET>> GetCommentEvents(string projectId, string? top = null, string? skip = null, string? filter = null, string? orderby = null)
        => _api.GetCommentEventsAsync(_version, projectId, top, skip, filter, orderby);

    public Task<List<TopicEventGET>> GetEvents(string projectId, string? top = null, string? skip = null, string? filter = null, string? orderby = null)
        => _api.GetEventsAsync(_version, projectId, top, skip, filter, orderby);

    public Task<List<TopicEventGET>> GetTopicEvents(string projectId, string topicId, string? top = null, string? skip = null, string? filter = null, string? orderby = null)
        => _api.GetTopicEventsAsync(_version, projectId, topicId, top, skip, filter, orderby);

}