using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VideoHosting.Abstractions.Dto;
using VideoHosting.Abstractions.Services;

namespace VideoHosting.Core.Controllers
{

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api")]
    [ApiController]
    public class CommentaryController : ControllerBase
    {
        private readonly ICommentaryService _commentaryService;
        public CommentaryController(ICommentaryService service)
        {
            _commentaryService = service;
        }

        [HttpGet]
        [Route("Commentary/{id}")]
        public async Task<ActionResult> GetCommentariesByVideoId(Guid id)
        {
            IEnumerable<CommentaryDto> commentaryDto = await _commentaryService.GetCommentariesByVideoId(id);
            return Ok(commentaryDto);
        }

        [HttpPost]
        [Route("Commentary")]
        public async Task<ActionResult> CreateCommentary(CommentaryDto model)
        {
            if (ModelState.IsValid)
            {
                await _commentaryService.AddCommentary(model);
                return Ok("You added Commentary");
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("Commentaries/{id}")]
        public async Task<ActionResult> DeleteCommentary(Guid id)
        {
            CommentaryDto commentary = await _commentaryService.GetCommentaryById(id);
            if (commentary.UserId == User.Identity.Name || User.IsInRole("Admin"))
            {
                await _commentaryService.RemoveCommentary(id);
                return Ok("This Commentary was deleted");
            }
            return Unauthorized();

        }
    }
}
