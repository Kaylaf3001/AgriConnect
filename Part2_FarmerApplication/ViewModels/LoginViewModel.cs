using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Part2_FarmerApplication.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
