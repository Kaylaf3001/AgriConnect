using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

//---------------------------------------------------------------------------------
// This class represents the view model for the login page.
// It contains properties for the email and password fields.
// It also includes validation attributes to ensure that the fields are required
//---------------------------------------------------------------------------------

namespace Part2_FarmerApplication.ViewModels
{
    public class LoginViewModel
    {
        //here we are using the [Required] attribute to ensure that the fields are not empty
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
//-------------------------------End--Of--File----------------------------------------------