using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesManagement.Data;
using NotesManagement.Models;

namespace NotesManagement.Controllers
{
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: NotesController
        public async Task<IActionResult> Index()
        {
            var notes = await _context.Notes.OrderByDescending(n => n.CreatedAt)
                        .ToListAsync();

            return View(notes);
        }

        // GET: NotesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NotesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NotesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] Note note)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    note.CreatedAt = DateTime.UtcNow;
                    _context.Add(note);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                var notes = await _context.Notes
                    .OrderByDescending(n => n.CreatedAt)
                    .ToListAsync();
                return View("Index", notes);
            }
            catch
            {
                return View();
            }
        }

        // GET: NotesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NotesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,Title,Content,CreatedAt")] Note note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    note.UpdatedAt = DateTime.UtcNow;
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: NotesController/Delete/5
        public ActionResult DeleteNote(int id)
        {
            return View();
        }

        // POST: NotesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var note = await _context.Notes.FindAsync(id);
                if (note != null)
                {
                    _context.Notes.Remove(note);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}
