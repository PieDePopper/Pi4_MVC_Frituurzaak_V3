# Pi4_MVC_Frituurzaak_V3

3.ASP.NET Core MVC-applicatie
Dit Deelhoofdstuk beschrijft de stappen die genomen zijn om een ASP.NET Core MVC-applicatie te maken. Het MVC-patroon (Model-View-Controller) wordt gebruikt om de scheiding van verantwoordelijkheden in de applicatie te vergemakkelijken.

Stappen
1. Modellen maken
De eerste stap in dit proces omvat het maken van modellen voor de applicatie. Modellen worden gebruikt om gegevens en entiteiten te vertegenwoordigen. Modellen kunnen worden gemaakt in de Models-map van het project.
In het geval van de Frituurzaak wordt hiervoor de class Item.cs aangemaakt. Zie onderstaande weergave:
namespace ModelViewController_TRISTAN.Models
{
    public class Item
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public float Price { get; set; }

     
    }
}

2. NuGet-packages installeren
Om de benodigde functionaliteit toe te voegen, moeten NuGet-packages worden geïnstalleerd, zoals Entity Framework Core en andere afhankelijkheden voor het project. Zie onderstaand weergave
 




3. DBContext maken
Een databasecontext (DBContext) moet worden gemaakt voor de applicatie. Dit wordt bereikt door een klasse te maken die is afgeleid van AppDbContext. In deze klasse kunnen configuraties voor de modellen worden gedefinieerd. Zie onderstaande weergave:
namespace ModelViewController_TRISTAN.Models
{
    using Microsoft.EntityFrameworkCore;

    namespace DemoVrijdag.Models
    {
        public class AppDBContext : DbContext
        {
            public DbSet<Item> Items { get; set; }

            public AppDBContext(DbContextOptions<AppDBContext> contextOptions) : base(contextOptions)
            {
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                base.OnConfiguring(optionsBuilder);
            }
        }
    }
}

De OnConfiguring-methode die in de bovenstaande weergave wordt getoond wordt vaak gebruikt om te vertellen hoe je toegang moet krijgen tot de database als er geen speciale configuratie is ingesteld via dependency injection (DI) in de Startup.cs-klasse.
Dit betekent dat als je niet al hebt verteld hoe je applicatie met de database moet praten, je de OnConfiguring-methode kunt gebruiken om dat te doen.

4. Connection strings in appsettings
De verbindingsreeksen voor de database moeten worden gedefinieerd in het appsettings.json-bestand. Hierin worden de benodigde gegevens opgeslagen om verbinding te maken met de database. Zie onderstaande weergave
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=DatabaseFrituurPI4;Trusted_Connection=True;TrustServerCertificate=True"
  }
}

