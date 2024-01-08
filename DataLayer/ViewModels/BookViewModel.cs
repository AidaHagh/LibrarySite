using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class BookViewModel
    {
        public int BookId { get; set; }

        [Display(Name = "نام کتاب")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نام کتاب وارد نشده است")]
        [StringLength(maximumLength: 60, ErrorMessage = "نام حداکثر 60 کاراکتر می باشد")]
        public string BookName { get; set; }

        [Display(Name = "توضیحات")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "توضیحات وارد نشده است")]
        public string BookDescription { get; set; }

        [Display(Name = "تعداد صفحات")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "تعداد صفحات کتاب وارد نشده است")]
        public int BookPageCount { get; set; }

        [Display(Name = "تصویر")]
        public string? BookImage { get; set; }

        [Display(Name = "نویسنده")]
        public int AuthorId { get; set; }

        [Display(Name = "گروه بندی")]
        public int GroupId { get; set; }

        [Display(Name = "مبلغ امانت")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "مبلغ امانت وارد نشده است")]
        public int Price { get; set; }

        [Display(Name = "موجودی")]
        public int BookStock { get; set; }

        //[Display(Name = "بازدید")]
        //public int BookViews { get; set; }

    }
}
