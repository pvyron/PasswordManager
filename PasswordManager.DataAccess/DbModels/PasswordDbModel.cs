﻿using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordManager.DataAccess.DbModels;

public class PasswordDbModel
{
    public Guid Id { get; set; }
    [ForeignKey(nameof(Category))]
    public Guid CategoryId { get; set; }
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public virtual UserDbModel? User { get; set; }
    public virtual PasswordCategoryDbModel? Category { get; set; }
    public required string Title { get; set; } = null!;
    public string? Description { get; set; }
    public required string Username { get; set; } = null!;
    public required string Password { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}
