﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CarDealershipApp.Web.UI.Models;
using CarDealershipApp.Common.Config;
using CarDealershipApp.Core.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CarDealershipApp.Web.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly string _userId;
        private readonly IHttpContextAccessor _httpContextAccessor = null;

        public AccountController(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> _userManager,
             IOptions<AppSettings> appSettings)
        {
            this._userManager = _userManager;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel formdata)
        {
            try
            {
                List<string> errorList = new List<string>();
                var user = await _userManager.FindByEmailAsync(formdata.Email);

                if (formdata == null)
                {
                    errorList.Add("username does not exist");
                    return Ok(new JsonResult(new { message = "username does not exist" }));
                }
                var roles = await _userManager.GetRolesAsync(user);
                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret));
                double tokenExpiryTime = Convert.ToDouble(_appSettings.ExpireTime);
                if (user != null && await _userManager.CheckPasswordAsync(user, formdata.Password))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenDesctiptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim(JwtRegisteredClaimNames.Sub,formdata.Email),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier,user.Id),
                        new Claim(ClaimTypes.Role,roles.FirstOrDefault()),
                        new Claim("LoggedOn",DateTime.Now.ToString()),
                        }),

                        SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                        Issuer = _appSettings.Site,
                        Audience = _appSettings.Audience,
                        Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTime)
                    };
                    //Generate Token
                    var token = tokenHandler.CreateToken(tokenDesctiptor);
                    return Ok(new { token = tokenHandler.WriteToken(token), tokenExpiryTime = token.ValidTo, username = user.Email, userRole = roles.FirstOrDefault(), message = "Registration Successful" });
                }
                return BadRequest(new JsonResult(new { message = "Please check your login credentials-Invalid username/Password was entered" }));

            }
            catch (Exception e)
            {

                return BadRequest(new JsonResult(new { message = "Please check your login credentials-Invalid username/Password was entered" }));
            }

        }

        [HttpPost("uploadfile"), DisableRequestSizeLimit]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Upload([FromServices]IUserService _userService)
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("AppResources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    DirectoryInfo directory = new DirectoryInfo(pathToSave);
                    //Delete Profile picture that is already saved
                    foreach (FileInfo fileSaved in directory.GetFiles())
                    {
                        fileSaved.Delete();
                    }
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    if (string.IsNullOrEmpty(dbPath))
                        return BadRequest();
                    //Save the Image's path to DB
                    bool isSaved=await _userService.UploadProfilePicture(dbPath);
                    if (!isSaved)
                        return BadRequest();

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("profile")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Profile([FromServices]IUserService _userService)
        {
            try
            {
                var profiledata =await _userService.GetUserProfile();
                return Ok(profiledata);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }
    }
}