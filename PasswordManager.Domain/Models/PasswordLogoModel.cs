using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Domain.Models;

public sealed class PasswordLogoModel
{
	public required string FileUrl { get; set; }
    public required string ThuumbnailUrl { get; set; }
    public required string FileExtension { get; set; }
    public required string ThuumbnailExtension { get; set; }

    public string FileLocation => $"{FileUrl}.{FileExtension}";

    public string ThuumbnailLocation => $"{ThuumbnailUrl}.{ThuumbnailExtension}";
}
