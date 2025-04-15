using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Motosfera.Data.DbContext;
using Motosfera.Models;

namespace Motosfera.Controllers
{
    public class PostController : Controller
    {
        private readonly MotosferaContext _context;

        public PostController(MotosferaContext context)
        {
            _context = context;            
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts.ToListAsync();
            return View(posts);
        }

        public async Task<IActionResult> Details(int id)
        {
            var post = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (post == null) return NotFound();

            return View(post);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] Post post, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                string fileName = UploadFile(imageFile);
                post.ImagePath = fileName;
            }
            if (ModelState.IsValid)
            {
                post.CreatedAt = DateTime.UtcNow;

                _context.Add(post);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(post);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit (int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null) return NotFound();
            
            return View(post);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var postToDelete = await _context.Posts.FindAsync(id);

            if (postToDelete == null) { return NotFound(); }

            _context.Posts.Remove(postToDelete);
            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Post post, IFormFile ImageFile)
        {
            if (id != post.Id) return NotFound();

            var postToEdit = await _context.Posts.FindAsync(id);
            if (postToEdit == null) return NotFound();

            if (ImageFile != null)
            {
                string fileName = UploadFile(ImageFile);
                postToEdit.ImagePath = fileName;
            }

            postToEdit.Title = post.Title;
            postToEdit.Content = post.Content;
            
            _context.Update(postToEdit);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        private string UploadFile(IFormFile file)
        {
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return "/uploads/" + uniqueFileName;
        }
    }
}
