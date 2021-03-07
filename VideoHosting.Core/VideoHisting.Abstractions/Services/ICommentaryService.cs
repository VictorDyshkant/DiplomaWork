using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoHosting.Abstractions.Dto;

namespace VideoHosting.Abstractions.Services
{
    public interface ICommentaryService
    {
        Task AddCommentary(CommentaryDto commentary);

        Task RemoveCommentary(Guid id);

        Task<CommentaryDto> GetCommentaryById(Guid id);

        Task<IEnumerable<CommentaryDto>> GetCommentariesByVideoId(Guid videoId);
    }
}
