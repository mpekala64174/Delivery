using Delivery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[Authorize]
public class UzytkownicyController : Controller
{
    private readonly ApplicationDbContext _context;

    public UzytkownicyController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Uzytkownicy.ToListAsync());
    }

    // GET: Uzytkownicy/Details/{id}
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var uzytkownik = await _context.Uzytkownicy
            .FirstOrDefaultAsync(m => m.ID_uzytkownika == id);

        if (uzytkownik == null)
        {
            return NotFound();
        }

        return View(uzytkownik);
    }


    // GET: Uzytkownicy/Create
    public IActionResult Create()
    {
        // Pass an empty Uzytkownicy model to the view
        return View(new Uzytkownicy());
    }


    // POST: Uzytkownicy/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Imie,Nazwisko,Login,Haslo,Rola")] Uzytkownicy uzytkownik)
    {
        if (ModelState.IsValid)
        {
            _context.Add(uzytkownik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(uzytkownik);
    }


    // GET: Uzytkownicy/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var uzytkownik = await _context.Uzytkownicy.FindAsync(id);
        if (uzytkownik == null)
        {
            return NotFound();
        }
        return View(uzytkownik);
    }

    // POST: Uzytkownicy/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ID_uzytkownika,Imie,Nazwisko,Login,Haslo,Rola")] Uzytkownicy uzytkownik)
    {
        // Ensure the ID matches the one in the URL
        if (id != uzytkownik.ID_uzytkownika)
        {
            return NotFound();
        }

        // Validate model state (important for form validation)
        if (ModelState.IsValid)
        {
            try
            {
                // Update the record in the database
                _context.Update(uzytkownik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect to the list page
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UzytkownicyExists(uzytkownik.ID_uzytkownika))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // If validation fails, return the same view with the existing data and validation errors
        return View(uzytkownik);
    }





    // GET: Uzytkownicy/Delete/{id}
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var uzytkownik = await _context.Uzytkownicy
            .FirstOrDefaultAsync(m => m.ID_uzytkownika == id);

        if (uzytkownik == null)
        {
            return NotFound();
        }

        return View(uzytkownik);
    }
    // POST: Uzytkownicy/Delete/{id} - DeleteConfirmed action
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var uzytkownik = await _context.Uzytkownicy.FindAsync(id);
        if (uzytkownik != null)
        {
            _context.Uzytkownicy.Remove(uzytkownik);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }



    private bool UzytkownicyExists(int id)
    {
        return _context.Uzytkownicy.Any(e => e.ID_uzytkownika == id);
    }
}
