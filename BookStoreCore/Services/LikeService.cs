using BookStoreCore.Interfaces;
using BookStoreModels;
using BookStoreModels.GraphQLTypes.Likes;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreCore.Services
{
    public class LikeService : ILikeService
    {
        private readonly BookStoreContext _context;

        public LikeService(BookStoreContext context)
        {
            _context = context;
        }
        public async Task<AddLikePayload> Add(AddLikeInput input)
        {
            var like = new Like
            {
                UserId = input.userId,
                BookId = input.bookId
            };
            var newLike = (await _context.Likes.AddAsync(like)).Entity;
            await _context.SaveChangesAsync();
            return new AddLikePayload(newLike);
        }

        public async Task<Like> Remove(int likeId)
        {
            var like = await _context.Likes.FirstOrDefaultAsync(l => l.Id == likeId);
            if (like != null)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
            }
            return like;
        }
    }
}
