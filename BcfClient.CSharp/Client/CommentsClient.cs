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

    public Task<CommentGET> CreateComment(string projectId, string topicId, CommentPOST payload)
        => _api.CreateCommentAsync(_version, projectId, topicId, payload);

    public Task DeleteComment(string projectId, string topicId, string commentId)
        => _api.DeleteCommentAsync(_version, projectId, topicId, commentId);

    public Task<CommentGET> GetCommentById(string projectId, string topicId, string commentId)
        => _api.GetCommentByIdAsync(_version, projectId, topicId, commentId);

    public Task<List<CommentGET>> GetTopicComment(string projectId, string topicId, string? filter = null, string? orderby = null)
        => _api.GetTopicCommentAsync(_version, projectId, topicId, filter, orderby);

    public Task<CommentGET> UpdateComment(string projectId, string topicId, string commentId, CommentPUT payload)
        => _api.UpdateCommentAsync(_version, projectId, topicId, payload);

}