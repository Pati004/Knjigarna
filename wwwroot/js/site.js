// Skripta za prikaz trenutnega časa in datuma
function posodobiTrenutniCas() {
    const datumElement = document.getElementById('trenutniCas');
    if (datumElement) {
        const trenutniDatum = new Date();

        // Oblikovanje datuma in časa v slovenščini
        const opcije = {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit'
        };

        // Prevod imen dni in mesecev v slovenščino
        const dnevi = ['nedelja', 'ponedeljek', 'torek', 'sreda', 'četrtek', 'petek', 'sobota'];
        const meseci = ['januar', 'februar', 'marec', 'april', 'maj', 'junij', 'julij', 'avgust', 'september', 'oktober', 'november', 'december'];

        const dan = dnevi[trenutniDatum.getDay()];
        const datum = trenutniDatum.getDate();
        const mesec = meseci[trenutniDatum.getMonth()];
        const leto = trenutniDatum.getFullYear();

        const ure = trenutniDatum.getHours().toString().padStart(2, '0');
        const minute = trenutniDatum.getMinutes().toString().padStart(2, '0');
        const sekunde = trenutniDatum.getSeconds().toString().padStart(2, '0');

        datumElement.textContent = `${dan}, ${datum}. ${mesec} ${leto}, ${ure}:${minute}:${sekunde}`;
    }
}


// Posodobi čas ob nalaganju strani
document.addEventListener('DOMContentLoaded', function () {
    posodobiTrenutniCas();
    // Posodobi čas vsako sekundo
    setInterval(posodobiTrenutniCas, 1000);
});

// Glede na velikost zaslona prilagodi prikaz
function prilagodiZaslonVelikost() {
    const sirina = window.innerWidth;

    // Prilagodi elemente glede na velikost zaslona
    if (sirina < 576) {
        // Mobilna naprava
        document.querySelectorAll('.card-title').forEach(naslov => {
            naslov.style.fontSize = '1.1rem';
        });
    } else if (sirina >= 576 && sirina < 992) {
        // Tablični prikaz
        document.querySelectorAll('.card-title').forEach(naslov => {
            naslov.style.fontSize = '1.25rem';
        });
    } else {
        // Namizni prikaz
        document.querySelectorAll('.card-title').forEach(naslov => {
            naslov.style.fontSize = '1.5rem';
        });
    }
}

// Klic funkcije ob nalaganju in spremembi velikosti zaslona
window.addEventListener('load', prilagodiZaslonVelikost);
window.addEventListener('resize', prilagodiZaslonVelikost);