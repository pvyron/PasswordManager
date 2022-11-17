using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Application.IServices
{
    public interface IAuthorizationService
    {
        Task Authenticate(string email, string password, CancellationToken cancellationToken);
    }
}
