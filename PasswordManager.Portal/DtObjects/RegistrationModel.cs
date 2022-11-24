using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Portal.DtObjects;

public sealed record RegistrationModel(string Email, string Password, string? FirstName, string? LastName);