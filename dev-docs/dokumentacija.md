# Dokumentacija postojećeg stanja - Aplikacija 2DRakun

Ovaj dokument opisuje arhitekturu i tijek rada postojeće aplikacije za izradu računa.

## 1. Struktura projekta

Aplikacija je razvijena korištenjem ASP.NET MVC 5 frameworka. Ključni direktoriji su:

-   **/Models**: Sadrži C# klase koje predstavljaju podatkovne modele (npr. `Invoice`, `Customer`). Klase su mapirane na tablice u bazi podataka pomoću Dapper atribute.
-   **/Controllers**: Sadrži kontrolere koji upravljaju korisničkim zahtjevima. `HomeController` sadrži glavnu logiku za prikaz stranica i obradu podataka vezanih za račune.
-   **/Views**: Sadrži Razor `.cshtml` datoteke koje definiraju korisničko sučelje. `Views/Home` sadrži poglede za kreiranje, pregled i ispis računa.
-   **/Code**: Sadrži pomoćne klase (helpers) i servise koji enkapsuliraju poslovnu logiku, kao što su generiranje PDF-a, rad s bazom podataka i generiranje barkoda.
-   **/Documents**: Ciljni direktorij u koji se spremaju generirani PDF računi.

## 2. Tijek rada: Kreiranje računa

Proces kreiranja novog računa odvija se u nekoliko koraka, kojima upravlja `HomeController`.

### Korak 1: Unos podataka o računu (`NewInvoice.cshtml`)

-   **Akcija:** `GET /Home/NewInvoice`
-   **Opis:** Korisniku se prikazuje forma za unos podataka. Forma je implementirana u `NewInvoice.cshtml` i koristi **Vue.js** za dinamičko dodavanje stavki računa i izračun ukupnog iznosa.
-   **Broj računa:** Korisnik **ručno unosi broj računa** u predviđeno polje. Aplikacija ne generira automatski brojeve računa.
-   **Podaci:** Korisnik unosi podatke o kupcu i jednu ili više stavki računa (opis, količina, cijena).
-   **Slanje:** Nakon unosa, forma se šalje na akciju `PreviewInvoice`.

### Korak 2: Pregled računa (`InvoicePreview.cshtml`)

-   **Akcija:** `POST /Home/PreviewInvoice`
-   **Opis:** Akcija prima `InvoiceViewModel` s podacima iz forme. Popunjava podatke o prodavatelju (trenutno prijavljeni korisnik) i prikazuje stranicu za pregled (`InvoicePreview.cshtml`) na kojoj korisnik može vizualno provjeriti kako će račun izgledati.
-   **Potvrda:** Stranica za pregled sadrži gumb koji vodi na konačnu potvrdu i generiranje računa.

### Korak 3: Potvrda i generiranje PDF-a (`ConfirmInvoice`)

-   **Akcija:** `POST /Home/ConfirmInvoice` (iako je implementirana kao GET, poziva se kao POST)
-   **Opis:** Ovo je ključna akcija gdje se odvija glavna poslovna logika:
    1.  **Spremanje kupca:** Podaci o kupcu se spremaju u bazu podataka (`InvoiceService.SaveCustomer`).
    2.  **Generiranje barkoda:**
        -   Poziva se metoda `InvoiceService.AddPdf417BarcodeToModel`.
        -   Ova metoda kreira tekstualni *payload* za 2D barkod prema HUB3A standardu.
        -   Zatim poziva `BarCodeService.GeneratePdf417BarcodeBase64` koji koristi biblioteku **ZXing.Net** za generiranje **PDF417** barkoda.
        -   Barkod se generira kao Base64 enkodirana PNG slika i sprema u `InvoiceViewModel`.
        -   **Napomena:** U projektu postoji i `Hub3aPayloadBuilder.cs` koji nije korišten; payload se sastavlja ručno u `InvoiceService`.
    3.  **Generiranje HTML-a:**
        -   Poziva se `PdfHelper.RenderViewToString` koji renderira Razor view `InvoiceTemplate.cshtml` u HTML string. `InvoiceViewModel` (s barkodom) prosljeđuje se view-u.
    4.  **Generiranje PDF-a:**
        -   HTML string se prosljeđuje metodi `PdfHelper.GeneratePdfFromHtml` koja koristi biblioteku **NReco.PdfGenerator** za konverziju HTML-a u PDF dokument.
    5.  **Spremanje PDF-a:** Generirani PDF se sprema na disk u direktorij `~/Documents/Invoices/`.
    6.  **Spremanje u bazu:**
        -   Glavni podaci o računu (`Invoice`) i njegove stavke (`InvoiceItem`) spremaju se u bazu podataka unutar jedne transakcije. Koristi se **Dapper.SimpleCRUD** za jednostavne `Insert` operacije.
    7.  **Prikaz poruke:** Korisnik se preusmjerava na stranicu s porukom o uspješno kreiranom računu.

## 3. Ključne biblioteke i tehnologije

-   **Backend:** ASP.NET MVC 5, C#
-   **Pristup bazi:** Dapper i Dapper.SimpleCRUD
-   **Generiranje PDF-a:** NReco.PdfGenerator (wrapper oko wkhtmltopdf)
-   **Generiranje barkoda:** ZXing.Net
-   **Frontend:** Vue.js (za dinamičku formu), Bootstrap 5

## 4. Podatkovni modeli

-   `Invoice`: Glavni model računa, mapiran na tablicu `Invoices`.
-   `InvoiceItem`: Stavke računa, mapirane na tablicu `InvoiceItems`.
-   `Customer`: Podaci o kupcima, mapirani na tablicu `Customers`.
-   `User`: Podaci o korisnicima aplikacije.
-   `InvoiceViewModel`: Model koji se koristi za prijenos podataka između kontrolera i view-ova. Sadrži sva polja potrebna za prikaz i obradu računa na korisničkom sučelju.
