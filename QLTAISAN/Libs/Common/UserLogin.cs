using System.ComponentModel.DataAnnotations;

namespace Libs.Common
{
    [Serializable]
    public class UserLoginSec
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string GroupID { get; set; }
    }

    public class InformationUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string RoleGroupID { get; set; }
        public string RoleGroupName { get; set; }
        public string FullName { get; set; }
    }

    public class RegisterUser
    {
        [Required]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }


        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Quyền")]
        public string Role { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public class CreateUserLogin
        {
            public long UserID { set; get; }
            public string UserName { set; get; }
            public string GroupID { set; get; }
        }
    }
}
