﻿using Microsoft.AspNetCore.Identity;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Models;
using System.Threading.Tasks;

namespace RepairshopWeb.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        //Task CheckRoleAsync(string roleName);

        //Task AddUserToRoleAsync(User user, string roleName);

        //Task<bool> IsUserInRoleAsync(User user, string roleName);
    }
}