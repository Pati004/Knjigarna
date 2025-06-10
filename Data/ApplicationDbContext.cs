using Microsoft.EntityFrameworkCore;
using Knjigarna.Models;

namespace Knjigarna.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Uporabnik> Uporabniki { get; set; }
        public DbSet<Avtor> Avtorji { get; set; }
        public DbSet<Kategorija> Kategorije { get; set; }
        public DbSet<Knjiga> Knjige { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguracija relacij in omejitev
            modelBuilder.Entity<Knjiga>()
                .HasOne(k => k.Avtor)
                .WithMany(a => a.Knjige)
                .HasForeignKey(k => k.AvtorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Knjiga>()
                .HasOne(k => k.Kategorija)
                .WithMany(k => k.Knjige)
                .HasForeignKey(k => k.KategorijaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Inicializacija podatkov (seeding)
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Začetni podatki za kategorije
            modelBuilder.Entity<Kategorija>().HasData(
                new Kategorija { Id = 1, Ime = "Fantazija", Opis = "Fantazijska književnost je žanr, ki pogosto vključuje čarobne elemente, domišljijska bitja in svetove." },
                new Kategorija { Id = 2, Ime = "Znanstvena fantastika", Opis = "Znanstvena fantastika je žanr, ki se ukvarja s spekulativnimi koncepti, kot so napredna znanost in tehnologija, vesoljska potovanja, časovna potovanja, vzporedni svetovi in zunajzemeljsko življenje." },
                new Kategorija { Id = 3, Ime = "Klasična literatura", Opis = "Klasična literatura se nanaša na dela, ki so preživela preizkus časa in so splošno priznana kot izjemna literarna dela z univerzalnimi temami in brezčasnim slogom." },
                new Kategorija { Id = 4, Ime = "Distopija", Opis = "Distopična literatura prikazuje družbo, ki jo zaznamuje zatiranje, totalitarizem in druge negativne družbene in politične razmere." },
                new Kategorija { Id = 5, Ime = "Kriminalka", Opis = "Kriminalni roman je literarni žanr, ki se osredotoča na reševanje zločina, običajno umora." }
            );

            // Začetni podatki za avtorje
            modelBuilder.Entity<Avtor>().HasData(
                new Avtor { Id = 1, Ime = "J.R.R.", Priimek = "Tolkien", DatumRojstva = new DateTime(1892, 1, 3), Drzava = "Združeno kraljestvo", Biografija = "John Ronald Reuel Tolkien je bil angleški pisatelj, pesnik, filolog in univerzitetni profesor." },
                new Avtor { Id = 2, Ime = "Isaac", Priimek = "Asimov", DatumRojstva = new DateTime(1920, 1, 2), Drzava = "Združene države Amerike", Biografija = "Isaac Asimov je bil ameriški pisatelj in profesor biokemije na Bostonski univerzi." },
                new Avtor { Id = 3, Ime = "Franz", Priimek = "Kafka", DatumRojstva = new DateTime(1883, 7, 3), Drzava = "Avstro-Ogrska (današnja Češka)", Biografija = "Franz Kafka je bil nemško govoreči judovski romanopisec in pisatelj kratkih zgodb." },
                new Avtor { Id = 4, Ime = "George", Priimek = "Orwell", DatumRojstva = new DateTime(1903, 6, 25), Drzava = "Indija (Britanski imperij)", Biografija = "George Orwell, pravo ime Eric Arthur Blair, je bil angleški romanopisec, esejist, novinar in kritik." }
            );

            // Začetni podatki za knjige
            modelBuilder.Entity<Knjiga>().HasData(
                new Knjiga { Id = 1, ISBN = "978-0-261-10320-7", Naslov = "Gospodar prstanov: Bratovščina prstana", Opis = "Prvi del epske trilogije Gospodar prstanov.", LetoIzdaje = 1954, Cena = 24.99m, SteviloStrani = 423, AvtorId = 1, KategorijaId = 1, SlikaNaslovnice = "lotr1.jpg", Zaloga = 15 },
                new Knjiga { Id = 2, ISBN = "978-0-553-29335-7", Naslov = "Fundacija", Opis = "Prvi roman v seriji Fundacija.", LetoIzdaje = 1951, Cena = 19.99m, SteviloStrani = 244, AvtorId = 2, KategorijaId = 2, SlikaNaslovnice = "foundation.jpg", Zaloga = 8 },
                new Knjiga { Id = 3, ISBN = "978-0-8052-0946-5", Naslov = "Proces", Opis = "Roman o bančnem uradniku Josefu K., obtožen neimenovanega zločina.", LetoIzdaje = 1925, Cena = 15.99m, SteviloStrani = 304, AvtorId = 3, KategorijaId = 3, SlikaNaslovnice = "theprocess.jpg", Zaloga = 5 },
                new Knjiga { Id = 4, ISBN = "978-0-452-28423-4", Naslov = "1984", Opis = "Distopični roman o totalitarni družbi, nadzoru in manipulaciji z resnico.", LetoIzdaje = 1949, Cena = 17.99m, SteviloStrani = 328, AvtorId = 4, KategorijaId = 4, SlikaNaslovnice = "1984.jpg", Zaloga = 12 }
            );
        }
    }
}