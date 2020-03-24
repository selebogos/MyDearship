using AutoMapper;
using CarDealershipApp.Common.Config;
using CarDealershipApp.Common.DTOs;
using CarDealershipApp.Core.Abstraction;
using CarDealershipApp.DAL.DBContext;
using CarDealershipApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CarDealershipApp.Core.Service
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductTypeService(IMapper mapper,
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ProductTypeDto> Get(int id)
        {
            try
            {
                var result = await _dbContext.ProductTypes.FirstAsync(p => p.Id == id);
                var resultDto = _mapper.Map<ProductTypeDto>(result);
                return resultDto;
            }
            catch (Exception e)
            {

                return null;
            }
        }
        public async Task<string> GetAll(DatatableParametersDto dataItems)
        {
            string output = "";
            string error = "";
            DataTableData<ProductTypeDto> dataTableData = new DataTableData<ProductTypeDto>();
            try
            {
                var query = _dbContext.ProductTypes.Select(x => new ProductTypeDto
                {
                    Description = x.Description,
                    Id = x.Id
                });

                var totalCount = query.Count();
                int sortColumn = -1;
                string sortDirection = "asc";

                if (dataItems.Length <= 0)
                {
                    dataItems.Length = totalCount;
                }
                if (dataItems.Draw <= 0)
                {
                    dataItems.Draw = 10;
                }
                if (dataItems.Start <= -1)
                {
                    dataItems.Start = 0;
                }

                dataTableData.draw = dataItems.Draw;
                dataTableData.recordsTotal = totalCount;
                // Paging
                if (dataItems.Length != totalCount)
                    query = query.Skip(dataItems.Start).Take(dataItems.Length);
                //Search
                // Apply filters
                if (totalCount > 0 && (!string.IsNullOrEmpty(dataItems.Search) || !string.IsNullOrWhiteSpace(dataItems.Search)))
                {
                    var value = dataItems.Search.Trim();
                    query = query.Where(p => p.Description.Contains(value));
                }
                var data =await query.ToListAsync();
                dataTableData.recordsFiltered = totalCount;
                dataTableData.data = data;
                output = JsonConvert.SerializeObject(dataTableData, Formatting.Indented);
            }
            catch (Exception ex)
            {
                error = ex.Message;

            }
            return output;
        }
        public async Task<int> AddProductType(ProductTypeDto testResultData)
        { 
            try
            {
                var ProductType = _mapper.Map<ProductType>(testResultData);
                _dbContext.ProductTypes.Add(ProductType);
                await _dbContext.SaveChangesAsync();
                return ProductType.Id;
            }
            catch (Exception)
            {
               return -1;
            }
        }
        public async Task<IEnumerable<ProductTypeDto>> GetAll()
        {
            try
            {
                List<ProductTypeDto> list = await _dbContext.ProductTypes.Select(x => new ProductTypeDto
                {
                    Description = x.Description,
                    Id = x.Id
                }).ToListAsync();
                return list;
            }
            catch (Exception e)
            {

                return null;
            }
        }
        public async Task<bool> RemoveProductType(int id)
        {
            try
            {
                var ProductType = _dbContext.ProductTypes.AsNoTracking().FirstOrDefault(p => p.Id == id);
                if (ProductType == null) 
                {
                    return false;
                }
                var ProductTypeToRemove= _mapper.Map<ProductType>(ProductType);
                _dbContext.Entry(ProductTypeToRemove).State = EntityState.Deleted;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<int> UpdateProductType(ProductTypeDto testResultData)
        {
            try
            {
                var savedProductType = _dbContext.ProductTypes.AsNoTracking().FirstOrDefault(p => p.Id == testResultData.Id);
                if (savedProductType == null)
                {
                    return -1;
                }
                _dbContext.Entry(_mapper.Map<ProductType>(testResultData)).State=EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return savedProductType.Id;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
    }
}
