using BcfClient.Generated.Api;
using BcfClient.Generated.Model;
 
namespace BcfClient.Client;

public class DocumentReferencesClient
{
    private readonly DocumentReferencesApi _api;
    private readonly string  _version;
 
    public DocumentReferencesClient(DocumentReferencesApi api, string version)
    {
        _api = api;
        _version = version;
    }

    public Task<DocumentReferenceGET> CreateDocumentReference(string projectId, string topicId, DocumentReferencePOST payload)
        => _api.CreateDocumentReferenceAsync(_version, projectId, topicId, payload);

    public Task<List<DocumentReferenceGET>> GetDocumentReferences(string projectId, string topicId)
        => _api.GetDocumentReferencesAsync(_version, projectId, topicId);

    public Task<DocumentReferenceGET> UpdateDocumentReference (string projectId, string topicId, string documentReferenceId, DocumentReferencePUT payload)
        => _api.UpdateDocumentReferenceAsync(_version, projectId, topicId, documentReferenceId, payload);
        
    
}