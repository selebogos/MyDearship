using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using CarDealershipApp.Web.UI.Models;
using CarDealershipApp.Common.Config;
using CarDealershipApp.Common.DTOs;
using CarDealershipApp.Core.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
namespace CarDealershipApp.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientProfileController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly string _userId;
        private readonly IHttpContextAccessor _httpContextAccessor = null;
        private readonly IClientProfileService _clientProfileService;
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientProfileController(IClientService clientService, IClientProfileService clientProfileService, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> _userManager,
            SignInManager<IdentityUser> _signInManager, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _clientProfileService = clientProfileService;
            _mapper = mapper;
            _clientService = clientService;
        }
        [HttpPost("add")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> AddClientProfile([FromBody]ClientProfileViewModel formdata)
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
                var clientProfile = _mapper.Map<ClientProfileDto>(formdata);
                var clientProfileData = await _clientProfileService.AddClientProfile(clientProfile);

                if (clientProfileData==Guid.Empty) 
                {
                    return NotFound();
                }
                clientProfile.Id = clientProfileData;
                var addedClientProfile = _mapper.Map<ClientProfileViewModel>(clientProfile);
                return CreatedAtAction(nameof(GetClientProfile), new { id = clientProfileData }, addedClientProfile);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Something went wrong inside add clientProfile action: {e.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientProfile(Guid id)
        {
            try
            {
                var clientProfile = await _clientProfileService.Get(id);
                if (clientProfile == null)
                    return NotFound();

                return Ok(clientProfile);
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = e.Message }));
            }
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateClientProfile(Guid id, [FromBody]ClientProfileViewModel formdata)
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
                if (id != formdata.Id)
                {
                    return BadRequest(new JsonResult(new { message = "please ensure you are updating right object" }));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object sent from client.");
                }
                var clientProfile = _mapper.Map<ClientProfileDto>(formdata);
                var clientProfileId=await _clientProfileService.UpdateClientProfile(clientProfile);
                if (clientProfileId == Guid.Empty) 
                {
                    return NotFound();
                }
                clientProfile.Id = clientProfileId;
                return CreatedAtAction(nameof(GetClientProfile), new { id = clientProfileId}, _mapper.Map<ClientProfileViewModel>(clientProfile));
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside update clientProfile action: {e.Message}" }));
            }
        }
        [HttpDelete("remove/{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> RemoveClientProfile(Guid id)
        {
            try
            {
                var removed =await _clientProfileService.RemoveClientProfile(id);
                if (!removed)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside the remove clientProfile action: {e.Message}" }));
            }
        }
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(int draw=0, int start=0, int length=0,string search="") =>
            length  == 0 && draw==0 && search=="" && start==0 ? await GetAllClientProfiles() : await GetAllClientProfiles(new DatatableParametersViewModel { Draw = draw, Search = search, Start = start, Length = length });
        private async Task<IActionResult> GetAllClientProfiles(DatatableParametersViewModel dataTableParameters)
        {
            try
            {
                var result = await _clientProfileService.GetAll(_mapper.Map<DatatableParametersDto>(dataTableParameters));
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside the getall clientProfiles action: {e.Message}" }));
            }
        }
        private async Task<IActionResult> GetAllClientProfiles()
        {
            try
            {
                var result = await _clientProfileService.GetAll();
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside the getall clientProfiles action: {e.Message}" }));
            }
        }
        
    }
}