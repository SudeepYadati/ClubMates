using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clubmates.Web.Controllers
{
    [Authorize(Policy = "MustBeAGuest")]
    public class ClubsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}