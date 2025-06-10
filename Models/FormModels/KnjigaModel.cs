using System;
using System.ComponentModel.DataAnnotations;

namespace Knjigarna.Models.FormModels
{
    public class KnjigaModel
    {
        [Required(ErrorMessage = "ISBN je obvezen")]
        [Display(Name = "ISBN", Description = "Vnesite mednarodni standardni številko knjige (npr. 978-3-16-148410-0)")]
        [StringLength(17, MinimumLength = 10, ErrorMessage = "ISBN mora biti dolg med 10 in 17 znakov")]
        [RegularExpression(@"^[0-9-]+$", ErrorMessage = "ISBN lahko vsebuje samo številke in pomišljaje")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Naslov knjige je obvezen")]
        [Display(Name = "Naslov knjige")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Naslov mora vsebovati med 2 in 100 znakov")]
        public string Naslov { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ime avtorja je obvezno")]
        [Display(Name = "Avtor", Description = "Vnesite ime in priimek avtorja")]
        public string Avtor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Leto izdaje je obvezno")]
        [Display(Name = "Leto izdaje", Description = "Vnesite leto prve izdaje knjige")]
        [Range(1000, 2050, ErrorMessage = "Leto izdaje mora biti med 1000 in 2050")]
        public int LetoIzdaje { get; set; }

        [Required(ErrorMessage = "Opis knjige je obvezen")]
        [Display(Name = "Opis knjige", Description = "Vnesite kratek opis vsebine knjige")]
        [DataType(DataType.MultilineText)]
        public string Opis { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategorija je obvezna")]
        [Display(Name = "Kategorija", Description = "Izberite kategorijo, v katero spada knjiga")]
        public string Kategorija { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cena je obvezna")]
        [Display(Name = "Cena (€)", Description = "Vnesite prodajno ceno knjige")]
        [Range(0.01, 500, ErrorMessage = "Cena mora biti med 0,01 € in 500 €")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public double Cena { get; set; }

        [Required(ErrorMessage = "Zaloga je obvezna")]
        [Display(Name = "Zaloga", Description = "Vnesite število knjig na zalogi")]
        [Range(0, 10000, ErrorMessage = "Zaloga mora biti med 0 in 10.000")]
        public int Zaloga { get; set; }

        [Display(Name = "Na voljo", Description = "Označite, če je knjiga na voljo za nakup")]
        public bool JeNaVoljo { get; set; }

        [Required(ErrorMessage = "Datum izida je obvezen")]
        [Display(Name = "Datum izida", Description = "Vnesite datum, ko bo knjiga na voljo v prodaji")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DatumIzida { get; set; }

        [Display(Name = "Število strani", Description = "Vnesite število strani v knjigi")]
        [Range(1, 10000, ErrorMessage = "Število strani mora biti med 1 in 10.000")]
        public int SteviloStrani { get; set; }

        [Display(Name = "Ocena", Description = "Vnesite oceno knjige (1-5)")]
        [Range(1, 5, ErrorMessage = "Ocena mora biti med 1 in 5")]
        public int? Ocena { get; set; }
    }
}