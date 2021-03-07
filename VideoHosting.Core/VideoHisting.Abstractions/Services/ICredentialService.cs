using System.Threading.Tasks;
using VideoHosting.Abstractions.Dto;

namespace VideoHosting.Abstractions.Services
{
    public interface ICredentialService
    {
        Task AddAdmin(string id);

        Task RemoveAdmin(string id);

        Task<int> DropPassword(string email);

        Task ResetPassword(int tempPassword, string email, string newPassword);

        Task ResetPassword(string userId, string newPassword, string oldPassword);

        Task UpdateLogin(UserLoginDto userDto);

        Task<UserLoginDto> GetUserLogin(string userId);
    }
}