5. Program.cs: Services toevoegen aan DI-container
In het Program.cs-bestand moeten de services worden toegevoegd aan de Dependency Injection (DI) container. Dit omvat het configureren van de databasecontext en het toevoegen van services die nodig zijn voor MVC. Zie onderstaande Weergave:
using Microsoft.EntityFrameworkCore;
using ModelViewController_TRISTAN.Models.DemoVrijdag.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDBContext>(
               DbContextOptions =>
               DbContextOptions.UseSqlServer(
                   builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

6. Migrations maken
Entity Framework Core biedt migraties om de database te maken op basis van de modelklassen. De volgende commando’s zijn gebruikt in de Package Manager Console om migraties te maken:
-Add-Migration    InitialCreate
Hiermee wordt een migratiebestand gegenereerd met de wijzigingen die moeten worden aangebracht in de database om de modelklassen weer te geven.

7. Database update
Om de database bij te werken volgens de gespecificeerde migraties, voert moet het volgende commando uitgevoerd worden
-Update-Database
Dit zorgt ervoor dat de database wordt aangepast op basis van de migraties.


Werking van de CRUD
In deze handleiding zal ik uitleggen hoe ik de HTML-weergaven en controllers heb gemaakt voor het uitvoeren van CRUD-operaties in mijn ASP.NET Core MVC-applicatie met behulp van het Item-model. CRUD staat voor Create, Read, Update en Delete, en dit zijn de basisbewerkingen voor gegevensbeheer in de meeste applicaties.



Create (Aanmaken)
Stap 1: Een nieuwe weergave maken voor het maken van een nieuw item
Om een nieuw item te kunnen maken, moest ik een HTML-weergave maken waar gebruikers de gegevens voor het nieuwe item kunnen invoeren.
Ik heb een nieuwe weergave aangemaakt met de naam Create. cshtml in de juiste map binnen de Views-map van mijn applicatie (bijvoorbeeld Views/Item).
In deze weergave heb ik een HTML-formulier gemaakt met invoervelden voor de naam en prijs van het item. Ik heb het form-element gebruikt om het formulier te definiëren.
Zie onderstaande weergave:
@model ModelViewController_TRISTAN.Models.Item

@{
    ViewData["Title"] = "Create";
}

<h1>Create</h1>

<h4>Item</h4>
<hr />
<div class="row">
    <div class="col-md-4">
        <form asp-action="Create">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            <div class="form-group">
                <label asp-for="Name" class="control-label"></label>
                <input asp-for="Name" class="form-control" />
                <span asp-validation-for="Name" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="Price" class="control-label"></label>
                <input asp-for="Price" type="number" step="0.01" class="form-control" />
                <span asp-validation-for="Price" class="text-danger"></span>
            </div>
            <div class="form-group">
                <input type="submit" value="Create" class="btn btn-primary" />
            </div>
        </form>
    </div>
</div>

<div>
    <a asp-action="Index">Back to List</a>
</div>

@section Scripts {
    @{
        await Html.RenderPartialAsync("_ValidationScriptsPartial");
    }
}
}



Stap 2: Een controlleractie maken voor het maken van een nieuw item
Nu moest ik een controlleractie maken die wordt aangeroepen wanneer het formulier wordt ingediend.
Ik heb een actie in de controller geïmplementeerd die de weergave voor het maken van een nieuw item weergeeft.
Ik heb een tweede actie in de controller geïmplementeerd om de gegevens van het ingediende formulier op te vangen, een nieuw item te maken en dit op te slaan in de databasecontext. Ik heb het HttpPost-attribuut gebruikt om deze actie aan te duiden als een HTTP POST-verzoek.
Zie onderstaande weergave:


namespace ModelViewController_TRISTAN.Controllers
{
    public class ItemsController : Controller
    {
        private readonly AppDBContext _context;

        public ItemsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
              return _context.Items != null ? 
                          View(await _context.Items.ToListAsync()) :
                          Problem("Entity set 'AppDBContext.Items'  is null.");
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Items == null)
            {
                return Problem("Entity set 'AppDBContext.Items'  is null.");
            }
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
          return (_context.Items?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}







Read (Lezen)
Stap 3: Een weergave maken voor het weergeven van items
Om items weer te geven, moest ik een weergave maken die de lijst met items toont. Ik heb een weergave gemaakt met de naam Index.cshtml in de juiste map binnen de Views-map van mijn applicatie (bijvoorbeeld Views/Item). In deze weergave heb ik een tabel gemaakt om de items weer te geven die zijn opgeslagen in de database.
@model ModelViewController_TRISTAN.Models.Item

@{
    ViewData["Title"] = "Details";
}

<h1>Details</h1>

<div>
    <h4>Item</h4>
    <hr />
    <dl class="row">
        <dt class = "col-sm-2">
            @Html.DisplayNameFor(model => model.Name)
        </dt>
        <dd class = "col-sm-10">
            @Html.DisplayFor(model => model.Name)
        </dd>
        <dt class = "col-sm-2">
            @Html.DisplayNameFor(model => model.Price)
        </dt>
        <dd class = "col-sm-10">
            @Html.DisplayFor(model => model.Price)
        </dd>
    </dl>
</div>
<div>
    <a asp-action="Edit" asp-route-id="@Model?.Id">Edit</a> |
    <a asp-action="Index">Back to List</a>
</div>


Stap 4: Een controlleractie maken voor het ophalen van items
Ik heb een actie in de controller geïmplementeerd (bijvoorbeeld Index) om de lijst met items op te halen uit de database en door te geven aan de Index-weergave.
        // GET: Items
        public async Task<IActionResult> Index()
        {
              return _context.Items != null ? 
                          View(await _context.Items.ToListAsync()) :
                          Problem("Entity set 'AppDBContext.Items'  is null.");
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }


Update (Bijwerken)
Stap 5: Een weergave maken voor het bewerken van een item
Om een bestaand item bij te werken, moest ik een weergave maken waarin gebruikers de gegevens van het item kunnen aanpassen.
Ik heb een weergave gemaakt met de naam Edit.cshtml in de juiste map binnen de Views-map van mijn applicatie (bijvoorbeeld Views/Item).
In deze weergave heb ik een HTML-formulier gemaakt dat lijkt op het formulier voor het maken van een nieuw item, maar met de huidige gegevens van het item ingevuld.
@model ModelViewController_TRISTAN.Models.Item

@{
    ViewData["Title"] = "Edit";
}

<h1>Edit</h1>

<h4>Item</h4>
<hr />
<div class="row">
    <div class="col-md-4">
        <form asp-action="Edit">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            <input type="hidden" asp-for="Id" />
            <div class="form-group">
                <label asp-for="Name" class="control-label"></label>
                <input asp-for="Name" class="form-control" />
                <span asp-validation-for="Name" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="Price" class="control-label"></label>
                <input asp-for="Price" type="number" step="0.01" class="form-control" />
                <span asp-validation-for="Price" class="text-danger"></span>
            </div>
            <div class="form-group">
                <input type="submit" value="Save" class="btn btn-primary" />
            </div>
        </form>
    </div>
</div>

<div>
    <a asp-action="Index">Back to List</a>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}


Stap 6: Een controlleractie maken voor het bijwerken van een item

Ik heb een actie in de controller geïmplementeerd die de gegevens van het te bewerken item ophaalt en deze weergeeft in de Edit-weergave.
Ik heb ook een tweede actie geïmplementeerd om de bijgewerkte gegevens van het formulier op te vangen, het item bij te werken in de databasecontext en de wijzigingen op te slaan.

Delete (Verwijderen)
Stap 7: Een weergave maken voor het verwijderen van een item
Om een item te verwijderen, heb ik een bevestigingsweergave gemaakt waarin gebruikers kunnen bevestigen dat ze het item willen verwijderen.
Ik heb een weergave gemaakt met de naam Delete.cshtml in de juiste map binnen de Views-map van mijn applicatie (bijvoorbeeld Views/Item).
In deze weergave heb ik een bevestigingsboodschap weergegeven en knoppen toegevoegd om het verwijderen te bevestigen of te annuleren.
@model ModelViewController_TRISTAN.Models.Item

@{
    ViewData["Title"] = "Delete";
}

<h1>Delete</h1>

<h3>Are you sure you want to delete this?</h3>
<div>
    <h4>Item</h4>
    <hr />
    <dl class="row">
        <dt class = "col-sm-2">
            @Html.DisplayNameFor(model => model.Name)
        </dt>
        <dd class = "col-sm-10">
            @Html.DisplayFor(model => model.Name)
        </dd>
        <dt class = "col-sm-2">
            @Html.DisplayNameFor(model => model.Price)
        </dt>
        <dd class = "col-sm-10">
            @Html.DisplayFor(model => model.Price)
        </dd>
    </dl>
    
    <form asp-action="Delete">
        <input type="hidden" asp-for="Id" />
        <input type="submit" value="Delete" class="btn btn-danger" /> |
        <a asp-action="Index">Back to List</a>
    </form>
</div>

Stap 8: Een controlleractie maken voor het verwijderen van een item
Ik heb een actie in de controller geïmplementeerd om de details van het item weer te geven dat ik wilde verwijderen.
Ik heb ook een tweede actie geïmplementeerd om het item daadwerkelijk te verwijderen wanneer de gebruiker bevestigt dat hij het wil verwijderen.
        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Items == null)
            {
                return Problem("Entity set 'AppDBContext.Items'  is null.");
            }
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
          return (_context.Items?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}


