using Mapster;
using NOTE.Solutions.DAL.Repository;
using NOTE.Solutions.Entities.Interfaces;
using Reader.BLL.Abstractions;
using Reader.BLL.Contracts.Group.Requests;
using Reader.BLL.Contracts.Group.Responses;
using Reader.BLL.Contracts.User.Requests;
using Reader.BLL.Contracts.User.Responses;
using Reader.BLL.Errors;
using Reader.BLL.Interfaces;
using Reader.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.BLL.Services;

public class GroupService(IUnitOfWork unitOfWork) : IGroupService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> CreateAsync(CreateGroupRequest request, CancellationToken cancellationToken = default)
    {
        var group = request.Adapt<Group>();

        await _unitOfWork.Groups.AddAsync(group, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var group = await _unitOfWork.Groups.FindAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (group is null)
            return Result.Failure(GroupErrors.NotFound);

        group.IsDeleted = true;

        _unitOfWork.Groups.Update(group);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<GroupResponse[]>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var groups = await _unitOfWork.Groups.FindAllAsync(x=> true,new string[] {$"{nameof(Group.GroupClients)}.{nameof(Client)}"},cancellationToken);
        return Result.Success(groups.Adapt<GroupResponse[]>());
    }

    public async Task<Result<GroupResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var group = await _unitOfWork.Groups.FindAsync(x => x.Id == id, new string[] { $"{nameof(Group.GroupClients)}.{nameof(Client)}" }, cancellationToken: cancellationToken);

        if (group is null)
            return Result.Failure<GroupResponse>(GroupErrors.NotFound);

        return Result.Success<GroupResponse>(group.Adapt<GroupResponse>());
    }

    public async Task<Result> UpdateAsync(int id, UpdateGroupRequest request, CancellationToken cancellationToken = default)
    {
        var group = await _unitOfWork.Groups.FindAsync(x => x.Id == id,new string[] {$"{nameof(Group.GroupClients)}.{nameof(Client)}"});

        if (group is null)
            return Result.Failure(GroupErrors.NotFound);

        foreach(var clientId in request.ClientIds)
        {
            if (!_unitOfWork.Clients.IsExist(x => x.Id == clientId))
                return Result.Failure(ClientErrors.NotFound);
        }

        request.Adapt(group);

        var newClientIds = request.ClientIds.Except(group.GroupClients.Select(x => x.ClientId));
        var removedClientIds = group.GroupClients.Select(x => x.ClientId).Except(request.ClientIds);

        foreach(var newClientId in newClientIds)
        {
            group.GroupClients.Add(new GroupClient()
            {
                ClientId = newClientId,
                GroupId = group.Id
            });
        }

        foreach(var removedClientId in removedClientIds)
        {
            var removedClientGroup = group.GroupClients.First(x => x.ClientId == removedClientId);
            _unitOfWork.GroupClients.Delete(removedClientGroup);
        }

        _unitOfWork.Groups.Update(group);
        await _unitOfWork.SaveAsync();

        return Result.Success();
    }
}
