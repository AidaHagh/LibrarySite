using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace DataLayer.ViewModels
{
    public class GroupViewModel
    {
        public int GroupId { get; set; }

        [Display(Name = "نام دسته بندی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نام گروه بندی وارد نشده است")]
        [StringLength(maximumLength: 60, ErrorMessage = "نام حداکثر 60 کاراکتر می باشد")]
        public string GroupName { get; set; }

        [Display(Name = "توضیحات")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "توضیحات وارد نشده است")]
        [StringLength(maximumLength: 60, ErrorMessage = "نام حداکثر 60 کاراکتر می باشد")]
        public string Description { get; set; }
    }
}