3.1.Weergave Pagina’s

Dit document bevat een overzicht van de verschillende pagina's in de Frituurapplicatie, samen met hun functies en toegangsvereisten. De Frituurapplicatie is ontworpen om zowel klanten als eigenaren van de frituur te ondersteunen. Hieronder volgt een beschrijving van de pagina's en hun functionaliteiten:


3.1.1 Home Pagina
 
De startpagina van de Frituurapplicatie verwelkomt bezoekers met een dynamische slideshow. Deze slideshow toont om de 5 seconden verschillende slides met afbeeldingen en tekst. Op de slides worden verschillende aanbiedingen, promoties en welkomstberichten weergegeven. Deze aantrekkelijke presentatie is bedoeld om klanten te enthousiasmeren en ze te verleiden om verder te verkennen.

In de navigatiebalk bovenaan de pagina zijn verschillende opties beschikbaar, waaronder:

Home: Hiermee kunnen gebruikers altijd terugkeren naar de startpagina.
Menu: Klanten kunnen op deze pagina het volledige menu van de frituur bekijken. Producten worden georganiseerd in categorieën en worden weergegeven met prijzen en beschrijvingen.
Bestellen: Deze pagina stelt klanten in staat om producten uit het menu toe te voegen aan hun winkelwagen en een bestelling te plaatsen.
Eigenaar Opties: Dit gedeelte is alleen toegankelijk voor de eigenaar van de frituur. Hier kan de eigenaar het assortiment beheren en de API van de applicatie inzien.
Onder de slideshow op de startpagina worden de populaire producten van de week gepresenteerd. Dit geeft klanten inzicht in welke items momenteel het meest in trek zijn.
De contactinformatie van de frituur is toegankelijk via de "Contact" sectie onder de populaire producten op de startpagina. Hier kunnen bezoekers belangrijke informatie vinden, zoals:

