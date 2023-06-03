using System.ComponentModel.DataAnnotations;

namespace DutchTreat.ViewModels
{
    public class ContactViewModel
    {
        [Required]
        [MinLength(5 , ErrorMessage ="Miniumun Length is 5 ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required Email Address")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter a Message ")]
        [MaxLength(100 , ErrorMessage ="Too Long Message")]
        public string Message { get; set; }
        [Required(ErrorMessage ="Must Add Phone Number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please Enter a Subject")]
        public string Subject { get; set; }
    }
}
