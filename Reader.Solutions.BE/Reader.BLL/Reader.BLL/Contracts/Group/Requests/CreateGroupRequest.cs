using Reader.BLL.Contracts.User.Requests;

namespace Reader.BLL.Contracts.Group.Requests;

public class CreateGroupRequest
{
    public string Name { get; set; } = string.Empty;
}
