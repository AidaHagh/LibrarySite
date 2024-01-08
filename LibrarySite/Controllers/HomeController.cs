using DataLayer.ViewModels;
using EntityLayer.Entities;
using LibrarySite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services.UnitOfWork;
using System.Diagnostics;

namespace LibrarySite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUsers> _userManager;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork,UserManager<ApplicationUsers> _userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }




        public async Task<IActionResult> Index()
        {
            var modelSite = new DaynamicSiteViewModel();

            //آخرین کتابها
            modelSite.LastBook = (from book in _unitOfWork.bookUW.Get() orderby book.BookId descending select book).Take(6).ToList();

            //آخرین اخبار
            modelSite.LastNews = (from news in _unitOfWork.newsUW.Get() orderby news.NewsId descending select news).Take(4).ToList();

            //user
            //var Uname = _unitOfWork.userManagerUW.Get().SingleOrDefault();
            //    if (Uname!=null)
            //{
            //    ViewBag.UserName = Uname;

            //}
            //تعداد کل کاربران فعال سایت
            var ActiveUsers = _unitOfWork.userManagerUW.Get().Where(u => u.IsActive);
            ViewBag.ActiveUsersCount = ActiveUsers.Count();

            //تعداد کل کتابها
            var Books = _unitOfWork.bookUW.Get();
            ViewBag.BooksCount = Books.Count();
            // تعداد کتابهای علمی
            var group = _unitOfWork.groupUW.Get().Where(x=>x.GroupName== "علمی").FirstOrDefault();
            var elmiId = group.GroupId;
            ViewBag.ElmiBooksCount = Books.Where(b => b.GroupId == elmiId).Count();

            // تعداد کتابهای روانشناسی
            var group2 = _unitOfWork.groupUW.Get().Where(x => x.GroupName == "روانشناسی").FirstOrDefault();
            var ravanId = group2.GroupId;
            ViewBag.RavanBooksCount = Books.Where(b => b.GroupId == ravanId).Count();

            return View(modelSite);
        }


        public IActionResult NewsDetail(int id)
        {
            return View(_unitOfWork.newsUW.GetById(id));
        }












        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}