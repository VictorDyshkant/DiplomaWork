using AutoMapper;
using System;
using System.IO;
using System.Threading.Tasks;
using VideoHosting.Abstractions.Dto;
using VideoHosting.Abstractions.Services;
using VideoHosting.Abstractions.UnitOfWork;
using VideoHosting.Domain.Entities;

namespace VideoHosting.Services.Services
{
    public class CredentialService : ICredentialService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public CredentialService(IUnitOfWork unit, IMapper mapper)
        {
            _unitOfWork = unit;
            _mapper = mapper;
        }

        public async Task AddAdmin(string id)
        {
            User user = await _unitOfWork.UserManager.FindByIdAsync(id);
            bool isAdmin = await _unitOfWork.UserManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin)
            {
                await _unitOfWork.UserManager.AddToRoleAsync(user, "Admin");
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task RemoveAdmin(string id)
        {
            User user = await _unitOfWork.UserManager.FindByIdAsync(id);

            if (await _unitOfWork.UserManager.IsInRoleAsync(user, "Admin"))
            {
                await _unitOfWork.UserManager.RemoveFromRoleAsync(user, "Admin");
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<int> DropPassword(string email)
        {
            User user = await _unitOfWork.UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new InvalidDataException("Invalid email");
            }

            int password = new Random().Next(1000, 1000000);
            user.TempPassword = password;

            await _unitOfWork.SaveAsync();
            return password;
        }

        public async Task ResetPassword(int tempPassword, string email, string newPassword)
        {
            User user = await _unitOfWork.UserManager.FindByEmailAsync(email);
            if (user.TempPassword == tempPassword && user.TempPassword != null)
            {
                user.TempPassword = null;
                await _unitOfWork.UserManager.RemovePasswordAsync(user);
                await _unitOfWork.UserManager.AddPasswordAsync(user, newPassword);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new InvalidDataException("Invalid password or login");
            }
        }

        public async Task ResetPassword(string userId, string newPassword, string oldPassword)
        {
            User user = await _unitOfWork.UserManager.FindByIdAsync(userId);
            await _unitOfWork.UserManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task UpdateLogin(UserLoginDto userDto)
        {
            User user = await _unitOfWork.UserManager.FindByIdAsync(userDto.Id);

            user.Email = userDto.Email ?? user.Email;
            user.PhoneNumber = userDto.PhoneNumber ?? user.PhoneNumber;

            await _unitOfWork.SaveAsync();
        }

        public async Task<UserLoginDto> GetUserLogin(string userId)
        {
            User user = await _unitOfWork.UserManager.FindByIdAsync(userId);
            return _mapper.Map<UserLoginDto>(user);
        }
    }
}
