using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HelpDesk.Models
{
	public class OldNewPasswordVM
    {
        [Display(Name = "Old Password")]
        [Required(ErrorMessage = "Old password is required.")]
        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}

