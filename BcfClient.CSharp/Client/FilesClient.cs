using BcfClient.Generated.Api;
using BcfClient.Generated.Model;
 
namespace BcfClient.Client;

public class FilesClient
{
    private readonly FilesApi _api;
    private readonly string  _version;
 
    public FilesClient(FilesApi api, string version)
    {
        _api = api;
        _version = version;
    }

    public Task<List<FileGET>> GetFiles(string projectId, string topicId)
        => _api.GetFilesAsync(_version, projectId, topicId);

    public Task<ProjectFileInformation> GetProjectFilesInformation(string version, string projectId)
        => _api.GetProjectFilesInformationAsync(_version, projectId);

    public Task<List<FileGET>> UpdateTopicFile(string projectId, string topicId, List<FilePUT> payload)
        => _api.UpdateTopicFileAsync(_version, projectId, topicId, payload);

}