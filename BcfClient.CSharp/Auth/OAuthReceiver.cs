
using System.Net;
using System.Web;

namespace BcfClient.Auth;

public class OAuthReceiver
{
    private const int Port = 8081;
    private const int TimeoutSeconds = 120;

    public string GetRedirectUri() => $"http://localhost:{Port}/";

    public async Task<string> WaitForCodeAsync(string expectedState)
    {
        using var listener = new HttpListener();
        listener.Prefixes.Add(GetRedirectUri());
        listener.Start();
 
        var contextTask = listener.GetContextAsync();
        var timeoutTask = Task.Delay(TimeSpan.FromSeconds(TimeoutSeconds));
 
        if (await Task.WhenAny(contextTask, timeoutTask) == timeoutTask)
            throw new BcfApiException($"Timed out waiting for OAuth callback after {TimeoutSeconds}s");
 
        var context  = await contextTask;
        var query    = HttpUtility.ParseQueryString(context.Request.Url?.Query ?? "");
        var code     = query["code"]  ?? throw new BcfApiException("OAuth callback missing 'code' parameter");
        var state    = query["state"] ?? throw new BcfApiException("OAuth callback missing 'state' parameter");
 
        var responseBody = "You have now authenticated :) You may close this browser window."u8.ToArray();
        context.Response.ContentLength64 = responseBody.Length;
        await context.Response.OutputStream.WriteAsync(responseBody);
        context.Response.Close();
 
        listener.Stop();
 
        if (state != expectedState)
            throw new BcfApiException("OAuth state mismatch — possible CSRF attack");
 
        return code;
    }
}