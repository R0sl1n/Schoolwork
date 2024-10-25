using BloggersUnite.Data.Repositories;
using BloggersUnite.Data.Repositories.Interfaces;
using BloggersUnite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BloggersUnite.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public CommentController(ICommentRepository commentRepository, IPostRepository postRepository, UserManager<IdentityUser> userManager)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _userManager = userManager;
        }

        // GET: Comment/Create
        public IActionResult Create(int postId)
        {
            var post = _postRepository.GetPostById(postId);
            if (post == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                PostId = postId
            };

            return View(comment);
        }

        // POST: Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                var post = _postRepository.GetPostById(comment.PostId);
                if (post == null)
                {
                    return NotFound();
                }

                comment.OwnerId = _userManager.GetUserId(User); // Sett eieren til innlogget bruker
                _commentRepository.CreateComment(comment);
                _commentRepository.Save();

                return RedirectToAction("Details", "Post", new { id = comment.PostId });
            }

            return View(comment);
        }

        // GET: Comment/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var comment = _commentRepository.GetCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId != comment.OwnerId)
            {
                return Forbid(); // Brukeren kan ikke redigere andres kommentarer
            }

            // Hent e-post for visning
            var owner = await _userManager.FindByIdAsync(comment.OwnerId);
            if (owner != null)
            {
                ViewBag.OwnerEmail = owner.Email;
            }

            return View(comment);
        }

        // POST: Comment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Comment comment)
        {
            // Hent den eksisterende kommentaren fra databasen uten tracking
            var existingComment = _commentRepository.GetCommentByIdAsNoTracking(comment.Id);
            if (existingComment == null)
            {
                return NotFound();
            }

            // Sjekk at nåværende bruker er eier av kommentaren
            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId != existingComment.OwnerId)
            {
                return Forbid(); // Brukeren kan ikke redigere andres kommentarer
            }

            // Behold OwnerId og PostId fra databasen
            comment.OwnerId = existingComment.OwnerId;
            comment.PostId = existingComment.PostId;

            // Behold Owner- og Post-objektene også, ellers blir de satt til null og ModelState feiler
            comment.Owner = existingComment.Owner;
            comment.Post = existingComment.Post;

            // Fjern valideringsfeil for "Post" og "Owner", siden de settes manuelt
            ModelState.Remove("Post");
            ModelState.Remove("Owner");

            // Sjekk om modellens felter er gyldige etter manuell setting
            if (ModelState.IsValid)
            {
                try
                {
                    // Oppdater kun Content
                    _commentRepository.UpdateComment(comment);
                    _commentRepository.Save();

                    return RedirectToAction("Details", "Post", new { id = comment.PostId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating comment: {ex.Message}");
                }
            }

            // Hvis ModelState feiler, sørg for at OwnerEmail vises riktig i viewet
            if (existingComment.Owner != null)
            {
                ViewBag.OwnerEmail = existingComment.Owner.Email;
            }

            return View(comment);
        }

        // GET: Comment/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var comment = _commentRepository.GetCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId != comment.OwnerId)
            {
                return Forbid(); // Brukeren kan ikke slette andres kommentarer
            }

            // Hent e-post for visning
            var owner = await _userManager.FindByIdAsync(comment.OwnerId);
            if (owner != null)
            {
                ViewBag.OwnerEmail = owner.Email;
            }

            return View(comment);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var comment = _commentRepository.GetCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId != comment.OwnerId)
            {
                return Forbid(); // Brukeren kan ikke slette andres kommentarer
            }

            _commentRepository.DeleteComment(comment);
            _commentRepository.Save();
            return RedirectToAction("Details", "Post", new { id = comment.PostId });
        }
    }
}
