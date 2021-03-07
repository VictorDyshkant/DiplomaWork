using System.Security.Claims;
using System.Threading.Tasks;

namespace VideoHosting.Abstractions.Services
{
    public interface IAuthorizationService
    {
        Task<ClaimsIdentity> Authenticate(string email, string password);
    }
}
