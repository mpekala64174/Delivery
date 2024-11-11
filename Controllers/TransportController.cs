using Delivery.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class TransportController : Controller
{
    private readonly ApplicationDbContext _context;

    public TransportController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Transport.Include(t => t.Uzytkownik).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transport = await _context.Transport
            .Include(t => t.Uzytkownik)
            .FirstOrDefaultAsync(m => m.ID_transport == id);
        if (transport == null)
        {
            return NotFound();
        }

        return View(transport);
    }
 
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ID_uzytkownika")] Transport transport)
    {
        // Directly add the transport record to the database without validation
        _context.Add(transport);
        await _context.SaveChangesAsync();

        // Redirect to the Index page after saving
        return RedirectToAction(nameof(Index));
    }




    /*Edit nie dziala*/
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transport = await _context.Transport.FindAsync(id);
        if (transport == null)
        {
            return NotFound();
        }
        ViewData["ID_uzytkownika"] = new SelectList(_context.Uzytkownicy, "ID_uzytkownika", "imie", transport.ID_uzytkownika);
        return View(transport);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ID_transport,ID_uzytkownika")] Transport transport)
    {
        if (id != transport.ID_transport)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(transport);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransportExists(transport.ID_transport))
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
        return View(transport);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transport = await _context.Transport
            .Include(t => t.Uzytkownik)
            .FirstOrDefaultAsync(m => m.ID_transport == id);

        if (transport == null)
        {
            return NotFound();
        }

        return View(transport);  // Passing the transport to the Delete view
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var transport = await _context.Transport.FindAsync(id);
        if (transport == null)
        {
            return NotFound();
        }

        _context.Transport.Remove(transport);
        await _context.SaveChangesAsync();

        // Redirecting to the Index page after successful deletion
        return RedirectToAction(nameof(Index));
    }


    private bool TransportExists(int id)
    {
        return _context.Transport.Any(e => e.ID_transport == id);
    }
}