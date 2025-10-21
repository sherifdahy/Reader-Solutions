using Reader.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.BLL.Authentication;

public interface IJWTProvider
{
    (string token, int expiresIn) GeneratedToken(ApplicationUser applicationUser, IEnumerable<string> applicationRoles);
    int? ValidateToken(string token);
}
