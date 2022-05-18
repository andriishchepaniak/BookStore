using BookStoreModels;
using BookStoreModels.GraphQLTypes.Dislikes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreCore.Interfaces
{
    public interface IDisLikeService
    {
        Task<DisLike> Add(AddDisLikeInput input);
        Task<DisLike> Remove(int disLikeId);
    }
}
