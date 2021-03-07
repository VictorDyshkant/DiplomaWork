using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoHosting.Abstractions.Dto;
using VideoHosting.Abstractions.Services;
using VideoHosting.Abstractions.UnitOfWork;
using VideoHosting.Domain.Entities;
using VideoHosting.Services.Exceptions;
using VideoHosting.Utilities.Constants;

namespace VideoHosting.Services.Services
{
    public class VideoService : IVideoService
    {
        protected IUnitOfWork _unit;
        protected readonly IMapper _mapper;

        public VideoService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task AddVideo(VideoDto videoDto)
        {
            User user = await _unit.UserRepository.GetUserById(videoDto.UserId);
            Video video = new Video
            {
                Name = videoDto.Name,
                Description = videoDto.Description,
                Views = 0,
                User = user,
                PhotoPath = videoDto.PhotoPath,
                VideoPath = videoDto.VideoPath,
                DayOfCreation = DateTime.Now,
            };

            await _unit.VideoRepository.AddVideo(video);
        }

        public async Task RemoveVideo(Guid videoId)
        {
            Video video = await _unit.VideoRepository.GetVideoById(videoId);
            if (video == null)
            {
                throw new NotFoundException($"Video with id = {videoId} does not exist.");
            }

            _unit.VideoRepository.RemoveVideo(video);
            await _unit.SaveAsync();
        }

        public async Task AddView(Guid videoId)
        {
            Video video = await _unit.VideoRepository.GetVideoById(videoId);
            if (video == null)
            {
                throw new NotFoundException($"Video with id = {videoId} does not exist.");
            }

            video.Views++;
            await _unit.SaveAsync();
        }

        public async Task<VideoDto> GetVideoById(Guid videoId, string userId)
        {
            User user = await _unit.UserRepository.GetUserById(userId);
            Video video = await _unit.VideoRepository.GetVideoById(videoId);

            VideoDto videoDto = _mapper.Map<VideoDto>(video);

            videoDto.PhotoPath = _unit.AppSwitchRepository.GetValue(AppSwitchConstants.VideoPhotoKey) + videoDto.PhotoPath;
            videoDto.VideoPath = _unit.AppSwitchRepository.GetValue(AppSwitchConstants.VideoKey) + videoDto.VideoPath;
            videoDto.Liked = video.Likes.FirstOrDefault(x => x.User == user) == null;
            videoDto.Disliked = video.Dislikes.FirstOrDefault(x => x.User == user) == null;

            return videoDto;
        }

        public async Task<IEnumerable<VideoDto>> GetDisLikedVideos(string userId)
        {
            User user = await _unit.UserRepository.GetUserById(userId);
            IEnumerable<VideoDto> videoDtos = _mapper.Map<IEnumerable<VideoDto>>(user.Dislikes.Select(x => x.Video));

            foreach (var video in videoDtos)
            {
                video.PhotoPath = _unit.AppSwitchRepository.GetValue(AppSwitchConstants.VideoPhotoKey) + video.PhotoPath;
                video.VideoPath = _unit.AppSwitchRepository.GetValue(AppSwitchConstants.VideoKey) + video.VideoPath;
            }

            return videoDtos;
        }

        public async Task<IEnumerable<VideoDto>> GetLikedVideos(string userId)
        {
            User user = await _unit.UserRepository.GetUserById(userId);
            IEnumerable<VideoDto> videoDtos = _mapper.Map<IEnumerable<VideoDto>>(user.Likes.Select(x => x.Video));

            foreach (var video in videoDtos)
            {
                video.PhotoPath = _unit.AppSwitchRepository.GetValue(AppSwitchConstants.VideoPhotoKey) + video.PhotoPath;
                video.VideoPath = _unit.AppSwitchRepository.GetValue(AppSwitchConstants.VideoKey) + video.VideoPath;
            }

            return videoDtos;
        }

        public async Task<IEnumerable<VideoDto>> GetVideosOfSubscripters(string userId)
        {
            User user = await _unit.UserRepository.GetUserById(userId);

            List<VideoDto> list = new List<VideoDto>();
            foreach (var id in user.Subscriptions.Select(x => x.SubscripterId))
            {
                list.AddRange(await GetVideosOfUser(id));
            }

            return list.OrderBy(x => x.DayOfCreation);
        }

        public async Task<IEnumerable<VideoDto>> GetVideosByName(string name, string userId)
        {
            IEnumerable<Video> videos = await _unit.VideoRepository.GetVideosByName(name);
            videos.OrderByDescending(x => x.DayOfCreation).ToList();
            
            IEnumerable<VideoDto> videoDtos = _mapper.Map<IEnumerable<VideoDto>>(videos);

            foreach (var video in videoDtos)
            {
                video.PhotoPath = _unit.AppSwitchRepository.GetValue(AppSwitchConstants.VideoPhotoKey) + video.PhotoPath;
                video.VideoPath = _unit.AppSwitchRepository.GetValue(AppSwitchConstants.VideoKey) + video.VideoPath;
            }

            return videoDtos;
        }

        public async Task<IEnumerable<VideoDto>> GetVideosOfUser(string userId)
        {
            User user = await _unit.UserRepository.GetUserById(userId);
            IEnumerable<VideoDto> videoDtos = _mapper.Map<IEnumerable<VideoDto>>(user.Videos);

            foreach (var video in videoDtos)
            {
                video.PhotoPath = _unit.AppSwitchRepository.GetValue(AppSwitchConstants.VideoPhotoKey) + video.PhotoPath;
                video.VideoPath = _unit.AppSwitchRepository.GetValue(AppSwitchConstants.VideoKey) + video.VideoPath;
            }

            return videoDtos;
        }

        public async Task PutLike(Guid videoId, string userId)
        {
            User user = await _unit.UserRepository.GetUserById(userId);
            Video video = await _unit.VideoRepository.GetVideoById(videoId);
            if (video == null)
            {
                throw new InvalidDataException("This video do not exist");
            }

            user.AddLike(video);
            await _unit.SaveAsync();
        }

        public async Task PutDislike(Guid videoId, string userId)
        {
            User user = await _unit.UserRepository.GetUserById(userId);
            Video video = await _unit.VideoRepository.GetVideoById(videoId);
            if (video == null)
            {
                throw new InvalidDataException("This video do not exist");
            }

            user.AddDislike(video);
            await _unit.SaveAsync();
        }
    }
}
