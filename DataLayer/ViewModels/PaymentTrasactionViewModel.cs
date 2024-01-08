using EntityLayer.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DataLayer.ViewModels
{
    public class PaymentTrasactionViewModel
    {
        public int ID { get; set; }

        [Display(Name = "تاریخ تراکنش")]
        public DateTime TransactioDate { get; set; }

        [Display(Name = "زمان تراکنش")]
        public DateTime TransactioTime { get; set; }

        [Display(Name = "مبلغ")]
        public long Amount { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "ایمیل را وارد نمایید")]
        public string Email { get; set; }

        [Display(Name = "شماره تماس")]
        [Required(ErrorMessage = "شماره تماس را وارد نمایید")]
        public string Mobile { get; set; }

        [Display(Name = "شماره تراکنش")]
        public string TransactionNumber { get; set; }

        //////////////////////////

        public string UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUsers Users { get; set; }

        [Display(Name = "نام و نام خانوادگی کاربر")]
        public string FullName { get; set; }
    }
}
