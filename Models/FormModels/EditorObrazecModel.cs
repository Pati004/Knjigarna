using System;
using System.ComponentModel.DataAnnotations;
using Knjigarna.Models.Validation;

namespace Knjigarna.Models.FormModels
{
    public class EditorObrazecModel
    {
        [Required(ErrorMessage = "Ime je obvezno")]
        [Display(Name = "Ime")]
        [StringLength(50, ErrorMessage = "Ime je lahko dolgo največ 50 znakov")]
        [RegularExpression(@"^[a-zA-ZčćšđžČĆŠĐŽ\s]+$", ErrorMessage = "Ime lahko vsebuje samo črke")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Priimek je obvezen")]
        [Display(Name = "Priimek")]
        [StringLength(50, ErrorMessage = "Priimek je lahko dolg največ 50 znakov")]
        [RegularExpression(@"^[a-zA-ZčćšđžČĆŠĐŽ\s]+$", ErrorMessage = "Priimek lahko vsebuje samo črke")]
        public string Priimek { get; set; }

        [Required(ErrorMessage = "Datum rojstva je obvezen")]
        [Display(Name = "Datum rojstva")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DatumRojstva { get; set; }

        [Required(ErrorMessage = "Kraj rojstva je obvezen")]
        [Display(Name = "Kraj rojstva")]
        public string KrajRojstva { get; set; }

        [Required(ErrorMessage = "EMŠO je obvezen")]
        [Display(Name = "EMŠO")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "EMŠO mora imeti 13 znakov")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "EMŠO mora vsebovati samo številke")]
        [EMSOValidation(ErrorMessage = "EMŠO ni veljaven (preverite kontrolno številko in datum)")]
        public string EMSO { get; set; }

        [Required(ErrorMessage = "E-naslov je obvezen")]
        [Display(Name = "E-naslov")]
        [EmailAddress(ErrorMessage = "Vnesite veljaven e-naslov v obliki ime@domena.com")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "E-naslov mora biti v obliki ime@domena.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Starost je obvezna")]
        [Display(Name = "Starost")]
        [Range(18, 100, ErrorMessage = "Starost mora biti med 18 in 100 let")]
        public int Starost { get; set; }

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