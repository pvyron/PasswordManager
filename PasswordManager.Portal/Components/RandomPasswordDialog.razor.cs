using Microsoft.AspNetCore.Components;
using MudBlazor;
using PasswordManager.Portal.ViewModels.PasswordGeneration;
using PasswordManager.Shared;

namespace PasswordManager.Portal.Components;

public partial class RandomPasswordDialog
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }
    RandomPasswordViewModel RandomPasswordViewModel { get; set; } = new();
    void PasswordLengthHasChanged(object? sender, int e) => InputsChanged().Wait();

    protected override Task OnInitializedAsync()
    {
        RandomPasswordViewModel.PasswordLengthHasChanged += PasswordLengthHasChanged;

        return base.OnInitializedAsync();
    }

    void CancelButtonClicked() => MudDialog?.Cancel();

    void UsePasswordButtonClicked()
    {
        MudDialog?.Close(DialogResult.Ok(RandomPasswordViewModel.PasswordText));
    }

    void RefreshPassword()
    {
        if (!RandomPasswordViewModel.CanGeneratePassword)
        {
            RandomPasswordViewModel.PasswordText = "Pick characters to use";
            return;
        }

        int length = RandomPasswordViewModel.PasswordLength;
        bool useSymbols = RandomPasswordViewModel.UsesSymbols;
        bool useNumbers = RandomPasswordViewModel.UsesNumbers;
        bool useCapital = RandomPasswordViewModel.UsesCapitalLetters;
        bool userLower = RandomPasswordViewModel.UsesLowercaseLetters;

        var generatedPassword = RandomPasswordGenerator.GenerateRandomPassword(
            length,
            includeSymbols: useSymbols,
            includeNumbers: useNumbers,
            includeCapitalLetters: useCapital,
            includeLowercaseLetters: userLower
            );

        //var evaluation = Zxcvbn.Core.EvaluatePassword(generatedPassword);

        //if (evaluation.Score < 4)
        //{
        //    Console.WriteLine(evaluation.Feedback.Warning);
        //    Console.WriteLine(string.Join(Environment.NewLine, evaluation.Feedback.Suggestions));
        //}

        RandomPasswordViewModel.PasswordText = generatedPassword;

        StateHasChanged();
    }

    async Task ChipsChanged(MudChip[] selectedChips)
    {
        RandomPasswordViewModel.IncludedCharacterSets = selectedChips.Select(c => (PasswordGenerationCharacters)c.Value).ToHashSet();

        await InputsChanged();
    }

    async Task InputsChanged()
    {
        RefreshPassword();
    }
}
