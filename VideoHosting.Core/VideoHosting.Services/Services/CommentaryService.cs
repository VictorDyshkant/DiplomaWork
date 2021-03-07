using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using VideoHosting.Abstractions.Dto;
using VideoHosting.Abstractions.Services;
using VideoHosting.Abstractions.UnitOfWork;
using VideoHosting.Domain.Entities;

namespace VideoHosting.Services.Services
{
    public class CommentaryService : ICommentaryService
    {
        protected readonly IUnitOfWork _unit;
        protected readonly IMapper _mapper;
        
        public CommentaryService(IUnitOfWork unit,IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task AddCommentary(CommentaryDto commentaryDto)
        {
           Commentary commentary = new Commentary
            {
                Content = commentaryDto.Content,
                User = await _unit.UserRepository.GetUserById(commentaryDto.UserId),
                Video = await _unit.VideoRepository.GetVideoById(commentaryDto.VideoId),
                DayOfCreation = DateTime.Now
            };
           _unit.CommentaryRepository.AddCommentary(commentary);
           await _unit.SaveAsync();
        }

        public async Task<IEnumerable<CommentaryDto>> GetCommentariesByVideoId(Guid videoId)
        {
            IEnumerable<Commentary> commentaries = await _unit.CommentaryRepository.GetCommentariesByVideoId(videoId);
            return _mapper.Map<IEnumerable<CommentaryDto>>(commentaries);
        }

        public async Task<CommentaryDto> GetCommentaryById(Guid id)
        {
            Commentary commentary = await _unit.CommentaryRepository.GetCommentaryById(id);
            if (commentary == null)
            {
                throw new InvalidDataException("Commentary do not exist");
            }

            return _mapper.Map<CommentaryDto>(commentary);
        }

        public async Task RemoveCommentary(Guid id)
        {
            Commentary commentary = await _unit.CommentaryRepository.GetCommentaryById(id);
            if (commentary == null)
            {
                throw new InvalidDataException("Commentary do not exist");
            }

            _unit.CommentaryRepository.RemoveCommentary(commentary);
            await _unit.SaveAsync();
        }
    }
}
