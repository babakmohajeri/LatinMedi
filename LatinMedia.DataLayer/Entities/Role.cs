using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [Display(Name ="عنوان نقش")]
        [Required(ErrorMessage ="مقدار {0} را وارد نمایید}")]
        [MaxLength(200, ErrorMessage ="{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string ToleTitle { get; set; }

        [Display(Name = "نام نقش")]
        [Required(ErrorMessage = "مقدار {0} را وارد نمایید}")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string RoleName { get; set; }
        public bool IsDelete { get; set; }

        #region Relations

        public List<UserRole> UserRoles { get; set; }

        #endregion
    }
}
