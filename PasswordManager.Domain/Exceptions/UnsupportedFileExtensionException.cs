using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
