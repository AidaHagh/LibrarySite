using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer.Entities
{
    public class PaymentTransaction
    {
        [Key]
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
        public string? Email { get; set; }

        [Display(Name = "شماره تماس")]
        [Required(ErrorMessage = "شماره تماس را وارد نمایید")]
        public string Mobile { get; set; }

        [Display(Name = "شماره تراکنش")]
        public string TransactionNumber { get; set; }

        //////////////////////////

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUsers Users { get; set; }
    }
}
