using AutoMapper;
using DataLayer.ViewModels;
using EntityLayer.Entities;
using LibrarySite.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.Services.UnitOfWork;

namespace LibrarySite.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class UserManageController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUsers> _userManager;
        public UserManageController(IUnitOfWork unitOfWork, IMapper mapper,
            UserManager<ApplicationUsers> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }




        #region Index
        public IActionResult Index(byte searchTypeselected = 0, string inputsearch = "", int page = 1)
        {
            int paresh = (page - 1) * 5;
            ViewBag.Page = page;
            int totalCount = _unitOfWork.userManagerUW.Get().Count();
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


            return View(_unitOfWork.userManagerUW.Get().Skip(paresh).Take(5));
        }
        #endregion



        #region ActiveOrDeactiveUser

        [HttpGet]
        public IActionResult ActiveOrDeactiveUser(string userID, byte IsActive)
        {
            if (userID == null)
            {
                return RedirectToAction("ErrorView", "Home");
            }
            var user = _unitOfWork.userManagerUW.GetById(userID);
            if (user == null)
            {
                return RedirectToAction("ErrorView", "Home");
            }

            if (user.IsActive == true)
            {
                //DeActive
                ViewBag.theme = "firebrick";
                ViewBag.ViewTitle = "غیرفعال کردن کاربر";
                return PartialView("_activeordeactiveuser", user);
            }
            else
            {
                //Active
                ViewBag.theme = "green";
                ViewBag.ViewTitle = "فعال کردن کاربر";
                return PartialView("_activeordeactiveuser", user);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActiveOrDeactiveUserPost(string Id, bool IsActive)
        {
            if (Id == null)
            {
                return RedirectToAction("Error", "View");
            }
            else
            {
                try
                {
                    if (IsActive == true)
                    {
                        //DeActive
                        var user = _unitOfWork.userManagerUW.GetById(Id);
                        user.IsActive = false;
                        TempData[WarningMessage] = "حساب کاربر با موفقیت غیر فعال شد";
                        _unitOfWork.save();
                    }
                    else
                    {
                        //Active
                        var user = _unitOfWork.userManagerUW.GetById(Id);
                        user.IsActive = true;
                        TempData[SuccessMessage] = "حساب کاربر با موفقیت فعال شد";
                        _unitOfWork.save();
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "View");
                }
            }
        }

        #endregion



        #region UserDetail

        [HttpGet]
        public IActionResult UserDetail(string UserId)
        {
            var user = _unitOfWork.userManagerUW.GetById(UserId);

            return View(user);
        }

        #endregion



        #region Search

        public IActionResult SearchUser(string inputsearch ,byte searchTypeselected = 0 )
        {
            ViewBag.searchTypeselected = searchTypeselected;
            ViewBag.inputsearch = inputsearch;

            if (searchTypeselected != 0 && inputsearch != null)
            {
                var model = _unitOfWork.userManagerUW.Get();


                if (searchTypeselected == 1 && inputsearch != null)
                {
                    model = model.Where(x => x.FirstName.Contains(inputsearch)).ToList();
                }

                if (searchTypeselected == 2 && inputsearch != null)
                {
                    model = model.Where(x => x.LastName.Contains(inputsearch)).ToList();
                }

                if (searchTypeselected == 3 && inputsearch != null)
                {
                    model = model.Where(x => x.NationalCode.Contains(inputsearch)).ToList();
                }

                if (searchTypeselected == 4 && inputsearch != null)
                {
                    model = model.Where(x => x.Email.Contains(inputsearch)).ToList();
                }

                if (searchTypeselected == 5 && inputsearch != null)
                {
                    model = model.Where(x => x.UserName.Contains(inputsearch)).ToList();
                }

                return View("Index", model);
            }
            else
            {
                TempData[WarningMessage] = "ابتدا نوع جستجو را انتخاب نموده سپس عبارت مورد نظر را در باکس جستجو وارد کنید.";
                return RedirectToAction(nameof(Index));
            }



        }

        #endregion



        #region Edit

        [HttpGet]
        public IActionResult Edit(string userId)
        {
            if (userId == null)
            {
                return RedirectToAction("ErrorView", "Home");
            }

            var user = _unitOfWork.userManagerUW.GetById(userId);

            if (user == null)
            {
                return RedirectToAction("ErrorView", "Home");
            }
            var userMap = _mapper.Map<EditUserViewModel>(user);

            return View(userMap);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user =await _userManager.FindByIdAsync(model.Id);

                IdentityResult result = await _userManager.UpdateAsync(_mapper.Map(model, user));
                if (result.Succeeded)
                {
                    TempData[SuccessMessage] = "حساب کاربر با موفقیت ویرایش شد";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }


        #endregion

    }
}
