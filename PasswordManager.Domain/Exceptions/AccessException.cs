namespace PasswordManager.Domain.Exceptions
{
    abstract public class AccessException<T> : Exception where T : class
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
}
