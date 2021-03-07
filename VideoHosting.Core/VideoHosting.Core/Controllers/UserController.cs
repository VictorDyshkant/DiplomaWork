using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using VideoHosting.Core.Models;
using Microsoft.AspNetCore.Authorization;
using VideoHosting.Abstractions.Dto;
using VideoHosting.Abstractions.Services;

namespace VideoHosting.Core.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IMapper _mapper;

        public UserController(IUserService service, IMapper mapper)
        {
            _mapper = mapper;
            _userService = service;
        }


        [HttpPost]
        [Route("Exist")]
        public ActionResult IsExist(LoginUserModel model)
        {
            if (ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            return Ok(_userService.DoesExist(model.Email, model.PhoneNumber));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("Logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Bearer");
            return Ok();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("AddPhoto")]
        public async Task<ActionResult> UploadPhoto()
        {
            var files = HttpContext.Request.Form.Files;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "UsersContent/UsersPhotos");

            if (files[0].FileName.Contains(".JPG") || files[0].FileName.Contains(".png") || files[0].FileName.Contains(".jpg"))
            {
                string storePath = files[0].FileName.Contains(".JPG") || files[0].FileName.Contains(".jpg") ? User.Identity.Name + ".JPG" : User.Identity.Name + ".png";
                string fullPath = Path.Combine(path, storePath);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                using (var stream = System.IO.File.Create(fullPath))
                {
                    await files[0].CopyToAsync(stream);
                }

                UserDto user = await _userService.GetUserById(User.Identity.Name, User.Identity.Name);
                user.PhotoPath = storePath;
                await _userService.UpdateProfile(user);
            }
            else
            {
                return BadRequest("Image should be .jpg or .png");
            }
            return Ok();
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("Subscribe/{Id}")]
        public async Task<ActionResult> Subscribe(string id)
        {
            await _userService.Subscribe(User.Identity.Name, id);
            return Ok("You subscribed");
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("UpdateUser")]
        public async Task<ActionResult> UpdateUser(UserDto model)
        {
            if (ModelState.IsValid)
            {
                model.PhotoPath = null;
                model.Id = User.Identity.Name;

                await _userService.UpdateProfile(model);
                return Ok("You changed data");
            }
            return BadRequest("Invalid data");
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("profileUser/{Id}")]
        public async Task<ActionResult> GetUser(string id)
        {
            UserDto user = await _userService.GetUserById(id, User.Identity.Name);
            return Ok(user);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("subscribers")]
        public async Task<ActionResult> GetSubscribers()
        {
            IEnumerable<UserDto> users = await _userService.GetSubscribers(User.Identity.Name);
            return Ok(users);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("subscriptions")]
        public async Task<ActionResult> GetSubscriptions()
        {
            IEnumerable<UserDto> users = await _userService.GetSubscriptions(User.Identity.Name);
            return Ok(users);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("findByName/{str}")]
        public async Task<ActionResult> GetUserByName(string str)
        {
            IEnumerable<UserDto> users = await _userService.GetUserBySubName(str, User.Identity.Name);
            return Ok(users);
        }
    }
}
