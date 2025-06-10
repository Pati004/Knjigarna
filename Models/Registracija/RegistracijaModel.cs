using System;
using System.ComponentModel.DataAnnotations;

namespace Knjigarna.Models.Registracija
{
    // Osnovni model, ki vsebuje vse podatke za registracijo
    public class RegistracijaModel
    {
        // Korak 1: Osebni podatki
        public string Ime { get; set; }
        public string Priimek { get; set; }
        public DateTime DatumRojstva { get; set; }
        public string EMSO { get; set; }
        public int Starost { get; set; }

        // Korak 2: Podatki o naslovu
        public string Naslov { get; set; }
        public string PostnaStevilka { get; set; }
        public string Posta { get; set; }
        public string Drzava { get; set; }

        // Korak 3: Podatki za prijavo
        public string Email { get; set; }
        public string Geslo { get; set; }
        public string PonovitevGesla { get; set; }
    }

    // Model za 1. korak
    public class Korak1Model
    {
        [Required(ErrorMessage = "Ime je obvezno")]
        [Display(Name = "Ime")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Priimek je obvezen")]
        [Display(Name = "Priimek")]
        public string Priimek { get; set; }

        [Required(ErrorMessage = "Datum rojstva je obvezen")]
        [Display(Name = "Datum rojstva")]
        [DataType(DataType.Date)]
        public DateTime DatumRojstva { get; set; }

        [Required(ErrorMessage = "EMŠO je obvezen")]
        [Display(Name = "EMŠO")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "EMŠO mora imeti 13 znakov")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "EMŠO mora vsebovati samo številke")]
        public string EMSO { get; set; }

        [Required(ErrorMessage = "Starost je obvezna")]
        [Display(Name = "Starost")]
        [Range(18, 100, ErrorMessage = "Starost mora biti med 18 in 100 let")]
        public int Starost { get; set; }
    }

    // Model za 2. korak
    public class Korak2Model
    {
        [Required(ErrorMessage = "Naslov je obvezen")]
        [Display(Name = "Naslov")]
        public string Naslov { get; set; }

        [Required(ErrorMessage = "Poštna številka je obvezna")]
        [Display(Name = "Poštna številka")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Poštna številka mora vsebovati 4 številke")]
        public string PostnaStevilka { get; set; }

        [Required(ErrorMessage = "Pošta je obvezna")]
        [Display(Name = "Pošta")]
        public string Posta { get; set; }

        [Required(ErrorMessage = "Država je obvezna")]
        [Display(Name = "Država")]
        public string Drzava { get; set; }
    }

    // Model za 3. korak
    public class Korak3Model
    {
        [Required(ErrorMessage = "E-naslov je obvezen")]
        [Display(Name = "E-naslov")]
        [EmailAddress(ErrorMessage = "Vnesite veljaven e-naslov")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Geslo je obvezno")]
        [Display(Name = "Geslo")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Geslo mora imeti vsaj {2} znakov", MinimumLength = 6)]
        public string Geslo { get; set; }

        [Required(ErrorMessage = "Ponovitev gesla je obvezna")]
        [Display(Name = "Ponovite geslo")]
        [DataType(DataType.Password)]
        [Compare("Geslo", ErrorMessage = "Gesli se ne ujemata")]
        public string PonovitevGesla { get; set; }
    }
}