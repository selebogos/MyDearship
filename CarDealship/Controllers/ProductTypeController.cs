using System;
using System.Threading.Tasks;
using AutoMapper;
using CarDealershipApp.Web.Models;
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
    public class ProductTypeController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly string _userId;
        private readonly IHttpContextAccessor _httpContextAccessor = null;
        private readonly IProductTypeService _productTypeService;
        private readonly IMapper _mapper;

        public ProductTypeController(IProductTypeService productTypeService, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> _userManager,
            SignInManager<IdentityUser> _signInManager, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _productTypeService = productTypeService;
            _mapper = mapper;
        }
        [HttpPost("add")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> AddProductType([FromBody]ProductTypeViewModel formdata)
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
                var productType = _mapper.Map<ProductTypeDto>(formdata);
                var productTypeData = await _productTypeService.AddProductType(productType);

                if (productTypeData==-1) 
                {
                    return NotFound();
                }
                productType.Id = productTypeData;
                var addedTestresult = _mapper.Map<ProductTypeViewModel>(productType);
                return CreatedAtAction(nameof(GetProductType), new { id = productTypeData }, addedTestresult);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Something went wrong inside add testresult action: {e.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductType(int id)
        {
            try
            {
                var productType = await _productTypeService.Get(id);
                if (productType == null)
                    return NotFound();

                return Ok(productType);
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = e.Message }));
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateProductType(int id, [FromBody]ProductTypeViewModel formdata)
        {
            try
            {

                if (formdata == null)
                {
                    return BadRequest(new JsonResult(new { message = "object sent from client is null." }));
                }
                if (id <=0)
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
                var productType = _mapper.Map<ProductTypeDto>(formdata);
                var productTypeId=await _productTypeService.UpdateProductType(productType);
                if (productTypeId == -1) 
                {
                    return NotFound();
                }
                productType.Id = productTypeId;
                return CreatedAtAction(nameof(GetProductType), new { id = productTypeId}, _mapper.Map<ProductTypeViewModel>(productType));
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside update ProductType action: {e.Message}" }));
            }
        }
        [HttpDelete("remove/{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> RemoveProductType(int id)
        {
            try
            {
                var removed =await _productTypeService.RemoveProductType(id);
                if (!removed)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside the remove ProductType action: {e.Message}" }));
            }
        }
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(int draw=0, int start=0, int length=0,string search="") =>
            length  == 0 && draw==0 && search=="" && start==0 ? await GetAllProductTypes() : await GetAllProductTypes(new DatatableParametersViewModel { Draw = draw, Search = search, Start = start, Length = length });
        private async Task<IActionResult> GetAllProductTypes(DatatableParametersViewModel dataTableParameters)
        {
            try
            {
                var result = await _productTypeService.GetAll(_mapper.Map<DatatableParametersDto>(dataTableParameters));
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside the getall testresults action: {e.Message}" }));
            }
        }
        private async Task<IActionResult> GetAllProductTypes()
        {
            try
            {
                var result = await _productTypeService.GetAll();
                if (result == null)
                {
                    return BadRequest();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside the getall testresults action: {e.Message}" }));
            }
        }
    }
}