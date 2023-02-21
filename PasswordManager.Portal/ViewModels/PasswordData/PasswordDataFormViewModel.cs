namespace PasswordManager.Portal.ViewModels.PasswordData;

public sealed class PasswordDataFormViewModel
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public CategoryViewModel? SelectedCategory { get; set; }
    public IEnumerable<CategoryViewModel> Categories { get; set; } = Enumerable.Empty<CategoryViewModel>();
    public Guid? ImageId { get; set; }

    public bool IsValid { get; set; } = false;
    public bool ShowPassword { get; set; } = false;
}

public sealed record CategoryViewModel(Guid Id, string Name);