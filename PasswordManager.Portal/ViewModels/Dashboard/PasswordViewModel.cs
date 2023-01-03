namespace PasswordManager.Portal.ViewModels.Dashboard;

public sealed class PasswordViewModel
{
    public Guid? Id { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public bool? Favorite { get; set; }

    public bool IsDesciptionExpanded { get; set; } = false;
}
