using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models
{
	public class Request
	{
		[Key]
		public string? RequestId { get; set; }

		[Required(ErrorMessage="Select a service")]
		[Display(Name ="Type Of Service")]
		public string? RequestType { get; set; }
		[Display(Name ="Description")]
		public string? Description { get; set; }
		public bool InProgress { get; set; }
		public bool IsCompleted { get; set; }
		public string? DateRequested { get; set; }
		public string? UserId { get; set; }
		[ForeignKey("UserId")]
		public User? RequestedUser { get; set; }
		public string? UserFullName { get; set; }

    }
}

