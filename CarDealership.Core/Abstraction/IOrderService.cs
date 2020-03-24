using CarDealershipApp.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipApp.Core.Abstraction
{
    public interface IOrderService
    {
        Task<Guid> AddOrder(OrderDto patientData);
        Task<IEnumerable<OrderDto>> GetAll();
        Task<string> GetAll(DatatableParametersDto dataItems);
        Task<OrderDto> Get(Guid id);
        Task<List<OrderDto>> SearchByNumber(int number);
        Task<bool> RemoveOrder(Guid id);
        Task<Guid> UpdateOrder(OrderDto patientData);
    }
}
