using System.Security.Claims;
using Delivery.Models; // Namespace where your models are stored
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _context.Uzytkownicy
                        .FirstOrDefaultAsync(u => u.Login == model.Login && u.Haslo == model.Haslo);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Role, user.Rola)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "UserCookie");
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync("UserCookie", new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home"); // Redirect to a secure page
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return View(model);
    }

    // GET: /Account/Logout
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        // Sign out the user by clearing their authentication cookies
        await HttpContext.SignOutAsync("UserCookie");

        // Redirect the user to the home page or any other page after logging out
        return RedirectToAction("Index", "Home");
    }
    // POST: /Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> LogoutConfirmed()
    {
        // Sign out the user
        await HttpContext.SignOutAsync("UserCookie");

        // Redirect to home page after logging out
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }


}
