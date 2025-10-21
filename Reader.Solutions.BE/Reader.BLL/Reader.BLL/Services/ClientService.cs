using Mapster;
using NOTE.Solutions.Entities.Interfaces;
using Reader.BLL.Abstractions;
using Reader.BLL.Contracts.Client.Requests;
using Reader.BLL.Contracts.User.Requests;
using Reader.BLL.Contracts.User.Responses;
using Reader.BLL.Errors;
using Reader.BLL.Interfaces;
using Reader.Entities.Entities;

namespace Reader.BLL.Services;

public class ClientService(IUnitOfWork unitOfWork) : IClientService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> CreateAsync(CreateClientRequest request, CancellationToken cancellationToken = default)
    {
        if (_unitOfWork.Clients.IsExist(x => x.Email == request.Email && !x.IsDeleted))
            return Result.Failure(ClientErrors.Duplicated);

        var client = request.Adapt<Client>();
        
        await _unitOfWork.Clients.AddAsync(client, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id,CancellationToken cancellationToken = default)
    {
        var client = await _unitOfWork.Clients.FindAsync(x=>x.Id == id,cancellationToken:cancellationToken);
        
        if(client is null)
            return Result.Failure(ClientErrors.NotFound);

        client.IsDeleted = true;

        _unitOfWork.Clients.Update(client);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<ClientResponse[]>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var clients = await _unitOfWork.Clients.FindAllAsync(x=> true, new string[] { $"{nameof(Client.GroupClients)}.{nameof(Group)}" }, cancellationToken);
        return Result.Success(clients.Adapt<ClientResponse[]>());
    }

    public async Task<Result<ClientResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var client = await _unitOfWork.Clients.FindAsync(x => x.Id == id,new string[] {$"{nameof(Client.GroupClients)}.{nameof(GroupClient.Group)}"}, cancellationToken: cancellationToken);

        if (client is null)
            return Result.Failure<ClientResponse>(ClientErrors.NotFound);

        return Result.Success<ClientResponse>(client.Adapt<ClientResponse>());
    }

    public async Task<Result> UpdateAsync(int id,UpdateClientRequest request, CancellationToken cancellationToken = default)
    {
        var client = await _unitOfWork.Clients.FindAsync(x => x.Id == id && !x.IsDeleted);

        if (client is null)
            return Result.Failure<ClientResponse>(ClientErrors.NotFound);

        if (_unitOfWork.Clients.IsExist(x => x.Email == request.Email && x.Id != id))
            return Result.Failure(ClientErrors.Duplicated);

        request.Adapt(client);

        var newGroupIds = request.GroupIds.Except(client.GroupClients.Select(x => x.GroupId));
        var removedGroupIds = client.GroupClients.Select(x => x.GroupId).Except(request.GroupIds);

        foreach (var newGroupId in newGroupIds)
        {
            client.GroupClients.Add(new GroupClient()
            {
                ClientId = client.Id,
                GroupId = newGroupId
            });
        }

        foreach (var removedGroupId in removedGroupIds)
        {
            var removedClientGroup = client.GroupClients.First(x => x.GroupId == removedGroupId);
            _unitOfWork.GroupClients.Delete(removedClientGroup);
        }

        _unitOfWork.Clients.Update(client);
        await _unitOfWork.SaveAsync();

        return Result.Success();
    }
}
