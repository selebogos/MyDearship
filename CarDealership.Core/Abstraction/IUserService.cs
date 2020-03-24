using CarDealershipApp.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipApp.Core.Abstraction
{
    public interface IUserService
    {
        Task<UserDto> GetUserProfile();
        Task<bool> UploadProfilePicture(string path);
        Task<UserDto> Login(UserDto userData);
    }
}
