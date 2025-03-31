using Clubmates.Web.Models;
using Clubmates.Web.Models.AccountViewModel;
using ClubMates.Web.Models.AccountViewModel;
using ClubMates.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Clubmates.Web.Controllers
{
    public class AccountController(UserManager<ClubMatesUser> userManager) : Controller
    {
        private readonly UserManager<ClubMatesUser> _userManager = userManager;
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            if (registerViewModel.Password != registerViewModel.ConfirmPassword)
            {
                ModelState.AddModelError("Password", "Password and Confirm Password do not match");
                return View(registerViewModel);
            }
            ClubMatesUser user = new()
            {
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email,
                Role =ClubMatesRole.SuperAdmin


            };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerViewModel);
            }
            else
            {
                var claims = new List<Claim>
                {
                    new (ClaimTypes.Name, registerViewModel.FullName),
                    new (ClaimTypes.Email, user.Email),
                    new (ClaimTypes.NameIdentifier, user.Id),
                    new (ClaimTypes.Role, "SuperAdmin")
                };
                var claimsResult = await _userManager.AddClaimsAsync(user, claims);
                if (!claimsResult.Succeeded)
                {
                    foreach (var error in claimsResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(registerViewModel);
                }
            }

            return View("Success");
        }
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string ReturnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                return View(loginViewModel);
            }
            var result = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt, Password is Incorrect");
                return View(loginViewModel);
            }
            else
            {
                var claims = user != null ? await _userManager.GetClaimsAsync(user) : null;
                if (claims != null)
                {
                    var scheme = IdentityConstants.ApplicationScheme;
                    var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, scheme));
                    var authenticationProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
                    };
                    await HttpContext.SignInAsync(scheme, principal, authenticationProperties);
                    return Redirect(ReturnUrl);
                }
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Redirect("/Home/Index");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}