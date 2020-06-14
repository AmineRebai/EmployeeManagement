﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EmployeeManagement2_2.Models;
using EmployeeManagement2_2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement2_2.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    City = model.City
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if(signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Admin");
                    }
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginVM model = new LoginVM
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid ligin attempt");
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
                                new { ReturnUrl = returnUrl });
            var properties = signInManager
                .ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
public async Task<IActionResult>
            ExternalLoginCallback(string returnUrl = null, string remoteError = null)
{
    returnUrl = returnUrl ?? Url.Content("~/");

    LoginVM loginViewModel = new LoginVM
    {
        ReturnUrl = returnUrl,
        ExternalLogins =
                (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
    };

    if (remoteError != null)
    {
        ModelState
            .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

        return View("Login", loginViewModel);
    }

    // Get the login information about the user from the external login provider
    var info = await signInManager.GetExternalLoginInfoAsync();
    if (info == null)
    {
        ModelState
            .AddModelError(string.Empty, "Error loading external login information.");

        return View("Login", loginViewModel);
    }

    // If the user already has a login (i.e if there is a record in AspNetUserLogins
    // table) then sign-in the user with this external login provider
    var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider,
        info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

    if (signInResult.Succeeded)
    {
        return LocalRedirect(returnUrl);
    }
    // If there is no record in AspNetUserLogins table, the user may not have
    // a local account
    else
    {
        // Get the email claim value
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);

        if (email != null)
        {
            // Create a new user without password if we do not have a user already
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                };

                await userManager.CreateAsync(user);
            }

            // Add a login (i.e insert a row for the user in AspNetUserLogins table)
            await userManager.AddLoginAsync(user, info);
            await signInManager.SignInAsync(user, isPersistent: false);

            return LocalRedirect(returnUrl);
        }

        // If we cannot find the user email we cannot continue
        ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
        ViewBag.ErrorMessage = "Please contact support";

        return View("Error");
    }
}
    }
}
