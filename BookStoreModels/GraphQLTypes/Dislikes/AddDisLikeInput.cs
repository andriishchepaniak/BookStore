using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreModels.GraphQLTypes.Dislikes
{
    public record AddDisLikeInput(int userId, int bookId);
}
