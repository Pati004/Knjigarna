using System;
using System.ComponentModel.DataAnnotations;

namespace Knjigarna.Models.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EMSOValidationAttribute : ValidationAttribute
    {
        public EMSOValidationAttribute()
            : base("EMŠO ni veljaven.")
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null || !(value is string emso))
                return false;

            // Dolžina mora biti 13 znakov
            if (emso.Length != 13)
                return false;

            // EMŠO mora vsebovati samo številke
            foreach (char c in emso)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            try
            {
                // Razčlenimo datumski del EMŠO
                int dan = int.Parse(emso.Substring(0, 2));
                int mesec = int.Parse(emso.Substring(2, 2));
                int leto = int.Parse(emso.Substring(4, 3));

                // Določimo stoletje (prvo ali drugo tisočletje)
                if (leto >= 500)
                    leto = 1000 + leto;  // 1500-1999
                else
                    leto = 2000 + leto;  // 2000-2499

                // Preverimo, če je datum veljaven
                if (!JeVeljavenDatum(dan, mesec, leto))
                    return false;

                // Izračun kontrolne številke
                int kontrolna = IzracunajKontrolnoStevilko(emso);
                int zadnjaStevilka = int.Parse(emso.Substring(12, 1));

                // Kontrolna številka se mora ujemati z zadnjo števko EMŠO
                return kontrolna == zadnjaStevilka;
            }
            catch
            {
                return false;
            }
        }

        // Metoda za preverjanje veljavnosti datuma
        private bool JeVeljavenDatum(int dan, int mesec, int leto)
        {
            if (mesec < 1 || mesec > 12)
                return false;

            // Največje število dni v mesecu
            int stDni;
            switch (mesec)
            {
                case 2: // februar
                    if (JePrestopnoLeto(leto))
                        stDni = 29;
                    else
                        stDni = 28;
                    break;
                case 4: // april
                case 6: // junij
                case 9: // september
                case 11: // november
                    stDni = 30;
                    break;
                default:
                    stDni = 31;
                    break;
            }

            return dan >= 1 && dan <= stDni;
        }

        // Metoda za preverjanje prestopnega leta
        private bool JePrestopnoLeto(int leto)
        {
            return (leto % 4 == 0 && leto % 100 != 0) || (leto % 400 == 0);
        }

        // Metoda za izračun kontrolne številke po algoritmu EMŠO
        private int IzracunajKontrolnoStevilko(string emso)
        {
            // Pretvori prvih 12 števk v številke
            int[] stevke = new int[12];
            for (int i = 0; i < 12; i++)
            {
                stevke[i] = int.Parse(emso.Substring(i, 1));
            }

            // Uteži za modulo 11 kontrolno vsoto
            int[] utezi = { 7, 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

            // Izračun utežene vsote
            int vsota = 0;
            for (int i = 0; i < 12; i++)
            {
                vsota += stevke[i] * utezi[i];
            }

            // Izračun kontrolne številke
            int ostanek = vsota % 11;
            int kontrolna = 11 - ostanek;

            // Če je kontrolna 10 ali 11, je rezultat 0
            if (kontrolna >= 10)
                kontrolna = 0;

            return kontrolna;
        }
    }
}