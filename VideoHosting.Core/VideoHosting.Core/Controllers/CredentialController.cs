using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VideoHosting.Abstractions.Dto;
using VideoHosting.Abstractions.Services;
using VideoHosting.Core.Models;

namespace VideoHosting.Core.Controllers
{

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api")]
    [ApiController]
    public class CredentialController : ControllerBase
    {
        private readonly ICredentialService _credentialService;
        private readonly IMapper _mapper;

        public CredentialController(ICredentialService credentialService, IMapper mapper)
        {
            _mapper = mapper;
            _credentialService = credentialService;
        }

        [HttpPut]
        [Route("ResetPassword")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                await _credentialService.ResetPassword(model.UserId, model.Password, model.OldPassword);
                return Ok("You have new password");
            }

            return BadRequest("Invalid data");

        }

        [HttpPut]
        [Route("RecoverByEmail")]
        public async Task<ActionResult> ResetPasswordByEmail(ResetPasswordModelByEmail model)
        {
            if (ModelState.IsValid)
            {
                await _credentialService.ResetPassword(model.TempPassword, model.Email, model.Password);
                return Ok("You have new password");
            }

            return BadRequest("Invalid data");
        }

        [HttpPut]
        [Route("DropByEmail")]
        public async Task<ActionResult> DropPassword(string email)
        {
            int pass = await _credentialService.DropPassword(email);

            string myEmail = "dyshkant2804@gmail.com";
            string password = "Qwerty280400";

            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            var loginInfo = new NetworkCredential(email, password);

            msg.From = new MailAddress(myEmail);
            msg.To.Add(new MailAddress(email));
            msg.Subject = "Recreation password";
            msg.Body = "Here is your temporary password " + pass;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;


            smtpClient.Send(msg);
            return Ok("We sent you temporary password");
        }

        [HttpPut]
        [Route("addAdmin/{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> AddAdmin(string id)
        {
            await _credentialService.AddAdmin(id);
            return Ok();
        }

        [HttpPut]
        [Route("addAdmin/{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> RemoveAdmin(string id)
        {
            await _credentialService.RemoveAdmin(id);
            return Ok();
        }

        [HttpPut]
        [Route("updateUserLogin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> UpdateUserLogin(LoginUserModel model)
        {
            if (ModelState.IsValid)
            {
                UserLoginDto user = _mapper.Map<UserLoginDto>(model);
                user.Id = User.Identity.Name;

                await _credentialService.UpdateLogin(user);
                return Ok("You changed data");
            }
            return BadRequest("Invalid data");
        }

        [HttpGet]
        [Route("loginUser")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetLoginUser()
        {
            UserLoginDto user = await _credentialService.GetUserLogin(User.Identity.Name);
            return Ok(_mapper.Map<LoginUserModel>(user));
        }
    }
}
