using AutoMapper;
using CarDealershipApp.Common.Config;
using CarDealershipApp.Common.DTOs;
using CarDealershipApp.Core.Abstraction;
using CarDealershipApp.DAL.DBContext;
using CarDealershipApp.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipApp.Core.Service
{
    public class ProductService : IProductService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly string _userId;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductService(IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> _userManager, ApplicationDbContext dbContext)
        {
            this._userManager = _userManager;
            _dbContext = dbContext;
            _mapper = mapper;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        public async Task<ProductDto> Get(Guid id)
        {
            try
            {
                var order = await _dbContext.Products.FirstAsync(p => p.Id == id);
                var orderDto = _mapper.Map<ProductDto>(order);
                return orderDto;
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
            DataTableData<ProductDto> dataTableData = new DataTableData<ProductDto>();
            try
            {
                var query = _dbContext.Products.Select(x => new ProductDto
                {
                    Name=x.Name,
                    Cost=x.Cost,
                    SellingPrice=x.SellingPrice,
                    ImagePath=x.ImagePath,
                    ProductType = _mapper.Map<ProductTypeDto>(x.ProductType),
                    ProductTypeId=x.ProductTypeId,
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
                if (totalCount>0 &&(!string.IsNullOrEmpty(dataItems.Search) || !string.IsNullOrWhiteSpace(dataItems.Search)))
                {
                    var value = dataItems.Search.Trim();
                    query = query.Where(p => p.Name.Contains(value));
                }
                var data = await query.ToListAsync();
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
        public async Task<Guid> AddProduct(ProductDto testData)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(_userId);
                var test = _mapper.Map<Product>(testData);
                _dbContext.Products.Add(test);
                await _dbContext.SaveChangesAsync();
                return test.Id;
            }
            catch (Exception e)
            {
                return Guid.Empty;
            }
        }
        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            try
            {
                List<ProductDto> list = await _dbContext.Products.Select(x => new ProductDto
                {
                    ProductType = _mapper.Map<ProductTypeDto>(x.ProductType),
                    Name = x.Name,
                    ProductTypeId = x.ProductTypeId,
                    Id = x.Id
                }).ToListAsync();
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<bool> RemoveProduct(Guid id)
        {
            try
            {
                var test = _dbContext.Products.AsNoTracking().FirstOrDefault(p => p.Id == id);
                if (test == null)
                {
                    return false;
                }
                var testToRemove = _mapper.Map<Product>(test);
                _dbContext.Entry(testToRemove).State = EntityState.Deleted;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<Guid> UpdateProduct(ProductDto testData)
        {
            try
            {
                var savedProduct = _dbContext.Products.AsNoTracking().FirstOrDefault(p => p.Id == testData.Id);
                if (savedProduct == null)
                {
                    return Guid.Empty;
                }
                _dbContext.Entry(_mapper.Map<Product>(testData)).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return savedProduct.Id;
            }
            catch (Exception e)
            {
                return Guid.Empty;
            }
        }
    }
}
