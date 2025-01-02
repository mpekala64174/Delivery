using Delivery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
[Authorize]
public class AutomatPaczkowyController : Controller
{
    private readonly ApplicationDbContext _context;

    public AutomatPaczkowyController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: AutomatPaczkowy/Index
    public async Task<IActionResult> Index()
    {
        var automatPaczkowy = await _context.AutomatPaczkowy.ToListAsync();
        return View(automatPaczkowy);
    }

    // GET: AutomatPaczkowy/Details/1
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var automatPaczkowy = await _context.AutomatPaczkowy
            .FirstOrDefaultAsync(m => m.ID_automat == id);

        if (automatPaczkowy == null)
        {
            return NotFound();
        }

        return View(automatPaczkowy);
    }

    // GET: AutomatPaczkowy/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: AutomatPaczkowy/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Lokalizacja")] AutomatPaczkowy automatPaczkowy)
    {
        if (ModelState.IsValid)
        {
            _context.Add(automatPaczkowy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(automatPaczkowy);
    }

    // GET: AutomatPaczkowy/Delete/1
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var automatPaczkowy = await _context.AutomatPaczkowy
            .FirstOrDefaultAsync(m => m.ID_automat == id);

        if (automatPaczkowy == null)
        {
            return NotFound();
        }

        return View(automatPaczkowy);
    }

    // POST: AutomatPaczkowy/Delete/1
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var automatPaczkowy = await _context.AutomatPaczkowy.FindAsync(id);
        if (automatPaczkowy != null)
        {
            _context.AutomatPaczkowy.Remove(automatPaczkowy);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool AutomatPaczkowyExists(int id)
    {
        return _context.AutomatPaczkowy.Any(e => e.ID_automat == id);
    }

    [HttpGet]
    public async Task<IActionResult> ExportToCsv()
    {
        var data = await _context.AutomatPaczkowy.ToListAsync();

        var csv = new StringBuilder();
        csv.AppendLine("ID_automat;Lokalizacja"); // Header with semicolon separator

        foreach (var item in data)
        {
            csv.AppendLine($"{item.ID_automat};{item.Lokalizacja}");
        }

        var bytes = Encoding.UTF8.GetBytes(csv.ToString());
        var output = new FileContentResult(bytes, "text/csv")
        {
            FileDownloadName = "AutomatPaczkowy.csv"
        };

        return output;
    }
}
