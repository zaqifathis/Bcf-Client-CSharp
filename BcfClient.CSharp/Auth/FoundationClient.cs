using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BcfClient.Auth;

public class FoundationClient
{
    private const string RequiredFlow  = "authorization_code_grant";
    private const string AuthEndpoint  = "foundation/1.1/auth";

    public string BaseUrl { get;}
    public string ClientId { get;}
    private readonly string _clientSecret;

    private string? _authUrl;
    private string? _tokenUrl;

    private string? _accessToken;
    private string? _refreshToken;
    private long _accessTokenExpiresOn;
    private long _refreshTokenExpiresOn = long.MaxValue;

    private readonly SemaphoreSlim _tokenLock = new(1, 1);

    private readonly HttpClient _httpClient;

    // -------------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------------

    public FoundationClient(string baseUrl, string clientId, string clientSecret)
    {
        BaseUrl       = baseUrl.EndsWith('/') ? baseUrl : baseUrl + '/';
        ClientId      = clientId;
        _clientSecret = clientSecret;
        _httpClient   = new HttpClient();

        _accessTokenExpiresOn = 0;
    }

    // -------------------------------------------------------------------------
    // PUBLIC: GetAccessTokenAsync
    // -------------------------------------------------------------------------

    public async Task<string> GetAccessTokenAsync()
    {
        await _tokenLock.WaitAsync();
        try
        {
            long now = EpochSeconds();

            if (_accessToken != null && _accessTokenExpiresOn > now)
                return _accessToken;

            if (_refreshToken != null && _refreshTokenExpiresOn > now)
                await RefreshAccessTokenAsync();
            else
                await LoginAsync();

            return _accessToken!;
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    // -------------------------------------------------------------------------
    // STEP 1: discover auth info
    // -------------------------------------------------------------------------

    public async Task<AuthInfo> GetAuthInfoAsync()
    {
        var response = await _httpClient.GetAsync(BaseUrl + AuthEndpoint);

        if (!response.IsSuccessStatusCode)
            throw new BcfApiException((int)response.StatusCode,
                $"Failed to fetch auth info — HTTP {(int)response.StatusCode}");

        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AuthInfo>(body)
               ?? throw new BcfApiException("Failed to parse auth info response");
    }

    // -------------------------------------------------------------------------
    // STEP 2: login
    // -------------------------------------------------------------------------

    public async Task LoginAsync()
    {
        var authInfo = await GetAuthInfoAsync();
        ValidateAuthFlow(authInfo.SupportedOauth2Flows);

        _authUrl  = authInfo.Oauth2AuthUrl;
        _tokenUrl = authInfo.Oauth2TokenUrl;

        var receiver    = new OAuthReceiver();
        var state       = Guid.NewGuid().ToString();
        var redirectUri = receiver.GetRedirectUri();
        var browserUrl  = BuildAuthUrl(_authUrl!, state, redirectUri);

        Console.WriteLine(">> Opening browser for login...");
        OpenBrowser(browserUrl);

        var code = await receiver.WaitForCodeAsync(state);
        await ExchangeCodeForTokenAsync(code, redirectUri);

        Console.WriteLine(">> Login successful.");
    }

    // -------------------------------------------------------------------------
    // STEP 3: token exchange + refresh
    // -------------------------------------------------------------------------

    private async Task ExchangeCodeForTokenAsync(string code, string redirectUri)
    {
        var body = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"]   = "authorization_code",
            ["code"]         = code,
            ["redirect_uri"] = redirectUri
        });
        await PostToTokenEndpointAsync(body);
    }

    private async Task RefreshAccessTokenAsync()
    {
        var body = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"]    = "refresh_token",
            ["refresh_token"] = _refreshToken!
        });
        await PostToTokenEndpointAsync(body);
    }

    private async Task PostToTokenEndpointAsync(FormUrlEncodedContent formBody)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _tokenUrl)
        {
            Content = formBody
        };
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Basic", BuildBasicAuthHeader());

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            throw new BcfApiException((int)response.StatusCode,
                $"Token exchange failed — HTTP {(int)response.StatusCode}");

        var body = await response.Content.ReadAsStringAsync();
        SetTokensFromResponse(body);
    }

    private void SetTokensFromResponse(string responseBody)
    {
        var json = JsonSerializer.Deserialize<TokenResponse>(responseBody)
                   ?? throw new BcfApiException("Failed to parse token response");

        long now = EpochSeconds();
        _accessToken          = json.AccessToken;
        _refreshToken         = json.RefreshToken;
        _accessTokenExpiresOn = now + json.ExpiresIn;

        if (json.RefreshTokenExpiresIn.HasValue)
            _refreshTokenExpiresOn = now + json.RefreshTokenExpiresIn.Value;
    }

    // -------------------------------------------------------------------------
    // Utilities
    // -------------------------------------------------------------------------

    private static void ValidateAuthFlow(List<string>? supportedFlows)
    {
        if (supportedFlows == null || !supportedFlows.Contains(RequiredFlow))
            throw new BcfApiException(
                $"BCF server does not support 'authorization_code_grant'. Supported: {string.Join(", ", supportedFlows ?? [])}");
    }

    private string BuildAuthUrl(string authEndpoint, string state, string redirectUri)
    {
        var sep = authEndpoint.Contains('?') ? "&" : "?";
        return $"{authEndpoint}{sep}client_id={Uri.EscapeDataString(ClientId)}" +
               $"&response_type=code" +
               $"&state={Uri.EscapeDataString(state)}" +
               $"&redirect_uri={Uri.EscapeDataString(redirectUri)}";
    }

    private string BuildBasicAuthHeader()
    {
        var raw     = $"{ClientId}:{_clientSecret}";
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(raw));
    }

    private static void OpenBrowser(string url)
    {
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (Exception e)
        {
            throw new BcfApiException($"Cannot open browser: {e.Message}");
        }
    }

    private static long EpochSeconds() =>
        DateTimeOffset.UtcNow.ToUnixTimeSeconds();

}

// -------------------------------------------------------------------------
// Internal DTOs for auth responses
// -------------------------------------------------------------------------

public class AuthInfo
{
    [JsonPropertyName("oauth2_auth_url")]
    public string? Oauth2AuthUrl { get; set; }

    [JsonPropertyName("oauth2_token_url")]
    public string? Oauth2TokenUrl { get; set; }

    [JsonPropertyName("supported_oauth2_flows")]
    public List<string>? SupportedOauth2Flows { get; set; }
}

public class TokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = "";

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("expires_in")]
    public long ExpiresIn { get; set; }

    [JsonPropertyName("refresh_token_expires_in")]
    public long? RefreshTokenExpiresIn { get; set; }
}