using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.DataAccess.DbModels
{
    public class PasswordDbModel
    {
        [BsonId]
        public Guid Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
