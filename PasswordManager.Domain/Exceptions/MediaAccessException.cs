namespace PasswordManager.Domain.Exceptions;

public sealed class MediaAccessException : AccessException<Stream>
{
    public MediaAccessException(string message) : base(message)
    {

    }

    public MediaAccessException(string message, Stream mediaStream) : base(message)
    {
        Model = mediaStream;
    }

    public override Stream? Model { get; init; }
}
