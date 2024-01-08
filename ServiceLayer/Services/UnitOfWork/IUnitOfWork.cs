using ServiceLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Entities;

namespace ServiceLayer.Services.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        GenericClass<ApplicationUsers> userManagerUW { get; }
        GenericClass<ApplicationRoles> roleManagerUW { get; }
        GenericClass<EntityLayer.Entities.Book> bookUW { get; }
        GenericClass<EntityLayer.Entities.Group> groupUW { get; }
        GenericClass<EntityLayer.Entities.BorrowBook> borrowBookUW { get; }
        GenericClass<EntityLayer.Entities.News> newsUW { get; }
        GenericClass<EntityLayer.Entities.Author> authorUW { get; }
        GenericClass<EntityLayer.Entities.PaymentTransaction> paymentUW { get; }

        void save();

    }
}
