using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Knjigarna.Models.Registracija
{
    public class RegistracijaStoritev
    {
        // Statično shranjevanje registracijskih podatkov
        private static Dictionary<string, RegistracijaModel> _registracijskiPodatki = new Dictionary<string, RegistracijaModel>();

        private readonly IHttpContextAccessor _httpContextAccessor;

        public RegistracijaStoritev(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Vrne ID uporabnika iz seje ali ustvari novega
        public string GetOrCreateUserId()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var userId = session.GetString("RegistracijaUserId");

            if (string.IsNullOrEmpty(userId))
            {
                userId = Guid.NewGuid().ToString();
                session.SetString("RegistracijaUserId", userId);
            }

            return userId;
        }

        // Pridobi registracijske podatke za uporabnika
        public RegistracijaModel GetRegistracijskiPodatki()
        {
            var userId = GetOrCreateUserId();

            if (!_registracijskiPodatki.ContainsKey(userId))
            {
                _registracijskiPodatki[userId] = new RegistracijaModel
                {
                    DatumRojstva = DateTime.Now.AddYears(-18),
                    Starost = 18
                };
            }

            return _registracijskiPodatki[userId];
        }

        // Shrani podatke za korak 1
        public void ShraniKorak1(Korak1Model model)
        {
            var userId = GetOrCreateUserId();
            var podatki = GetRegistracijskiPodatki();

            podatki.Ime = model.Ime;
            podatki.Priimek = model.Priimek;
            podatki.DatumRojstva = model.DatumRojstva;
            podatki.EMSO = model.EMSO;
            podatki.Starost = model.Starost;

            _registracijskiPodatki[userId] = podatki;
            _httpContextAccessor.HttpContext.Session.SetString("RegistracijaKorak", "1");
        }

        // Pretvori podatke v model za korak 1
        public Korak1Model GetKorak1Model()
        {
            var podatki = GetRegistracijskiPodatki();

            return new Korak1Model
            {
                Ime = podatki.Ime,
                Priimek = podatki.Priimek,
                DatumRojstva = podatki.DatumRojstva,
                EMSO = podatki.EMSO,
                Starost = podatki.Starost
            };
        }

        // Shrani podatke za korak 2
        public void ShraniKorak2(Korak2Model model)
        {
            var userId = GetOrCreateUserId();
            var podatki = GetRegistracijskiPodatki();

            podatki.Naslov = model.Naslov;
            podatki.PostnaStevilka = model.PostnaStevilka;
            podatki.Posta = model.Posta;
            podatki.Drzava = model.Drzava;

            _registracijskiPodatki[userId] = podatki;
            _httpContextAccessor.HttpContext.Session.SetString("RegistracijaKorak", "2");
        }

        // Pretvori podatke v model za korak 2
        public Korak2Model GetKorak2Model()
        {
            var podatki = GetRegistracijskiPodatki();

            return new Korak2Model
            {
                Naslov = podatki.Naslov,
                PostnaStevilka = podatki.PostnaStevilka,
                Posta = podatki.Posta,
                Drzava = podatki.Drzava
            };
        }

        // Shrani podatke za korak 3
        public void ShraniKorak3(Korak3Model model)
        {
            var userId = GetOrCreateUserId();
            var podatki = GetRegistracijskiPodatki();

            podatki.Email = model.Email;
            podatki.Geslo = model.Geslo;
            podatki.PonovitevGesla = model.PonovitevGesla;

            _registracijskiPodatki[userId] = podatki;
            _httpContextAccessor.HttpContext.Session.SetString("RegistracijaKorak", "3");
        }

        // Pretvori podatke v model za korak 3
        public Korak3Model GetKorak3Model()
        {
            var podatki = GetRegistracijskiPodatki();

            return new Korak3Model
            {
                Email = podatki.Email,
                Geslo = podatki.Geslo,
                PonovitevGesla = podatki.PonovitevGesla
            };
        }

        // Preveri, ali je korak že izpolnjen
        public bool JeKorakIzpolnjen(int korak)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var trenutniKorak = session.GetString("RegistracijaKorak");

            if (string.IsNullOrEmpty(trenutniKorak))
                return korak == 1; // Če ni nobenega koraka, je dovoljen samo korak 1

            return int.Parse(trenutniKorak) >= korak - 1; // Dovoli trenutni korak ali naslednjega
        }

        // Počisti podatke po koncu registracije
        public void PocistiPodatke()
        {
            var userId = GetOrCreateUserId();
            if (_registracijskiPodatki.ContainsKey(userId))
            {
                _registracijskiPodatki.Remove(userId);
            }

            _httpContextAccessor.HttpContext.Session.Remove("RegistracijaKorak");
        }
    }
}