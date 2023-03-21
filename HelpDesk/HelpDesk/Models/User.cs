using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HelpDesk.Models
{
	public class User : IdentityUser
	{
		[Display(Name = "First Name")]
		public string? FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

		[NotMapped]
		public IList<string>? RoleNames { get; set; }

		public ICollection<Request>? Requests { get; set; }
		
	}
}

