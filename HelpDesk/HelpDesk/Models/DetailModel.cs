using System;
namespace HelpDesk.Models
{
	public class DetailModel
	{
		public List<Request>? Request { get; set; }
		public EmployeeRequest? Employee { get; set; }
	}
}

