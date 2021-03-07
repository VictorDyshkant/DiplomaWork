using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using VideoHosting.Abstractions.Repositories;
using VideoHosting.Domain.Entities;

namespace VideoHosting.Abstractions.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        IVideoRepository VideoRepository { get; }

        ICommentaryRepository CommentaryRepository { get; }

        IAppSwitchRepository AppSwitchRepository { get; }

        UserManager<User> UserManager { get; }

        RoleManager<UserRole> RoleManager { get; }

        Task<int> SaveAsync();
    }
}
