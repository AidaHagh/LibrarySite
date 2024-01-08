
using System.ComponentModel.DataAnnotations;


namespace DataLayer.ViewModels
{
    public class ManageRequestViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
        public byte Status { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusDescription { get; set; }

        [Display(Name = "نام کاربر")]
        public string UserFullName { get; set; }

        [Display(Name = "نام کتاب")]
        public string BookName { get; set; }

        [Display(Name = "موجودی")]
        public int BookStock { get; set; }

        [Display(Name = "تاریخ درخواست")]
        public DateTime RequestDate { get; set; }

        [Display(Name = "تاریخ پاسخ")]
        public DateTime? AnswereDate { get; set; }

        [Display(Name = "تاریخ بازگشت")]
        public DateTime? GiveBackDate { get; set; }

        [Display(Name = "مبلغ")]
        public int Price { get; set; }
    }
}
