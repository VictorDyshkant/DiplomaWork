using System.Collections.Generic;
using System.Threading.Tasks;
using VideoHosting.Abstractions.Dto;

namespace VideoHosting.Abstractions.Services
{
    public interface IUserService
    {
        bool DoesExist(string email);

        Task AddUser(UserDto userDto, UserLoginDto userLoginDto);

        Task<UserDto> GetUserById(string id, string userId);

        Task<IEnumerable<UserDto>> GetSubscribers(string id);

        Task<IEnumerable<UserDto>> GetSubscriptions(string id);

        Task<IEnumerable<UserDto>> GetUserBySubName(string str, string userId);

        Task Subscribe(string subscriberId, string subscriptionId);

        Task UpdateProfile(UserDto userDto);
    }
}
