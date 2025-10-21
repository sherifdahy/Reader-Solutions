using Reader.BLL.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Reader.BLL.Errors;

public static class GroupErrors
{
    public static Error NotFound => new Error("Client.NotFound", "Group Not Found.", StatusCodes.Status404NotFound);


}
