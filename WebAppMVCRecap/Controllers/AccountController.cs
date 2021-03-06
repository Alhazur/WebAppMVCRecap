﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppMVCRecap.Models;

namespace WebAppMVCRecap.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(RoleManager<IdentityRole> roleManager,
                                 UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager)// Constructor Injection´s
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(LoginVM login)
        {
            if (ModelState.IsValid)
            {
                var SignInResultr = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, false, false);

                switch (SignInResultr.ToString())
                {
                    case "Succeeded":
                        return RedirectToAction("Index", "Home");

                    case "Failed":
                        ViewBag.msg = "Failed - Username of/and Password is incorrect";
                        break;
                    case "Lockedout":
                        ViewBag.msg = "Locked Out";
                        break;
                    default:
                        ViewBag.msg = SignInResultr.ToString();
                        break;
                }
            }

            return View(login);

            /**
             * Alternative way to use SignInResult
             * 
            if (SignInResultr.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (SignInResultr.IsLockedOut)
            {
                ViewBag.msg = "Locked Out";
            }
            else if (SignInResultr.RequiresTwoFactor)
            {
                ViewBag.msg = "Twofactor authorizon requierd";
            }
            else if (SignInResultr.IsNotAllowed)
            {
                ViewBag.msg = "Not allowed to login";
            }
            else//Failed
            {
                ViewBag.msg = "Failed - Username of/and Password is incorrect";
            }
            **/
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserVM createuser)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser()
                { UserName = createuser.UserName,
                    Email = createuser.Email
                };
                var result = await _userManager.CreateAsync(user, createuser.Password);

                if (result.Succeeded)
                {
                    ViewBag.msg = "User was created";
                    return RedirectToAction("CreateUser");
                }
                else
                {
                    ViewBag.errorlist = result.Errors;
                }
            }

            return View(createuser);
        }

        public IActionResult RoleList()
        {            
            return View(_roleManager.Roles.ToList());
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return View();
            }

            var resul = await _roleManager.CreateAsync(new IdentityRole(name));

            if (resul.Succeeded)
            {
                return RedirectToAction("RoleList");
            }


            return View();
        }

        [HttpGet]
        public IActionResult AddUserToRole(string role)
        {
            ViewBag.Role = role;
            return View(_userManager.Users.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> AddUserToRoleSave(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.AddToRoleAsync(user, role);            

            return RedirectToAction(nameof(RoleList));
        }
    }
}