Openingstijden: De tijden waarop de frituur geopend is, zodat klanten weten wanneer ze een bestelling kunnen plaatsen of hun favoriete snacks kunnen halen.
Adres: Het fysieke adres van de frituur, zodat klanten weten waar ze naartoe moeten.
Telefoonnummer: Een telefoonnummer waarmee klanten direct contact kunnen opnemen met de frituur voor vragen of bestellingen.
E-mailadres: Een e-mailadres voor klanten die de frituur liever via e-mail willen bereiken.
Dit gedeelte is bedoeld om klanten de benodigde informatie te verschaffen om contact op te nemen met de frituur, ongeacht of ze vragen hebben over het menu, openingstijden of andere zaken.

 
  
Onder de slideshow op de startpagina worden de populaire producten van de week gepresenteerd. Dit geeft klanten inzicht in welke items momenteel het meest in trek zijn. 
 
  
De contactinformatie van de frituur is toegankelijk via de "Contact" sectie onder de populaire producten op de startpagina. Hier kunnen bezoekers belangrijke informatie vinden, zoals: 
Openingstijden: De tijden waarop de frituur geopend is, zodat klanten weten wanneer ze een bestelling kunnen plaatsen of hun favoriete snacks kunnen halen. 
Adres: Het fysieke adres van de frituur, zodat klanten weten waar ze naartoe moeten. 
Telefoonnummer: Een telefoonnummer waarmee klanten direct contact kunnen opnemen met de frituur voor vragen of bestellingen. 
E-mailadres: Een e-mailadres voor klanten die de frituur liever via e-mail willen bereiken. 
Dit gedeelte is bedoeld om klanten de benodigde informatie te verschaffen om contact op te nemen met de frituur, ongeacht of ze vragen hebben over het menu, openingstijden of andere zaken. 

 
 3.1.2.Menu-Pagina 
 
 
 
De Menu-pagina van de Frituurapplicatie biedt een overzicht van alle beschikbare producten, samen met hun prijzen. De pagina toont een lijst met producten met een productbeschrijving in het midden en de prijs rechts. Hier kunnen klanten eenvoudig door de beschikbare items bladeren, de productbeschrijvingen lezen en de prijzen bekijken voordat ze een keuze maken. 
Dit gedeelte van de applicatie is ontworpen om het klanten gemakkelijk te maken om te vinden wat ze willen bestellen. Het gebruiksvriendelijke ontwerp en de duidelijke presentatie van producten en prijzen stellen klanten in staat om snel hun keuzes te maken. De "Menu" -pagina draagt bij aan een soepele en aantrekkelijke winkelervaring voor de gebruikers. 
 
3.1.3.Bestel Pagina 
 
 
Bij aankomst op de Bestel-pagina worden klanten begroet met een overzicht van hun huidige bestelling. Hier kunnen ze alle geselecteerde items, samen met hun prijzen en de hoeveelheid van elk item in hun winkelwagen, in één oogopslag zien. Dit overzicht is een handige manier om ervoor te zorgen dat alles correct is voordat de bestelling wordt geplaatst. 
 
  
De Bestel-pagina biedt klanten volledige controle over hun bestelling. Als ze een ander product willen toevoegen, kunnen ze door de lijst met beschikbare producten bladeren en eenvoudigweg de gewenste items aan hun winkelwagen toevoegen. Onmiddellijk wordt de winkelwagen automatisch bijgewerkt om de nieuwe selectie te weerspiegelen. 
Klanten hebben de flexibiliteit om hun bestelling aan te passen. Soms veranderen ze van gedachten of selecteren ze per ongeluk het verkeerde item. Met slechts een paar klikken kunnen klanten producten verwijderen uit hun winkelwagen, waardoor ze de volledige controle hebben over hun bestelproces. 
De Bestel-pagina biedt klanten de mogelijkheid om kortingscodes in te voeren. Klanten kunnen hun kortingscode invoeren en het systeem valideert de code. Eventuele kortingen worden onmiddellijk toegepast op de totale bestelling, waardoor klanten kunnen profiteren van aanbiedingen en deals. 
De totale prijs wordt altijd duidelijk weergegeven op de Bestel-pagina, zodat klanten altijd de huidige kosten van hun bestelling kunnen controleren. 
  
