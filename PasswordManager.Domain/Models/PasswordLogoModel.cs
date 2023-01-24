using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Domain.Models;

public sealed class PasswordLogoModel
{
    public required string Title { get; set; }
    public required string FileUrl { get; set; }
    public required string ThumbnailUrl { get; set; }
    public required string FileExtension { get; set; }
    public required string ThumbnailExtension { get; set; }

    public string FileLocation => $"{FileUrl}.{FileExtension}";

    public string ThumbnailLocation => $"{ThumbnailUrl}.{ThumbnailExtension}";
}
