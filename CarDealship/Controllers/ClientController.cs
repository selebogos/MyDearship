using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarDealershipApp.Common.Config;
using CarDealershipApp.Common.DTOs;
using CarDealershipApp.Core.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using CarDealershipApp.Web.UI.Models;

namespace CarDealershipApp.Web.UI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly string _userId;
        private readonly IHttpContextAccessor _httpContextAccessor = null;
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientController(IClientService clientService, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> _userManager,
            SignInManager<IdentityUser> _signInManager, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _clientService = clientService;
            _mapper = mapper;
        }
        [HttpPost("add")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> AddClient([FromBody]ClientViewModel formdata)
        {
            try
            {
                if (formdata == null)
                {
                    return BadRequest(new JsonResult(new { message = "object sent from client is null." }));
                }
                else if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object sent from client.");
                }
                var client = _mapper.Map<ClientDto>(formdata);
                var clientData = await _clientService.AddClient(client);

                if (clientData.Item1==Guid.Empty) 
                {
                    return NotFound();
                }
                client.Id = clientData.Item1;
                client.CreatedBy = clientData.Item2;
                var addedClient = _mapper.Map<ClientViewModel>(client);
                return CreatedAtAction(nameof(GetClient), new { id = clientData.Item1 }, addedClient);
            }   
            catch (Exception e)
            {
                return StatusCode(500, $"Something went wrong inside add client action: {e.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(Guid id)
        {
            try
            {
                var client = await _clientService.Get(id);
                if (client == null)
                    return NotFound();

                return Ok(client);
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = e.Message }));
            }
        }
        [HttpGet("search")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetClientByName(string name)
        {
            try
            {
                var client = await _clientService.SearchByName(name);
                if (client == null)
                    return NotFound();

                return Ok(client);
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = e.Message }));
            }
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody]ClientViewModel formdata)
        {
            try
            {

                if (formdata == null)
                {
                    return BadRequest(new JsonResult(new { message = "object sent from client is null." }));
                }
                if (id == null || id==Guid.Empty)
                {
                    return BadRequest(new JsonResult(new { message = "object sent from client is null." }));
                }
                if (id !=formdata.Id)
                {
                    return BadRequest(new JsonResult(new { message = "please ensure you are updating right client"}));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object sent from client.");
                }
                var client = _mapper.Map<ClientDto>(formdata);
                var clientId=await _clientService.UpdateClient(client);
                if (clientId == Guid.Empty) 
                {
                    return NotFound();
                }
                client.Id = clientId;
                return CreatedAtAction(nameof(GetClient), new { id = clientId}, _mapper.Map<ClientViewModel>(client));
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside update client action: {e.Message}" }));
            }
        }
        [HttpDelete("remove/{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> RemoveClient(Guid id)
        {
            try
            {
                var removed =await _clientService.RemoveClient(id);
                if (!removed)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside the remove client action: {e.Message}" }));
            }
        }
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(int draw=0, int start=0, int length=0,string search="") =>
            length  == 0 && draw==0 && search=="" && start==0 ? await GetAllClients() : await GetAllClients(new DatatableParametersViewModel { Draw = draw, Search = search, Start = start, Length = length });
        private async Task<IActionResult> GetAllClients(DatatableParametersViewModel dataTableParameters)
        {
            try
            {
                var result = await _clientService.GetAll(_mapper.Map<DatatableParametersDto>(dataTableParameters));
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside the getall clients action: {e.Message}" }));
            }
        }
        private async Task<IActionResult> GetAllClients()
        {
            try
            {
                var result = await _clientService.GetAll();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside the getall clients action: {e.Message}" }));
            }
        }
    }
}