using System.Linq;
using VideoHosting.Abstractions.Repositories;
using VideoHosting.Domain.Infrastructure;

namespace VideoHosting.DataBase.Repositories
{
    public class AppSwitchRepository : IAppSwitchRepository
    {
        private readonly DataBaseContext _context;

        public AppSwitchRepository(DataBaseContext context)
        {
            _context = context;
        }

        public string GetValue(string key)
        {
            AppSwitch appSwitch = _context.AppSwitches.FirstOrDefault(x => x.Key == key);
            return appSwitch?.Value;
        }
    }
}
