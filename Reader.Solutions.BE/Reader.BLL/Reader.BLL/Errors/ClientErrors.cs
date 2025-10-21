using Reader.BLL.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Reader.BLL.Errors;

public static class ClientErrors
{
    public static Error Duplicated => new Error("Client.Duplicated", "Email is Already exists.", StatusCodes.Status409Conflict);
    public static Error NotFound => new Error("Client.NotFound", "Client Not Found.", StatusCodes.Status404NotFound);


}
