using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoHosting.Abstractions.Repositories;
using VideoHosting.Domain.Entities;

namespace VideoHosting.DataBase.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly DataBaseContext _context;

        public VideoRepository(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<Video> GetVideoById(Guid id)
        {
           return await _context.Videos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Video>> GetVideos()
        {
            return await _context.Videos.ToListAsync();
        }

        public async Task<IEnumerable<Video>> GetVideosByName(string name)
        {
            return await _context.Videos.Where(x=> x.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }

        public async Task AddVideo(Video video)
        {
            await _context.Videos.AddAsync(video);
        }

        public void RemoveVideo(Video video)
        {
            _context.Videos.Remove(video);
        }
    }
}
