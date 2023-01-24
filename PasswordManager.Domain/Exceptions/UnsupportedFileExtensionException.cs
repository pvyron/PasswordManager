using System.Diagnostics.CodeAnalysis;

namespace PasswordManager.Domain.Exceptions;

public sealed class UnsupportedFileExtensionException : Exception
{
    public required string FileExtension { get; init; }

    [SetsRequiredMembers]
    public UnsupportedFileExtensionException(string fileExtension) : base($"{fileExtension} is not supported")
    {
        FileExtension = fileExtension;
    }
}
