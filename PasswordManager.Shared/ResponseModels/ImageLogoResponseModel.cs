using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Shared.ResponseModels;

public sealed class ImageLogoResponseModel
{
    public string? PublicUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
}
