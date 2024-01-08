using AutoMapper;
using DataLayer.ViewModels;
using EntityLayer.Entities;
using LibrarySite.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services.UnitOfWork;

namespace LibrarySite.Areas.AdminArea.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly RoleManager<ApplicationRoles> _roleManager;
        public RoleController(IUnitOfWork unitOfWork, IMapper mapper,
            RoleManager<ApplicationRoles> roleManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _mapper = mapper;
        }


        public IActionResult Index()
        {
            return View();
        }


        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(RoleViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var roleMap = _mapper.Map<ApplicationRoles>(model);
        //        IdentityResult result = await _roleManager.CreateAsync(roleMap);
        //    }
        //    return View();
        //}

        #endregion

    }
}
