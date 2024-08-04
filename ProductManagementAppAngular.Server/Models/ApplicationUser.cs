using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace ProductManagementAppAngular.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Preferred Name is required")]
        public string PreferredName { get; set; }
    }
}
