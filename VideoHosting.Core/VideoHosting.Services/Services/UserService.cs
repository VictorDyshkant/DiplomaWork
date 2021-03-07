using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoHosting.Abstractions.Dto;
using VideoHosting.Abstractions.Services;
using VideoHosting.Abstractions.UnitOfWork;
using VideoHosting.Domain.Entities;
using VideoHosting.Utilities.Constants;

namespace VideoHosting.Services.Services
{
    public class UserService : IUserService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public UserService(IUnitOfWork unit, IMapper mapper)
        {
            _unitOfWork = unit;
            _mapper = mapper;
        }

        public bool DoesExist(string email)
        {
            User user = _unitOfWork.UserManager.Users.FirstOrDefault(x => x.Email == email);
            return user != null;
        }

        public async Task AddUser(UserDto userDto, UserLoginDto userLoginDto)
        {
            User user = _mapper.Map<User>(userDto);

            await _unitOfWork.UserManager.CreateAsync(user, userLoginDto.Password);
            await _unitOfWork.UserManager.UpdateAsync(user);
            await _unitOfWork.UserManager.AddToRoleAsync(user, "User");
            await _unitOfWork.SaveAsync();
        }

        public async Task<UserDto> GetUserById(string id, string userId)
        {
            User user = await _unitOfWork.UserRepository.GetUserById(id);
            User userSub = await _unitOfWork.UserRepository.GetUserById(userId);

            UserDto userDto = _mapper.Map<UserDto>(user);
            userDto.DoSubscribed = user.Subscribers.FirstOrDefault(x => x.Subscripter == userSub) != null;
            userDto.Admin = await _unitOfWork.UserManager.IsInRoleAsync(userSub, "Admin");
            userDto.PhotoPath = _unitOfWork.AppSwitchRepository.GetValue(AppSwitchConstants.UserPhotoKey) + userDto.PhotoPath;

            return userDto;
        }

        public async Task<IEnumerable<UserDto>> GetSubscribers(string id)
        {
            User user = await _unitOfWork.UserRepository.GetUserById(id);
            List<UserDto> userDtos = new List<UserDto>();

            foreach (var subscriber in user.Subscribers.Select(x => x.Subscripter))
            {
                await GetUserById(subscriber.Id,id);
            }

            return userDtos;
        }

        public async Task<IEnumerable<UserDto>> GetSubscriptions(string id)
        {
            User user = await _unitOfWork.UserRepository.GetUserById(id);
            List<UserDto> userDtos = new List<UserDto>();

            foreach (var subscripter in user.Subscriptions.Select(x => x.Subscripter))
            {
                await GetUserById(subscripter.Id, id);
            }

            return userDtos;
        }

        public async Task<IEnumerable<UserDto>> GetUserBySubName(string str, string userId)
        {
            IEnumerable<User> users = await _unitOfWork.UserRepository.GetUserBySubName(str);
            List<UserDto> userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                await GetUserById(user.Id, userId);
            }

            return userDtos;
        }

        public async Task Subscribe(string subscriberId, string subscriptionId)
        {
            User subscriber = await _unitOfWork.UserRepository.GetUserById(subscriberId);
            User subscription = await _unitOfWork.UserRepository.GetUserById(subscriptionId);

            if (subscriber.Subscribers.FirstOrDefault(x => x.Subscripter == subscription) == null)
            {
                subscriber.Subscribe(subscription);
            }
            else
            {
                subscriber.Unsubscribe(subscription);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateProfile(UserDto userDto)
        {
            User user = await _unitOfWork.UserRepository.GetUserById(userDto.Id);

            user.Name = userDto.Name ?? user.Name;
            user.Surname = userDto.Surname ?? user.Surname;

            user.Group = userDto.Group ?? user.Group;
            user.Faculty = userDto.Faculty ?? user.Faculty;
            user.PhotoPath = userDto.PhotoPath ?? user.PhotoPath;
            user.Sex = userDto.Sex;

            await _unitOfWork.SaveAsync();
        }
    }
}
