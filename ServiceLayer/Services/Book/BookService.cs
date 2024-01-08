using DataLayer.DbContext;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Book
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        private readonly IServiceProvider _serviceProvider;

        public BookService(ApplicationDbContext context, IServiceProvider erviceProvider)
        { 
            _context = context; 
            _serviceProvider = erviceProvider;
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public List<EntityLayer.Entities.Book> BookList()
        {
            var query = (from b in _context.Books
                         select new EntityLayer.Entities.Book
                         {
                             BookId = b.BookId,
                             BookName = b.BookName,
                             BookDescription = b.BookDescription,
                             Group = b.Group,
                             Author = b.Author,
                             BookPageCount = b.BookPageCount,
                             BookImage = b.BookImage,
                             Price = b.Price,
                             BookStock = b.BookStock,
                             BookViews = b.BookViews,
                             BookLikeCount = b.BookLikeCount,
                         }).AsEnumerable();

            return query.ToList();
        }


        public List<EntityLayer.Entities.Book> BookViewCount(int BookId)
        {
            using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
            {
                var result = (from b in db.Books where b.BookId == BookId select b);
                var currentBook = result.FirstOrDefault();

                if (result.Count() != 0)
                {
                    currentBook.BookViews++;
                    db.Books.Attach(currentBook);
                    db.Entry(currentBook).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return null;
        }

        public List<EntityLayer.Entities.Book> BookContent()
        {
            var query = (from b in _context.Books
                         select new EntityLayer.Entities.Book
                         {
                             BookId = b.BookId,
                             BookName = b.BookName,
                             Price = b.Price,
                         }).ToList();

            return query.ToList();
        }
    }
}
