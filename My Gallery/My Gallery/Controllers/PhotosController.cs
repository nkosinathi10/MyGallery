using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using My_Gallery.Models;

namespace My_Gallery.Controllers
{
    public class PhotosController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PhotosController(DatabaseContext context,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Photos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Photos.ToListAsync());
        }

        // GET: Photos/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photos = await _context.Photos
                .FirstOrDefaultAsync(m => m.Email == id);
            if (photos == null)
            {
                return NotFound();
            }

            return View(photos);
        }

        // GET: Photos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,ImageFile,Location")] Photos photos)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(photos.ImageFile.FileName);
                string extensioName = Path.GetExtension(photos.ImageFile.FileName);
                photos.ImageName = fileName = fileName + extensioName;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                photos.Location = path;
                photos.Email = @User.Identity.Name;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await photos.ImageFile.CopyToAsync(fileStream);
                }

                    _context.Add(photos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(photos);
        }

        // GET: Photos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photos = await _context.Photos.FindAsync(id);
            if (photos == null)
            {
                return NotFound();
            }
            return View(photos);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Email,ImageName,Location")] Photos photos)
        {
            if (id != photos.Email)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotosExists(photos.Email))
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
            return View(photos);
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photos = await _context.Photos
                .FirstOrDefaultAsync(m => m.Email == id);
            if (photos == null)
            {
                return NotFound();
            }

            return View(photos);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string Email)
        {
            var Photos = await _context.Photos.FindAsync(Email);

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Image", Photos.ImageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            var photos = await _context.Photos.FindAsync(Email);
            _context.Photos.Remove(photos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotosExists(string id)
        {
            return _context.Photos.Any(e => e.Email == id);
        }
    }
}
