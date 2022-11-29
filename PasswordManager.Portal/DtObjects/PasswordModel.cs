﻿namespace PasswordManager.Portal.DtObjects;

public sealed class PasswordResponseModel
{
    public required Guid Id { get; set; }
    public Guid? CategoryId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}