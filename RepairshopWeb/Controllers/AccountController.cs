using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Helpers;
using RepairshopWeb.Models;

namespace RepairshopWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;

        public AccountController(IUserHelper userHelper, IConfiguration configuration)
        {
            _userHelper = userHelper;
            _configuration = configuration;
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

                if (result.Succeeded) 
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    
                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login!");
            return View(model); 
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
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
                        UserName = model.Username
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);

                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn´t be created.");
                        return View(model); 
                    }

                    var loginViewModel = new LoginViewModel 
                    {
                        Password = model.Password,
                        RemenberMe = false,
                        Username = model.Username
                    };

                    var result2 = await _userHelper.LoginAsync(loginViewModel);

                    if (result2.Succeeded)
                        return RedirectToAction("Index", "Home");

                    ModelState.AddModelError(string.Empty, "The user couldn´t be logged.");
                }
            }
            return View(model);
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
                        ViewBag.UserMessage = "User updated!";

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

        //TOKEN

        //[HttpPost]
        //public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        //{
        //    if (this.ModelState.IsValid)
        //    {
        //        var user = await _userHelper.GetUserByEmailAsync(model.Username);
        //        if (user != null)
        //        {
        //            var result = await _userHelper.ValidatePasswordAsync(
        //                user,
        //                model.Password);

        //            if (result.Succeeded)
        //            {
        //                var claims = new[]
        //                {
        //                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        //                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //                };

        //                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
        //                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //                var token = new JwtSecurityToken(
        //                    _configuration["Tokens:Issuer"],
        //                    _configuration["Tokens:Audience"],
        //                    claims,
        //                    expires: DateTime.UtcNow.AddDays(90),
        //                    signingCredentials: credentials);

        //                var results = new
        //                {
        //                    token = new JwtSecurityTokenHandler().WriteToken(token),
        //                    expiration = token.ValidTo
        //                };

        //                return this.Created(string.Empty, result);
        //            }
        //        }
        //    }
        //    return BadRequest();
        //}

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
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                }
            }
            return View(model);
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        //{
        //    if (this.ModelState.IsValid)
        //    {
        //        var user = await _userHelper.GetUserByEmailAsync(model.Email);
        //        if (user == null)
        //        {
        //            ModelState.AddModelError(string.Empty, "The e-mail doesn´t correspont to a registered user.");
        //            return View(model);
        //        }
        //        var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

        //        var link = this.Url.Action(
        //            "ResetPassword",
        //            "Account",
        //            new { token = myToken}, protocol: HttpContext.Request.Scheme);

        //        Response response = _mailHelper.SendEmail(model.Email, "Repairshop Password Reset", $"<h1>Repairshop Password Reset</h1>" +
        //        $"To reset the password click in this link:</br></br>" +
        //        $"<a href = \"(link)\">Reset Password</a>");

        //        if (response.IsSuccess)
        //            this.ViewBag.Message = "The instructions to recover your password has been sent to your e-mail.";

        //        return this.View();
        //    }
   
        //}

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
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password reset successful.";
                    return this.View();
                }
                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }
            this.ViewBag.Message = "User not found";
            return View(model);
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
