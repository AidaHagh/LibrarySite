using DataLayer.DbContext;
using EntityLayer.Entities;
using ServiceLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }


        private GenericClass<ApplicationUsers> _userManager;
        private GenericClass<ApplicationRoles> _roleManager;
        private GenericClass<EntityLayer.Entities.Group> _group;
        private GenericClass<EntityLayer.Entities.Author> _author;
        private GenericClass<EntityLayer.Entities.Book> _book;
        private GenericClass<EntityLayer.Entities.News> _news;
        private GenericClass<EntityLayer.Entities.BorrowBook> _borrowBook;
        private GenericClass<EntityLayer.Entities.PaymentTransaction> _payment;



        //کاربران
        public GenericClass<ApplicationUsers> userManagerUW
        {
            //فقط خواندنی    
            get
            {
                if (_userManager == null)
                {
                    _userManager = new GenericClass<ApplicationUsers>(_context);
                }
                return _userManager;
            }
        }


        //نقش ها
        public GenericClass<ApplicationRoles> roleManagerUW
        {
            //فقط خواندنی    
            get
            {
                if (_roleManager == null)
                {
                    _roleManager = new GenericClass<ApplicationRoles>(_context);
                }
                return _roleManager;
            }
        }


        //گروه بندی
        public GenericClass<Group> groupUW
        {
            //فقط خواندنی    
            get
            {
                if (_group == null)
                {
                    _group = new GenericClass<Group>(_context);
                }
                return _group;
            }
        }


        //نویسنده
        public GenericClass<Author> authorUW
        {
            //فقط خواندنی    
            get
            {
                if (_author == null)
                {
                    _author = new GenericClass<Author>(_context);
                }
                return _author;
            }
        }


        //کتاب
        public GenericClass<EntityLayer.Entities.Book> bookUW
        {
            //فقط خواندنی    
            get
            {
                if (_book == null)
                {
                    _book = new GenericClass<EntityLayer.Entities.Book>(_context);
                }
                return _book;
            }
        }


        //اخبار
        public GenericClass<EntityLayer.Entities.News> newsUW
        {
            //فقط خواندنی    
            get
            {
                if (_news == null)
                {
                    _news = new GenericClass<EntityLayer.Entities.News>(_context);
                }
                return _news;
            }
        }


        //امانت کتاب
        public GenericClass<EntityLayer.Entities.BorrowBook> borrowBookUW
        {
            //فقط خواندنی    
            get
            {
                if (_borrowBook == null)
                {
                    _borrowBook = new GenericClass<EntityLayer.Entities.BorrowBook>(_context);
                }
                return _borrowBook;
            }
        }


        //تراکنشها
        public GenericClass<PaymentTransaction> paymentUW
        {
            //فقط خواندنی    
            get
            {
                if (_payment == null)
                {
                    _payment = new GenericClass<EntityLayer.Entities.PaymentTransaction>(_context);
                }
                return _payment;
            }
        }


        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void save()
        {
            _context.SaveChanges();
        }
    }
}
