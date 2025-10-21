using Microsoft.AspNetCore.Identity;
using Reader.Entities.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.Entities.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
}
