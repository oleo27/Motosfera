using Microsoft.AspNetCore.Mvc;
using Motosfera.Data.DbContext;
using Motosfera.Models;

namespace Motosfera.Controllers
{
	public class CommentController : Controller
	{
		private readonly MotosferaContext _context;

        public CommentController(MotosferaContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int postId, string name, string content)
        {
			if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(content))
            {
                return RedirectToAction("Details", "Post", new {id=postId});
            }

            var comment = new Comment
            {
                PostId = postId,
                Name = name,
                Content = content,
                CreatedAt = DateTime.UtcNow,
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

			return RedirectToAction("Details", "Post", new { id = postId });
		}

		public async Task<IActionResult> Delete(int id)
		{
			var commentToDelete = await _context.Comments.FindAsync(id);

			int postId = commentToDelete!.PostId;

			_context.Comments.Remove(commentToDelete);
			await _context.SaveChangesAsync();

			return RedirectToAction("Details", "Post", new { id = postId });
		}
	}
}
