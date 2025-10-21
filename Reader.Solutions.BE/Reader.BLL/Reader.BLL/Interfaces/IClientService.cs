using Reader.BLL.Abstractions;
using Reader.BLL.Contracts.Client.Requests;
using Reader.BLL.Contracts.User.Requests;
using Reader.BLL.Contracts.User.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.BLL.Interfaces;

public interface IClientService
{
    Task<Result> CreateAsync(CreateClientRequest request,CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, UpdateClientRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<ClientResponse[]>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<ClientResponse>> GetAsync(int id ,CancellationToken cancellationToken = default);
}
