
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace DataLayer.ViewModels
{

    public class RegisterViewModel
    {

        [Display(Name = "نام")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا نام خود را وارد کنید")]
        [StringLength(maximumLength: 60, ErrorMessage = "نام حداکثر 60 کاراکتر می باشد")]
        public string FirstName { get; set; }


        [Display(Name = "نام خانوادگی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا نام خانوادگی خود را وارد کنید")]
        [StringLength(maximumLength: 60, ErrorMessage = "نام حداکثر 60 کاراکتر می باشد")]
        public string LastName { get; set; }

        [Display(Name = "کد ملی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا {0} خود را وارد کنید")]
        [MinLength(10, ErrorMessage = "{0} باید 10 رقمی باشد")]
        [MaxLength(10, ErrorMessage = "{0} باید 10 رقمی باشد")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "فرمت {0} صحیح نیست.")]
        public string NationalCode { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} خود را وارد نمایید")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "آدرس")]
        [Required(AllowEmptyStrings = false, ErrorMessage = " لطفا{0} خود را وارد کنید.")]
        public string Address { get; set; }

        [Display(Name = "تحصیلات")]
        public byte Education { get; set; }

        [Display(Name = "جنسیت")]
        public byte Gender { get; set; }


        [Display(Name = "شماره موبایل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا {0} خود را وارد کنید")]
        [StringLength(11, ErrorMessage = "شماره موبایل 11 رقمی می باشد.")]
        [RegularExpression(@"^[^\\/:*;\.\)\(]+$", ErrorMessage = "کاراکترهای غیر مجاز وارد شده است.")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "رمز عبور وارد نشده است.")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "تکرار رمز عبور وارد نشده است.")]
        [Compare("Password", ErrorMessage = "کلمه عبور با تکرار آن یکسان نیست")]
        public string ConfirmPassword { get; set; }
        public bool IsActive { get; set; }
    }

    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "شماره همراه وارد نشده است")]
        [StringLength(11, ErrorMessage = "شماره موبایل 11 رقمی می باشد.")]
        [RegularExpression(@"^[^\\/:*;\.\)\(]+$", ErrorMessage = "کاراکترهای غیر مجاز وارد شده است.")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "رمز عبور وارد نشده است.")]
        public string Password { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "نام")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نام کوچک وارد نشده است")]
        [StringLength(maximumLength: 60, ErrorMessage = "نام حداکثر 60 کاراکتر می باشد")]
        public string FirstName { get; set; }


        [Display(Name = "نام خانوادگی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نام خانوادگی وارد نشده است")]
        [StringLength(maximumLength: 60, ErrorMessage = "نام حداکثر 60 کاراکتر می باشد")]
        public string LastName { get; set; }

        [Display(Name = "کد ملی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} وارد نشده است.")]
        [MinLength(10, ErrorMessage = "{0} باید 10 رقمی باشد")]
        [MaxLength(10, ErrorMessage = "{0} باید 10 رقمی باشد")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "فرمت {0} صحیح نیست.")]
        public string NationalCode { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا ایمیل را وارد نمایید")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "آدرس")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} وارد نشده است.")]
        public string Address { get; set; }

        [Display(Name = "تحصیلات")]
        public byte Education { get; set; }

        [Display(Name = "جنسیت")]
        public byte Gender { get; set; }


        [Display(Name = "شناسه کاربری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "شناسه کاربری یا شماره همراه.")]
        [StringLength(11, ErrorMessage = "شماره موبایل 11 رقمی می باشد.")]
        [RegularExpression(@"^[^\\/:*;\.\)\(]+$", ErrorMessage = "کاراکترهای غیر مجاز وارد شده است.")]
        public string UserName { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Display(Name = "رمز عبور قدیمی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "رمز عبور قدیمی را وارد کنید.")]
        public string OldPassword { get; set; }

        [Display(Name = "رمز عبور جدید")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "رمز عبور جدید را وارد کنید.")]
        [StringLength(maximumLength: 30, MinimumLength = 6, ErrorMessage = "رمز عبور حداقل 6 کاراکتر و حداکثر 30 کاراکتر باید باشد")]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار رمز عبور جدید")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "تکرار رمز عبور جدید را وارد کنید.")]
        [Compare("NewPassword", ErrorMessage = "رمز عبور جدید با تکرار آن یکسان نیست.")]
        public string ConfirmNewPassword { get; set; }
    }
}
