using PasswordManager.Portal.Models;
using System.Security.Claims;

namespace PasswordManager.Portal.Services;

public sealed class ClientStateData
{
    public bool IsAuthenticated { get; private set; }

    public User? User { get; private set; }

    public ClientStateData()
    {
        IsAuthenticated = false;
    }

    public void LoggedIn(User user)
    {
        if (string.IsNullOrWhiteSpace(user.AccessToken) || string.IsNullOrWhiteSpace(user.Email))
        {
            throw new Exception("Invalid user");
        }

        User = ChangeValue(nameof(User), User, user);
        IsAuthenticated = ChangeValue(nameof(IsAuthenticated), IsAuthenticated, true);
    }

    public event EventHandler<ClientStateEventArgs>? StateHasChanged;

    private T ChangeValue<T>(string propertyName, T oldValue, T newValue)
    {
        StateHasChanged?.Invoke(this, new()
        {
            PropertyChange = propertyName,
            OldValue = oldValue,
            NewValue = newValue,
        });
        return newValue;
    }
}

public sealed class ClientStateEventArgs : EventArgs
{
    public required string PropertyChange { get; init; }
    public object? OldValue { get; set; }
    public object? NewValue { get; set; }
}
