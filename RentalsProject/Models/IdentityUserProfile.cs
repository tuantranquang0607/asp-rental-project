using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RentalsProject.Models
{
    public class IdentityUserProfile : IdentityUser
    {
        [ProtectedPersonalData]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }


        [ProtectedPersonalData]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }


        [ProtectedPersonalData]
        public bool isPhoneNumber { get; set; }
    }
}
