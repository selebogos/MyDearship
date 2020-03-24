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
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CarDealershipApp.Core.Service
{

    public class ClientService : IClientService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly string _userId;
        private readonly IHttpContextAccessor _httpContextAccessor = null;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IClientProfileService _clientProfileService;

        public ClientService(IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> _userManager,
            SignInManager<IdentityUser> _signInManager, IOptions<AppSettings> appSettings, IClientProfileService clientProfileService,
            ApplicationDbContext dbContext)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _mapper = mapper;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _clientProfileService = clientProfileService;
        }
        public async Task<ClientDto> Get(Guid id)
        {
            try
            {
                var client = await _dbContext.Clients.FirstAsync(p => p.Id == id);
                var clientDto = _mapper.Map<ClientDto>(client);
                clientDto.CreatedBy = client.User.Email;
                return clientDto;
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
            DataTableData<ClientDto> dataTableData = new DataTableData<ClientDto>();
            try
            {
                var query = (from client in _dbContext.Clients
                            join user in _dbContext.Users
                            on client.AddedById equals user.Id
                            select new ClientDto
                {
                    Address = client.Address,
                    FullName = client.FullName,
                    CreatedBy = client.User.Email,
                    Id = client.Id
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
                if (dataItems.Length !=totalCount)
                    query = query.Skip(dataItems.Start).Take(dataItems.Length);
                //Search
                // Apply filters
                if (totalCount > 0 && (!string.IsNullOrEmpty(dataItems.Search) || !string.IsNullOrWhiteSpace(dataItems.Search)))
                {
                    var value = dataItems.Search.Trim();
                    query = query.Where(p => p.FullName.Contains(value));
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
        public async Task<Tuple<Guid, string>> AddClient(ClientDto clientData)
        { 
            try
            {
                var user = await _userManager.FindByEmailAsync(_userId);
                var client = _mapper.Map<Client>(clientData);
                client.AddedById = user.Id;
                _dbContext.Clients.Add(client);
                _dbContext.ClientProfiles.Add(new ClientProfile { ClientId=client.Id});
                await _dbContext.SaveChangesAsync();
                return new Tuple<Guid, string>(client.Id,client.User.Email);
            }
            catch (Exception)
            {
               return new Tuple<Guid, string>(Guid.Empty, string.Empty);
            }
        }
        public async Task<IEnumerable<ClientDto>> GetAll()
        {
            try
            {
                List<ClientDto> list = await _dbContext.Clients.Select(x => new ClientDto
                {
                    Address = x.Address,
                    FullName = x.FullName,
                    CreatedBy = x.User.Email,
                    Id = x.Id
                }).ToListAsync();
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<bool> RemoveClient(Guid id)
        {
            try
            {
                var client = _dbContext.Clients.AsNoTracking().FirstOrDefault(p => p.Id == id);
                if (client == null) 
                {
                    return false;
                }
                var clientToRemove= _mapper.Map<Client>(client);
                _dbContext.Entry(clientToRemove).State = EntityState.Deleted;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<Guid> UpdateClient(ClientDto clientData)
        {
            try
            {
                var savedClient = _dbContext.Clients.AsNoTracking().FirstOrDefault(p => p.Id == clientData.Id);
                if (savedClient == null)
                {
                    return Guid.Empty;
                }
                clientData.AddedById = savedClient.AddedById;
                _dbContext.Entry(_mapper.Map<Client>(clientData)).State=EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return savedClient.Id;
            }
            catch (Exception e)
            {
                return Guid.Empty;
            }
        }
        public async Task<List<ClientProfileDataDto>> SearchByName(string name)
        {
            try
            {
                var clients =await (from client in _dbContext.Clients
                               join profile in _dbContext.ClientProfiles
                               on client.Id equals profile.ClientId
                               where client.FullName.Contains(name)
                               select new ClientProfileDataDto { ClientId=client.Id,profileId=profile.Id,FullName=client.FullName}).ToListAsync();
                return clients;
            }
            catch (Exception e)
            {

                return null;
            }
        }
    }
}
