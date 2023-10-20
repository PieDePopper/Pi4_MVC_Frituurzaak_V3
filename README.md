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
Home Pagina

Op de Home pagina van de applicatie word je welkom geheten met een vriendelijk welkomstbericht. Boven aan de pagina bevindt zich een handige navigatiebalk waarmee je kunt schakelen tussen verschillende secties van de applicatie.
Een van de beschikbare opties in de navigatiebalk is "Items". Wanneer je op "Items" klikt, word je naar een overzichtspagina geleid waar alle beschikbare items worden weergegeven. Voor elk item wordt zowel de naam als de prijs getoond, waardoor je snel een idee krijgt van wat elk item vertegenwoordigt.
Items Pagina

Naast de itemgegevens zie je ook actieknoppen, zoals "Edit", "Details" en "Delete". Deze knoppen bieden de mogelijkheid om specifieke acties uit te voeren voor elk item. Als je op "Edit" klikt, kun je de details van het item aanpassen. Als je op "Details" klikt, kun je gedetailleerde informatie over het item bekijken. En als je op "Delete" klikt, kun je het item permanent uit het systeem verwijderen.

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
