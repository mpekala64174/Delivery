using Delivery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
[Authorize]
public class PaczkiTransportController : Controller
{
    private readonly ApplicationDbContext _context;

    public PaczkiTransportController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var paczkiTransport = _context.PaczkiTransport
                                      .Include(pt => pt.Paczka) // Zakładając, że masz zdefiniowane relacje
                                      .Include(pt => pt.Transport);
        return View(await paczkiTransport.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var paczkiTransport = await _context.PaczkiTransport
            .Include(pt => pt.Paczka) // Fetch the related Paczka data
            .Include(pt => pt.Transport) // Fetch the related Transport data
            .FirstOrDefaultAsync(m => m.ID_paczki_transport == id);

        if (paczkiTransport == null)
        {
            return NotFound();
        }

        return View(paczkiTransport);  // Passing the paczkiTransport to the Details view
    }

    public IActionResult Create()
    {
        // Fetch the related lists from the database
        var paczkiList = _context.Magazyn.ToList();  // Assuming Magazyn is related to PaczkiTransport
        var transportList = _context.Transport.ToList();  // Assuming Transport is related to PaczkiTransport

        // Handle case if one of the lists is null
        if (paczkiList == null || transportList == null)
        {
            throw new InvalidOperationException("One or more required lists could not be retrieved from the database.");
        }

        // Pass the lists to the view via ViewData
        ViewData["PaczkiList"] = paczkiList;
        ViewData["TransportList"] = transportList;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ID_paczki, ID_transport")] PaczkiTransport paczkiTransport)
    {
        // Log the submitted data for debugging purposes
        Console.WriteLine($"Submitted Paczka ID: {paczkiTransport.ID_paczki}");
        Console.WriteLine($"Submitted Transport ID: {paczkiTransport.ID_transport}");

        // Set data_odbioru to today's date (instead of getting it from the form)
        paczkiTransport.DataOdbioru = DateTime.Now.Date;

        try
        {
            // If everything is valid, add the new record to the database
            _context.PaczkiTransport.Add(paczkiTransport);
            await _context.SaveChangesAsync();

            // Update the status of the paczka to "w trasie"
            var paczka = await _context.Magazyn.FindAsync(paczkiTransport.ID_paczki);
            if (paczka != null)
            {
                paczka.Status = "W trasie";
                _context.Update(paczka);
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
        return View(paczkiTransport);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var paczkiTransport = await _context.PaczkiTransport.FindAsync(id);
        if (paczkiTransport == null)
        {
            return NotFound();
        }
        ViewData["ID_paczki"] = new SelectList(_context.Magazyn, "ID_paczki", "rozmiar", paczkiTransport.ID_paczki);
        ViewData["ID_transport"] = new SelectList(_context.Transport, "ID_transport", "ID_transport", paczkiTransport.ID_transport);
        return View(paczkiTransport);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ID_paczki_transport,ID_paczki,ID_transport,data_odbioru,data_oddania")] PaczkiTransport paczkiTransport)
    {
        if (id != paczkiTransport.ID_paczki_transport)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(paczkiTransport);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaczkiTransportExists(paczkiTransport.ID_paczki_transport))
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
        ViewData["ID_paczki"] = new SelectList(_context.Magazyn, "ID_paczki", "rozmiar", paczkiTransport.ID_paczki);
        ViewData["ID_transport"] = new SelectList(_context.Transport, "ID_transport", "ID_transport", paczkiTransport.ID_transport);
        return View(paczkiTransport);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var paczkiTransport = await _context.PaczkiTransport
            .Include(pt => pt.Paczka)  // Include related Paczka
            .Include(pt => pt.Transport)  // Include related Transport
            .FirstOrDefaultAsync(m => m.ID_paczki_transport == id);

        if (paczkiTransport == null)
        {
            return NotFound();
        }

        return View(paczkiTransport);  // Pass the paczkiTransport to the Delete view
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var paczkiTransport = await _context.PaczkiTransport.FindAsync(id);
        if (paczkiTransport == null)
        {
            return NotFound();
        }

        _context.PaczkiTransport.Remove(paczkiTransport);  // Remove the PaczkiTransport entry
        await _context.SaveChangesAsync();  // Save changes to the database

        // Redirect to the Index page after successful deletion
        return RedirectToAction(nameof(Index));
    }

    private bool PaczkiTransportExists(int id)
    {
        return _context.PaczkiTransport.Any(e => e.ID_paczki_transport == id);
    }
    [HttpGet]
    public async Task<IActionResult> ExportToCsv()
    {
        // Retrieve the data from PaczkiTransport, including related Paczka and Transport entities
        var paczkiTransportData = await _context.PaczkiTransport
                                                 .Include(pt => pt.Paczka)
                                                 .Include(pt => pt.Transport)
                                                 .ToListAsync();

        var csv = new StringBuilder();
        csv.AppendLine("ID_paczki_transport;ID_paczki;Rozmiar_paczki;ID_transport;Data_odbioru;Data_oddania");

        foreach (var item in paczkiTransportData)
        {
            csv.AppendLine($"{item.ID_paczki_transport};{item.ID_paczki};{item.Paczka?.Rozmiar};{item.ID_transport};{item.DataOdbioru:yyyy-MM-dd};{item.DataOddania:yyyy-MM-dd}");
        }

        var bytes = Encoding.UTF8.GetBytes(csv.ToString());
        var output = new FileContentResult(bytes, "text/csv")
        {
            FileDownloadName = "PaczkiTransportData.csv"
        };

        return output;
    }

}
