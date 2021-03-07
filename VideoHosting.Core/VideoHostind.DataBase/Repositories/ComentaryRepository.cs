using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoHosting.Abstractions.Repositories;
using VideoHosting.Domain.Entities;

namespace VideoHosting.DataBase.Repositories
{
    public class CommentaryRepository : ICommentaryRepository
    {
        private readonly DataBaseContext _context;

        public CommentaryRepository(DataBaseContext context)
        {
            _context = context;
        }

        public void AddCommentary(Commentary commentary)
        {
            _context.Commentaries.Add(commentary);
        }

        public async Task<IEnumerable<Commentary>> GetCommentariesByVideoId(Guid id)
        {
            Video com = await _context.Videos.FirstOrDefaultAsync(x => x.Id == id);
            return com.Commentaries.ToList();
        }

        public async Task<Commentary> GetCommentaryById(Guid id)
        {
            return await _context.Commentaries.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void RemoveCommentary(Commentary commentary)
        {
            _context.Commentaries.Remove(commentary);
        }
    }
}
