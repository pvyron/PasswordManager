namespace PasswordManager.Domain.Exceptions;

public interface IAccessException { }

public abstract class AccessException<T> : Exception, IAccessException where T : class
{
    public AccessException(string message) : base(message)
    {

    }

    public AccessException(string message, T model) : base(message)
    {
        Model = model;
    }

    public abstract T? Model { get; init; }
}
