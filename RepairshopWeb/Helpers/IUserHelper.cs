﻿using Microsoft.AspNetCore.Identity;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepairshopWeb.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<bool> IsEmailConfirmedAsync(User user);

        Task<IdentityResult> EmailConfirmAsync(User user, string token);

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<SignInResult> ValidatePasswordAsync(User user, string password);

        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);

        Task<string> GenerateConfirmEmailTokenAsync(User user);

    }
}
