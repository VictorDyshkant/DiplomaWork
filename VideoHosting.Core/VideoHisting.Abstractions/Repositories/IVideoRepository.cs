using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoHosting.Domain.Entities;

namespace VideoHosting.Abstractions.Repositories
{
    public interface IVideoRepository
    {
        Task AddVideo(Video video);

        void RemoveVideo(Video video);

        Task<IEnumerable<Video>> GetVideosByName(string name);

        Task<IEnumerable<Video>> GetVideos();

        Task<Video> GetVideoById(Guid id);
        
    }
}
