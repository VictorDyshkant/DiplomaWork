using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using VideoHosting.Abstractions.Repositories;
using VideoHosting.Abstractions.UnitOfWork;
using VideoHosting.Domain.Entities;

namespace VideoHosting.DataBase.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataBaseContext _context;

        public UnitOfWork(UserManager<User> userManager, RoleManager<UserRole> roleManager, DataBaseContext dataBaseContext,
            IUserRepository userRepository, IVideoRepository videoRepository, ICommentaryRepository commentaryRepository,
            IAppSwitchRepository appSwitchRepository)
        {
            UserManager = userManager;
            _context = dataBaseContext;
            RoleManager = roleManager;
            UserRepository = userRepository;
            VideoRepository = videoRepository;
            CommentaryRepository = commentaryRepository;
            AppSwitchRepository = appSwitchRepository;
        }

        public IUserRepository UserRepository { get; }

        public IVideoRepository VideoRepository { get; }

        public ICommentaryRepository CommentaryRepository { get; }
        
        public IAppSwitchRepository AppSwitchRepository { get; }

        public UserManager<User> UserManager { get; }

        public RoleManager<UserRole> RoleManager { get; }

        public async Task<int> SaveAsync()
        {
           return await _context.SaveChangesAsync();
        }
    }
}
