using AutoMapper;
using DataLayer.ViewModels;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ServiceLayer.Services.UnitOfWork;

namespace LibrarySite.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<ApplicationRoles> _roleManager;
        private readonly SignInManager<ApplicationUsers> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AccountController(IUnitOfWork unitOfWork, IMapper mapper,
            UserManager<ApplicationUsers> userManager, RoleManager<ApplicationRoles> roleManager,
            SignInManager<ApplicationUsers> signInManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _roleManager = roleManager;
            _signInManager = signInManager;

        }


        #region Register

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, byte r1)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userPhone = await _userManager.FindByNameAsync(model.UserName);
                    if (userPhone != null)
                    {
                        ModelState.AddModelError("UserName", "این شماره تماس قبلا ثبت شده است. ");
                        return View(model);
                    }
                    var mapUser = _mapper.Map<ApplicationUsers>(model);
                    mapUser.PhoneNumber = model.UserName;
                    mapUser.Gender = r1;
                    mapUser.IsActive = false;
                    mapUser.UserType = 2;  //User
                    IdentityResult result = await _userManager.CreateAsync(mapUser, model.Password);

                    if (result.Succeeded)
                    {
                        if (model.UserName == "09387876638" && model.Email == "aida@gmail.com")
                        {
                            mapUser.IsActive = true;
                            mapUser.UserType = 1; //Admin
                            ApplicationRoles adminRole = new ApplicationRoles()
                            {
                                EnTitle = "Admin",
                                FaTitle = "مدیر",
                                Name = "Admin",
                            };
                          
                            IdentityResult resul = await _roleManager.CreateAsync(adminRole);
                            await _userManager.AddToRoleAsync(mapUser, "Admin");
                        }
                        else
                        {
                            var roleUser = _unitOfWork.roleManagerUW.Get().Where(x => x.EnTitle == "User").SingleOrDefault();
                            if (roleUser != null)
                            {
                                await _userManager.AddToRoleAsync(mapUser, "User");
                            }
                            else
                            {
                                ApplicationRoles roles = new ApplicationRoles()
                                {
                                    EnTitle = "User",
                                    FaTitle = "کاربر",
                                    Name = "User",
                                };
                                IdentityResult resulRole = await _roleManager.CreateAsync(roles);
                                await _userManager.AddToRoleAsync(mapUser, "User");

                            }
                        }

                        TempData[SuccessMessage] = "ثبت نام با موفقیت انجام شد";
                        return RedirectToAction("SuccesfullyRegister");
                    }
                    return RedirectToAction("Error", "Home");
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            return View();
        }



        [HttpGet]
        public IActionResult SuccesfullyRegister()
        {
            return View();
        }

        #endregion



        #region Login
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user =await _userManager.GetUserAsync(HttpContext.User);

                if (user.UserType == 1)
                {
                    //Admin
                    return Redirect("/AdminArea/Dashboard/Index");
                }
                else
                {
                    //User
                    return Redirect("/UserArea/UserHome/Index");
                }

            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("Password", "اطلاعات ورود صحیح نیست");
                    return View(model);
                }
                else
                {
                    if (user.IsActive == false)
                    {
                        //کنترل غیرفعال بودن اکانت کاربر
                        ModelState.AddModelError("Password", "این اکانت غیر فعال می باشد.");
                        return View(model);
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (user.UserType == 1)
                    {
                        //Admin
                        TempData[SuccessMessage] = user.FirstName + " " + user.LastName + " عزیز : " + "احراز هویت شما با موفقیت انجام شد";
                        return Redirect("/AdminArea/Dashboard/Index");
                    }
                    else if (user.UserType == 2)
                    {
                        //User
                        TempData[SuccessMessage] = user.FirstName + " " + user.LastName + " عزیز : " + "احراز هویت شما با موفقیت انجام شد";
                        return Redirect("/userArea/UserHome/Index");
                    }

                }
                else
                {
                    ModelState.AddModelError("Password", "اطلاعات ورود صحیح نیست");
                    return View(model);
                }

            }
            return View(model);
        }

        #endregion



        #region LogOut

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            //if (Request.Cookies["_brbook"] != null)
            //{
            //    Response.Cookies.Delete("_brbook");
            //}
            await _signInManager.SignOutAsync();
            return Redirect("/Account/Login");
        }
        #endregion
    }
}
