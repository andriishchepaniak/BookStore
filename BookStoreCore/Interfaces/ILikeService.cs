using BookStoreModels;
using BookStoreModels.GraphQLTypes.Likes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreCore.Interfaces
{
    public interface ILikeService
    {
        Task<AddLikePayload> Add(AddLikeInput like);
        Task<Like> Remove(int likeId);
    }
}
