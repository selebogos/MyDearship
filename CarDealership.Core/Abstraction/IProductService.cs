using CarDealershipApp.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarDealershipApp.Core.Abstraction
{
    public interface IProductService
    {
        Task<Guid> AddProduct(ProductDto patientData);
        Task<IEnumerable<ProductDto>> GetAll();
        Task<string> GetAll(DatatableParametersDto dataItems);
        Task<ProductDto> Get(Guid id);
        Task<bool> RemoveProduct(Guid id);
        Task<Guid> UpdateProduct(ProductDto patientData);
    }
}
