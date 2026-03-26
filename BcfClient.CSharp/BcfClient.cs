using System.Net.Http;
using System.Text.Json;
using BcfClient.Auth;
using BcfClient.Client;
using BcfClient.Generated.Api;
using BcfClient.Generated.Client;

namespace BcfClient;

public class BcfClient
{
    private const string VersionsEndpoint = "foundation/versions";
    private const string TargetApiId = "bcf";
 
    private readonly FoundationClient _foundation;
    private readonly string _targetVersion;
    private readonly HttpClient _plainHttpClient;

    public ProjectClient? Projects { get; private set; }
    public TopicsClient? Topics { get; private set; }
    public CommentsClient? Comments { get; private set; }
    public ViewpointsClient? Viewpoints { get; private set; }
    public DocumentsClient? Documents { get; private set; }
    public DocumentReferencesClient? DocumentReferences { get; private set; }
    public FilesClient? Files { get; private set; }
    public EventsClient?  Events { get; private set; }
    public RelatedTopicsClient? RelatedTopics { get; private set; }
    public SnippetsClient? Snippets  { get; private set; }


    public BcfClient(FoundationClient foundation, string targetVersion)
    {
        _foundation = foundation;
        _targetVersion = targetVersion;
        _plainHttpClient = new HttpClient();
    }


    public async Task ResolveVersionAsync()
    {
        var versions = await GetVersionsAsync();
 
        var bcf = versions.Versions?.FirstOrDefault(v =>
            v.ApiId == TargetApiId && v.VersionId == _targetVersion);
 
        if (bcf == null)
        {
            var available = string.Join(", ",
                versions.Versions?.Select(v => $"{v.ApiId} {v.VersionId}") ?? []);
            throw new BcfApiException(
                $"Server does not support BCF {_targetVersion}. Available: {available}");
        }
 
        var bcfBaseUrl = bcf.Href?.TrimEnd('/') ?? $"{_foundation.BaseUrl.TrimEnd('/')}/bcf/{_targetVersion}";
 
        var (apiClient, config) = GeneratedApiClientFactory.Create(_foundation, bcfBaseUrl);
 
        Projects = new ProjectClient(new ProjectApi(apiClient, apiClient, config), _targetVersion);
        Topics = new TopicsClient(new TopicsApi(apiClient, apiClient, config), _targetVersion);
        Comments = new CommentsClient(new CommentsApi(apiClient, apiClient, config), _targetVersion);
        Viewpoints = new ViewpointsClient(new ViewpointsApi(apiClient, apiClient, config), _targetVersion);
        Documents = new DocumentsClient(new DocumentsApi(apiClient, apiClient, config), _targetVersion);
        DocumentReferences = new DocumentReferencesClient(new DocumentReferencesApi(apiClient, apiClient, config), _targetVersion);
        Files = new FilesClient(new FilesApi(apiClient, apiClient, config), _targetVersion);
        Events = new EventsClient(new EventsApi(apiClient, apiClient, config), _targetVersion);
        RelatedTopics = new RelatedTopicsClient(new RelatedTopicsApi(apiClient, apiClient, config), _targetVersion);
        Snippets = new SnippetsClient(new SnippetsApi(apiClient, apiClient, config), _targetVersion);
    }

    private async Task<VersionsResponse> GetVersionsAsync()
    {
        var url = _foundation.BaseUrl + VersionsEndpoint;
        var response = await _plainHttpClient.GetAsync(url);
 
        if (!response.IsSuccessStatusCode)
            throw new BcfApiException((int)response.StatusCode, $"Failed to fetch versions — HTTP {(int)response.StatusCode}");
 
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<VersionsResponse>(body) ?? throw new BcfApiException("Failed to parse versions response");
    }

}

internal class VersionsResponse
{
    [System.Text.Json.Serialization.JsonPropertyName("versions")]
    public List<VersionEntry>? Versions { get; set; }
}
 
internal class VersionEntry
{
    [System.Text.Json.Serialization.JsonPropertyName("api_id")]
    public string? ApiId { get; set; }
 
    [System.Text.Json.Serialization.JsonPropertyName("version_id")]
    public string? VersionId { get; set; }
 
    [System.Text.Json.Serialization.JsonPropertyName("detailed_version")]
    public string? DetailedVersion { get; set; }
 
    [System.Text.Json.Serialization.JsonPropertyName("href")]
    public string? Href { get; set; }
}
 