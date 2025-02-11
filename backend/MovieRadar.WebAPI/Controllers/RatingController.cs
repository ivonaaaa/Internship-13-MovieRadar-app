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
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService ratingService;
        public RatingController(IRatingService ratingService)
        {
            this.ratingService = ratingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rating>>> GetAllComments()
        {
            var allComments = await ratingService.GetAll();
            return Ok(allComments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rating>> GetCommentById(int id)
        {
            var comment = await ratingService.GetById(id);
            if(comment == null)
                return NotFound();

            return Ok(comment);
        }

        [HttpPost]
        public async Task<ActionResult> AddComment(Rating newComment)
        {
            var id = await ratingService.Add(newComment);
            return CreatedAtAction(nameof(GetCommentById), new { id }, newComment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(Rating updatedComment, int id)
        {
            if(id != updatedComment.Id)
                return BadRequest();

            var updated = await ratingService.Update(updatedComment);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var deleted = await ratingService.DeleteById(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
