using Microsoft.AspNetCore.Mvc;
using MovieRadar.Application.Interfaces;
using MovieRadar.Domain.Entities;

namespace MovieRadar.WebAPI.Controllers
{
    //public class CommentController : Controller
    //{
    //    public IActionResult Index()
    //    {
    //        return View();
    //    }
    //}

    [ApiController]
    [Route("api/[comments]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService commentService;
        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllComments()
        {
            var allComments = await commentService.GetAll();
            return Ok(allComments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetCommentById(int id)
        {
            var comment = await commentService.GetById(id);
            if(comment == null)
                return NotFound();

            return Ok(comment);
        }

        [HttpPost]
        public async Task<ActionResult> AddComment(Comment newComment)
        {
            var id = await commentService.Add(newComment);
            return CreatedAtAction(nameof(GetCommentById), new { id }, newComment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(Comment updatedComment, int id)
        {
            if(id != updatedComment.CommentId)
                return BadRequest();

            var updated = await commentService.Update(updatedComment);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var deleted = await commentService.DeleteById(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
