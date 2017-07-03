using System.ComponentModel.DataAnnotations;

namespace MyValidatorApp.ViewModel
{
    public class IdValidatorModel : ValidationAttribute
    {
        [Required]
        public string IdNumber { get; set; }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            else return false;
        }
    }
}