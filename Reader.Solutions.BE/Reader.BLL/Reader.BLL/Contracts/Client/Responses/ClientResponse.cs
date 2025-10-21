using Reader.BLL.Contracts.Group.Responses;


namespace Reader.BLL.Contracts.User.Responses;

public class ClientResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string AppPassword { get; set; } = string.Empty;
}
