using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services.ManageRequest;

namespace LibrarySite.Areas.UserArea.Controllers
{
    [Area("UserArea")]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUsers> _userManager;

        public DashboardController( UserManager<ApplicationUsers> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var query_fullname = (from u in _userManager.Users where u.Id == _userManager.GetUserId(HttpContext.User) select u).SingleOrDefault();
            ViewBag.fullname = query_fullname.FirstName + " " + query_fullname.LastName;
            ViewBag.phone = query_fullname.UserName;
         
            return View();
        }
    }
}
