using BookStoreCore.Interfaces;
using BookStoreModels;
using BookStoreModels.GraphQLTypes.Dislikes;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreCore.Services
{
    public class DisLikeService : IDisLikeService
    {
        private readonly BookStoreContext _context;

        public DisLikeService(BookStoreContext context)
        {
            _context = context;
        }
        
        public async Task<DisLike> Add(AddDisLikeInput input)
        {
            var disLike = new DisLike
            {
                UserId = input.userId,
                BookId = input.bookId
            };
            var newDisLike = (await _context.DisLikes.AddAsync(disLike)).Entity;
            await _context.SaveChangesAsync();
            return newDisLike;
        }

        public async Task<DisLike> Remove(int disLikeId)
        {
            var disLike = await _context.DisLikes.FirstOrDefaultAsync(l => l.Id == disLikeId);
            if (disLike != null)
            {
                _context.DisLikes.Remove(disLike);
                await _context.SaveChangesAsync();
            }
            return disLike;
        }
    }
}
