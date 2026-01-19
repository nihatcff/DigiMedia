using System.Threading.Tasks;
using DigiMedia.Models;
using DigiMedia.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DigiMedia.Controllers;

public class AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<IdentityRole> _roleManager) : Controller
{
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        AppUser user = new()
        {
            Fullname = vm.Fullname,
            Email = vm.Email,
            UserName = vm.Username
        };

        var result = await _userManager.CreateAsync(user, vm.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(vm);
        }
        await _userManager.AddToRoleAsync(user, "Member");

        await _signInManager.SignInAsync(user,false);

        return RedirectToAction("Index","Home");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var user = await _userManager.FindByEmailAsync(vm.Email);
        if(user is null)
        {
            ModelState.AddModelError("","Password or email is wrong!");
            return View(vm);
        }

         var result = await _signInManager.PasswordSignInAsync(user, vm.Password, false, false);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Password or email is wrong!");
            return View(vm);
        }

        await _signInManager.SignInAsync(user,false);


        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    public async Task<IActionResult> CreateRoles()
    {
        await _roleManager.CreateAsync(new IdentityRole()
        {
            Name = "Admin"
        });
        await _roleManager.CreateAsync(new IdentityRole()
        {
            Name = "Member"
        });

        return Ok("Roles Created");
    }


}
