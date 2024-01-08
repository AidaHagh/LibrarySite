using AutoMapper;
using CommonLayer.GenericClass;
using DataLayer.ViewModels;
using EntityLayer.Entities;
using LibrarySite.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceLayer.Services.News;
using ServiceLayer.Services.UnitOfWork;

namespace LibrarySite.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class NewsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUploadFiles _upload;
        private readonly INewsService _newsService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        public NewsController(IUnitOfWork unitOfWork, INewsService newsService,
            IMapper mapper, IUploadFiles upload, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _newsService = newsService;
            _upload = upload;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }


        #region Index
        public IActionResult Index(byte searchTypeselected = 0, string inputsearch = "", int page = 1)
        {
            int paresh = (page - 1) * 5;
            ViewBag.Page = page;
            int totalCount = _unitOfWork.newsUW.Get().Count();
            double remain = totalCount % 5;
            if (remain == 0)
            {
                ViewBag.PageCount = totalCount / 5;
            }
            else
            {
                ViewBag.PageCount = (totalCount / 5) + 1;
            }

            ViewBag.searchTypeselected = searchTypeselected;
            ViewBag.inputsearch = inputsearch;

            return View(_unitOfWork.newsUW.Get().Skip(paresh).Take(5));
        }

        #endregion


        #region UploadImage
        public IActionResult UploadImageFile(IEnumerable<IFormFile> filearray, string path)
        {
            string filename = _upload.UploadFileFunc(filearray, path);
            return Json(new { status = "success", imagename = filename });
        }
        #endregion


        #region Create

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(NewsViewModel model, string newImagePathName)
        {

            if (ModelState.IsValid)
            {
                model.NewsDate = DateTime.Now;
                var newsMap = _mapper.Map<News>(model);
                newsMap.NewsAttachment = newImagePathName;
                _unitOfWork.newsUW.Create(newsMap);
                _unitOfWork.save();

                TempData[SuccessMessage] = "اطلاعات با موفقیت ثبت شد.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        #endregion


        #region Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {

            if (id == 0)
            {
                TempData[ErrorMessage] = "عملیات با خطا مواجه شد";
                return RedirectToAction(nameof(Index));
            }
            var news = _unitOfWork.newsUW.GetById(id);
            var newsMap = _mapper.Map<NewsViewModel>(news);
            return View(newsMap);

        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(NewsViewModel model, string newImagePathName)
        {

            if (ModelState.IsValid)
            {
                model.NewsDate = DateTime.Now;
                model.NewsAttachment = newImagePathName;
                var newsMap = _mapper.Map<News>(model);
                _unitOfWork.newsUW.Update(newsMap);
                _unitOfWork.save();

                TempData[SuccessMessage] = "اطلاعات با موفقیت ویرایش شد";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        #endregion


        #region Delete

        [HttpGet]
        public ActionResult Delete(int newsId)
        {
            if (newsId == 0)
                return RedirectToAction("ErrorView", "Home");

            var news = _unitOfWork.newsUW.GetById(newsId);

            if (news == null)
                return RedirectToAction("ErrorView", "Home");

            return PartialView("_deleteNews", news);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DeletePost(int newsId)
        {
            if (newsId == 0)
            {
                TempData[ErrorMessage] = "عملیات با خطا مواجه شد";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                //برای حذف تصویر از روت سایت
                var deleteImg = _unitOfWork.newsUW.GetById(newsId);
                var pathImageName = Path.Combine(_webHostEnvironment.WebRootPath, "upload\\news\\") + deleteImg.NewsAttachment;
                if (System.IO.File.Exists(pathImageName))
                {
                    System.IO.File.Delete(pathImageName);
                }

                _unitOfWork.newsUW.DeleteById(newsId);
                _unitOfWork.save();

                TempData[SuccessMessage] = "حذف اطلاعات با موفقیت انجام شد";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "عملیات با خطا مواجه شد";
                return RedirectToAction(nameof(Index));
            }

        }

        #endregion



        #region Search

        public IActionResult SearchNews(string inputsearch)
        {
            ViewBag.inputsearch = inputsearch;


            if ( inputsearch != null)
            {
                var model = _unitOfWork.newsUW.Get();

                model = model.Where(x => x.NewsTitle.Contains(inputsearch)).ToList();

                return View("Index", model);
            }
            else
            {
                TempData[WarningMessage] = "لطفا عبارت مورد نظر را در باکس جستجو وارد کنید.";
                return RedirectToAction(nameof(Index));
            }



        }

        #endregion




    }
}
