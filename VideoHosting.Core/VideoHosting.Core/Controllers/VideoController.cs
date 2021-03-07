using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VideoHosting.Abstractions.Dto;
using VideoHosting.Abstractions.Services;
using VideoHosting.Services.Services;

namespace VideoHosting.Core.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [HttpPost]
        [Route("video")]
        public async Task<ActionResult> AddVideo(VideoDto model)
        {
            var files = HttpContext.Request.Form.Files;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "UsersContent");

            for (int i = 0; i < 2; i++)
            {
                string save = Guid.NewGuid().ToString();
                if (files[i].FileName.Contains(".JPG") || files[i].FileName.Contains(".jpg"))
                {
                    model.PhotoPath = save + ".jpg";
                    using (var stream = System.IO.File.Create(Path.Combine(path, "VideosPhotos", model.PhotoPath)))
                    {
                        await files[i].CopyToAsync(stream);
                    }
                }
                if (files[i].FileName.Contains(".png"))
                {
                    model.PhotoPath = save + ".png";
                    using (var stream = System.IO.File.Create(Path.Combine(path, "VideosPhotos", model.PhotoPath)))
                    {
                        await files[i].CopyToAsync(stream);
                    }
                }
                if (files[i].FileName.Contains(".mp4"))
                {
                    model.VideoPath = save + ".mp4";
                    using (var stream = System.IO.File.Create(Path.Combine(path, "VideosPhotos", model.VideoPath)))
                    {
                        await files[i].CopyToAsync(stream);
                    }
                }
            }

            await _videoService.AddVideo(model);

            return Ok(new [] { model.PhotoPath, model.VideoPath });
        }

        [HttpDelete]
        [Route("video/{id}")]
        public async Task<ActionResult> DeleteVideo(Guid id)
        {
            VideoDto videoDto = await _videoService.GetVideoById(id, User.Identity.Name);
            if (videoDto.UserId == User.Identity.Name || User.IsInRole("Admin"))
            {
                await _videoService.RemoveVideo(id);

                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "UsersContent/VideosPhotos/" + videoDto.PhotoPath));
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "UsersContent/UsersVideos/" + videoDto.VideoPath));

                return Ok("This video was deleted");
            }

            throw new Exception("You do not have permission");
        }

        [HttpGet]
        [Route("video/{id}")]
        public async Task<ActionResult> GetVideoById(Guid id)
        {
            VideoDto videoDto = await _videoService.GetVideoById(id, User.Identity.Name);
            await _videoService.AddView(id);
            return Ok(videoDto);
        }

        [HttpGet]
        [Route("videosuser/{id}")]
        public async Task<ActionResult> GetVideosOfUser(string id)
        {
            IEnumerable<VideoDto> videos = await _videoService.GetVideosOfUser(id);
            return Ok(videos);
        }

        [HttpGet]
        [Route("videos")]
        public async Task<ActionResult> GetVideosSubscribers()
        {
            IEnumerable<VideoDto> videos = await _videoService.GetVideosOfSubscripters(User.Identity.Name);
            return Ok(videos);
        }

        [HttpGet]
        [Route("videosLiked/{id}")]
        public async Task<ActionResult> GetLikedVideos(string id)
        {
            IEnumerable<VideoDto> videos = await _videoService.GetLikedVideos(id);
            return Ok(videos);
        }

        [HttpGet]
        [Route("videosDisliked/{id}")]
        public async Task<ActionResult> GetDislikedVideos(string id)
        {
            IEnumerable<VideoDto> videos = await _videoService.GetDisLikedVideos(id);
            return Ok(videos);
        }

        //[HttpGet]
        //[Route("allVideos")]
        //public async Task<ActionResult> GetVideo()
        //{
        //    IEnumerable<VideoDto> videos = await _videoService.GetVideos(User.Identity.Name);
        //    return Ok(videos);
        //}

        [HttpGet]
        [Route("videos/{name}")]
        public async Task<ActionResult> GetVideosName(string name)
        {
            IEnumerable<VideoDto> videos = await _videoService.GetVideosByName(name, User.Identity.Name);
            return Ok(videos);
        }


        [HttpPut]
        [Route("like/{id}")]
        public async Task<ActionResult> PutLike(Guid id)
        {
            await _videoService.PutLike(id, User.Identity.Name);
            return Ok();
        }

        [HttpPut]
        [Route("dislike/{id}")]
        public async Task<ActionResult> PutDislike(Guid id)
        {
            await _videoService.PutDislike(id, User.Identity.Name);
            return Ok();
        }
    }
}
