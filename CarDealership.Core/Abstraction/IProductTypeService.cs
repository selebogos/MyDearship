using CarDealershipApp.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipApp.Core.Abstraction
{
    public interface IProductTypeService
    {
        Task<int> AddProductType(ProductTypeDto testResultData);
        Task<IEnumerable<ProductTypeDto>> GetAll();
        Task<string> GetAll(DatatableParametersDto dataItems);
        Task<ProductTypeDto> Get(int id);
        Task<bool> RemoveProductType(int id);
        Task<int> UpdateProductType(ProductTypeDto ProductTypeData);
    }
}
