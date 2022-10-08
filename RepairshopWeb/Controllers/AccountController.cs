using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Helpers;
using RepairshopWeb.Models;
using SendGrid;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RepairshopWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly IEmailHelper _emailHelper;

        public AccountController(IUserHelper userHelper,
            IConfiguration configuration,
            IEmailHelper emailHelper)
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _emailHelper = emailHelper;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                var emailConfirmed = await _userHelper.IsEmailConfirmedAsync(user);

                if (result.Succeeded && emailConfirmed == true)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                        return Redirect(this.Request.Query["ReturnUrl"].First());

                    return this.RedirectToAction("Index", "Home");
                }
                else if (emailConfirmed == false)
                {
                    ViewBag.Message = "Unconfirmed e-mail.";
                    return View(model);
                }
            }

            ViewBag.Message = "Failed to login! Please enter correct login and password.";
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        Role = model.Role,
                        UserName = model.Username
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);

                    await _userHelper.AddUserToRoleAsync(user, model.Role);

                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn´t be created.");
                        return View(model);
                    }

                    var token = await _userHelper.GenerateConfirmEmailTokenAsync(user);

                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = model.Username }, Request.Scheme);

                    await _emailHelper.SendEmail($"{model.Username}", $"Welcome to RepairShop", $"Mr. /Ms. {user.FirstName} {user.LastName},<br/><br/> " +
                      $"Welcome to RepairShop!<br/><br/> Here's your login and password: <br/><br/> Login: {model.UserName}<br/>Password:{model.Password}" +
                        "<br/><br/>For your security it is recommended that you change your password.<br/>To do this go to: <b>My Account >> Change password</b>" +
                        $"<br/><br/> " +
                        $"To confirme your account click in this link: " +
                       $"<a href = \"{confirmationLink}\">Account Confirmation</a>" +
                        "<br/><br/>Best regards, " +
                        "<br/>RepairShop");

                    ViewBag.Message = "User created successfully!";
                    return this.View();
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);
            if(user == null)
            {
                return View("Error");
            }

            if(token == String.Empty)
            {
                return View("Error");
            }

            await _userHelper.EmailConfirmAsync(user, token);
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();

            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;

                    var response = await _userHelper.UpdateUserAsync(user);

                    if (response.Succeeded)
                        this.ViewBag.Message = "Your data has been updated!";

                    else
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        await _userHelper.LogoutAsync();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        ViewBag.Message = "Invalid password! Please, enter your current password correctly.";
                }
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);

                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);
                    }
                }
            }
            return BadRequest();
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                var link = this.Url.Action("ResetPassword", "Account", new { token = myToken }, protocol: HttpContext.Request.Scheme);

                await _emailHelper.SendEmail(user.Email, "RepairShop Reset Password<br/>",
                       $"Mr. /Mrs. {user.FirstName} {user.LastName},<br/> " +
                       $"To reset your password click on the link below:<br/><br/>" +
                       $"<a href = \"{link}\">Reset Password</a>" +
                        "<br/><br/>Best regards, " +
                        "<br/>RepairShop");

                this.ViewBag.Message = "The instructions to recover your password has been sent to your email.";

                return this.View();
            }

            return this.View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.UserName);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.NewPassword);

                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password reset successful.";
                    return this.View();
                }

                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            this.ViewBag.Message = "User not found!";
            return View(model);
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        public IActionResult UserNotFound()
        {
            return View();
        }
    }
}
