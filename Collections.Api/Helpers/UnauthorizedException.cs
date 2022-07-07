namespace Collections.Api.Helpers;

using System.Globalization;

public class UnauthorizedException : Exception
{
    public UnauthorizedException() : base()
    {
    }

    public UnauthorizedException(string message) : base(message)
    {
    }

    public UnauthorizedException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture,
        message, args))
    {
    }

    public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
