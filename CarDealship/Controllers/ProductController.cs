using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;

namespace CarDealershipApp.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly string _userId;
        private readonly IHttpContextAccessor _httpContextAccessor = null;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> _userManager,
            SignInManager<IdentityUser> _signInManager, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
            _mapper = mapper;
        }
        [HttpPost("add")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> AddProduct([FromBody]ProductViewModel formdata)
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
                var product = _mapper.Map<ProductDto>(formdata);
                var productData = await _productService.AddProduct(product);

                if (productData==Guid.Empty) 
                {
                    return NotFound();
                }
                product.Id = productData;
                var addedProduct = _mapper.Map<ProductViewModel>(product);
                return CreatedAtAction(nameof(GetProduct), new { id = productData }, addedProduct);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Something went wrong inside add product action: {e.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            try
            {
                var product = await _productService.Get(id);
                if (product == null)
                    return NotFound();

                return Ok(product);
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = e.Message }));
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody]ProductViewModel formdata)
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
                var product = _mapper.Map<ProductDto>(formdata);
                var productId=await _productService.UpdateProduct(product);
                if (productId == Guid.Empty) 
                {
                    return NotFound();
                }
                product.Id = productId;
                return CreatedAtAction(nameof(GetProduct), new { id = productId}, _mapper.Map<ProductViewModel>(product));
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside update product action: {e.Message}" }));
            }
        }
        [HttpDelete("remove/{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> RemoveProduct(Guid id)
        {
            try
            {
                var removed =await _productService.RemoveProduct(id);
                if (!removed)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside the remove product action: {e.Message}" }));
            }
        }
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(int draw=0, int start=0, int length=0,string search="") =>
            length  == 0 && draw==0 && search=="" && start==0 ? await GetAllProducts() : await GetAllProducts(new DatatableParametersViewModel { Draw = draw, Search = search, Start = start, Length = length });
        private async Task<IActionResult> GetAllProducts(DatatableParametersViewModel dataTableParameters)
        {
            try
            {
                var result = await _productService.GetAll(_mapper.Map<DatatableParametersDto>(dataTableParameters));
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside the getall products action: {e.Message}" }));
            }
        }
        private async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var result = await _productService.GetAll();
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside the getall products action: {e.Message}" }));
            }
        }
    }
}