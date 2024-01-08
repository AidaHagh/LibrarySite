using DataLayer.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Repository
{
    public class GenericClass<Tentity> where Tentity : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Tentity> _table;

        public GenericClass(ApplicationDbContext context)
        {
            _context = context;
            _table = context.Set<Tentity>();//مقداری که حین اجرا فرستاده میشود--مثل جدول یوزر
        }

        public virtual void Create(Tentity entity)//هر جا خواستیم اطلاعاتی ثب کنیم و ورودی هم نام جدول می نویسیم
        {
            _table.Add(entity);
        }

        public virtual void Update(Tentity entity)
        {
            _table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual Tentity GetById(object id)//یک تابع از نوع آن جدول که قبل از دیلیت آنرا پیدا می کند
        {
            return _table.Find(id);//رکورد را اینجا پیدا کردیم
        }

        public virtual void Delete(Tentity entity)//گاهی رکورد ایجاد میشود و قبل از اینکه سیو شود در دیتابیس میخواهد حذف هم بشود
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _table.Attach(entity);
            }
            _table.Remove(entity);
        }

        public virtual void DeleteById(object id)//بعد از یافتن ای دی آنرا حذف میکند
        {
            var entity = GetById(id);
            Delete(entity);
        }

        public virtual void DeleteByRange(Expression<Func<Tentity, bool>> whereVariable = null)
        {
            IQueryable<Tentity> query = _table;
            if (whereVariable != null)
            {
                query = query.Where(whereVariable);
            }
            _table.RemoveRange(query);
        }

        //گرفتن کل اطلاعات جدول 
        public virtual IEnumerable<Tentity> Get(Expression<Func<Tentity, bool>> whereVariable = null,
           Func<IQueryable<Tentity>, IOrderedQueryable<Tentity>> orderbyVariable = null,
            string joinString = "")
        {
            IQueryable<Tentity> query = _table;

            if (whereVariable != null)
            {
                query = query.Where(whereVariable);
            }
            if (orderbyVariable != null)
            {
                query = orderbyVariable(query);
            }
            if (joinString != "")
            {
                foreach (string item in joinString.Split(','))
                {
                    query = query.Include(item);//هر جدول با یک ویلگول با جدول دیگر جد میشود -- 
                    //includ کار join را انجام میدهد
                }
            }
            return query.ToList();
        }

    }
}


//در کنترل به کلاسهایی که به دیتابیس متصل اند نباید ارتباط برقرار کنیم و این کلاس عملا لایه ریپازیتوری ما است 
//ولایه کنترل میشود لایه یو ای و لایه بین اینها میشود اینترفیس
//بعد از انجام مثلا عملیات اینزرت این کلاس صدا زده میشود و در دیتابیس ثبت میشود و دیتابیس نیو میشود و برای آبدیت هم دوباره این کلاس را صدا میزند
//و دوباره دیتابیسساخته میشود که این منطقی نیست ..که برای رفع این مشکل از یونیت اف ورک استفاده میشود و الگوی برای عملیات اسی ار یو دی به کار میرود
////پس این الگو برای دستورات سی ار یو دی و کلاسهای جنریک است