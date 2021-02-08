using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using My_Gallery.Models;

namespace My_Gallery.Controllers
{
    public class TagsController : Controller
    {
        private readonly DatabaseContext _context;

        public TagsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Tags
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tags.ToListAsync());
        }

        // GET: Tags/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tags = await _context.Tags
                .FirstOrDefaultAsync(m => m.Email == id);
            if (tags == null)
            {
                return NotFound();
            }

            return View(tags);
        }

        // GET: Tags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,ImageName,Location")] Tags tags)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tags);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tags);
        }

        // GET: Tags/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tags = await _context.Tags.FindAsync(id);
            if (tags == null)
            {
                return NotFound();
            }
            return View(tags);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Email,ImageName,Location")] Tags tags)
        {
            if (id != tags.Email)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tags);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagsExists(tags.Email))
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
            return View(tags);
        }

        // GET: Tags/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tags = await _context.Tags
                .FirstOrDefaultAsync(m => m.Email == id);
            if (tags == null)
            {
                return NotFound();
            }

            return View(tags);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tags = await _context.Tags.FindAsync(id);
            _context.Tags.Remove(tags);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TagsExists(string id)
        {
            return _context.Tags.Any(e => e.Email == id);
        }
    }
}
