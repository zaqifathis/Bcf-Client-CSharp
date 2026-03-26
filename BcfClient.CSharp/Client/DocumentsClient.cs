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

    public Task<DocumentGET> CreateDocument(string projectId, string topicId, FileParameter payload)
        => _api.CreateDocumentAsync(_version, projectId, topicId, payload);

    public Task<List<DocumentGET>> GetDocument(string projectId, string topicId)
        => _api.GetDocumentAsync(_version, projectId, topicId);

    public Task<FileParameter> GetDocumentById(string projectId, string documentId)
        => _api.GetDocumentByIdAsync(_version, projectId, documentId);
}