using System.Text;

namespace PasswordManager.Shared;

public static class RandomPasswordGenerator
{
    private const int DEFAULT_NUMBER_LENGTH = 12;
    private static readonly char[] NUMBERS = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
    private static readonly char[] CAPITAL_LETTERS = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    private static readonly char[] LOWERCASE_LETTERS = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
    private static readonly char[] SYMBOLS = { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '+', '-', '=', '{', '}', '[', ']', '|', '\\', ':', ';', '"', '\'', '<', '>', ',', '.', '?' };

    private static readonly int NumbersCount = NUMBERS.Length;
    private static readonly int SymbolsCount = SYMBOLS.Length;
    private static readonly int CapitalLettersCount = CAPITAL_LETTERS.Length;
    private static readonly int LowercaseLettersCount = LOWERCASE_LETTERS.Length;

    public static string GenerateRandomPassword()
    {
        return GenerateRandomPassword(DEFAULT_NUMBER_LENGTH);
    }

    public static string GenerateRandomPassword(
        int length,
        bool includeSymbols = true,
        bool includeNumbers = true,
        bool includeCapitalLetters = true,
        bool includeLowercaseLetters = true)
    {
        int availableCharactersLength = (includeSymbols ? SymbolsCount : 0)
            + (includeNumbers ? NumbersCount : 0)
            + (includeCapitalLetters ? CapitalLettersCount : 0)
            + (includeLowercaseLetters ? LowercaseLettersCount : 0);

        IEnumerable<char> availableCharactersEnum = Array.Empty<char>();

        if (includeSymbols)
        {
            availableCharactersEnum = availableCharactersEnum.Concat(SYMBOLS);
        }

        if (includeNumbers)
        {
            availableCharactersEnum = availableCharactersEnum.Concat(NUMBERS);
        }

        if (includeCapitalLetters)
        {
            availableCharactersEnum = availableCharactersEnum.Concat(CAPITAL_LETTERS);
        }

        if (includeLowercaseLetters)
        {
            availableCharactersEnum = availableCharactersEnum.Concat(LOWERCASE_LETTERS);
        }

        var availableCharacters = availableCharactersEnum.ToArray();

        var builder = new StringBuilder();
        
        for (int i = 0; i < length; i++)
        {
            var characterIndex = Random.Shared.Next(availableCharactersLength);
            var character = availableCharacters[characterIndex];
            builder.Append(character);
        }

        return builder.ToString();
    }
}
