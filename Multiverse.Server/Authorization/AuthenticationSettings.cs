using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.Server.Authorization
{
    public class AuthenticationSettings
    {
        public string JwtSecret { get; set; } = string.Empty;
    }
}
