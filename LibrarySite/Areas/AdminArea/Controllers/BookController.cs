using AutoMapper;
using CommonLayer.GenericClass;
using DataLayer.ViewModels;
using EntityLayer.Entities;
using LibrarySite.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Services.Book;
using ServiceLayer.Services.BorrowBook;
using ServiceLayer.Services.UnitOfWork;

namespace LibrarySite.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class BookController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly IUploadFiles _upload;
        private readonly IBookService _bookService;
        private readonly IBorrowBookService _borrowService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        public BookController(IUnitOfWork unitOfWork, IBookService bookService,
            IMapper mapper, IUploadFiles upload, IWebHostEnvironment webHostEnvironment,
            IBorrowBookService borrowService, UserManager<ApplicationUsers> userManager)
        {
            _unitOfWork = unitOfWork;
            _bookService = bookService;
            _upload = upload;
            _mapper = mapper;
            _borrowService = borrowService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }


        #region Index
        public IActionResult Index(byte searchTypeselected = 0, string inputsearch = "", int page = 1)
        {
            int paresh = (page - 1) * 5;
            ViewBag.Page = page;
            int totalCount = _unitOfWork.bookUW.Get().Count();
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

            ViewBag.Group = new SelectList(_unitOfWork.groupUW.Get(), "GroupId", "GroupName");
            ViewBag.Author = new SelectList(_unitOfWork.authorUW.Get(), "AuthorId", "AuthorName");

            return View(_unitOfWork.bookUW.Get().Skip(paresh).Take(5));
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
            ViewBag.Group = new SelectList(_unitOfWork.groupUW.Get(), "GroupId", "GroupName");
            ViewBag.Author = new SelectList(_unitOfWork.authorUW.Get(), "AuthorId", "AuthorName");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(BookViewModel model, string newImagePathName)
        {
            ViewBag.Group = new SelectList(_unitOfWork.groupUW.Get(), "GroupId", "GroupName");
            ViewBag.Author = new SelectList(_unitOfWork.authorUW.Get(), "AuthorId", "AuthorName");

            if (ModelState.IsValid)
            {
                var bookMap = _mapper.Map<Book>(model);
                bookMap.BookImage = newImagePathName;
                _unitOfWork.bookUW.Create(bookMap);
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
            ViewBag.Group = new SelectList(_unitOfWork.groupUW.Get(), "GroupId", "GroupName");
            ViewBag.Author = new SelectList(_unitOfWork.authorUW.Get(), "AuthorId", "AuthorName");

            if (id == 0)
            {
                TempData[ErrorMessage] = "عملیات با خطا مواجه شد";
                return RedirectToAction(nameof(Index));
            }
            var book = _unitOfWork.bookUW.GetById(id);
            var bookMap = _mapper.Map<BookViewModel>(book);
            return View(bookMap);

        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(BookViewModel model, string newImagePathName)
        {
            ViewBag.Group = new SelectList(_unitOfWork.groupUW.Get(), "GroupId", "GroupName");
            ViewBag.Author = new SelectList(_unitOfWork.authorUW.Get(), "AuthorId", "AuthorName");

            if (ModelState.IsValid)
            {
                model.BookImage = newImagePathName;
                var bookMap = _mapper.Map<Book>(model);
                _unitOfWork.bookUW.Update(bookMap);
                _unitOfWork.save();

                TempData[SuccessMessage] = "اطلاعات با موفقیت ویرایش شد";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        #endregion


        #region Delete

        [HttpGet]
        public ActionResult Delete(int bookId)
        {
            if (bookId == 0)
                return RedirectToAction("ErrorView", "Home");

            var book = _unitOfWork.bookUW.GetById(bookId);

            if (book == null)
                return RedirectToAction("ErrorView", "Home");

            return PartialView("_deleteBook", book);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DeletePost(int bookId)
        {
            if (bookId == 0)
            {
                TempData[ErrorMessage] = "عملیات با خطا مواجه شد";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                //برای حذف تصویر از روت سایت
                var deleteImg = _unitOfWork.bookUW.GetById(bookId);
                var pathImageName = Path.Combine(_webHostEnvironment.WebRootPath, "upload\\book\\") + deleteImg.BookImage;
                if (System.IO.File.Exists(pathImageName))
                {
                    System.IO.File.Delete(pathImageName);
                }

                _unitOfWork.bookUW.DeleteById(bookId);
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

        public IActionResult SearchBook(string inputsearch, byte searchTypeselected = 0)
        {
            ViewBag.searchTypeselected = searchTypeselected;
            ViewBag.inputsearch = inputsearch;

            ViewBag.Group = _unitOfWork.groupUW.Get();
            ViewBag.Author = _unitOfWork.authorUW.Get();

            if (searchTypeselected != 0 && inputsearch != null)
            {
                var model = _unitOfWork.bookUW.Get();


                if (searchTypeselected == 1 && inputsearch != null)
                {
                    model = model.Where(x => x.BookName.Contains(inputsearch)).ToList();
                }

                if (searchTypeselected == 2 && inputsearch != null)
                {
                    model = model.Where(x => x.Group.GroupName.Contains(inputsearch)).ToList();
                }

                if (searchTypeselected == 3 && inputsearch != null)
                {
                    model = model.Where(x => x.Author.AuthorName.Contains(inputsearch)).ToList();
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



        [HttpGet]
        [AllowAnonymous]
        public IActionResult BookDetails(int BookId)
        {
            ViewBag.Group = _unitOfWork.groupUW.Get();
            ViewBag.Author = _unitOfWork.authorUW.Get();

            var model = _unitOfWork.bookUW.GetById(BookId);

            //برای بازدید

            _bookService.BookViewCount(BookId);

            return View(model);
        }


        #region LikeDisLike

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Like(int Bookid)
        {
            var query = _bookService.BookList().Where(b => b.BookId == Bookid).FirstOrDefault();
            if (query == null)
            {
                // برمیگردونه به آخرین اکشنی که بودی
                return Redirect(Request.Headers["Referer"].ToString());
            }
            if (Request.Cookies["iscookiLike"] == null)
            {
                Response.Cookies.Append("iscookiLike", "," + Bookid + ",",
                new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.Now.AddYears(5) });

                query.BookLikeCount++;
                _unitOfWork.bookUW.Update(query);
                _unitOfWork.save();

                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                string cookieContent = Request.Cookies["iscookiLike"].ToString();
                if (cookieContent.Contains("," + Bookid + ","))
                {
                    return Redirect(Request.Headers["Referer"].ToString());
                }
                else
                {
                    cookieContent += "," + Bookid + ",";
                    Response.Cookies.Append("iscookiLike", cookieContent,
                    new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.Now.AddYears(5) });

                    query.BookLikeCount++;
                    _unitOfWork.bookUW.Update(query);
                    _unitOfWork.save();

                    return Redirect(Request.Headers["Referer"].ToString());

                }
            }

        }

        [AllowAnonymous]
        public async Task<IActionResult> DisLike(int Bookid)
        {
            var query = _bookService.BookList().Where(b => b.BookId == Bookid).FirstOrDefault();
            if (query == null)
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }

            if (Request.Cookies["iscookidisLike"] == null)
            {
                Response.Cookies.Append("iscookidisLike", "," + Bookid + ",",
                    new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.Now.AddYears(5) });
                query.BookLikeCount--;
                _unitOfWork.bookUW.Update(query);
                _unitOfWork.save();

                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                string cookieContent = Request.Cookies["iscookidisLike"].ToString();
                if (cookieContent.Contains("," + Bookid + ","))
                {
                    return Redirect(Request.Headers["Referer"].ToString());
                }
                else
                {
                    cookieContent += "," + Bookid + ",";
                    Response.Cookies.Append("iscookidisLike", cookieContent,
                        new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.Now.AddYears(5) });

                    query.BookLikeCount--;
                    _unitOfWork.bookUW.Update(query);
                    _unitOfWork.save();

                    return Redirect(Request.Headers["Referer"].ToString());
                }
            }
        }

        #endregion


        #region Borrow

        [AllowAnonymous]
        public IActionResult Borrow(int Id)
        {
            //کنترل اینکه آیا ای دی ارسال شده وجود دارد یا خیر
            var query = _bookService.BookList().Where(b => b.BookId == Id).FirstOrDefault();
            if (query == null)
            {
                return Json(new { status = "fail", message = "این کتاب در کتابخانه موجود نیست" });
            }

            //اگر کتاب موجود نبود
            if (query.BookStock == 0)
            {
                return Json(new { status = "success", message = "این کتاب موجود نیست" });
            }
            else
            {
                //اگر موجود بود
                if (Request.Cookies["_brbook"] == null)
                {
                    Response.Cookies.Append("_brbook", "," + Id + ",", new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.Now.AddMinutes(30) });

                    return Json(new { status = "success", message = "کتاب به لیست درخواستی شما افزوده شد", sabadcount = 1 });
                }
                else
                {
                    string cookeieContent = Request.Cookies["_brbook"].ToString();
                    if (cookeieContent.Contains("," + Id + ","))
                    {
                        return Json(new { status = "success", message = "این کتاب قبلا در لیست درخواستی شما وجود دارد" });

                    }
                    else
                    {
                        cookeieContent += "," + Id + ",";
                        Response.Cookies.Append("_brbook", cookeieContent, new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.Now.AddMinutes(30) });

                        String[] requestbookCount = cookeieContent.Split(',');
                        requestbookCount = requestbookCount.Where(r => r != "").ToArray();

                        return Json(new { status = "success", message = "کتاب به لیست درخواستی شما افزوده شد", sabadcount = requestbookCount.Count() });

                    }
                }
            }

        }

        #endregion


        #region RequestedBook(ChooseBook)
        [AllowAnonymous]
        public IActionResult RequestedBook()
        {
            try
            {
                string cookieContent = Request.Cookies["_brbook"].ToString();

                String[] bookIdRequested = cookieContent.Split(',');
                bookIdRequested = bookIdRequested.Where(b => b != "").ToArray();

                if (Request.Cookies["_brbook"] != null)
                {
                    String[] requestedBook = cookieContent.Split(',');
                    requestedBook = requestedBook.Where(r => r != "").ToArray();

                    var query = (from b in _bookService.BookList()
                                 where requestedBook.Contains(b.BookId.ToString())
                                 select new Book
                                 {
                                     BookId = b.BookId,
                                     BookName = b.BookName,
                                     Price = b.Price

                                 }).ToList();

                    return View(query);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {

                return View();
            }

        }
        #endregion



        #region DeleterequestedBook
        [AllowAnonymous]
        [Authorize(Roles = "User")]
        public IActionResult DeleterequestedBook(int BookId)
        {
            string cookieContent = Request.Cookies["_brbook"].ToString();
            String[] bookIdRequestied = cookieContent.Split(',');
            bookIdRequestied = bookIdRequestied.Where(r => r != "").ToArray();

            //اضافه کردن یک آرایه به لیست
            List<string> idList = new List<string>(bookIdRequestied);//یک لیست جنریک که محتویات آرایه بالا را میگیرد
            idList.Remove(BookId.ToString());//پس میتوان ای دی را توسط یک لیست حذف کرد

            cookieContent = ""; //خالی کن - محتوایات لیست ای دی است
            for (int i = 0; i < idList.Count(); i++)
            {
                cookieContent += "," + idList[i] + ",";//مقدار کوکی را دوباره میسازیم با محتویات جدید
            }

            Response.Cookies.Append("_brbook", cookieContent, new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.Now.AddMinutes(30) });
            //کوکی با شرایط جدید ایجاد شد

            //عملا  صفحه یکبار رفرش میشود
            if (Request.Cookies["_brbook"] != null)
            {
                String[] requestedBook = cookieContent.Split(',');
                requestedBook = requestedBook.Where(r => r != "").ToArray();

                var query = (from b in _bookService.BookList()
                             where requestedBook.Contains(b.BookId.ToString())
                             select new Book
                             {
                                 BookId = b.BookId,
                                 BookName = b.BookName,
                                 Price = b.Price

                             }).ToList();

                return View("requestedbook", query);
            }
            else
            {
                return View();
            }

        }
        #endregion



        #region BookrequestFinally
        [AllowAnonymous]
        [Authorize(Roles = "User")]
        public IActionResult BookrequestFinally(string userid, string tp)
        {
            string cookieContent = Request.Cookies["_brbook"].ToString();
            String[] bookIdRequested = cookieContent.Split(',');
            bookIdRequested = bookIdRequested.Where(b => b != "").ToArray();

            //کنترل اینکه کاربر مجددا کتابی سفارش نده
            if (Request.Cookies["_brbook"] != null)
            {
                String[] requestedBook = cookieContent.Split(',');
                requestedBook = requestedBook.Where(r => r != "").ToArray();

                var query = (from b in _borrowService.BorrowList()
                             where requestedBook.Contains(b.BookId.ToString())
                             && b.UserId == userid
                             && b.Status == 1
                             select b).ToList();

                if (query.Count > 0)
                {
                    return Json(new { status = "success", message = "لیست درخواستی شما شامل کتابهایی می باشد که قبلا سفارش داده اید" });
                }
            }

            //کنترل موجودی کیف پول کاربر
            var kifpulQuery = (from u in _unitOfWork.userManagerUW.Get() where u.Id == _userManager.GetUserId(User) select u).SingleOrDefault();
            if (kifpulQuery.Wallet < Convert.ToInt32(tp))
            {
                return Json(new { status = "fail", message = "موجودی کیف پول شما کافی نمی باشد" });


            }
            else
            {
                for (int i = 0; i < bookIdRequested.Count(); i++)
                {
                    var bookPrice = (from b in _unitOfWork.bookUW.Get() where b.BookId == Convert.ToInt32(bookIdRequested[i]) select b);
                    var result1 = bookPrice.SingleOrDefault();

                    BorrowBook br = new BorrowBook
                    {
                        BookId = Convert.ToInt32(bookIdRequested[i]),
                        UserId = userid,
                        RequestDate = DateTime.Now,
                        Status = 1,
                        Price = result1.Price
                    };
                    kifpulQuery.Wallet = kifpulQuery.Wallet - Convert.ToInt32(tp);

                    _unitOfWork.borrowBookUW.Create(br);
                    _unitOfWork.save();

                }
            }
            return Json(new { status = "success", message = "کتابهای درخواستی شما ثبت شد" });

        }
        #endregion



    }
}
