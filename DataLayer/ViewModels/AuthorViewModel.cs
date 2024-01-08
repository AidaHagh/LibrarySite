using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace DataLayer.ViewModels
{
    public class AuthorViewModel
    {
        public int AuthorId { get; set; }

        [Display(Name = "نام نویسنده")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نام نویسنده وارد نشده است")]
        [StringLength(maximumLength: 100, ErrorMessage = "نام حداکثر 100 کاراکتر می باشد")]
        public string AuthorName { get; set; }

        [Display(Name = "توضیحات")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "توضیحات وارد نشده است")]
        [StringLength(maximumLength: 100, ErrorMessage = "نام حداکثر 100 کاراکتر می باشد")]
        public string Description { get; set; }
    }
}
