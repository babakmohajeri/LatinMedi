using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities
{
    public class User
    {
        public int UserId { get; set; }

        [Display(Name = "نام کاربر")]
        [Required(ErrorMessage = "مقدار {0} را وارد نمایید}")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی کاربر")]
        [Required(ErrorMessage = "مقدار {0} را وارد نمایید}")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string LastName { get; set; }

        [Display(Name = "آدرس ایمیل")]
        [Required(ErrorMessage = "مقدار {0} را وارد نمایید}")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage ="آدرس ایمیل را صحیح وارد نمایید")]
        public string Email { get; set; }

        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "مقدار {0} را وارد نمایید}")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Mobile { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "مقدار {0} را وارد نمایید}")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; }

        [Display(Name = "کد فعال سازی")]
        [Required(ErrorMessage = "مقدار {0} را وارد نمایید}")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string AcrivationCode { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        [Display(Name = "تصویر کاربر")]
        public string UserAvatar { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime CreateDate { get; set; }
        public bool IdDelete { get; set; }

        #region Relations

        public List<UserRole> UserRoles { get; set; }

        #endregion

    }
}
