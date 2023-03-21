using System;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models
{
	public class EmployeeRequest
	{
		[Key]
		public string? EmployeeId { get; set; }
		public List<Request>? RequestsHandled { get; set; }


	}
}

