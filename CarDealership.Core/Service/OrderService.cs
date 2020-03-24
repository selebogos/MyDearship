using AutoMapper;
using CarDealershipApp.Common.Config;
using CarDealershipApp.Common.DTOs;
using CarDealershipApp.Core.Abstraction;
using CarDealershipApp.DAL.DBContext;
using CarDealershipApp.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarDealershipApp.Core.Service
{
    public class OrderService : IOrderService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly string _userId;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderService(IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> _userManager, ApplicationDbContext dbContext)
        {
            this._userManager = _userManager;
            _dbContext = dbContext;
            _mapper = mapper;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        public async Task<OrderDto> Get(Guid id)
        {
            try
            {
                var order = await _dbContext.Orders.FirstAsync(p => p.Id == id);
                var orderDto = _mapper.Map<OrderDto>(order);
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
            DataTableData<OrderDto> dataTableData = new DataTableData<OrderDto>();
            try
            {
                var query = _dbContext.Orders.Select(x => new OrderDto
                {
                    DateCreated=x.DateCreated,
                    ProfileId = x.ProfileId,
                    OrderNumber=x.OrderNumber,
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
                /*if (!string.IsNullOrEmpty(dataItems.Search) || !string.IsNullOrWhiteSpace(dataItems.Search))
                {
                    var value = dataItems.Search.Trim();
                    query = query.Where(p => p..ToString().Contains(value));
                }*/
                var data = await query.Select(x=>new OrderDto { Id=x.Id,DateCreated=x.DateCreated,Amount=x.Amount}).ToListAsync();
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
        public async Task<Guid> AddOrder(OrderDto orderData)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(_userId);
                var noOfOrders = await GetAll();
                var order = new Order { OrderNumber = noOfOrders.Count() + 1 ,ProfileId=orderData.ProfileId,AddedById=user.Id};
                 order.OrderNumber = noOfOrders.Count() + 1;
                _dbContext.Orders.Add(order);
                foreach (var item in orderData.OrderItems)
                {
                    var product = _dbContext.Products.FirstOrDefault(p=>p.Id==item.ProductId);
                    if(product==null)
                        return Guid.Empty;
                    order.TotalCost += product.Cost;
                    order.TotalSellingPrice += product.SellingPrice;
                    _dbContext.OrderItems.Add(new OrderItem{OrderId=order.Id,ProductId=item.ProductId });
                }
                
                await _dbContext.SaveChangesAsync();
                return order.Id;
            }
            catch (Exception e)
            {
                return Guid.Empty;
            }
        }
        public async Task<IEnumerable<OrderDto>> GetAll()
        {
            try
            {
                List<OrderDto> list = await _dbContext.Orders.Select(x => new OrderDto
                {
                    OrderItems = _mapper.Map<ICollection<OrderItem>, ICollection<OrderItemDto>>(x.OrderItems),//MapAddress(x.Address),
                    ProfileId = x.ProfileId,
                    Profile = _mapper.Map<ClientProfileDto>(x.Profile),
                    Id = x.Id
                }).ToListAsync();
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<bool> RemoveOrder(Guid id)
        {
            try
            {
                var order = _dbContext.Orders.AsNoTracking().FirstOrDefault(p => p.Id == id);
                if (order == null)
                {
                    return false;
                }
                var orderToRemove = _mapper.Map<Order>(order);
                _dbContext.Entry(orderToRemove).State = EntityState.Deleted;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<Guid> UpdateOrder(OrderDto orderData)
        {
            try
            {
                var savedOrder = _dbContext.Orders.AsNoTracking().FirstOrDefault(p => p.Id == orderData.Id);
                if (savedOrder == null)
                {
                    return Guid.Empty;
                }
                _dbContext.Entry(_mapper.Map<Order>(orderData)).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return savedOrder.Id;
            }
            catch (Exception e)
            {
                return Guid.Empty;
            }
        }

        public async Task<List<OrderDto>> SearchByNumber(int number)
        {
            try
            {
                var orders = await _dbContext.Orders.Where(p=>p.OrderNumber.ToString().Contains(number.ToString())).ToListAsync();
                var orderDtos = _mapper.Map<List<Order>, List<OrderDto>>(orders);
                return orderDtos;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
