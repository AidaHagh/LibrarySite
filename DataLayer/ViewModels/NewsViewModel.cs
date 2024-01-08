using System.ComponentModel.DataAnnotations;

namespace DataLayer.ViewModels
{
    public class NewsViewModel
    {
        public int NewsID { get; set; }

        [Display(Name = "عنوان اطلاعیه")]
        [Required(ErrorMessage = "لطفا عنوان اطلاعیه را وارد نمایید")]
        public string NewsTitle { get; set; }

        [Display(Name = "متن اطلاعیه")]
        [Required(ErrorMessage = "لطفا متن اطلاعیه را وارد نمایید")]
        public string NewsContent { get; set; }

        [Display(Name = "تاریخ اطلاعیه")]
        public DateTime NewsDate { get; set; }

        [Display(Name = "تصویر")]
        public string? NewsAttachment { get; set; }
    }
}
