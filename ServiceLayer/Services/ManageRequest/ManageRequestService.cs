using DataLayer.DbContext;
using DataLayer.ViewModels;

namespace ServiceLayer.Services.ManageRequest
{
    public class ManageRequestService : IManageRequestService
    {
        private readonly ApplicationDbContext _context;
        public ManageRequestService(ApplicationDbContext context)
        {
            _context = context;
        }


        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }


        public List<ManageRequestViewModel> GetManageRequest()
        {
            var query = (from br in _context.BorrowBooks
                         join b in _context.Books on br.BookId equals b.BookId
                         join u in _context.Users on br.UserId equals u.Id

                         select new ManageRequestViewModel
                         {

                             Id = br.Id,
                             BookId = br.BookId,
                             UserId = br.UserId,
                             UserFullName = u.FirstName + " " + u.LastName,
                             BookStock = b.BookStock,
                             BookName = b.BookName,
                             Status = br.Status,
                             RequestDate = br.RequestDate,
                             AnswereDate = br.AnswereDate,
                             GiveBackDate = br.GiveBackDate,
                             Price = br.Price,
                             StatusDescription =
                             (
                                 br.Status == 1 ? "درخواست امانت" :
                                 br.Status == 2 ? "به امانت برده" :
                                 br.Status == 3 ? "رد درخواست" :
                                 br.Status == 4 ? "برگردانده" : "نامشخص"
                             )

                         }).AsEnumerable();

            return query.ToList();
        }

        // برای پنل کاربر
        public List<ManageRequestViewModel> GetUserManageRequest(string userId)
        {
            var query = (from br in _context.BorrowBooks
                         join b in _context.Books on br.BookId equals b.BookId
                         join u in _context.Users on br.UserId equals u.Id

                         where u.Id == userId

                         select new ManageRequestViewModel
                         {

                             Id = br.Id,
                             BookId = br.BookId,
                             UserId = br.UserId,
                             UserFullName = u.FirstName + " " + u.LastName,
                             BookStock = b.BookStock,
                             BookName = b.BookName,
                             Status = br.Status,
                             RequestDate = br.RequestDate,
                             AnswereDate = br.AnswereDate,
                             GiveBackDate = br.GiveBackDate,
                             Price = br.Price,
                             StatusDescription =
                             (
                                 br.Status == 1 ? "درخواست امانت" :
                                 br.Status == 2 ? "به امانت برده" :
                                 br.Status == 3 ? "رد درخواست" :
                                 br.Status == 4 ? "برگردانده" : "نامشخص"
                             )

                         }).AsEnumerable();

            return query.ToList();
        }

        public List<ManageRequestViewModel> GetRejectRequest(int Id)
        {
            var query = (from br in _context.BorrowBooks
                         join b in _context.Books on br.BookId equals b.BookId
                         join u in _context.Users on br.UserId equals u.Id

                         where br.Id == Id

                         select new ManageRequestViewModel
                         {
                             UserFullName = u.FirstName + " " + u.LastName,
                             BookName = b.BookName,
                         }).AsEnumerable();

            return query.ToList();
        }


        public void PostRequestReject(ManageRequestViewModel model)
        {
            var query = (from br in _context.BorrowBooks where br.Id == model.Id select br);
            var result = query.SingleOrDefault();

            if (query.Count() != 0)
            {
                result.Status = 3;
                result.AnswereDate = DateTime.Now;

                _context.BorrowBooks.Attach(result);
                _context.Entry(result).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                //برگشت مبلغ به کیف پول کاربر
                var RejectPul = (from u in _context.Users where u.Id == result.UserId select u).SingleOrDefault();
                RejectPul.Wallet = RejectPul.Wallet + result.Price;

                _context.SaveChanges();
            }

        }


        public List<ManageRequestViewModel> GetAcceptRequest(int Id)
        {
            var query = (from br in _context.BorrowBooks
                         join b in _context.Books on br.BookId equals b.BookId
                         join u in _context.Users on br.UserId equals u.Id

                         where br.Id == Id

                         select new ManageRequestViewModel
                         {
                             UserFullName = u.FirstName + " " + u.LastName,
                             BookName = b.BookName,
                         }).AsEnumerable();

            return query.ToList();
        }


        public void PostAcceptRequest(ManageRequestViewModel model)
        {
            var query = (from br in _context.BorrowBooks where br.Id == model.Id select br);
            var result = query.SingleOrDefault();

            //کسر از موجودی کتاب
            var findbookquery = (from b in _context.Books where b.BookId == result.BookId select b);
            var resultbook = findbookquery.SingleOrDefault();
            if (findbookquery.Count() != 0)
            {
                resultbook.BookStock--;
                _context.Books.Attach(resultbook);
                _context.Entry(result).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

            if (query.Count() != 0)
            {
                result.Status = 2;
                result.AnswereDate = DateTime.Now;

                _context.BorrowBooks.Attach(result);
                _context.Entry(result).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }

        }


        public List<ManageRequestViewModel> GetBackBook(int Id)
        {
            var query = (from br in _context.BorrowBooks
                         join b in _context.Books on br.BookId equals b.BookId
                         join u in _context.Users on br.UserId equals u.Id

                         where br.Id == Id

                         select new ManageRequestViewModel
                         {
                             UserFullName = u.FirstName + " " + u.LastName,
                             BookName = b.BookName,
                         }).AsEnumerable();

            return query.ToList();
        }

        public void PostBackBook(ManageRequestViewModel model)
        {
            var query = (from br in _context.BorrowBooks where br.Id == model.Id select br);
            var result = query.SingleOrDefault();

            //افزودن به موجودی کتاب
            var findbookquery = (from b in _context.Books where b.BookId == result.BookId select b);
            var resultbook = findbookquery.SingleOrDefault();
            if (findbookquery.Count() != 0)
            {
                resultbook.BookStock++;
                _context.Books.Attach(resultbook);
                _context.Entry(result).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

            if (query.Count() != 0)
            {
                result.Status = 4;
                result.GiveBackDate = DateTime.Now;

                _context.BorrowBooks.Attach(result);
                _context.Entry(result).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }

        }
    }
}
