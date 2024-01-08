using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services.UnitOfWork;

namespace LibrarySite.Areas.UserArea.Controllers
{
    public class PaymentController : Controller
    {
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly IUnitOfWork _context;
        public PaymentController(UserManager<ApplicationUsers> userManager, IUnitOfWork context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Payment()
        {
            //نمایش اطلاعات کاربر
            var query_fullname = (from u in _userManager.Users where u.Id == _userManager.GetUserId(HttpContext.User) select u).SingleOrDefault();
            ViewBag.fullname = query_fullname.FirstName + " " + query_fullname.LastName;
            ViewBag.phone = query_fullname.UserName;
            ViewBag.Kifpul = query_fullname.Wallet;


            var query_Info = (from u in _context.userManagerUW.Get() where u.Id == _userManager.GetUserId(HttpContext.User) select u).SingleOrDefault();
            ViewBag.Email = query_Info.Email;
            ViewBag.Mobile = query_Info.PhoneNumber;
            return View();
        }

        [HttpPost]
        //این اکشن مربوط به قبل از ورود به درگاه پرداخت می باشد
        public async Task<IActionResult> Payment(PaymentTransaction PT)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            //دریافت اطلاعات پرداخت
            var getBasketInfo =
                _context.paymentUW.Get(o => o.UserId == user.Id).ToList();


            var payment = await new ZarinpalSandbox.Payment(Convert.ToInt32(PT.Amount))
                            .PaymentRequest("شارژ کیف پول",
                            Url.Action(nameof(PaymentVerify), "Payment", new
                            {
                                amount = Convert.ToInt32(Convert.ToInt32(PT.Amount)),
                                description = "شارژ کیف پول",
                                mobile = user.PhoneNumber,
                                email = user.Email,
                            }, Request.Scheme), user.Email, user.PhoneNumber);

            if (payment.Status == 100)
            {
                return Redirect(payment.Link);
            }
            else
            {
                return View("failedpay");
            }
        }

        public async Task<IActionResult> PaymentVerify(int amount, string description, string Email, string Mobile, string Authority, string Status)
        {
            if (Status == "NOK")
            {
                return RedirectToAction("failedpay");
            }
            //گرفتن تاییدیه پرداخت
            var verificationId = await new ZarinpalSandbox.Payment(amount).Verification(Authority);
            if (verificationId.Status != 100)
            {
                return RedirectToAction("failedpay");
            }

            //کد تراکنش
            //var RefId = verificationId.RefId;
            try
            {
                string getUserId = _userManager.GetUserId(HttpContext.User);
                //مرحله اول - ثبت اطلاعات تراکنش بانکی
                PaymentTransaction T = new PaymentTransaction
                {
                    Amount = amount,
                    Description = description,
                    TransactioDate = DateTime.Now,
                    TransactioTime = DateTime.Now,
                    Mobile = Mobile,
                    Email = Email,
                    UserId = getUserId,
                    TransactionNumber = verificationId.RefId.ToString()
                };
                _context.paymentUW.Create(T);

                var updateQuery = (from u in _context.userManagerUW.Get() where u.Id == _userManager.GetUserId(User) select u).SingleOrDefault();
                updateQuery.Wallet = updateQuery.Wallet + amount;

                _context.save();
            }
            catch (Exception)
            {
                return RedirectToAction("failedpay");
            }

            //ارسال اطلاعات از یک اکشن به اکشن دیگر
            TempData["totalamount"] = amount;
            TempData.Keep("totalamount");
            TempData["trnsactionNo"] = verificationId.RefId.ToString();
            TempData.Keep("trnsactionNo");

            return RedirectToAction("SuccessfullyPayment");
        }

        public IActionResult failedpay()
        {
            //نمایش اطلاعات کاربر
            var query_fullname = (from u in _userManager.Users where u.Id == _userManager.GetUserId(HttpContext.User) select u).SingleOrDefault();
            ViewBag.fullname = query_fullname.FirstName + " " + query_fullname.LastName;
            ViewBag.phone = query_fullname.UserName;
            ViewBag.Kifpul = query_fullname.Wallet;

            return View();
        }

        public IActionResult SuccessfullyPayment()
        {
            //نمایش اطلاعات کاربر
            var query_fullname = (from u in _userManager.Users where u.Id == _userManager.GetUserId(HttpContext.User) select u).SingleOrDefault();
            ViewBag.fullname = query_fullname.FirstName + " " + query_fullname.LastName;
            ViewBag.phone = query_fullname.UserName;
            ViewBag.Kifpul = query_fullname.Wallet;

            ViewBag.totalamount = TempData["totalamount"];
            ViewBag.transactionNo = TempData["trnsactionNo"];
            return View();
        }
    }
}
