using BcfClient.Generated.Api;
using BcfClient.Generated.Model;
 
namespace BcfClient.Client;

public class ViewpointsClient
{
    private readonly ViewpointsApi _api;
    private readonly string  _version;
 
    public ViewpointsClient(ViewpointsApi api, string version)
    {
        _api = api;
        _version = version;
    }

    public Task<ViewpointGET> CreateViewpoints(string projectId, string topicId, ViewpointPOST payload)
        => _api.CreateViewpointsAsync(_version, projectId, topicId, payload);
    
    public Task DeleteViewpointById(string projectId, string topicId, string viewpointId)
        => _api.DeleteViewpointByIdAsync(_version, projectId, topicId, viewpointId);

    public Task<FileParameter> GetBitmap(string projectId, string topicId, string viewpointId, string bitmapId)
        => _api.GetBitmapAsync(_version, projectId, topicId, viewpointId, bitmapId);

    public Task<ColoringGET> GetColoring(string projectId, string topicId, string viewpointId)
        => _api.GetColoringAsync(_version, projectId, topicId, viewpointId);

    public Task<SelectionGET> GetSelection(string projectId, string topicId, string viewpointId)
        => _api.GetSelectionAsync(_version, projectId, topicId, viewpointId);

    public Task<FileParameter> GetSnapshot(string projectId, string topicId, string viewpointId)
        => _api.GetSnapshotAsync(_version, projectId, topicId, viewpointId);

    public Task<ViewpointGET> GetViewpointById(string projectId, string topicId, string viewpointId)
        => _api.GetViewpointByIdAsync(_version, projectId, topicId, viewpointId);

    public Task<List<ViewpointGET>> GetViewpoints(string projectId, string topicId)
        => _api.GetViewpointsAsync(_version, projectId, topicId);

    public Task<VisibilityGET> GetVisibility(string projectId, string topicId, string viewpointId)
        => _api.GetVisibilityAsync(_version, projectId, topicId, viewpointId);

}