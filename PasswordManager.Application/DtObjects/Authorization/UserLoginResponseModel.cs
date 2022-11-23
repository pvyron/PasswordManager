using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Application.DtObjects.Authorization;

public sealed record UserLoginResponseModel
{
    public required string AccessToken { get; set; }
}
