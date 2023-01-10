namespace PasswordManager.Portal.ViewModels.PasswordGeneration;

public sealed class RandomPasswordViewModel
{
    private int _PasswordLength = 12;
    public int PasswordLength 
    { 
        get => _PasswordLength;
        set
        {
            _PasswordLength = value;
            PasswordLengthHasChanged.Invoke(this, _PasswordLength);
        } 
    }
    public string? PasswordText { get; set; }
    public HashSet<PasswordGenerationCharacters> IncludedCharacterSets { get; set; } = new()
    {
        PasswordGenerationCharacters.Symbols,
        PasswordGenerationCharacters.Numbers,
        PasswordGenerationCharacters.Lowercase,
        PasswordGenerationCharacters.Uppercase
    };
    public bool UsesNumbers => IncludedCharacterSets.Contains(PasswordGenerationCharacters.Numbers);
    public bool UsesSymbols => IncludedCharacterSets.Contains(PasswordGenerationCharacters.Symbols);
    public bool UsesCapitalLetters => IncludedCharacterSets.Contains(PasswordGenerationCharacters.Uppercase);
    public bool UsesLowercaseLetters => IncludedCharacterSets.Contains(PasswordGenerationCharacters.Lowercase);

    public bool CanGeneratePassword => IncludedCharacterSets.Length() > 0;

    public event EventHandler<int>? PasswordLengthHasChanged;
}

public enum PasswordGenerationCharacters
{
    Symbols,
    Numbers,
    Uppercase,
    Lowercase
}