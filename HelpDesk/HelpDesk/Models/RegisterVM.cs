using System;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models
{
	public class RegisterVM
	{
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required.")]
        public string? firstname { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required.")]
        public string? lastname { get; set; }

        [Display(Name = "Username")]
		[Required(ErrorMessage = "Username is required.")]
		public string? username { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email is required.")]
        public string? email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
		[DataType(DataType.Password)]
        public string? password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Passwords do not match.")]
        public string? confirmpassword { get; set; }
    }
}

