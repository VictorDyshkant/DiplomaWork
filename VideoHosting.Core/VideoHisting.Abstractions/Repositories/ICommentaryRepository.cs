using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoHosting.Domain.Entities;

namespace VideoHosting.Abstractions.Repositories
{
    public interface ICommentaryRepository
    {
        void AddCommentary(Commentary commentary);

        void RemoveCommentary(Commentary commentary);

        Task<IEnumerable<Commentary>> GetCommentariesByVideoId(Guid id);

        Task<Commentary> GetCommentaryById(Guid id);
    }
}
