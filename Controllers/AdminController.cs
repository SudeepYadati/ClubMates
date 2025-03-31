using Clubmates.Web.Models.AccountViewModel;
using Clubmates.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using ClubMates.Web.Models.AccountViewModel;
using ClubMates.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clubmates.Web.Controllers
{
    [Authorize(Policy= "MustBeASuperAdmin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ClubMatesUser> _userManager;

        public AdminController(UserManager<ClubMatesUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ManageUsers()
        {
            var listOfUserAccounts = _userManager.Users
                .Where(x => x.Role != ClubMatesRole.SuperAdmin)
                .Select(x => new UserViewModel
                {
                    Email = x.Email,
                    Name = x.UserName,
                    Role = x.Role
                }).ToList();
            return View(listOfUserAccounts);
        }


        public async Task<IActionResult> EditUser(string email)
        {
            var accountUser = await _userManager.FindByEmailAsync(email);
            if (accountUser != null)
            {
                UserViewModel userViewModel = new UserViewModel
                {
                    Email = accountUser.Email ?? string.Empty,
                    Name = accountUser.UserName,
                    Role = accountUser.Role,
                    Roles = Enum.GetValues<ClubMatesRole>()
                    .Select(x => new SelectListItem
                    {
                        Text = Enum.GetName(x),
                        Value = x.ToString()
                    })
                };

                return View(userViewModel);
            }
            return NotFound();
        }


        public async Task<IActionResult> DeleteUser(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("User name cannot be null or empty.");
            }

            var accountUser = await _userManager.FindByNameAsync(name);
            if (accountUser != null)
            {
                var result = await _userManager.DeleteAsync(accountUser);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Error deleting user.");
            }
            return NotFound();
        }
    }
}