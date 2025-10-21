using Reader.BLL.Abstractions;
using Reader.BLL.Contracts.Group.Requests;
using Reader.BLL.Contracts.Group.Responses;

namespace Reader.BLL.Interfaces;

public interface IGroupService
{
    Task<Result> CreateAsync(CreateGroupRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, UpdateGroupRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<GroupResponse[]>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<GroupResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
}
