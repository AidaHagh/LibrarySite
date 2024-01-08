using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services.ManageRequest;

namespace LibrarySite.Areas.UserArea.Controllers
{
    [Area("UserArea")]
    public class UserHomeController : Controller
    {
        private readonly IManageRequestService _requestService;
        private readonly UserManager<ApplicationUsers> _userManager;

        public UserHomeController(IManageRequestService requestService, UserManager<ApplicationUsers> userManager)
        {
            _requestService = requestService;
            _userManager = userManager;
        }
        public IActionResult Index(int page = 1)
        {
            int paresh = (page - 1) * 5;

            int totalCount = _requestService.GetUserManageRequest(_userManager.GetUserId(HttpContext.User)).Count();
            ViewBag.Page = page;
            double remain = totalCount % 5;
            if (remain == 0)
            {
                ViewBag.PageCount = totalCount / 5;
            }
            else
            {
                ViewBag.PageCount = (totalCount / 5) + 1;
            }

            var model = _requestService.GetUserManageRequest(_userManager.GetUserId(HttpContext.User)).Skip(paresh).Take(5);

            var query_fullname = (from u in _userManager.Users where u.Id == _userManager.GetUserId(HttpContext.User) select u).SingleOrDefault();
            ViewBag.fullname = query_fullname.FirstName + " " + query_fullname.LastName;
            ViewBag.phone = query_fullname.UserName;
            ViewBag.Wallet = query_fullname.Wallet;

            return View(model);
        }
    }
}
