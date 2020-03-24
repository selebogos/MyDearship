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
    public class ClientProfileService : IClientProfileService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly string _userId;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ClientProfileService(IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> _userManager, ApplicationDbContext dbContext)
        {
            this._userManager = _userManager;
            _dbContext = dbContext;
            _mapper = mapper;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        public async Task<ClientProfileDto> Get(Guid id)
        {
            try
            {
                var clientProfile = await _dbContext.ClientProfiles.FirstAsync(p => p.Id == id);
                var clientProfileDto = _mapper.Map<ClientProfileDto>(clientProfile);
                return clientProfileDto;
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
            DataTableData<ClientProfileDto> dataTableData = new DataTableData<ClientProfileDto>();
            try
            {
                var query = _dbContext.ClientProfiles.Select(x => new ClientProfileDto
                {
                    Orders = _mapper.Map<ICollection<Order>, ICollection<OrderDto>>(x.Orders),//MapAddress(x.Address),
                    ClientId = x.ClientId,
                    Client = _mapper.Map<ClientDto>(x.Client),
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
        public async Task<Guid> AddClientProfile(ClientProfileDto clientProfileData)
        {
            try
            {
                if (await ClientHasBusinessProfile(clientProfileData.ClientId))
                {
                    return Guid.Empty;
                }
                var user = await _userManager.FindByEmailAsync(_userId);
                var clientProfile = _mapper.Map<ClientProfile>(clientProfileData);
                _dbContext.ClientProfiles.Add(clientProfile);
                await _dbContext.SaveChangesAsync();
                return clientProfile.Id;
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }
        public async Task<IEnumerable<ClientProfileDto>> GetAll()
        {
            try
            {
                List<ClientProfileDto> list = await _dbContext.ClientProfiles.Select(x => new ClientProfileDto
                {
                    Orders = _mapper.Map<ICollection<Order>, ICollection<OrderDto>>(x.Orders),//MapAddress(x.Address),
                    ClientId = x.ClientId,
                    Client = _mapper.Map<ClientDto>(x.Client),
                    Id = x.Id
                }).ToListAsync();
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<bool> RemoveClientProfile(Guid id)
        {
            try
            {
                var clientProfile = _dbContext.ClientProfiles.AsNoTracking().FirstOrDefault(p => p.Id == id);
                if (clientProfile == null)
                {
                    return false;
                }
                var clientProfileToRemove = _mapper.Map<ClientProfile>(clientProfile);
                _dbContext.Entry(clientProfileToRemove).State = EntityState.Deleted;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<Guid> UpdateClientProfile(ClientProfileDto clientProfileData)
        {
            try
            {
                var savedClientProfile = _dbContext.ClientProfiles.AsNoTracking().FirstOrDefault(p => p.Id == clientProfileData.Id);
                if (savedClientProfile == null)
                {
                    return Guid.Empty;
                }
                _dbContext.Entry(_mapper.Map<ClientProfile>(clientProfileData)).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return savedClientProfile.Id;
            }
            catch (Exception e)
            {
                return Guid.Empty;
            }
        }
        public async Task<bool> ClientHasBusinessProfile(Guid patientId)
        {
            try
            {
                var clientProfile = await _dbContext.ClientProfiles.FirstOrDefaultAsync(p => p.ClientId == patientId);
                if (clientProfile == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
