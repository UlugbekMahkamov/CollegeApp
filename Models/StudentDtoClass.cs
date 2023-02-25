using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CollageApp.Models
{
    public class StudentDtoClass
    {
        [ValidateNever]
        public int Id { get; set; }

        [Required(ErrorMessage ="Student name is required")]
        [StringLength(30)]
        public string StudentName { get; set; }

        [Required(ErrorMessage ="Please enter valid email adress")]
        public string Email { get; set; }

       // [Range(10, 30)]
        //public int Age { get; set; }

        [Required]
        public string Adress { get; set; }

      /*  public string Password { get; set; }

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }*/
    }
}
