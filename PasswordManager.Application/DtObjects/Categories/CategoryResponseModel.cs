using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PasswordManager.Application.DtObjects.Categories;

public sealed class CategoryResponseModel
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
}
