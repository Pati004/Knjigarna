using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Knjigarna.Models
{
    public class TabObrazecModel
    {
        [Required(ErrorMessage = "Ime je obvezno")]
        [Display(Name = "Ime")]
        [RegularExpression(@"^[a-zA-ZčćšđžČĆŠĐŽ\s]+$", ErrorMessage = "Ime lahko vsebuje samo črke")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Priimek je obvezen")]
        [Display(Name = "Priimek")]
        [RegularExpression(@"^[a-zA-ZčćšđžČĆŠĐŽ\s]+$", ErrorMessage = "Priimek lahko vsebuje samo črke")]
        public string Priimek { get; set; }

        [Required(ErrorMessage = "Datum rojstva je obvezen")]
        [Display(Name = "Datum rojstva")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DatumRojstvaValidacija(ErrorMessage = "Datum rojstva mora biti med 1.1.1900 in današnjim dnem")]
        public DateTime DatumRojstva { get; set; }

        [Required(ErrorMessage = "Kraj rojstva je obvezen")]
        [Display(Name = "Kraj rojstva")]
        public string KrajRojstva { get; set; }

        [Required(ErrorMessage = "EMŠO je obvezen")]
        [Display(Name = "EMŠO")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "EMŠO mora imeti 13 znakov")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "EMŠO mora vsebovati samo številke")]
        public string EMSO { get; set; }

        [Required(ErrorMessage = "E-naslov je obvezen")]
        [Display(Name = "E-naslov")]
        [EmailAddress(ErrorMessage = "Vnesite veljaven e-naslov v obliki ime@domena.com")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "E-naslov mora biti v obliki ime@domena.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Geslo je obvezno")]
        [Display(Name = "Geslo")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Geslo mora imeti vsaj 6 znakov", MinimumLength = 6)]
        [GesloValidacija(ErrorMessage = "Geslo mora vsebovati vsaj eno številko in en poseben znak")]
        public string Geslo { get; set; }

        [Required(ErrorMessage = "Ponovitev gesla je obvezna")]
        [Display(Name = "Ponovite geslo")]
        [DataType(DataType.Password)]
        [Compare("Geslo", ErrorMessage = "Gesli se ne ujemata")]
        public string PonovitevGesla { get; set; }
    }

    // Validacijski atribut za datum rojstva
    public class DatumRojstvaValidacijaAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date;

            if (value is DateTime)
            {
                date = (DateTime)value;
            }
            else if (DateTime.TryParse(value?.ToString(), out date))
            {
                // ok
            }
            else
            {
                return false;
            }

            return date > new DateTime(1900, 1, 1) && date <= DateTime.Today;
        }
    }

    // Validacijski atribut za geslo
    public class GesloValidacijaAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string geslo = value as string;

            if (string.IsNullOrEmpty(geslo))
                return false;

            // Preveri, če geslo vsebuje vsaj eno številko
            bool vsebujeStevilko = Regex.IsMatch(geslo, @"\d");

            // Preveri, če geslo vsebuje vsaj en poseben znak
            bool vsebujePosebenZnak = Regex.IsMatch(geslo, @"[^\w\s]");

            return vsebujeStevilko && vsebujePosebenZnak;
        }
    }
}