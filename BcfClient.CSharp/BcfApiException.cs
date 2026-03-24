namespace BcfClient;

public class BcfApiException : Exception
{
    public int StatusCode { get; }

    public BcfApiException(string message)
        : base(message)
    {
        StatusCode = -1;
    }

    public BcfApiException(int statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }
}