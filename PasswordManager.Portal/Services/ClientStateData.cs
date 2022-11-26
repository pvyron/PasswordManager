using Blazored.LocalStorage;
using LanguageExt.Pipes;
using Microsoft.Extensions.Logging.Abstractions;
using PasswordManager.Portal.Models;
using System.Security.Claims;

namespace PasswordManager.Portal.Services;

public sealed class ClientStateData
{
    private readonly ISyncLocalStorageService _localStorage;

    public bool IsAuthenticated 
    {
        get
        {
            if (!_localStorage.ContainKey("isAuthenticated"))
                return false;

            return _localStorage.GetItem<bool>("isAuthenticated");
        }
        private set
        {
            _localStorage.SetItem("isAuthenticated", value);
        }
    }

    public User? User 
    { 
        get
        {
            if (!_localStorage.ContainKey("user"))
                return null;

            return _localStorage.GetItem<User>("user");
        }
        private set
        {
            _localStorage.SetItem("user", value);
        }
    }

    public ClientStateData(ISyncLocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public void LoggedIn(User user)
    {
        if (string.IsNullOrWhiteSpace(user.AccessToken) || string.IsNullOrWhiteSpace(user.Email))
        {
            throw new Exception("Invalid user");
        }

        User = user;
        StateHasChanged?.Invoke(this, new ClientStateEventArgs { PropertyChanged = nameof(User)});

        IsAuthenticated = true;
        StateHasChanged?.Invoke(this, new ClientStateEventArgs { PropertyChanged = nameof(IsAuthenticated) });
    }

    public void Logout()
    {
        User = null;
        StateHasChanged?.Invoke(this, new ClientStateEventArgs { PropertyChanged = nameof(User) });

        IsAuthenticated = false;
        StateHasChanged?.Invoke(this, new ClientStateEventArgs { PropertyChanged = nameof(IsAuthenticated) });
    }

    public event EventHandler<ClientStateEventArgs>? StateHasChanged;
}

public sealed class ClientStateEventArgs : EventArgs
{
    public required string PropertyChanged { get; init; }
}