Na het selecteren van de gewenste items en eventuele kortingen, kunnen klanten doorgaan naar de bevestigingspagina. Hier kunnen ze: 
Bank Kiezen: Klanten kiezen hun bank voor betaling. Er is een selectie van beschikbare banken om uit te kiezen. 
Totaalprijs Controleren: Klanten krijgen een laatste overzicht van hun bestelling en de totaalprijs voordat ze doorgaan. 
Bestelling Bevestigen: Zodra klanten tevreden zijn met hun bestelling, kunnen ze deze bevestigen. 
  
Na het bevestigen van de bestelling worden klanten doorverwezen naar een nieuwe pagina waar ze hun unieke ophaalnummer ontvangen. Dit nummer zal hen helpen bij het afhalen van hun bestelling. Ook is er een knop beschikbaar waarmee klanten eenvoudig kunnen terugkeren naar de homepagina. 






3.1.4. Eigenaar opties 
 3.1.4.1.Items Pagina 
 
 
De eigenaar van de frituur kan eenvoudig naar de Items-pagina navigeren door in te loggen op het eigenaarsaccount en vanuit het dashboard of het navigatiemenu de "Items" -optie te selecteren. 
Bij het openen van de Items-pagina wordt de eigenaar begroet met een lijst van alle beschikbare producten in het assortiment. Elke vermelding omvat de naam van het product, een beschrijving, de huidige prijs en een afbeelding van het item. 
De eigenaar heeft volledige controle over de items in het assortiment. Hij/zij kan: 
Toevoegen van Items: Er is een duidelijke optie om nieuwe items toe te voegen aan het assortiment. Hier kan de eigenaar productinformatie invullen, zoals naam, beschrijving en de prijs van het product. 
Bewerken van Items: Bestaande items kunnen eenvoudig worden bewerkt. De eigenaar kan bijvoorbeeld de naam, beschrijving of prijs bijwerken. 
Verwijderen van Items: Als een product niet langer beschikbaar is of uit het assortiment moet worden verwijderd, kan de eigenaar ervoor kiezen om het item te verwijderen. 
De Items-pagina is ontworpen met gebruiksgemak in gedachten. Alle acties zijn intuïtief en toegankelijk. De eigenaar kan snel wijzigingen aanbrengen en het assortiment aanpassen aan de behoeften van de frituur. 
 
3.1.4.2.API inzien 
Om toegang te krijgen tot de API-functionaliteit, kunnen eigenaars de "Eigenaar Opties" selecteren in het navigatiemenu. Binnen dit menu vinden ze een optie genaamd "API," waarmee ze toegang krijgen tot de API van de applicatie. 
De gedetailleerde documentatie voor de API-functionaliteit binnen de Frituurapplicatie is te vinden in Hoofdstuk 3.2, waarin alle beschikbare eindpunten, verzoeken en responsen in detail worden beschreven. 
 
3.1.5. Login en Registratie-Pagina 
Het login- en registratiegedeelte van de Frituurapplicatie biedt gebruikers de mogelijkheid om toegang te krijgen tot hun persoonlijke accounts en om nieuwe accounts aan te maken. Dit hoofdstuk beschrijft de login- en registratiefunctionaliteit en hoe deze wordt aangeboden in de applicatie. 
3.1.5.1.Login Pagina 
  
De loginpagina stelt bestaande gebruikers in staat om in te loggen op hun accounts door de volgende stappen te volgen: 
Email en Wachtwoord: Gebruikers moeten hun geregistreerde e-mailadres en wachtwoord invoeren. 
Inloggen: Na het invoeren van de vereiste gegevens kunnen gebruikers op de "Inloggen" knop klikken om toegang te krijgen tot hun account. 
Wachtwoord Vergeten: Als gebruikers hun wachtwoord zijn vergeten, kunnen ze op de "Wachtwoord vergeten" link klikken om instructies te ontvangen voor het herstellen van hun wachtwoord. 
 
