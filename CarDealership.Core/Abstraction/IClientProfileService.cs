using CarDealershipApp.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipApp.Core.Abstraction
{
    public interface IClientProfileService
    {
        Task<Guid> AddClientProfile(ClientProfileDto patientData);
        Task<IEnumerable<ClientProfileDto>> GetAll();
        Task<string> GetAll(DatatableParametersDto dataItems);
        Task<ClientProfileDto> Get(Guid id);
        Task<bool> RemoveClientProfile(Guid id);
        Task<Guid> UpdateClientProfile(ClientProfileDto patientData);
        Task<bool> ClientHasBusinessProfile(Guid patientId);
    }
}
