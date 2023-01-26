﻿using Microsoft.AspNetCore.Components;
using static MudBlazor.CategoryTypes;
using System.Net.Http;
using PasswordManager.Portal.Models;
using PasswordManager.Portal.Services;

namespace PasswordManager.Portal.Components;

public partial class ChangePasswordImageDrawer
{
    [Inject] PasswordsService _passwordsService { get; set; } = default!;

    [Parameter] public bool IsDrawerOpen { get; set; } = false;
    [Parameter] public LogoModel? SelectedLogo { get; set; }
    [Parameter] public EventCallback<LogoModel> OnLogoPicked { get; set; }

    List<LogoModel> _logoModels { get; set; } = new();
    string _searchText { get; set; } = "";

    protected override async Task OnInitializedAsync()
    {
        await LoadLogos();   
    }

    private async Task LogoPickingButtonClicked(LogoModel logoModel)
    {
        await OnLogoPicked.InvokeAsync(logoModel);

        await LoadLogos();
    }

    async Task LoadLogos()
    {
        _logoModels = new();

        await foreach (var logoModel in _passwordsService.GetAllLogos(CancellationToken.None))
        {
            if (logoModel.ImageId == (SelectedLogo?.ImageId).GetValueOrDefault())
                continue;

            _logoModels.Add(logoModel);
            StateHasChanged();
        }
    }
}
