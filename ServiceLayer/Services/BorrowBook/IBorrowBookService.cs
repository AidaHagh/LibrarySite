using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.BorrowBook
{
    public interface IBorrowBookService :IDisposable
    {
        List<EntityLayer.Entities.BorrowBook> BorrowList();

    }
}
