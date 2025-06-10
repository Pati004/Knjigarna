using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Knjigarna.Models.Validation;

namespace Knjigarna.Models
{
    // Osnovni model Uporabnik za bazo podatkov (brez gesel)
    public class Uporabnik
    {
        [Key]
        public int Id { get; set; }

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

        // Polno ime (izračunana lastnost)
        [NotMapped]
        public string PolnoIme
        {
            get { return $"{Ime} {Priimek}"; }
        }

        // Datum registracije - avtomatsko se nastavi ob ustvarjanju
        [Display(Name = "Datum registracije")]
        [DataType(DataType.DateTime)]
        public DateTime DatumRegistracije { get; set; } = DateTime.Now;

        // Status aktivnosti uporabnika
        [Display(Name = "Aktiven")]
        public bool JeAktiven { get; set; } = true;
    }
}