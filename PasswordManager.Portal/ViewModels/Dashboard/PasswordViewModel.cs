using PasswordManager.Portal.Models;

namespace PasswordManager.Portal.ViewModels.Dashboard;

public sealed class PasswordCardViewModel
{
    public Guid? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public bool? Favorite { get; set; }
    public string? ImageUrl { get; set; }

    public bool IsDesciptionExpanded { get; set; } = false;

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
        };
    }
}
