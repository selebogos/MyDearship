using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;

namespace CarDealershipApp.Web.UI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly string _userId;
        private readonly IHttpContextAccessor _httpContextAccessor = null;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> _userManager,
            SignInManager<IdentityUser> _signInManager, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _orderService = orderService;
            _mapper = mapper;
        }
        [HttpPost("add")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> AddOrder([FromBody]OrderViewModel formdata)
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
                if (formdata.OrderItems.Count == 0) 
                {
                    return BadRequest(new JsonResult(new { message = "Please choose product(s)" }));
                }
                var order = _mapper.Map<OrderDto>(formdata);
                var orderData = await _orderService.AddOrder(order);

                if (orderData==Guid.Empty) 
                {
                    return NotFound();
                }
                order.Id = orderData;
                var addedOrder = _mapper.Map<OrderViewModel>(order);
                return CreatedAtAction(nameof(GetOrder), new { id = orderData }, addedOrder);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Something went wrong inside add order action: {e.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            try
            {
                var order = await _orderService.Get(id);
                if (order == null)
                    return NotFound();
                return Ok(order); 
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = e.Message }));
            }
        }
        [HttpGet("search")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetOrderByNumber(int orderNumber)
        {
            try
            {
                var orderDtos = await _orderService.SearchByNumber(orderNumber);
                if (orderDtos == null)
                    return NotFound();

                return Ok(orderDtos);
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = e.Message }));
            }
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody]OrderViewModel formdata)
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
                var order = _mapper.Map<OrderDto>(formdata);
                var orderId=await _orderService.UpdateOrder(order);
                if (orderId == Guid.Empty) 
                {
                    return NotFound();
                }
                order.Id = orderId;
                return CreatedAtAction(nameof(GetOrder), new { id = orderId}, _mapper.Map<OrderViewModel>(order));
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside update order action: {e.Message}" }));
            }
        }
        [HttpDelete("remove/{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> RemoveOrder(Guid id)
        {
            try
            {
                var removed =await _orderService.RemoveOrder(id);
                if (!removed)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside the remove order action: {e.Message}" }));
            }
        }
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(int draw=0, int start=0, int length=0,string search="") =>
            length  == 0 && draw==0 && search=="" && start==0 ? await GetAllOrders() : await GetAllOrders(new DatatableParametersViewModel { Draw = draw, Search = search, Start = start, Length = length });
        private async Task<IActionResult> GetAllOrders(DatatableParametersViewModel dataTableParameters)
        {
            try
            {
                var result = await _orderService.GetAll(_mapper.Map<DatatableParametersDto>(dataTableParameters));
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside the getall orders action: {e.Message}" }));
            }
        }
        private async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var result = await _orderService.GetAll();
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new JsonResult(new { message = $"Something went wrong inside the getall orders action: {e.Message}" }));
            }
        }

    }
}