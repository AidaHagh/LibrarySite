using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Book
{
    public interface IBookService : IDisposable
    {
        List<EntityLayer.Entities.Book> BookList();
        List<EntityLayer.Entities.Book> BookViewCount(int BookId);
        List<EntityLayer.Entities.Book> BookContent();
    }
}
