using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.StaticClass;


public static class StaticValue
{
    public class EducationTypeModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<EducationTypeModel> GetCustomeType()
        {
            List<EducationTypeModel> model = new List<EducationTypeModel>
        {
            new EducationTypeModel { Id = 0, Title = "....تحصیلات...." },
            new EducationTypeModel { Id = 1, Title = "زیردیپلم" },
            new EducationTypeModel { Id = 2, Title = "دیپلم" },
            new EducationTypeModel { Id = 3, Title = "کاردانی" },
            new EducationTypeModel { Id = 4, Title = "کارشناسی" },
            new EducationTypeModel { Id = 5, Title = "کارشناسی ارشد" },
            new EducationTypeModel { Id = 6, Title = "دکترا" },
        };
            return model;
        }
    }



    //جستجو در کاربران
    public class UserSearchTypeModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<UserSearchTypeModel> GetUserTypeModel()
        {
            var model = new List<UserSearchTypeModel>
            {
                new UserSearchTypeModel {Id = 0, Title = "---انتخاب---"},
                new UserSearchTypeModel {Id = 1, Title = "نام"},
                new UserSearchTypeModel {Id = 2, Title = "نام خانوادگی"},
                new UserSearchTypeModel {Id = 3, Title = "کد ملی"},
                new UserSearchTypeModel {Id = 4, Title = "ایمیل"},
                new UserSearchTypeModel {Id = 5, Title = "نام کاربری"},

            };
            return model;
        }
    }



    //جستجو در کتابها
    public class BookSearchTypeModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<BookSearchTypeModel> GetBookTypeModel()
        {
            var model = new List<BookSearchTypeModel>
            {
                new BookSearchTypeModel {Id = 0, Title = "---انتخاب---"},
                new BookSearchTypeModel {Id = 1, Title = "نام کتاب"},
                new BookSearchTypeModel {Id = 2, Title = "گروه بندی"},
                new BookSearchTypeModel {Id = 3, Title = "نویسنده"},

            };
            return model;
        }
    }
}
