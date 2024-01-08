using AutoMapper;
using DataLayer.ViewModels;
using EntityLayer.Entities;
using LibrarySite.Controllers;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services.UnitOfWork;

namespace LibrarySite.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class AuthorController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AuthorController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        #region Index
        public IActionResult Index(string inputsearch = "", int page = 1)
        {
            int paresh = (page - 1) * 5;
            ViewBag.Page = page;
            int totalCount = _unitOfWork.authorUW.Get(null, null, "Books").Count();
            double remain = totalCount % 5;
            if (remain == 0)
            {
                ViewBag.PageCount = totalCount / 5;
            }
            else
            {
                ViewBag.PageCount = (totalCount / 5) + 1;
            }

            ViewBag.inputsearch = inputsearch;

            return View(_unitOfWork.authorUW.Get(null, null, "Books").Skip(paresh).Take(5));
        }

        #endregion



        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(AuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var authorMap = _mapper.Map<Author>(model);
                _unitOfWork.authorUW.Create(authorMap);
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
            var author = _unitOfWork.authorUW.GetById(id);
            var authorMap = _mapper.Map<AuthorViewModel>(author);
            return View(authorMap);

        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(AuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var authorMap = _mapper.Map<Author>(model);
                _unitOfWork.authorUW.Update(authorMap);
                _unitOfWork.save();

                TempData[SuccessMessage] = "اطلاعات با موفقیت ویرایش شد";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        #endregion


        #region Delete

        [HttpGet]
        public ActionResult Delete(int authorId)
        {
            if (authorId == 0)
                return RedirectToAction("ErrorView", "Home");

            var author = _unitOfWork.authorUW.GetById(authorId);

            if (author == null)
                return RedirectToAction("ErrorView", "Home");

            return PartialView("_deleteAuthor", author);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DeletePost(int authorId)
        {
            if (authorId == 0)
                return RedirectToAction("ErrorView", "Home");

            try
            {
                _unitOfWork.authorUW.DeleteById(authorId);
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

        public IActionResult SearchAuthor(string inputsearch = "")
        {
            ViewBag.inputsearch = inputsearch;

            var author = _unitOfWork.authorUW.Get();
            if (inputsearch != null)
            {
				author = author.Where(x => x.AuthorName.Contains(inputsearch)).ToList();

				return View("Index", author);
			}
			TempData[WarningMessage] = "ابتدا عبارت مورد نظر را در باکس جستجو وارد کنید.";
			return RedirectToAction(nameof(Index));

		}
        #endregion
    }
}
