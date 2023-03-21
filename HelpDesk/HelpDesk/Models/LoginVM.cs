using System;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models
{
	public class LoginVM
	{
		[Display(Name = "Username")]
		[Required(ErrorMessage = "Username is required.")]
		public string? username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
		[DataType(DataType.Password)]
        public string? password { get; set; }
	}
}

