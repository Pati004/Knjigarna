using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knjigarna.Models
{
    // Razširjen model Uporabnik z gesli - ni mapiran v bazo podatkov
    [NotMapped]
    public class UporabnikZGesli : Uporabnik
    {
        [Required(ErrorMessage = "Geslo je obvezno")]
        [Display(Name = "Geslo")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Geslo mora imeti vsaj 6 znakov", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[^a-zA-Z0-9]).+$", ErrorMessage = "Geslo mora vsebovati vsaj eno številko in en poseben znak")]
        public string Geslo { get; set; }

        [Required(ErrorMessage = "Ponovitev gesla je obvezna")]
        [Display(Name = "Ponovite geslo")]
        [DataType(DataType.Password)]
        [Compare("Geslo", ErrorMessage = "Gesli se ne ujemata")]
        public string PonovitevGesla { get; set; }
    }
}