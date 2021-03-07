using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VideoHosting.Abstractions.Dto;
using VideoHosting.Abstractions.Services;
using VideoHosting.Core.Models;
using IAuthorizationService = VideoHosting.Abstractions.Services.IAuthorizationService;

namespace VideoHosting.Core.Controllers
{
    [Route("api")]
    [ApiController]
    public class Authorization : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public Authorization(IAuthorizationService authorizationService, IUserService userService, IMapper mapper)
        {
            _authorizationService = authorizationService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(UserRegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                UserDto userDto = _mapper.Map<UserDto>(model);
                UserLoginDto userLogin = _mapper.Map<UserLoginDto>(model);

                await _userService.AddUser(userDto, userLogin);
                return Ok();
            }

            return BadRequest("Invalid data," + ModelState.Values);
        }

        [HttpPost]
        [Route("Authenticate")]
        public async Task<ActionResult> Authenticate(UserEnterModel model)
        {
            if (ModelState.IsValid)
            {
                var identity = await _authorizationService.Authenticate(model.Email, model.Password);

                if (identity == null)
                {
                    return BadRequest(new { errorText = "Invalid username or password." });
                }

                var now = DateTime.UtcNow;
                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                var response = new
                {
                    access_token = encodedJwt,
                    username = identity.Name
                };
                return Ok(response);
            }
            return BadRequest("Invalid login");
        }
    }
}
