using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knjigarna.Models
{
    public class Avtor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ime avtorja je obvezno")]
        [Display(Name = "Ime")]
        [StringLength(50, ErrorMessage = "Ime je lahko dolgo največ 50 znakov")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Priimek avtorja je obvezen")]
        [Display(Name = "Priimek")]
        [StringLength(50, ErrorMessage = "Priimek je lahko dolg največ 50 znakov")]
        public string Priimek { get; set; }

        [Display(Name = "Datum rojstva")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DatumRojstva { get; set; }

        [Display(Name = "Država")]
        [StringLength(50)]
        public string Drzava { get; set; }

        [Display(Name = "Biografija")]
        [DataType(DataType.MultilineText)]
        public string Biografija { get; set; }

        // Navigacijska lastnost - ena avtor ima več knjig
        public virtual ICollection<Knjiga> Knjige { get; set; }

        // Izračunana lastnost za polno ime
        [NotMapped]
        public string PolnoIme
        {
            get { return $"{Ime} {Priimek}"; }
        }
    }
}