3.1.5.2.Registratie Pagina 
  
Nieuwe gebruikers kunnen zich registreren voor een account met behulp van de volgende stappen: 
Registratieformulier: Gebruikers moeten persoonlijke informatie verstrekken waaronder hun e-mailadres en een wachtwoord. 
 Account Aanmaken: Na het invoeren van de vereiste informatie kunnen ze op de "Account aanmaken" knop klikken om hun account aan te maken. 
 
Bevestiging: Na registratie wordt een bevestigingspagina weergegeven om gebruikers te vragen hun e-mailadres te bevestigen. 
 
 3.1.5.3.Profiel Pagina 
 
Eenmaal ingelogd kunnen gebruikers hun persoonlijke profiel beheren. Ze hebben toegang tot de volgende functionaliteiten: 
 
Profielgegevens: Gebruikers kunnen hun profielgegevens, zoals hun naam en contactinformatie, bekijken en bijwerken. 
E-mail Adres: Het geregistreerde e-mailadres kan worden gewijzigd indien nodig. 
Wachtwoord Wijzigen: Gebruikers hebben de mogelijkheid om hun wachtwoord te wijzigen voor beveiligingsdoeleinden. 
Uitloggen: Gebruikers kunnen op elk moment uitloggen om hun sessie te beëindigen. 


3.2.API
Dit deelhoofdstuk biedt gedetailleerde documentatie voor de API die beschikbaar is binnen de Frituurapplicatie. De API stelt ontwikkelaars in staat om menu-items te beheren en biedt diverse functionaliteiten met betrekking tot producten, prijzen, en beschrijvingen. Het beschrijft in dit geval de API_ItemsController en geeft aan dat deze is ingebouwd in de MVC-applicatie, zonder de noodzaak van een afzonderlijk project. Bovendien wordt benadrukt dat de API-documentatie toegankelijk is via SwaggerUI.

Controller: API_ItemsController
De API_ItemsController is een essentieel onderdeel van de Frituurapplicatie, specifiek ontworpen voor productbeheer en menubeheerfunctionaliteiten. Deze controller, ingebed in de MVC-architectuur, dient als API-endpoint voor het benaderen van verschillende functies in de applicatie.

Beschikbare API-methoden
Hieronder vindt u een lijst van beschikbare API-methoden die ontwikkelaars kunnen aanroepen via de API_ItemsController:

GET /api/items

Beschrijving: Haal alle menu-items op.
Query Parameters: N/A
Verwachte Reactie: Een lijst met menu-items in JSON-indeling.
GET /api/items/{id}

Beschrijving: Haal een specifiek menu-item op op basis van het ID.
Query Parameters: N/A
Verwachte Reactie: Een enkel menu-item in JSON-indeling.
POST /api/items

Beschrijving: Voeg een nieuw menu-item toe.
Verzoek Lichaam: JSON-gegevens voor het nieuwe menu-item.
Verwachte Reactie: Het toegevoegde menu-item in JSON-indeling.
PUT /api/items/{id}

Beschrijving: Werk een bestaand menu-item bij op basis van het ID.
Verzoek Lichaam: JSON-gegevens met updates voor het menu-item.
Verwachte Reactie: Het bijgewerkte menu-item in JSON-indeling.
DELETE /api/items/{id}

Beschrijving: Verwijder een specifiek menu-item op basis van het ID.
Query Parameters: N/A
Verwachte Reactie: Een bevestiging van de verwijderingsactie.
Toegangscontrole:

De API_ItemsController vereist mogelijk specifieke autorisatieniveaus voor toegang, afhankelijk van de configuratie in de Frituurapplicatie. Gebruikers moeten mogelijk ingelogd zijn om bepaalde acties uit te voeren, zoals het toevoegen of bijwerken van menu-items.


SwaggerUI-documentatie
SwaggerUI, geïntegreerd in de Frituurapplicatie, biedt een intuïtieve interface om de API-documentatie te verkennen. Ontwikkelaars kunnen SwaggerUI openen via de Swagger-URL van de applicatie, waar ze inzicht krijgen in de beschikbare API-functionaliteiten, parameters en voorbeeldaanroepen. Hieronder is een weergave te zien van de API-ItemsController binnen SwaggerUI. Hierbinnen zijn alle m
