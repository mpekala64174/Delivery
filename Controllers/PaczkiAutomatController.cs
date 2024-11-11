using Delivery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
[Authorize]
public class PaczkiAutomatController : Controller
{
    private readonly ApplicationDbContext _context;

    public PaczkiAutomatController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var paczkiAutomat = _context.PaczkiAutomat
                                    .Include(pa => pa.PaczkiTransport) // Relacja z tabelą PaczkiTransport
                                    .Include(pa => pa.AutomatPaczkowy); // Relacja z tabelą AutomatPaczkowy
        return View(await paczkiAutomat.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var paczkiAutomat = await _context.PaczkiAutomat
            .Include(pa => pa.PaczkiTransport)
            .Include(pa => pa.AutomatPaczkowy)
            .FirstOrDefaultAsync(m => m.ID_paczki_automat == id);

        if (paczkiAutomat == null)
        {
            return NotFound();
        }

        return View(paczkiAutomat);
    }

    public IActionResult Create()
    {
        var paczkiTransportList = _context.PaczkiTransport.ToList();
        var automatList = _context.AutomatPaczkowy.ToList();

        if (paczkiTransportList == null || automatList == null)
        {
            throw new InvalidOperationException("One or more required lists could not be retrieved from the database.");
        }

        ViewData["PaczkiTransportList"] = paczkiTransportList;
        ViewData["AutomatList"] = automatList;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ID_paczki_transport, ID_automat")] PaczkiAutomat paczkiAutomat)
    {
        // Log submitted values for debugging purposes
        Console.WriteLine($"Submitted Transport ID: {paczkiAutomat.ID_paczki_transport}");
        Console.WriteLine($"Submitted Automat ID: {paczkiAutomat.ID_automat}");

        try
        {
            // Add the new PaczkiAutomat entity to the context
            _context.PaczkiAutomat.Add(paczkiAutomat);

            // Save changes to the database for the new PaczkiAutomat entry
            await _context.SaveChangesAsync();

            // Find the associated PaczkiTransport entry by ID
            var paczkaTransport = await _context.PaczkiTransport
                                                .FirstOrDefaultAsync(pt => pt.ID_paczki_transport == paczkiAutomat.ID_paczki_transport);

            if (paczkaTransport != null)
            {
                // Set the DataOddania to today's date
                paczkaTransport.DataOddania = DateTime.Now.Date;

                // Update the status in the Magazyn table (if applicable)
                var paczka = await _context.Magazyn.FindAsync(paczkaTransport.ID_paczki);
                if (paczka != null)
                {
                    paczka.Status = "Do odbioru";
                    _context.Update(paczka);
                }

                // Save the changes to PaczkiTransport and Magazyn
                _context.Update(paczkaTransport);
                await _context.SaveChangesAsync();
            }

            // Log success
            Console.WriteLine("Data saved successfully!");

            // Redirect to the Index action on success
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error saving data: {ex.Message}");

            // Optionally: Add an error message and return the same view
            ModelState.AddModelError("", "An error occurred while saving the data.");
        }

        // Return to the view in case of an error or invalid state
        return View(paczkiAutomat);
    }


    /*Bez edytowania*/

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var paczkiAutomat = await _context.PaczkiAutomat.FindAsync(id);
        if (paczkiAutomat == null)
        {
            return NotFound();
        }

        return View(paczkiAutomat);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ID_paczki_automat, ID_paczki_transport, ID_automat")] PaczkiAutomat paczkiAutomat)
    {
        if (id != paczkiAutomat.ID_paczki_automat)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(paczkiAutomat);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaczkiAutomatExists(paczkiAutomat.ID_paczki_automat))
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

        return View(paczkiAutomat);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var paczkiAutomat = await _context.PaczkiAutomat
            .Include(pa => pa.PaczkiTransport)
            .Include(pa => pa.AutomatPaczkowy)
            .FirstOrDefaultAsync(m => m.ID_paczki_automat == id);

        if (paczkiAutomat == null)
        {
            return NotFound();
        }

        return View(paczkiAutomat);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var paczkiAutomat = await _context.PaczkiAutomat.FindAsync(id);
        if (paczkiAutomat == null)
        {
            return NotFound();
        }

        _context.PaczkiAutomat.Remove(paczkiAutomat);
        await _context.SaveChangesAsync();

        // Redirect to Index after successful deletion
        return RedirectToAction(nameof(Index));
    }

    private bool PaczkiAutomatExists(int id)
    {
        return _context.PaczkiAutomat.Any(e => e.ID_paczki_automat == id);
    }
}
