using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.DataAccess.StorageModels;

public sealed class UploadedFileModel
{
    public required string FileName { get; set; }
    public required string PublicUrl { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = new();
}
