using Microsoft.AspNetCore.Http;
using Reader.BLL.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOTE.Solutions.BLL.Errors;
public static class UserErrors
{
    public static readonly Error InvalidCredentials
        = new Error("User.InvalidCredentials", "Invalid Email / Password.", StatusCodes.Status401Unauthorized);

    public static readonly Error UserNotAllowed
        = new Error("User.UserNotAllowed", "UserNotAllowed.", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidJwtToken
        = new Error("User.InvalidJwtToken", "Invalid Jwt Token.", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidRefreshToken
        = new Error("User.InvalidRefreshToken", "Invalid Refresh Token.", StatusCodes.Status401Unauthorized);


    public static readonly Error DuplicatedEmail
        = new Error("User.DuplicatedEmail", "Another User with the same email is already exists.", StatusCodes.Status409Conflict);

    public static readonly Error EmailNotConfirmed
        = new Error("User.EmailNotConfirmed", "Email Not Confirmed", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidCode
        = new Error("User.InvalidCode", "Invalid Code", StatusCodes.Status401Unauthorized);
     
    public static Error DisabledUser 
        = new Error("Auth.DisabledUser", "Disabled User please Contact administrator.", StatusCodes.Status401Unauthorized);

    public static Error LockedUser
        = new Error("Auth.LockedUser", "Locked User please contact administrator.", StatusCodes.Status401Unauthorized);


}
