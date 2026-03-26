using BcfClient.Generated.Api;
using BcfClient.Generated.Model;
 
namespace BcfClient.Client;

public class ProjectClient
{
    private readonly ProjectApi _api;
    private readonly string  _version;
    
    public ProjectClient(ProjectApi api, string version)
    {
        _api = api;
        _version = version;
    }

    public Task<List<ProjectGET>> GetAllProjects()
        => _api.GetAllProjectsAsync(_version);
    
    public Task<ProjectGET> GetProjectById(string projectId)
        => _api.GetProjectByIdAsync(_version, projectId);

    public Task<ExtensionsGET> GetProjectExtension(string projectId)
        => _api.GetProjectExtensionAsync(_version, projectId);

    public Task<ProjectGET> UpdateProjectById(string projectId, ProjectPUT payload)
        => _api.UpdateProjectByIdAsync(_version, projectId, payload);

}