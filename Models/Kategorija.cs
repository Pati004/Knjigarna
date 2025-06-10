using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Knjigarna.Models
{
    public class Kategorija
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ime kategorije je obvezno")]
        [Display(Name = "Ime kategorije")]
        [StringLength(50, ErrorMessage = "Ime kategorije je lahko dolgo največ 50 znakov")]
        public string Ime { get; set; }

        [Display(Name = "Opis")]
        [DataType(DataType.MultilineText)]
        public string Opis { get; set; }

        // Navigacijska lastnost - ena kategorija vsebuje več knjig
        public virtual ICollection<Knjiga> Knjige { get; set; }
    }
}