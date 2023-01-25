namespace PasswordManager.Portal.Models;

public sealed class LogoModel
{
    public required Guid ImageId { get; init; }
    public required string Title { get; init; }
    public required string FileUrl { get; init; }

    private bool? isValid;

    public bool IsValid
    {
        get
        {
            if (isValid.HasValue)
                return isValid.Value;

            isValid = true;

            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(FileUrl))
            {
                isValid = false;
            }

            return isValid.Value;
        }
    }

    public override string ToString()
    {
        return Title;
    }
}
