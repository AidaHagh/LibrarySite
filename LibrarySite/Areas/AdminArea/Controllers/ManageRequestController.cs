using AutoMapper;
using DataLayer.ViewModels;
using LibrarySite.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Services.ManageRequest;
using ServiceLayer.Services.UnitOfWork;

namespace LibrarySite.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class ManageRequestController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IManageRequestService _requestService;
        private readonly IMapper _mapper;
        public ManageRequestController(IUnitOfWork unitOfWork, IMapper mapper,
            IManageRequestService requestService)
        {
            _requestService = requestService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        #region Index
        public IActionResult Index(int page = 1)
        {
            int paresh = (page - 1) * 5;

            int totalCount = _requestService.GetManageRequest().Count();
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

            var model = _requestService.GetManageRequest().Skip(paresh).Take(5);

            return View(model);
        }

        #endregion



        #region RejectRquest

        [HttpGet]
        public IActionResult RejectRquest(int Id)
        {
            var model = _requestService.GetRejectRequest(Id);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.PartialTyp = 1;
            ViewBag.rejectId = Id;
            return PartialView("_manageRequest", model);
        }

        [HttpPost]
        public IActionResult RejectRquestPost(ManageRequestViewModel model)
        {
            if (model == null)
            {
                return RedirectToAction("Error", "View");
            }

            _requestService.PostRequestReject(model);

            TempData[WarningMessage] = "رد درخواست انجام شد";
            return RedirectToAction("Index", model);
        }

        #endregion


        #region AcceptRquest

        [HttpGet]
        public IActionResult AcceptRquest(int Id)
        {
            var model = _requestService.GetAcceptRequest(Id);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.PartialTyp = 2;
            ViewBag.acceptmyId = Id;
            return PartialView("_manageRequest", model);
        }

        [HttpPost]
        public IActionResult AcceptRquestPost(ManageRequestViewModel model)
        {
            if (model == null)
            {
                return RedirectToAction("Error", "View");
            }

            _requestService.PostAcceptRequest(model);

            TempData[SuccessMessage] = "پذیرفتن درخواست انجام شد";
            return RedirectToAction("Index", model);
        }

        #endregion


        #region GetBackBook

        [HttpGet]
        public IActionResult GetBackBook(int Id)
        {
            var model = _requestService.GetBackBook(Id);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.PartialTyp = 3;
            ViewBag.backId = Id;
            return PartialView("_manageRequest", model);
        }

        [HttpPost]
        public IActionResult BackBookPost(ManageRequestViewModel model)
        {
            if (model == null)
            {
                return RedirectToAction("Error", "View");
            }

            _requestService.PostBackBook(model);

            TempData[InfoMessage] = "کتاب بازگردانده شد";
            return RedirectToAction("Index", model);
        }
        #endregion
    }
}
