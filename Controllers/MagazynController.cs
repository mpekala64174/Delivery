using Microsoft.AspNetCore.Mvc;
using Delivery.Models; // Twoje modele
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class MagazynController : Controller
{
    private readonly ApplicationDbContext _context;

    public MagazynController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Magazyn
    public async Task<IActionResult> Index()
    {
        var magazyn = await _context.Magazyn.ToListAsync();
        return View(magazyn);
    }

    // GET: Magazyn/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var magazyn = await _context.Magazyn
            .FirstOrDefaultAsync(m => m.ID_paczki == id);
        if (magazyn == null)
        {
            return NotFound();
        }

        return View(magazyn);
    }

    // GET: Magazyn/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Magazyn/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Waga,Rozmiar,MiejsceOdbioru,Status")] Magazyn magazyn)
    {
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                // Log or output the error message to the console or a log file
                Console.WriteLine($"Error: {error.ErrorMessage}");
            }
        }

        if (ModelState.IsValid)
        {
            _context.Add(magazyn);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(magazyn);
    }



    // GET: Magazyn/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var magazyn = await _context.Magazyn.FindAsync(id);
        if (magazyn == null)
        {
            return NotFound();
        }
        return View(magazyn);
    }

    // POST: Magazyn/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ID_paczki,Waga,Rozmiar,MiejsceOdbioru,Status")] Magazyn magazyn)
    {
        if (id != magazyn.ID_paczki)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(magazyn);  // Update the existing record
                await _context.SaveChangesAsync(); // Save changes to the database
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MagazynExists(magazyn.ID_paczki))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index)); // Redirect to the index page after successful update
        }
        return View(magazyn); // If validation fails, return the same view with errors
    }



    // GET: Magazyn/Delete/{id}
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var magazyn = await _context.Magazyn
            .FirstOrDefaultAsync(m => m.ID_paczki == id);

        if (magazyn == null)
        {
            return NotFound();
        }

        return View(magazyn);
    }

    // POST: Magazyn/Delete/{id} - DeleteConfirmed action
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var magazyn = await _context.Magazyn.FindAsync(id);
        if (magazyn != null)
        {
            _context.Magazyn.Remove(magazyn);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }



    private bool MagazynExists(int id)
    {
        return _context.Magazyn.Any(e => e.ID_paczki == id);
    }
}
