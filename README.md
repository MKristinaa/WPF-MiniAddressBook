# WPF-MiniAddressBook

**Mini Address Book** je jednostavna desktop aplikacija napravljena u WPF-u (.NET Framework) za upravljanje kontaktima.  
Omogućava korisniku da dodaje nove kontakte, menja postojeće, briše selektovane kontakte i pretražuje kontakte po imenu ili email-u.

## Funkcionalnosti

- Prikaz liste kontakata pri pokretanju aplikacije.
- Dodavanje novog kontakta.
- Izmena selektovanog kontakta izborom iz tabele.
- Brisanje kontakta uz potvrdu.
- Pretraga kontakata po imenu i email-u.
- Dugme **Clear** za resetovanje forme.
- Validacija obaveznih polja: **First Name**, **Last Name** i **Email** (* označeno u formi).
- Dugme **Save** je onemogućeno dok obavezna polja nisu validna.

## Tehnologije i biblioteke

- WPF (.NET Framework)
- Entity Framework (Code First pristup)
- MySql.Data i MySql.Data.Entity.EF6
- ObservableCollection i data binding
- ICommand za dugmad

## Baza podataka

- Projekat koristi **MySQL** bazu podataka.
- Pristup bazi je realizovan pomoću **Code First** pristupa, tako da se tabela kreira automatski pri prvom pokretanju aplikacije.
- Tabela `contacts` sadrži kolone:
  - `id` (PK, auto increment)
  - `first_name` (obavezno)
  - `last_name` (obavezno)
  - `email` (obavezno)
  - `phone` (opciono)
  - `city` (opciono)
- Konekcioni string za MySQL treba podesiti u `App.config` (server, database, user, password).
- Opcionalno, može se koristiti SQL skripta za kreiranje tabele sa početnim podacima.

## Pokretanje aplikacije

1. Kloniraj ili preuzmi projekat sa GitHub-a.
2. Otvori solution u **Visual Studio**.
3. Ako je potrebno, uredi konekcioni string za MySQL bazu u `App.config` (server, database, user, password).
4. Instaliraj potrebne **NuGet pakete**:
   - `EntityFramework` 6.0.0.0  
   - `MySql.Data` 6.10.9.0  
   - `MySql.Data.Entity.EF6` 6.10.9.0
5. **Build-uj projekat**. Pri prvom pokretanju, Code First pristup će automatski kreirati bazu i tabelu `contacts`.
6. Pokreni aplikaciju (**F5**).

### U aplikaciji:

- Forma omogućava dodavanje novih kontakata.
- Selektovanjem kontakta iz tabele moguće je izvršiti izmenu ili brisanje.
- Dugme **Save** je onemogućeno dok obavezna polja (**First Name**, **Last Name** i **Email**) nisu popunjena.
- Dugme **Clear** prazni formu za unos novog kontakta.


