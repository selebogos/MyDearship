using CarDealershipApp.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarDealershipApp.Core.Abstraction
{
    public interface IClientService
    {
        Task<Tuple<Guid, string>> AddClient(ClientDto patientData);
        Task<IEnumerable<ClientDto>> GetAll();
        Task<string> GetAll(DatatableParametersDto dataItems);
        Task<ClientDto> Get(Guid id);
        Task<List<ClientProfileDataDto>> SearchByName(string name);
        Task<bool> RemoveClient(Guid id);
        Task<Guid> UpdateClient(ClientDto patientData);
    }
}
