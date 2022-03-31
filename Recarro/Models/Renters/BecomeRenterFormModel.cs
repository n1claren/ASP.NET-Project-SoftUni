using System.ComponentModel.DataAnnotations;

using static Recarro.Data.DataConstants;

namespace Recarro.Models.Renters
{
    public class BecomeRenterFormModel
    {
        [Required]
        [StringLength(RenterNameMaxLength, MinimumLength = RenterNameMinLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(RenterPhoneNumberMaxLength)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
