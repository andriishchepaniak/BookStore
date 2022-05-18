using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreModels.GraphQLTypes.Likes
{
    public record AddLikeInput(int userId, int bookId);
}
