using System;
using Microsoft.AspNetCore.Identity;

namespace HelpDesk.Models
{
	public class UserViewModel
	{
		public IEnumerable<User>? Users { get; set; }
		public IEnumerable<IdentityRole>? Roles { get; set; }

	}
}

