using DataLayer.DbContext;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.BorrowBook
{
    public class BorrowBookService : IBorrowBookService
    {
        private readonly ApplicationDbContext _context;
        public BorrowBookService(ApplicationDbContext context)
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

        public List<EntityLayer.Entities.BorrowBook> BorrowList()
        {
            var query = (from b in _context.BorrowBooks
                         select new EntityLayer.Entities.BorrowBook
                         {
                             BookId = b.BookId,
                             UserId = b.UserId,
                             Status = b.Status,

                         }).AsEnumerable();

            return query.ToList();
        }



    }
}
