using System;
namespace HelpDesk.Models
{
	public class ServiceRequest
	{
		public List<Service>? Services { get; set; }
		public Request? Request { get; set; }
	}
}

