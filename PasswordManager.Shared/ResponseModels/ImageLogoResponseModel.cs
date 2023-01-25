namespace PasswordManager.Shared.ResponseModels;

public sealed class LogoImageResponseModel
{
    public Guid? LogoImageId {get; set; }
    public string? Title { get; set; }
    public string? PublicUrl { get; set; }
}
