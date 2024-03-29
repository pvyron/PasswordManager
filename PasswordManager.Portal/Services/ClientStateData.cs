﻿using Blazored.LocalStorage;
using Blazored.SessionStorage;
using PasswordManager.Portal.Models;

namespace PasswordManager.Portal.Services;

public sealed class ClientStateData
{
    private readonly ISyncLocalStorageService _localStorage;
    private readonly ISyncSessionStorageService _sessionStorage;

    public bool IsAuthenticated
    {
        get
        {
            if (_sessionStorage.ContainKey("isAuthenticated"))
                return _sessionStorage.GetItem<bool>("isAuthenticated");

            if (_localStorage.ContainKey("isAuthenticated"))
                return _localStorage.GetItem<bool>("isAuthenticated");

            return false;
        }
        private set
        {
            _sessionStorage.SetItem("isAuthenticated", value);

            if (User?.RemainLoggedIn ?? false)
                _localStorage.SetItem("isAuthenticated", value);
        }
    }

    public UserModel? User
    {
        get
        {
            if (!_localStorage.ContainKey("user"))
                return null;

            return _localStorage.GetItem<UserModel>("user");
        }
        private set
        {
            _sessionStorage.SetItem("user", value);

            if (value?.RemainLoggedIn ?? false)
                _localStorage.SetItem("user", value);
        }
    }

    public byte[] DecryptionToken => User?.DecryptionToken ?? throw new NullReferenceException();

    public ClientStateData(ISyncLocalStorageService localStorage, ISyncSessionStorageService sessionStorage)
    {
        _localStorage = localStorage;
        _sessionStorage = sessionStorage;
    }

    public void LoggedIn(UserModel user)
    {
        if (string.IsNullOrWhiteSpace(user.AccessToken) || string.IsNullOrWhiteSpace(user.Email))
        {
            throw new Exception("Invalid user");
        }

        User = user;
        StateHasChanged?.Invoke(this, new ClientStateEventArgs { PropertyChanged = nameof(User) });

        IsAuthenticated = true;
        StateHasChanged?.Invoke(this, new ClientStateEventArgs { PropertyChanged = nameof(IsAuthenticated) });
    }

    public void Logout()
    {
        _sessionStorage.Clear();
        _localStorage.Clear();
    }

    public event EventHandler<ClientStateEventArgs>? StateHasChanged;
}

public sealed class ClientStateEventArgs : EventArgs
{
    public required string PropertyChanged { get; init; }
}