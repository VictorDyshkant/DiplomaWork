using AutoMapper;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using VideoHosting.Abstractions.Services;
using VideoHosting.Abstractions.UnitOfWork;
using VideoHosting.Domain.Entities;

namespace VideoHosting.Services.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public AuthorizationService(IUnitOfWork unit, IMapper mapper)
        {
            _unitOfWork = unit;
            _mapper = mapper;
        }

        public async Task<ClaimsIdentity> Authenticate(string email, string password)
        {
            User user = await _unitOfWork.UserManager.FindByEmailAsync(email);
            if (await _unitOfWork.UserManager.CheckPasswordAsync(user, password) == false)
            {
                throw new InvalidDataException("Incorrect password");
            }

            var claims = new List<Claim>();
            var roles = await _unitOfWork.UserManager.GetRolesAsync(user);

            claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
            }

            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
