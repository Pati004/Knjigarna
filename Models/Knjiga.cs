using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knjigarna.Models
{
    public class Knjiga
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "ISBN je obvezen")]
        [Display(Name = "ISBN", Description = "Mednarodni standardni številka knjige")]
        [StringLength(20, ErrorMessage = "ISBN je lahko dolg največ 20 znakov")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Naslov knjige je obvezen")]
        [Display(Name = "Naslov")]
        [StringLength(100, ErrorMessage = "Naslov je lahko dolg največ 100 znakov")]
        public string Naslov { get; set; }

        [Display(Name = "Opis")]
        [DataType(DataType.MultilineText)]
        public string Opis { get; set; }

        [Display(Name = "Leto izdaje")]
        [Range(1000, 2100, ErrorMessage = "Leto mora biti med 1000 in 2100")]
        public int? LetoIzdaje { get; set; }

        [Required(ErrorMessage = "Cena je obvezna")]
        [Display(Name = "Cena (€)")]
        [DataType(DataType.Currency)]
        [Range(0.01, 1000, ErrorMessage = "Cena mora biti med 0,01 € in 1000 €")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cena { get; set; }

        [Display(Name = "Število strani")]
        [Range(1, 10000, ErrorMessage = "Število strani mora biti med 1 in 10000")]
        public int? SteviloStrani { get; set; }

        // Navigacijske lastnosti - relacije
        [Display(Name = "Avtor")]
        public int AvtorId { get; set; }

        [ForeignKey("AvtorId")]
        public virtual Avtor Avtor { get; set; }

        [Display(Name = "Kategorija")]
        public int KategorijaId { get; set; }

        [ForeignKey("KategorijaId")]
        public virtual Kategorija Kategorija { get; set; }

        [Display(Name = "Slika naslovnice")]
        [StringLength(255)]
        public string SlikaNaslovnice { get; set; }

        [Display(Name = "Zaloga")]
        [Range(0, 10000, ErrorMessage = "Zaloga mora biti med 0 in 10000")]
        public int Zaloga { get; set; } = 0;

        [Display(Name = "Na voljo")]
        public bool JeNaVoljo { get; set; } = true;

        [Display(Name = "Datum dodajanja")]
        [DataType(DataType.DateTime)]
        public DateTime DatumDodajanja { get; set; } = DateTime.Now;
    }
}