using System;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models
{
	public class Service
	{
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string? description { get; set; }
        [Required(ErrorMessage = "Image URL is required")]
        public string? imageURL { get; set; }
    }
}

