using PasswordManager.Portal.Models;

namespace PasswordManager.Portal.ViewModels.Dashboard;

public sealed class PasswordCardViewModel
{
    private PasswordCardViewModel()
    {
        IsDescriptionExpanded = false;
    }

    public Guid? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public bool? Favorite { get; set; }
    public string? ImageUrl { get; set; }

    public bool IsDescriptionExpanded { get; set; }

    public static PasswordCardViewModel FromPassword(PasswordModel passwordModel)
    {
        return new PasswordCardViewModel
        {
            Id = passwordModel.Id,
            Description = passwordModel.Description,
            Username = passwordModel.Username,
            Password = passwordModel.Password,
            Favorite = passwordModel.IsFavorite,
            ImageUrl = passwordModel.Logo?.FileUrl,
            Title = passwordModel.Title,
            _passwordModel = passwordModel,
        };
    }

    private PasswordModel _passwordModel = null!;
    public PasswordModel GetPasswordModel()
    {
        return _passwordModel;
    }
}
