﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealershipApp.DAL.DBContext
{
    public static class ApplicationDbInitializer
    {
        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByEmailAsync("hello@cardealer.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "hello@cardealer.com",
                    Email = "hello@cardealer.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "No1Know$").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
