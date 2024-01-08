using AutoMapper;
using DataLayer.ViewModels;
using EntityLayer.Entities;
using LibrarySite.Controllers;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services.UnitOfWork;

namespace LibrarySite.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class GroupController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GroupController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        #region Index
        public IActionResult Index( string inputsearch = "", int page = 1)
        {
            int paresh = (page - 1) * 5;
            ViewBag.Page = page;
            int totalCount = _unitOfWork.groupUW.Get(null,null,"Books").Count();
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

            return View(_unitOfWork.groupUW.Get(null, null, "Books").Skip(paresh).Take(5));
        }

        #endregion



        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(GroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var groupMap = _mapper.Map<Group>(model);
                _unitOfWork.groupUW.Create(groupMap);
                _unitOfWork.save();

                TempData[SuccessMessage] = "اطلاعات با موفقیت ثبت شد.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        #endregion


        #region Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("ErrorView", "Home");

            }
            var group = _unitOfWork.groupUW.GetById(id);
            var groupMap = _mapper.Map<GroupViewModel>(group);
            return View(groupMap);

        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(GroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var groupMap = _mapper.Map<Group>(model);
                _unitOfWork.groupUW.Update(groupMap);
                _unitOfWork.save();

                TempData[SuccessMessage] = "اطلاعات با موفقیت ویرایش شد";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        #endregion


        #region Delete

        [HttpGet]
        public ActionResult Delete(int groupId)
        {
            if (groupId == 0)
                return RedirectToAction("ErrorView", "Home");

            var group = _unitOfWork.groupUW.GetById(groupId);

            if (group == null)
                return RedirectToAction("ErrorView", "Home");

            return PartialView("_deleteGroup", group);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DeletePost(int groupId)
        {
            if (groupId == 0)
                return RedirectToAction("ErrorView", "Home");

            try
            {
                _unitOfWork.groupUW.DeleteById(groupId);
                _unitOfWork.save();

                TempData[SuccessMessage] = "حذف اطلاعات با موفقیت انجام شد";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorView", "Home");
            }

        }

        #endregion


        #region Search

        public IActionResult SearchGroup(string inputsearch = "")
        {
            ViewBag.inputsearch = inputsearch;

            var group = _unitOfWork.groupUW.Get();

            group=group.Where(x=>x.GroupName.Contains(inputsearch)).ToList();

            return View("Index",group);
        }
        #endregion
    }
}
