using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_LW.Data;
using Web_LW.Entities;

namespace Web_LW.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // 🔐 Vérification Admin
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        private IActionResult ProtectAdmin()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            return null;
        }

        // GET : /Categories
        public async Task<IActionResult> Index()
        {
            var protect = ProtectAdmin();
            if (protect != null) return protect;

            return View(await _context.Categories.ToListAsync());
        }

        // GET : /Categories/Create
        public IActionResult Create()
        {
            var protect = ProtectAdmin();
            if (protect != null) return protect;

            return View();
        }

        // POST : /Categories/Create
        [HttpPost]
        public async Task<IActionResult> Create(Categorie c)
        {
            Console.WriteLine(">>> POST REÇU : " + c.Nom);

            var protect = ProtectAdmin();
            if (protect != null) return protect;

            if (ModelState.IsValid)
            {
                _context.Add(c);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(c);
        }

        // GET : /Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var protect = ProtectAdmin();
            if (protect != null) return protect;

            var cat = await _context.Categories.FindAsync(id);
            if (cat == null) return NotFound();

            return View(cat);
        }

        // POST : /Categories/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Categorie c)
        {
            var protect = ProtectAdmin();
            if (protect != null) return protect;

            if (ModelState.IsValid)
            {
                _context.Update(c);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(c);
        }

        // GET : /Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var protect = ProtectAdmin();
            if (protect != null) return protect;

            var cat = await _context.Categories.FindAsync(id);
            if (cat == null) return NotFound();

            return View(cat);
        }

        // POST : Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var protect = ProtectAdmin();
            if (protect != null) return protect;

            var cat = await _context.Categories.FindAsync(id);
            if (cat != null)
            {
                _context.Categories.Remove(cat);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
